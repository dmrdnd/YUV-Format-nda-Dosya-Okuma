using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace yazlab1._3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OpenFileDialog dialog;
        PictureBox picturebox;
        List<Image> image;

        public OpenFileDialog Dialog { get => dialog; set => dialog = value; }
        public PictureBox Picturebox { get => picturebox; set => picturebox = value; }
     //   public List<Image> mage { get => image; set => image = value; }

        private void button1_Click(object sender, EventArgs e)
        {
            string road="";
           // openFileDialog1.Filter = "Resim Dosyaları|" + "*.bmp;*.jmp;*.gif;*.wmf;*.tif;*.png;*.jpg;*.yuv;";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {                           
                road = openFileDialog1.FileName;
            }
            MessageBox.Show(road);
        
            Byte[] byt = File.ReadAllBytes(road);

            var width = 300;
            var height = 100;

            var artis = 3*width * height;
            var R = 0.0;
            var G = 0.0;
            var B =0.0;
            int Y=0, U=0, V=0;

            for (int i=0; i<byt.Length; i+=artis)
            {
                for(int j=0; j<width*height; j++)
                {                    
                    Y+= byt[j];                   
                    U += byt[j + width * height];                    
                    V += byt[j + 2 * width * height];
                }
                R = Y + 1.140 * V;
                G = Y - 0.395 * U - 0.581 * V;
                B = Y + 2.032 * U;
               // MessageBox.Show(R.ToString()+"   " +G.ToString()+ "  "+B.ToString());
            }

            for(int i=0; i<byt.Length/1000; i++)
            {
                listBox1.Items.Add(byt[i]);
            }
            /*  byte[] buff = null;
              FileStream fs = new FileStream(road,
                                             FileMode.Open,
                                             FileAccess.Read);
              BinaryReader br = new BinaryReader(fs);
              long numBytes = new FileInfo(road).Length;
              buff = br.ReadBytes((int)numBytes);
              */
            /*byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];
            Random rand = new Random();
            rand.NextBytes(r);
            rand.NextBytes(g);
            rand.NextBytes(b);
            Bitmap bmpRGB = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0,0,width,height);

            BitmapData bmpData = bmpRGB.LockBits(rect, ImageLockMode.WriteOnly, bmpRGB.PixelFormat);

            int padding= bmpData.Stride - 3 * width;

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                for (int y = 0; y < height; y++) 
                {
                    for(int x=0; x<width; x++)
                    {
                        ptr[2] = r[y * width + x];
                        ptr[1] = g[y * width + x];
                        ptr[0] = b[y * width + x];
                        ptr += 3;
                    }
                    ptr += padding;
                }
            }
            bmpRGB.UnlockBits(bmpData);
          //  bmpRGB.Save("rgb_test.bmp", ImageFormat.Bmp);

            pictureBox1.Image= bmpRGB;
            MessageBox.Show(byt.Length.ToString());
          //var image = new Bitmap(10, 10);*/



            byte[] getNV21(int inputWidth, int inputHeight, Bitmap scaled)
            {

                int[] argb = new int[inputWidth * inputHeight];
                scaled.getPixels(argb, 0, inputWidth, 0, 0, inputWidth, inputHeight);



                byte[] yuv = new byte[inputWidth * inputHeight * 3 / 2];
                encodeYUV420SP(yuv, argb, inputWidth, inputHeight);

                scaled.recycle();
            

                return yuv;
            }

            void encodeYUV420SP(byte[] yuv420sp, int[] argb, int width, int height)
            {
                final int frameSize = width * height;

                int yIndex = 0;
                int uvIndex = frameSize;

                int a, R, G, B, Y, U, V;
                int index = 0;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {

                        a = (argb[index] & 0xff000000) >> 24; // a is not used obviously
                        R = (argb[index] & 0xff0000) >> 16;
                        G = (argb[index] & 0xff00) >> 8;
                        B = (argb[index] & 0xff) >> 0;

                        // well known RGB to YUV algorithm
                        Y = ((66 * R + 129 * G + 25 * B + 128) >> 8) + 16;
                        U = ((-38 * R - 74 * G + 112 * B + 128) >> 8) + 128;
                        V = ((112 * R - 94 * G - 18 * B + 128) >> 8) + 128;

                        // NV21 has a plane of Y and interleaved planes of VU each sampled by a factor of 2
                        //    meaning for every 4 Y pixels there are 1 V and 1 U.  Note the sampling is every other
                        //    pixel AND every other scanline.
                        yuv420sp[yIndex++] = (byte)((Y < 0) ? 0 : ((Y > 255) ? 255 : Y));
                        if (j % 2 == 0 && index % 2 == 0)
                        {
                            yuv420sp[uvIndex++] = (byte)((V < 0) ? 0 : ((V > 255) ? 255 : V));
                            yuv420sp[uvIndex++] = (byte)((U < 0) ? 0 : ((U > 255) ? 255 : U));
                        }

                        index++;
                    }
                }
            }

        }
    }
}
