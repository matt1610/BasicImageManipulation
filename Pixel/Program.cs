using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Pixel
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.ShowDialog();
                var bmp = new Bitmap(ofd.FileName);
                List<Color> pixels = GetAllPixels(bmp);

                //bmp.ApplyEffect(MakeThingsBlack);
                bmp.ApplyEffect(LightenUp);

                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.ShowDialog();
                    fbd.ShowNewFolderButton = true;
                    bmp.Save(fbd.SelectedPath + "\\" + ofd.SafeFileName);
                }

                Console.WriteLine("Done, pixel count is " + pixels.Count);
                Console.ReadKey();
            }
        }

        private static bool MakeThingsBlack(int x, int y, Bitmap bmp)
        {
            bmp.SetPixel(x, y, Color.Black);
            return true;
        }

        private static bool LightenUp(int x, int y, Bitmap bmp)
        {
            int amount = 100;
            Color pixel = bmp.GetPixel(x, y);
            bmp.SetPixel(x,y, Color.FromArgb(
                pixel.A,
                pixel.R < 255 - amount ? pixel.R + amount : pixel.R - (255 - amount),
                pixel.G < 255 - amount ? pixel.G + amount : pixel.G - (255 - amount),
                pixel.B < 255 - amount ? pixel.B + amount : pixel.B - (255 - amount)
                ));
            return true;
        }

        private static List<Color> GetAllPixels(Bitmap bmp)
        {
            List<Color> pixels = new List<Color>();
            int x = 0;
            int y = 0;
            for (int i = 0; i < bmp.Width * bmp.Height; i++)
            {
                pixels.Add(bmp.GetPixel(x, y));
                x++;
                if (x == bmp.Width)
                {
                    x = 0;
                    y++;
                }
            }
            return pixels;
        }

        private static void ApplyEffect(this Bitmap bmp, Func<int, int, Bitmap, bool> effect)
        {

            for (int Xcount = 0; Xcount < bmp.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < bmp.Height; Ycount++)
                {
                    effect(Xcount,Ycount,bmp);
                }
            }

        }
    }
}