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
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using WebSocketSharp;
using WebSocketSharp.Server;

//223; 30 UNUTMAAA!

namespace Misya_Yüksek_İrtifa_Yer_İstasyonu
{

    public partial class Form1 : Form
    {
        int zoomMiktar = 18;
        //Seri port aktarım değişkenleri
        private SerialPort roketPort;
        private SerialPort gorevPort;
        private SerialPort hakemPort;

        private bool roketPortDurum;
        private bool gorevPortDurum;
        private bool hakemPortDurum;

        float deneme = 10;

        string receivedData = "";



        //Video aktarım değişkenleri
        private Process ffmpegProcess;
        private Thread ffmpegThread;


        //HYİ'ye yollanacak veri paketi
        byte[] olusturalacak_paket = new byte[78];

        GMap.NET.WindowsForms.GMapControl gmap;
        GMap.NET.WindowsForms.GMapControl gmap2;
        private GMapOverlay routesOverlay = new GMapOverlay("routes");
        private List<PointLatLng> points = new List<PointLatLng>();

        //Modelleme
        private WebSocketServer _wss;

        //veriler
        float
            RoketIrtifa = 10.2F,
            RoketGPSIrtifa = 10.2F,
            RoketEnlem = 10.2F,
            RoketBoylam = 10.2F,
            GorevYukuGPSIrtifa = 10.2F,
            GorevYukuEnlem = 10.2F,
            GorevYukuBoylam = 10.2F,
            JiroskopX = 10.2F,
            JiroskopY = 10.2F,
            JiroskopZ = 10.2F,
            IvmeX = 10.2F,
            IvmeY = 10.2F,
            IvmeZ = 10.2F,
            Aci = 10.2F;

        byte durum = 1;

