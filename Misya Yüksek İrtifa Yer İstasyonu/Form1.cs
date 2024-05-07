using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Drawing;
using System.Net;
using System.Diagnostics;



namespace Misya_Yüksek_İrtifa_Yer_İstasyonu
{

    public partial class Form1 : Form
    {
        //Seri port aktarım değişkenleri
        private SerialPort serialPort;
        private int baudRate = 9600;

        //Video aktarım değişkenleri
        private Process ffmpegProcess;
        private Thread ffmpegThread;
        public Form1()
        {
            InitializeComponent();




            //-------------------------------------------------------------------------------------------------
            string[] ports = SerialPort.GetPortNames();  //Port isimlerini çek.
            comboBoxCOM.Items.AddRange(ports);  //Port isimlerini ekle.

            if (ports.Length == 0)
            {
                MessageBox.Show("COM portu bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); //Eğer port bulamazsan hata attır.
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxCOM.SelectedItem != null)
            {
                // Seçilen COM port adını alın
                string selectedPort = comboBoxCOM.SelectedItem.ToString();

                // SerialPort oluşturun ve bağlantıyı açın
                serialPort = new SerialPort(selectedPort, baudRate); // COM port adı ve baud rate (9600) girilir
                try
                {
                    serialPort.Open();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.ToString());
                }


                // Veri alımı için bir event handler ekle
                serialPort.DataReceived += SerialPort_DataReceived;

                // Bağlantı durumunu güncelleyin
                labelStatue.Text = "Bağlı";
            }
            else
            {
                MessageBox.Show("Lütfen bir COM port seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Veri alındığında burası çalışır
            // Gelen veriyi okuyun
            string receivedData = serialPort.ReadLine();

            // Veriyi işleyin, ekrana yazdırabilir veya başka bir işlem yapabilirsiniz
            // Örneğin, bu örnekte gelen veriyi direkt olarak ekrana yazdırıyoruz
            MessageBox.Show(receivedData);

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
    }
}
