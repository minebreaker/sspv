using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace sspv


{
    public partial class MainWindow : Window
    {

        // FIXME: This is dumb.
        private readonly string[] PictureExts = { "bmp", "jpg", "jpeg", "png", "gif" };

        private string[] Files;
        private int Index;

        public MainWindow()
        {
            InitializeComponent();

            var app = (sspv.App)Application.Current;

            this.Files = app.Files
                ?.Where(f => this.PictureExts.Any(ext => f.ToLower().EndsWith(ext)))
                ?.ToArray();
            var target = app.Target;
            if (target == null || this.Files == null || this.Files.Length == 0) { return; }

            this.Index = Array.FindIndex(this.Files, f => f.Equals(target));
            this.Index = this.Index == -1 ? 0 : this.Index;
            this.Load();
        }

        private void Load()
        {
            var fileName = this.Files[this.Index];

            // Show the file name on the title bar
            this.Title = fileName;

            // Read image. Fix rotation if necessary
            var image = new BitmapImage(new Uri(fileName));
            ImageSource rotatedImage = image;
            if (fileName.ToLower().EndsWith("jpg") || fileName.ToLower().EndsWith("jpeg"))
            {
                var frame = BitmapFrame.Create(new Uri(fileName));
                var meta = ((BitmapMetadata)frame.Metadata).GetQuery(@"/app1/ifd/exif:{uint=274}");
                var rotation = Convert.ToUInt32(meta);
                var transform =
                    rotation == 3 ? new RotateTransform(180) :
                    rotation == 6 ? new RotateTransform(90) :
                    rotation == 8 ? new RotateTransform(270) as Transform :
                    rotation == 2 ? new ScaleTransform(-1, 1, 0, 0) :
                    rotation == 4 ? new ScaleTransform(1, -1, 0, 0) :
                    // FIXME: Should find the good way to write this
                    // rotation == 5 ? new TransformGroup( new RotateTransform(90), new ScaleTransform(-1, 1, 0, 0)) :
                    // rotation == 7  new RotateTransform(270) new ScaleTransform(-1, 1, 0, 0)
                    null;
                var transformation = Utils.Consume(new TransformedBitmap(), t =>
                {
                    t.BeginInit();
                    t.Source = image;
                    if (transform != null)
                    {
                        t.Transform = transform;
                    }
                    t.EndInit();
                });
                rotatedImage = transformation;
            }

            this.image.Source = rotatedImage;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.PageDown))
            {
                this.Index = Index + 1 == this.Files.Length ? 0 : Index + 1;
                this.Load();
            }
            else if (e.Key.Equals(Key.Left) || e.Key.Equals(Key.PageUp))
            {
                this.Index = Index == 0 ? this.Files.Length - 1 : this.Index - 1;
                this.Load();
            }
        }
    }
}