        private void StartWebSocketServer()
        {
            _wss = new WebSocketServer("ws://127.0.0.1:8080");
            _wss.AddWebSocketService<ChatBehavior>("/chat");
            _wss.Start();
            Console.WriteLine("WebSocket Server started at ws://127.0.0.1:8080");
        }
        public class ChatBehavior : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine($"Received: {e.Data}");
                Send($"Echo: {e.Data}");
            }
        }





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
            StartWebSocketServer();

            richTextBox1.ReadOnly = true;
            richTextBox3.ReadOnly = true;
            //-------------------------------------------------------------------------------------------------
            gmap = gMapControl2;
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            //gmap.Dock = DockStyle.Fill;
            gmap.CanDragMap = false;
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.ShowCenter = false;
            gmap.MinZoom = zoomMiktar;
            gmap.MaxZoom = zoomMiktar;




            //gmap2 = gMapControl4;
            //gmap2.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            ////gmap.Dock = DockStyle.Fill;
            //gmap2.CanDragMap = false;
            //gmap2.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            //gmap2.ShowCenter = false;
            //gmap2.MinZoom = 5;
            //gmap2.MaxZoom = 5;





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
                        timer2.Start();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Serial Port açılırken hata oluştu ! Hata: \n " + error.ToString());
                    }


                    // Veri alımı için bir event handler ekle. Her data geldiğinde tetiklenecek
                    //roketPort.DataReceived += RoketSerial_Handler;

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
            //receivedData = roketPort.ReadLine();

            // Veriyi işleyin, ekrana yazdırabilir veya başka bir işlem yapabilirsiniz
            // Örneğin, bu örnekte gelen veriyi direkt olarak ekrana yazdırıyoruz
          //this.Invoke(new MethodInvoker(delegate ()
          //{
          //    richTextBox1.AppendText(receivedData); //UI threading
          //    richTextBox1.ScrollToCaret();
          //}));

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
            button18.BackColor = Color.WhiteSmoke;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

            button13.BackColor = Color.LightGray;
            button10.BackColor = Color.WhiteSmoke;
            button16.BackColor = Color.WhiteSmoke;
            button18.BackColor = Color.WhiteSmoke;
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

            button16.BackColor = Color.LightGray;
            button13.BackColor = Color.WhiteSmoke;
            button10.BackColor = Color.WhiteSmoke;
            button18.BackColor = Color.WhiteSmoke;
        }
        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;

            button18.BackColor = Color.LightGray;
            button16.BackColor = Color.WhiteSmoke;
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            PointLatLng startPoint = new PointLatLng(40.7128, -74.0060); // New York
            points.Add(startPoint);

            // Başlangıç noktasına marker ekleme
            GMapMarker startMarker = new GMarkerGoogle(startPoint, GMarkerGoogleType.green_dot);
            routesOverlay.Markers.Add(startMarker);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            double lat = 40.0 + (random.NextDouble() - 0.5) * 10.0; // 35.0 ile 45.0 arasında bir enlem
            double lng = -75.0 + (random.NextDouble() - 0.5) * 10.0; // -80.0 ile -70.0 arasında bir boylam
            PointLatLng newPoint = new PointLatLng(lat, lng);
            points.Add(newPoint);

            // Yeni noktaya marker ekleme
            GMapMarker newMarker = new GMarkerGoogle(newPoint, GMarkerGoogleType.red_dot);
            routesOverlay.Markers.Add(newMarker);

            // Güzergah oluşturma
            GMapRoute route = new GMapRoute(points, "My Route")
            {
                Stroke = new Pen(Color.Red, 3) // Güzergah çizgisinin rengi ve kalınlığı
            };

            // Mevcut güzergahları kaldırma ve yeni güzergahı ekleme
            routesOverlay.Routes.Clear();
            routesOverlay.Routes.Add(route);
            gMapControl1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {



        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate("http://localhost:8000");
        }

        private void button25_Click(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            _wss.WebSocketServices["/chat"].Sessions.Broadcast(textBox10.Text + "*" + textBox11.Text + "*" + textBox13.Text);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            UpdateDatas();

        }

        private void UpdateDatas()
        {
           
            try
            {
                receivedData = roketPort.ReadLine();
                string[] datas = receivedData.Split("/");

                string RoketEnlemString = datas[2].Replace(".", ",");
                string RoketBoylamString = datas[3].Replace(".", ",");
                RoketIrtifa = float.Parse(datas[0]);
                RoketGPSIrtifa = float.Parse(datas[1]);
                RoketEnlem = float.Parse(datas[2]);
                RoketBoylam = float.Parse(datas[3]);
                GorevYukuGPSIrtifa = float.Parse(datas[4]);
                GorevYukuEnlem = float.Parse(datas[5]);
                GorevYukuBoylam = float.Parse(datas[6]);
                JiroskopX = float.Parse(datas[7]);
                JiroskopY = float.Parse(datas[8]);
                JiroskopZ = float.Parse(datas[9]);
                IvmeX = float.Parse(datas[10]);
                IvmeY = float.Parse(datas[11]);
                IvmeZ = float.Parse(datas[12]);
                Aci = float.Parse(datas[13]);
                durum = byte.Parse(datas[14]);

                this.Invoke((MethodInvoker)delegate
                {
                    gmap.Position = new PointLatLng(Convert.ToDouble(RoketEnlemString), Convert.ToDouble(RoketBoylamString));
                    gmap.Zoom = zoomMiktar;
                    gmap.Update();
                    gmap.Refresh();
                });

                //gmap2.Position = new PointLatLng(Convert.ToDouble(GorevYukuEnlem), Convert.ToDouble(GorevYukuBoylam));
                //gmap2.Zoom = 1;
                //gmap2.Update();
                //gmap2.Refresh();

                if (label45.InvokeRequired)
                {
                    label45.Invoke((MethodInvoker)delegate
                    {
                        label45.Text = datas[0];
                    });
                }
                else
                {
                    label45.Text = datas[0];
                }

                if (label46.InvokeRequired)
                {
                    label46.Invoke((MethodInvoker)delegate
                    {
                        label46.Text = datas[1];
                    });
                }
                else
                {
                    label46.Text = datas[1];
                }

                if (label47.InvokeRequired)
                {
                    label47.Invoke((MethodInvoker)delegate
                    {
                        label47.Text = datas[2];
                    });
                }
                else
                {
                    label47.Text = datas[2];
                }

                if (label48.InvokeRequired)
                {
                    label48.Invoke((MethodInvoker)delegate
                    {
                        label48.Text = datas[3];
                    });
                }
                else
                {
                    label48.Text = datas[3];
                }

                if (label49.InvokeRequired)
                {
                    label49.Invoke((MethodInvoker)delegate
                    {
                        label49.Text = datas[7];
                    });
                }
                else
                {
                    label49.Text = datas[7];
                }

                if (label50.InvokeRequired)
                {
                    label50.Invoke((MethodInvoker)delegate
                    {
                        label50.Text = datas[8];
                    });
                }
                else
                {
                    label50.Text = datas[8];
                }

                if (label51.InvokeRequired)
                {
                    label51.Invoke((MethodInvoker)delegate
                    {
                        label51.Text = datas[9];
                    });
                }
                else
                {
                    label51.Text = datas[9];
                }

                if (label52.InvokeRequired)
                {
                    label52.Invoke((MethodInvoker)delegate
                    {
                        label52.Text = datas[10];
                    });
                }
                else
                {
                    label52.Text = datas[10];
                }

                if (label53.InvokeRequired)
                {
                    label53.Invoke((MethodInvoker)delegate
                    {
                        label53.Text = datas[11];
                    });
                }
                else
                {
                    label53.Text = datas[11];
                }

                if (label54.InvokeRequired)
                {
                    label54.Invoke((MethodInvoker)delegate
                    {
                        label54.Text = datas[12];
                    });
                }
                else
                {
                    label54.Text = datas[12];
                }

                if (label55.InvokeRequired)
                {
                    label55.Invoke((MethodInvoker)delegate
                    {
                        label55.Text = datas[13];
                    });
                }
                else
                {
                    label55.Text = datas[13];
                }

                if (label56.InvokeRequired)
                {
                    label56.Invoke((MethodInvoker)delegate
                    {
                        label56.Text = datas[14];
                    });
                }
                else
                {
                    label56.Text = datas[14];
                }

                if (label57.InvokeRequired)
                {
                    label57.Invoke((MethodInvoker)delegate
                    {
                        label57.Text = datas[4];
                    });
                }
                else
                {
                    label57.Text = datas[4];
                }

                if (label58.InvokeRequired)
                {
                    label58.Invoke((MethodInvoker)delegate
                    {
                        label58.Text = datas[5];
                    });
                }
                else
                {
                    label58.Text = datas[5];
                }

                if (label59.InvokeRequired)
                {
                    label59.Invoke((MethodInvoker)delegate
                    {
                        label59.Text = datas[6];
                    });
                }
                else
                {
                    label59.Text = datas[6];
                }

                //if (label60.InvokeRequired)
                //{
                //    label60.Invoke((MethodInvoker)delegate {
                //        label60.Text = datas[18];
                //    });
                //}
                //else
                //{
                //    label60.Text = datas[18];
                //}



            }
            catch (Exception err)
            {
               MessageBox.Show(err.ToString());



            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            UpdateDatas();   
        }
    }
}
