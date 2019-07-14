using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuv
{
    class Processes
    {
       public static List<Bitmap> image = new List<Bitmap>();

        public List<Bitmap> ReadYUVFile(double value,string road, int width, int height)
        {
            int foto_size = width * height;
            int frame_size = Convert.ToInt32(foto_size * value);

            byte[] yuv = new byte[frame_size];
            byte[] rgb = new byte[3 * foto_size];

            using (FileStream fileStream = File.OpenRead(road))
            {

                Console.WriteLine((int)fileStream.Length / frame_size);
                using (BinaryReader reader = new BinaryReader(fileStream, Encoding.ASCII))
                {
                    int j = 0;                
                    while (reader.PeekChar() != -1)
                    {
                        reader.Read(yuv, 0, frame_size);                    
                        Bitmap bitmap = BitmaptoYUV(yuv, rgb, width, height);
                        image.Add(bitmap);                      
                        j++;
                    }
                }
            }
            return image;
        }

        private Bitmap BitmaptoYUV(byte[] frameyuv, byte[] framergb, int width, int height)
        {
            //   int uInx = width * height;
            //  int vInx = uInx + ((width * height) >> 2);
            int gIndx = width * height;
            int bIndx = gIndx * 2;

            int frametemp = 0;
            Bitmap bitmap = new Bitmap(width, height);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    frametemp = (int)(frameyuv[i * width + j]);
                    framergb[i * width + j] = (byte)(frametemp < 0 ? 0 : (frametemp > 255 ? 255 : frametemp));

                    frametemp = (int)(frameyuv[i * width + j]);
                    framergb[gIndx + i * width + j] = (byte)(frametemp < 0 ? 0 : (frametemp > 255 ? 255 : frametemp));

                    frametemp = (int)(frameyuv[i * width + j]);
                    framergb[bIndx + i * width + j] = (byte)(frametemp < 0 ? 0 : (frametemp > 255 ? 255 : frametemp));

                    Color c = Color.FromArgb(framergb[i * width + j], framergb[gIndx + i * width + j], framergb[bIndx + i * width + j]);
                    bitmap.SetPixel(j, i, c);
                }
            }
            return bitmap;
        }

    }
}
