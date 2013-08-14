using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoboothCombiner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**************************************************");

            if (args.Length != 2)
            {
                Console.WriteLine("You must provide source and target path");
            }
            else
            {
                HandleFiles(args[0], args[1]);
            }

            Console.WriteLine("**************************************************");
        }

        private static void HandleFiles(string p, string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!dir.EndsWith(@"\"))
            {
                dir = dir + @"\";
            }

            var files = Directory.GetFiles(p, "*.jpg");

            Parallel.ForEach(files, file => DoubleFileForPrint(file, dir));
        }

        private static void DoubleFileForPrint(string file, string targetDir)
        {
            try
            {
                using (var image = Image.FromFile(file))
                {
                    var size = image.Size;
                    size.Width = size.Width * 2;

                    using (var bitmap = new Bitmap(image, size))
                    {
                        using (var graphic = Graphics.FromImage(bitmap))
                        {
                            graphic.DrawImage(image, 0, 0, image.Width, image.Height);
                            graphic.DrawImage(image, image.Width, 0, image.Width, image.Height);
                        }

                        var lastSlash = file.LastIndexOf('\\') + 1;
                        var filename = file.Substring(lastSlash, file.Length - lastSlash);

                        var fileName = targetDir + filename;

                        bitmap.Save(fileName);
                    }
                }

                Console.WriteLine("Processed " + file);
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
