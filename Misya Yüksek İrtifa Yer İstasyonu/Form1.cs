using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Drawing;
using System.Net;
using System.Diagnostics;
using GMap.NET;
using System.Xml.Linq;
using OpenTK.WinForms;
using GMap.NET.MapProviders;
using System.Runtime.InteropServices;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using CartesianChart = LiveCharts.WinForms.CartesianChart;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//1528; 904 UNUTMAAA!

namespace Misya_Yüksek_İrtifa_Yer_İstasyonu
{

    public partial class Form1 : Form
    {
        //Seri port aktarım değişkenleri
        private SerialPort roketPort;
        private SerialPort gorevPort;
        private SerialPort hakemPort;

        private bool roketPortDurum;
        private bool gorevPortDurum;
        private bool hakemPortDurum;



        //Video aktarım değişkenleri
        private Process ffmpegProcess;
        private Thread ffmpegThread;


        //HYİ'ye yollanacak veri paketi
        byte[] olusturalacak_paket = new byte[78];

        GMap.NET.WindowsForms.GMapControl gmap;

        private GLControl glControl;
        private float angle = 0.0f;
        private ObjLoader objLoader;

        //Rounded yap

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
      (
          int nLeftRect,     // x-coordinate of upper-left corner
          int nTopRect,      // y-coordinate of upper-left corner
          int nRightRect,    // x-coordinate of lower-right corner
          int nBottomRect,   // y-coordinate of lower-right corner
          int nWidthEllipse, // height of ellipse
          int nHeightEllipse // width of ellipse
      );

        public Form1()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
            richTextBox3.ReadOnly = true;
            //-------------------------------------------------------------------------------------------------
            gmap = gMapControl1;
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            //gmap.Dock = DockStyle.Fill;
            gmap.CanDragMap = false;
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.ShowCenter = false;
            gmap.MinZoom = 5;
            gmap.MaxZoom = 5;



            //-------------------------------------------------------------------------------------------------
            string[] ports = SerialPort.GetPortNames();  //Port isimlerini çek.
            comboBoxCOM.Items.AddRange(ports);  //Port isimlerini ekle.
            comboBox4.Items.AddRange(ports);

