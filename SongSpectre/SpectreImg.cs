using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using StreamRef = Windows.Storage.Streams.IRandomAccessStreamReference;
using StreamWContent = Windows.Storage.Streams.IRandomAccessStreamWithContentType;

namespace SongSpectre {
    internal static class SpectreImg {
        public static async Task<Bitmap?> RefToThumb(StreamRef Ref) {
            Bitmap Img = Resources.ErrorThumb;
            if (Ref != null) {
                try {
                    using (StreamWContent stream = await Ref.OpenReadAsync()) {
                        using (MemoryStream mStream = new()) {
                            await stream.AsStreamForRead().CopyToAsync(mStream);
                            Img = (Bitmap)Image.FromStream(mStream);
                        }
                    }
                } catch (Exception ex) {
                    WriteLine(ex.ToString());
                }
            }
            if (Img.Width != 300 || Img.Height != 300) {
                Img = ResizeToThumb(Img);
            }
            return Img;
        }
        public static Bitmap ResizeToThumb(Bitmap thumb) {
            Bitmap tmpImg = new(300, 300, PixelFormat.Format32bppArgb);
            float scale = Math.Min(300f / thumb.Width, 300f / thumb.Height);
            int newW = (int)(thumb.Width * scale);
            int newH = (int)(thumb.Height * scale);

            using (Graphics g = Graphics.FromImage(tmpImg)) {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);
                g.DrawImage(thumb, ((300 - newW) / 2), ((300 - newH) / 2), newW, newH);
            }
            return tmpImg;
        }
        public static void DebugViewImage(Image img, string title) {
            using (MemoryStream ms = new()) {
                if (img != null) {
                    img.Save(ms, ImageFormat.Png);
                } else {
                    WriteLine(title + " NULL THUMB");
                    Beep();
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
                string htmlFileName = Path.Combine(tempDir, "Spectre-" + title + "-thumb.html");
                File.WriteAllText(htmlFileName, html);
                Process.Start(new ProcessStartInfo(htmlFileName) { UseShellExecute = true });
            }
        }
    }
}
