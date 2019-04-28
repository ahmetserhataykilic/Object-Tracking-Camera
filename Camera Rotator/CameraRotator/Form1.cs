using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.IO.Ports;
namespace CameraRotator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            comboBox2.DataSource = SerialPort.GetPortNames();        
        }
        int sayac;
        int R, G, B, yatayX, dikeyY;
        Graphics g;
        int mode;
        Bitmap video;
        private FilterInfoCollection CaptureDevice; //capture device isminde tanımladığımız değişken bilgisayara kaç kamera bağlıysa onları tutan bir dizi. 
        private VideoCaptureDevice CıkısVideo;      //cıkısvideo ise bizim kullanacağımız aygıt.
        Mirror filter = new Mirror(false, true);


        // -------------------------- Form1 Açıldığında --------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);// capture device dizisine mevcut kameraları dolduruyoruz.
            foreach (FilterInfo Device in CaptureDevice)
            {
                comboBox1.Items.Add(Device.Name);// kameraları combobox a dolduruyoruz.

            }
            comboBox1.SelectedIndex = 0;
            CıkısVideo = new VideoCaptureDevice();
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button3.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
        }


        //-------------------------- Kamera Aç Butonu ----------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            CıkısVideo = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            CıkısVideo.DesiredFrameRate = 100;//görüntü kalitesi için
            CıkısVideo.NewFrame += CıkısVideo_NewFrame;
            CıkısVideo.Start();
            button1.Enabled = false;
            button3.Enabled = true;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
        }

        //--------------------------Step Motor  Çalıştır Butonu ------------------
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
            {
                mode = 1;
            }
            button2.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
        }
        // ------------------------ Step Motor Kapat Butonu ----------------------
        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
            {
                mode = 0;
            }
            button5.Enabled = false;
            button2.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
            button8.Enabled = true;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
        }
        //----------------------------- Led Aç -----------------------------------
        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
            {
                mode = 3;
            }
            button6.Enabled = false;
            button7.Enabled = true;
            button5.Enabled = false;
            button2.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
        }
        //----------------------------- Led Kapat -----------------------------------
        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
            {
                mode = 0;
            }
            button7.Enabled = false;
            button6.Enabled = true;
            button2.Enabled = true;
            button5.Enabled = false;
            button8.Enabled = true;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;

        }

        void CıkısVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            video = (Bitmap)eventArgs.Frame.Clone();  //aldığımız görüntüyü pictureBox1 a atarak görüntüyü alıyoruz. 
            Bitmap video2 = (Bitmap)eventArgs.Frame.Clone();
            
            filter.ApplyInPlace(video2);

            switch (mode)
            {
                case 2:
                    {

                        g = Graphics.FromImage(video2);//Değişiklik için grafik nesnesi oluşturduk
                        g.DrawString(sayac.ToString(), new Font("Arial", 100), new SolidBrush(Color.Black), new PointF(2, 2));
                        g.Dispose();

                    }
                    break;

                case 1:
                    {
                        EuclideanColorFiltering filter = new EuclideanColorFiltering();

                        filter.CenterColor = new RGB(215, 30, 30);
                        filter.Radius = 100;
                        filter.ApplyInPlace(video2);

                        //blob filtre
                        BlobCounter blob = new BlobCounter();
                        blob.MinHeight = 200;
                        blob.MinWidth = 200;
                        blob.ObjectsOrder = ObjectsOrder.Size;
                        blob.ProcessImage(video2);
                        Rectangle[] rects = blob.GetObjectsRectangles();
                        if (rects.Length > 0)
                        {
                            Rectangle obje = rects[0];
                            Graphics g = Graphics.FromImage(video2);
                            using (Pen pen = new Pen(Color.White, 3))
                            {
                                g.DrawRectangle(pen, obje);
                            }
                            yatayX = obje.X;
                            dikeyY = obje.Y;
     
                            g = Graphics.FromImage(video2);
                            // 1
                            if (yatayX >= 0 && 200 > yatayX && dikeyY >= 0 && dikeyY < 100)
                            {      
                                serialPort1.Write("A");
                            }
                            // 2
                            else if (yatayX >= 200 && yatayX < 400 && dikeyY >= 0 && dikeyY < 100)
                            {    
                                serialPort1.Write("B");
                            }
                            // 3
                            else if (yatayX >= 400 && yatayX < 600 && dikeyY >= 0 && dikeyY < 100)
                            {
                                serialPort1.Write("C");
                            }
                            // 4
                            else if (yatayX >= 0 && yatayX < 200 && dikeyY >= 100 && 200 >= dikeyY)
                            {
                                serialPort1.Write("D");      
                            }
                            // 5
                            else if (yatayX >= 200 && yatayX < 400 && dikeyY >= 100 && 200 >= dikeyY)
                            {
                                serialPort1.Write("E");
                            }
                            // 6
                            else if (yatayX >= 400 && 600 >= yatayX && dikeyY >= 100 && 200 >= dikeyY)
                            {
                                serialPort1.Write("F");
                            }
                            // 7
                            else if (yatayX >= 0 && 200 >= yatayX && dikeyY >= 200 && 300 >= dikeyY)
                            {
                                serialPort1.Write("G");
                            }
                            // 8
                            else if (yatayX >= 200 && 400 >= yatayX && dikeyY >= 200 && 300 >= dikeyY)
                            {
                                serialPort1.Write("H");
                            }
                            // 9
                            else if (yatayX >= 400 && 600 >= yatayX && dikeyY >= 200 && 300 >= dikeyY)
                            {
                                serialPort1.Write("K");
                            }

                            g.Dispose();
                        }
                        // GO HOME
                        else { serialPort1.Write("X"); }
                        
                        pictureBox2.Image = video2;

                    }
                    break;
                    
                case 3:
                    {
                        EuclideanColorFiltering filter = new EuclideanColorFiltering();

                        filter.CenterColor = new RGB(215, 30, 30);
                        filter.Radius = 100;
                        filter.ApplyInPlace(video2);

                        //blob filtre
                        BlobCounter blob = new BlobCounter();
                        blob.MinHeight = 200;
                        blob.MinWidth = 200;
                        blob.ObjectsOrder = ObjectsOrder.Size;
                        blob.ProcessImage(video2);
                        Rectangle[] rects = blob.GetObjectsRectangles();
                        if (rects.Length > 0)
                        {
                            Rectangle obje = rects[0];
                            Graphics g = Graphics.FromImage(video2);
                            using (Pen pen = new Pen(Color.White, 3))
                            {
                                g.DrawRectangle(pen, obje);
                            }
                            yatayX = obje.X;
                            dikeyY = obje.Y;
                           
                            g = Graphics.FromImage(video2);
                            if (yatayX >= 0 && 200 > yatayX && dikeyY >= 0 && dikeyY < 100)
                            {                              
                                serialPort1.Write("M");
                            }
                            else if (yatayX >= 200 && yatayX < 400 && dikeyY >= 0 && dikeyY < 100)
                            {
                                serialPort1.Write("N");
                            }

                            else if (yatayX >= 400 && yatayX < 600 && dikeyY >= 0 && dikeyY < 100)
                            {
                                serialPort1.Write("S");
                            }

                            else if (yatayX >= 0 && yatayX < 200 && dikeyY >= 100 && 200 >= dikeyY)
                            {
                                serialPort1.Write("L");
                            }
                            else if (yatayX >= 200 && yatayX < 400 && dikeyY >= 100 && 200 >= dikeyY)
                            {
                                serialPort1.Write("R");
                            }
                            else if (yatayX >= 400 && 600 >= yatayX && dikeyY >= 100 && 200 >= dikeyY)
                            {
                                serialPort1.Write("P");

                            }
                            else if (yatayX >= 0 && 200 >= yatayX && dikeyY >= 200 && 300 >= dikeyY)
                            {
                                serialPort1.Write("T");

                            }
                            else if (yatayX >= 200 && 400 >= yatayX && dikeyY >= 200 && 300 >= dikeyY)
                            {
                                serialPort1.Write("Y");
                            }
                            else if (yatayX >= 400 && 600 >= yatayX && dikeyY >= 200 && 300 >= dikeyY)
                            {
                                serialPort1.Write("Z");
                            }

                            g.Dispose();
                        }
                        pictureBox2.Image = video2;
                    }
                    break;
            }

            pictureBox1.Image = video;
            
        }

        // -------------------------- Form Kapanırken --------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CıkısVideo.IsRunning == true)
            {
                CıkısVideo.Stop();
            }
        }


        // -------------------------- İşlemciyi Aç --------------------------
        private void button3_Click(object sender, EventArgs e)
        {

            serialPort1.PortName = "COM3"; 
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            if (serialPort1.IsOpen == true)
            {
      
                button4.Enabled = true;
                button3.Enabled = false;
                button2.Enabled = true;
                button6.Enabled = true;
                button8.Enabled = true;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;
                button13.Enabled = false;
            }
            else
            {
                /////

            }
        }

 
        // -------------------------- İşlemciyi Kapat --------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                button3.Enabled = true;
                button4.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button5.Enabled = false;
                button2.Enabled = false;
                button9.Enabled = false;
                button8.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;
                button13.Enabled = false;
            }
        }

        // ---------------- MANUEL STEP KONTROL AÇ ----------------------------
        private void button8_Click(object sender, EventArgs e)
        {


  

            button9.Enabled = true;
            button8.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button2.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button13.Enabled = true;
        }
        //  STEP YUKARI
        private void button10_Click(object sender, EventArgs e)
        {
            serialPort1.Write("U");
        }
        // STEP AŞAĞI
        private void button11_Click(object sender, EventArgs e)
        {
            serialPort1.Write("O");
        }
        // STEP SOLA
        private void button12_Click(object sender, EventArgs e)
        {
            serialPort1.Write("J");
        }
        // STEP SAĞA
        private void button13_Click(object sender, EventArgs e)
        {
            serialPort1.Write("W");
        }


        // ---------------- MANUEL STEP KONTROL KAPAT ----------------------------
        private void button9_Click(object sender, EventArgs e)
        {
 
            button9.Enabled = false;
            button8.Enabled = true;
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button2.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
        }

        // -------------------------- Renk Ayarlamaları --------------------------


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
            label1.Text = "R: " + trackBar1.Value;

        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            G = trackBar2.Value;
            label2.Text = "G: " + trackBar2.Value;
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {

            B = trackBar3.Value;
            label3.Text = "B: " + trackBar3.Value;
        }


    }
}