            if (ports.Length == 0)
            {
                MessageBox.Show("COM portu bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); //Eğer port bulamazsan hata attır.
            }

            //Round yap

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!roketPortDurum)
            {
                if (comboBoxCOM.SelectedItem != null)
                {
                    // Seçilen COM port adını alın
                    string selectedPort = comboBoxCOM.SelectedItem.ToString();
                    int baundRate = Convert.ToInt32(comboBox1.Text);

                    // SerialPort oluşturun ve bağlantıyı açın
                    roketPort = new SerialPort(selectedPort, baundRate); // COM port adı ve baud rate (9600) girilir

                    try
                    {
                        roketPort.Open();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Serial Port açılırken hata oluştu ! Hata: \n " + error.ToString());
                    }


                    // Veri alımı için bir event handler ekle. Her data geldiğinde tetiklenecek
                    roketPort.DataReceived += RoketSerial_Handler;

                    if (roketPort.IsOpen)
                    {
                        // Bağlantı durumunu güncelleyin
                        roketPortDurum = true;
                        label9.Text = "BAĞLANDI";
                        label9.BackColor = Color.Green;
                        button1.Text = "Bağlantıyı Kes";
                        label67.Text = "FIRLATMAYA HAZIR";

                    }
                }
                else
                {
                    MessageBox.Show("Lütfen bir COM port seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                try
                {
                    roketPort.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("HATA !" + error);
                }
                if (!roketPort.IsOpen)
                {
                    label9.Text = "BAĞLANTI KESİLDİ";
                    label9.BackColor = Color.Red;
                    button1.Text = "Bağlan";
                    roketPortDurum = false;
                }
            }
        }
        private void RoketSerial_Handler(object sender, SerialDataReceivedEventArgs e)
        {
            // Veri alındığında burası çalışır
            // Gelen veriyi okuyun
            string receivedData = roketPort.ReadLine();

            // Veriyi işleyin, ekrana yazdırabilir veya başka bir işlem yapabilirsiniz
            // Örneğin, bu örnekte gelen veriyi direkt olarak ekrana yazdırıyoruz
            this.Invoke(new MethodInvoker(delegate ()
            {
                richTextBox1.AppendText(receivedData); //UI threading
                richTextBox1.ScrollToCaret();
            }));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartFFmpeg();
        }

        private void StartFFmpeg()
        {
            ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = "ffmpeg.exe";
            ffmpegProcess.StartInfo.Arguments = "-i udp://192.168.137.113:5001 -f image2pipe -vcodec mjpeg -";
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            ffmpegProcess.EnableRaisingEvents = true;
            ffmpegProcess.OutputDataReceived += new DataReceivedEventHandler(FFmpegOutputHandler);
            ffmpegProcess.ErrorDataReceived += new DataReceivedEventHandler(FFmpegErrorHandler);

            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();
        }

        private void FFmpegOutputHandler(object sender, DataReceivedEventArgs e)
        {
            // FFmpeg çıktısını işleyin
            if (!String.IsNullOrEmpty(e.Data))
            {
                // Çıktıyı PictureBox kontrolünde göstermek için Invoke kullanın
                videoBox.Invoke((MethodInvoker)delegate
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(e.Data)))
                    {
                        videoBox.Image = Image.FromStream(ms);
                    }
                });
            }
        }

        private void FFmpegErrorHandler(object sender, DataReceivedEventArgs e)
        {
            // FFmpeg hata çıktısını işleyin
            if (!String.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine("FFmpeg Hata: " + e.Data);
            }
        }

        private void videoBox_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void glControl1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxCOM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!hakemPortDurum)
            {
                if (comboBox4.SelectedItem != null && comboBox5.SelectedItem != null)
                {
                    // Seçilen COM port adını alın
                    string selectedPort = comboBox4.SelectedItem.ToString();
                    int baundRate = Convert.ToInt32(comboBox5.Text);

                    // SerialPort oluşturun ve bağlantıyı açın
                    hakemPort = new SerialPort(selectedPort, baundRate); // COM port adı ve baud rate (9600) girilir

                    try
                    {
                        hakemPort.Open();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Serial Port açılırken hata oluştu ! Hata: \n " + error.ToString());
                    }

                    if (hakemPort.IsOpen)
                    {
                        // Bağlantı durumunu güncelleyin
                        hakemPortDurum = true;
                        label24.Text = "BAĞLANDI";
                        label24.BackColor = Color.Green;
                        button9.Text = "Bağlantıyı Kes";

                    }
                }
                else
                {
                    MessageBox.Show("Lütfen bir COM port seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                try
                {
                    hakemPort.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("HATA !" + error);
                }
                if (!hakemPort.IsOpen)
                {
                    label24.Text = "BAĞLANTI KESİLDİ";
                    label24.BackColor = Color.Red;
                    button9.Text = "Bağlan";
                    hakemPortDurum = false;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //DEBUG
            //EĞER SERİ PORT AÇILIRSA KISMI EKLE

        }

        void PaketOlustur
            (
            //Gönderilecek Veriler
            float RoketIrtifa = 10.2F,
            float RoketGPSIrtifa = 10.2F,
            float RoketEnlem = 10.2F,
            float RoketBoylam = 10.2F,
            float GorevYukuGPSIrtifa = 10.2F,
            float GorevYukuEnlem = 10.2F,
            float GorevYukuBoylam = 10.2F,
            float JiroskopX = 10.2F,
            float JiroskopY = 10.2F,
            float JiroskopZ = 10.2F,
            float IvmeX = 10.2F,
            float IvmeY = 10.2F,
            float IvmeZ = 10.2F,
            float Aci = 10.2F,
            byte durum = 1
            )
        {
            olusturalacak_paket[0] = 0xFF;  //Sabit
            olusturalacak_paket[1] = 0xFF;  //Sabit
            olusturalacak_paket[2] = 0x54;  //Sabit
            olusturalacak_paket[3] = 0x52;  //Sabit

            //-------------------------------------------------------

            olusturalacak_paket[4] = 0; //Takım ID ÖNEMLİ AMINAKOYİMM
            olusturalacak_paket[5] = 0; //Sayaç

            //------------------------------------------------------- ROKET IRTIFA

            byte[] irtifa_float32_uint8_donusturucu = BitConverter.GetBytes(RoketIrtifa);

            olusturalacak_paket[6] = irtifa_float32_uint8_donusturucu[0];
            olusturalacak_paket[7] = irtifa_float32_uint8_donusturucu[1];
            olusturalacak_paket[8] = irtifa_float32_uint8_donusturucu[2];
            olusturalacak_paket[9] = irtifa_float32_uint8_donusturucu[3];


            //------------------------------------------------------- ROKET IRTIFA

            byte[] roket_gps_irtifa_float32_uint8_donusturucu = BitConverter.GetBytes(RoketGPSIrtifa);

            olusturalacak_paket[10] = roket_gps_irtifa_float32_uint8_donusturucu[0];
            olusturalacak_paket[11] = roket_gps_irtifa_float32_uint8_donusturucu[1];
            olusturalacak_paket[12] = roket_gps_irtifa_float32_uint8_donusturucu[2];
            olusturalacak_paket[13] = roket_gps_irtifa_float32_uint8_donusturucu[3];

            //------------------------------------------------------- ROKET ENLEM

            byte[] roket_enlem_float32_uint8_donusturucu = BitConverter.GetBytes(RoketEnlem);

            olusturalacak_paket[14] = roket_enlem_float32_uint8_donusturucu[0];
            olusturalacak_paket[15] = roket_enlem_float32_uint8_donusturucu[1];
            olusturalacak_paket[16] = roket_enlem_float32_uint8_donusturucu[2];
            olusturalacak_paket[17] = roket_enlem_float32_uint8_donusturucu[3];

            //------------------------------------------------------- ROKET BOYLAM

            byte[] roket_boylam_irtifa_float32_uint8_donusturucu = BitConverter.GetBytes(RoketBoylam);

            olusturalacak_paket[18] = roket_boylam_irtifa_float32_uint8_donusturucu[0];
            olusturalacak_paket[19] = roket_boylam_irtifa_float32_uint8_donusturucu[1];
            olusturalacak_paket[20] = roket_boylam_irtifa_float32_uint8_donusturucu[2];
            olusturalacak_paket[21] = roket_boylam_irtifa_float32_uint8_donusturucu[3];

            //------------------------------------------------------- GOREV YUKU GPS

            byte[] gorev_yuku_gps_irtifa_float32_uint8_donusturucu = BitConverter.GetBytes(GorevYukuGPSIrtifa);

            olusturalacak_paket[22] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu[0];
            olusturalacak_paket[23] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu[1];
            olusturalacak_paket[24] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu[2];
            olusturalacak_paket[25] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu[3];

            //------------------------------------------------------- GOREV YUKU ENLEM

            byte[] gorev_yuku_enlem_float32_uint8_donusturucu = BitConverter.GetBytes(GorevYukuEnlem);

            olusturalacak_paket[26] = gorev_yuku_enlem_float32_uint8_donusturucu[0];
            olusturalacak_paket[27] = gorev_yuku_enlem_float32_uint8_donusturucu[1];
            olusturalacak_paket[28] = gorev_yuku_enlem_float32_uint8_donusturucu[2];
            olusturalacak_paket[29] = gorev_yuku_enlem_float32_uint8_donusturucu[3];

            //------------------------------------------------------- GOREV YUKU BOYLAM

            byte[] gorev_yuku_boylam_irtifa_float32_uint8_donusturucu = BitConverter.GetBytes(GorevYukuBoylam);

            olusturalacak_paket[26] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu[0];
            olusturalacak_paket[27] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu[1];
            olusturalacak_paket[28] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu[2];
            olusturalacak_paket[29] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu[3];

            //------------------------------------------------------- KADEME VERİSİ (YOOOK)








            //------------------------------------------------------- JİROSKOP X VERİSİ

            byte[] jiroskop_x_float32_uint8_donusturucu = BitConverter.GetBytes(JiroskopX);

            olusturalacak_paket[46] = jiroskop_x_float32_uint8_donusturucu[0];
            olusturalacak_paket[47] = jiroskop_x_float32_uint8_donusturucu[1];
            olusturalacak_paket[48] = jiroskop_x_float32_uint8_donusturucu[2];
            olusturalacak_paket[49] = jiroskop_x_float32_uint8_donusturucu[3];

            //------------------------------------------------------- JİROSKOP Y VERİSİ

            byte[] jiroskop_y_float32_uint8_donusturucu = BitConverter.GetBytes(JiroskopY);

            olusturalacak_paket[50] = jiroskop_y_float32_uint8_donusturucu[0];
            olusturalacak_paket[51] = jiroskop_y_float32_uint8_donusturucu[1];
            olusturalacak_paket[52] = jiroskop_y_float32_uint8_donusturucu[2];
            olusturalacak_paket[53] = jiroskop_y_float32_uint8_donusturucu[3];


            //------------------------------------------------------- JİROSKOP Z VERİSİ

            byte[] jiroskop_z_float32_uint8_donusturucu = BitConverter.GetBytes(JiroskopZ);

            olusturalacak_paket[54] = jiroskop_z_float32_uint8_donusturucu[0];
            olusturalacak_paket[55] = jiroskop_z_float32_uint8_donusturucu[1];
            olusturalacak_paket[56] = jiroskop_z_float32_uint8_donusturucu[2];
            olusturalacak_paket[57] = jiroskop_z_float32_uint8_donusturucu[3];

            //------------------------------------------------------- İVME X VERİSİ

            byte[] ivme_x_float32_uint8_donusturucu = BitConverter.GetBytes(IvmeX);

            olusturalacak_paket[58] = ivme_x_float32_uint8_donusturucu[0];
            olusturalacak_paket[59] = ivme_x_float32_uint8_donusturucu[1];
            olusturalacak_paket[60] = ivme_x_float32_uint8_donusturucu[2];
            olusturalacak_paket[61] = ivme_x_float32_uint8_donusturucu[3];

            //------------------------------------------------------- İVME Y VERİSİ

            byte[] ivme_y_float32_uint8_donusturucu = BitConverter.GetBytes(IvmeY);

            olusturalacak_paket[62] = ivme_y_float32_uint8_donusturucu[0];
            olusturalacak_paket[63] = ivme_y_float32_uint8_donusturucu[1];
            olusturalacak_paket[64] = ivme_y_float32_uint8_donusturucu[2];
            olusturalacak_paket[65] = ivme_y_float32_uint8_donusturucu[3];

            //------------------------------------------------------- İVME Z VERİSİ

            byte[] ivme_z_float32_uint8_donusturucu = BitConverter.GetBytes(IvmeZ);

            olusturalacak_paket[66] = ivme_z_float32_uint8_donusturucu[0];
            olusturalacak_paket[67] = ivme_z_float32_uint8_donusturucu[1];
            olusturalacak_paket[68] = ivme_z_float32_uint8_donusturucu[2];
            olusturalacak_paket[69] = ivme_z_float32_uint8_donusturucu[3];


            //------------------------------------------------------- AÇI VERİSİ

            byte[] aci_float32_uint8_donusturucu = BitConverter.GetBytes(Aci);

            olusturalacak_paket[70] = aci_float32_uint8_donusturucu[0];
            olusturalacak_paket[71] = aci_float32_uint8_donusturucu[1];
            olusturalacak_paket[72] = aci_float32_uint8_donusturucu[2];
            olusturalacak_paket[73] = aci_float32_uint8_donusturucu[3];

            //------------------------------------------------------- DURUM Verisi

            olusturalacak_paket[74] = (byte)durum;

            //------------------------------------------------------- CHEKCSUM VERİSİ

            //olusturalacak_paket[75] = check_sum_hesapla(); //BURASI YAZILACAK KNK !!!!!!!!!!

            //------------------------------------------------------- SABİTLER

            olusturalacak_paket[76] = 0x0D; // Sabit
            olusturalacak_paket[77] = 0x0A; // Sabit

        }

        void PaketiYolla()
        {
            hakemPort.Write(olusturalacak_paket, 0, olusturalacak_paket.Length);
            richTextBox3.AppendText("YOLLANAN VERİ: \n");
            richTextBox3.AppendText(BitConverter.ToString(olusturalacak_paket));
            richTextBox3.AppendText("\n \n");
            richTextBox3.ScrollToCaret();
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            PaketOlustur();
            PaketiYolla();
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void label67_Click(object sender, EventArgs e)
        {

        }



        private void button12_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            gmap.Position = new PointLatLng(Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox4.Text));
            gmap.Zoom = 5;
            gmap.Update();
            gmap.Refresh();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int zoomLevel = Convert.ToInt32(textBox3.Text);
            gmap.MinZoom = zoomLevel;
            gmap.MaxZoom = zoomLevel;
            gmap.Zoom = zoomLevel;
            gmap.Update();
            gmap.Refresh();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            switch (comboBox7.SelectedItem.ToString())
            {
                case "Uydu":
                    gmap.MapProvider = GMapProviders.GoogleSatelliteMap;
                    break;
                case "Hibrit":
                    gmap.MapProvider = GMapProviders.GoogleHybridMap;
                    break;
                case "Terrain":
                    gmap.MapProvider = GMapProviders.GoogleTerrainMap;
                    break;
                case "GMAP":
                    gmap.MapProvider = GMapProviders.GoogleMap;
                    break;
                default:
                    MessageBox.Show("allani si");
                    break;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            comboBoxCOM.Items.Clear();
            comboBox4.Items.Clear();
            string[] ports = SerialPort.GetPortNames();  //Port isimlerini çek.
            comboBoxCOM.Items.AddRange(ports);  //Port isimlerini ekle.
            comboBox4.Items.AddRange(ports);

            tabControl1.SelectedIndex = 0;

            button10.BackColor = Color.LightGray;
            button13.BackColor = Color.WhiteSmoke;
            button16.BackColor = Color.WhiteSmoke;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

            button13.BackColor = Color.LightGray;
            button10.BackColor = Color.WhiteSmoke;
            button16.BackColor = Color.WhiteSmoke;
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

            button16.BackColor = Color.LightGray;
            button13.BackColor = Color.WhiteSmoke;
            button10.BackColor = Color.WhiteSmoke;
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void labelStatue_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }
    }
}
