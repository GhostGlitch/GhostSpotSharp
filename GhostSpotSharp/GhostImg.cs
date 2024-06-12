using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GhostSpotSharp {
    internal class GhostImg {
        public static async Task<Bitmap?> RefToThumb(IRandomAccessStreamReference streamRef) {
            Bitmap Img = Resources.ErrorThumb;
            if (streamRef != null) {
                try {
                    using (IRandomAccessStreamWithContentType stream = await streamRef.OpenReadAsync()) {
                        using (MemoryStream mStream = new()) {
                            await stream.AsStreamForRead().CopyToAsync(mStream);
                            Img = (Bitmap)Image.FromStream(mStream);
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
            if (Img.Width != 300 || Img.Height != 300) {
                Img = ResizeToThumb(Img);
            }
            return Img;
        }
        public static Bitmap ResizeToThumb(Bitmap thumb) {
            Bitmap tmpImg = new(300, 300, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            float scale = Math.Min(300f / thumb.Width, 300f / thumb.Height);
            int newW = (int)(thumb.Width * scale);
            int newH = (int)(thumb.Height * scale);

            using (Graphics g = Graphics.FromImage(tmpImg)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);
                g.DrawImage(thumb, ((300 - newW) / 2), ((300 - newH) / 2), newW, newH);
            }
            return tmpImg;
        }
        public static void DebugViewImage(Image img, string title) {
            using (MemoryStream ms = new()) {
                if (img != null) {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                } else {
                    Console.WriteLine(title + " NULL THUMB");
                    Console.Beep();
                    return;
                }

                byte[] imageBytes = ms.ToArray();

                // Convert the byte array to a Base64 string
                string base64String = Convert.ToBase64String(imageBytes);
                // Generate HTML code to display the image
                string html = $"<img src='data:image/png;base64,{base64String}' />";
                string tempDir = Path.Combine(Path.GetTempPath(), "GhostTmp");
                if (!Directory.Exists(tempDir)) {
                    Directory.CreateDirectory(tempDir);
                }
                // Save to tmp and open in browser
                string htmlFileName = Path.Combine(tempDir, "Ghost-" + title + "-thumb.html");
                File.WriteAllText(htmlFileName, html);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(htmlFileName) { UseShellExecute = true });
            }
        }
    }
}
