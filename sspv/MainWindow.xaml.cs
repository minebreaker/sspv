using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace sspv


{
    public partial class MainWindow : Window
    {

        // FIXME: This is dumb.
        private readonly string[] PictureExts = { "bmp", "jpg", "jpeg", "png", "gif" };

        private string[] Files;
        private string Target;
        private int Index;

        public MainWindow()
        {
            InitializeComponent();

            var app = (sspv.App)Application.Current;

            this.Files = app.Files
                ?.Where(f => PictureExts.Any(ext => f.ToLower().EndsWith(ext)))
                ?.ToArray();
            this.Target = app.Target;
            if (Target == null || Files == null || Files.Length == 0) { return; }

            this.Index = Array.FindIndex(Files, f => f.Equals(Target));
            this.Index = Index == -1 ? 0 : Index;
            Load();
        }

        private void Load()
        {
            this.image.Source = new BitmapImage(new Uri(Files[Index]));
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.PageDown))
            {
                this.Index = Index + 1 == Files.Length ? 0 : Index + 1;
                Load();
            }
            else if (e.Key.Equals(Key.Left) || e.Key.Equals(Key.PageUp)) {
                this.Index = Index == 0 ? Files.Length - 1 : Index - 1;
                Load();
            }
        }
    }
}
