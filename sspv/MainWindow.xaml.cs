using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sspv


{
    public partial class MainWindow : Window
    {

        // FIXME: This is dumb.
        private string[] pictureExts = { "bmp", "jpg", "jpeg", "png", "gif" };

        private string[] files;
        private string target;
        private int index;

        public MainWindow()
        {
            InitializeComponent();

            var app = (sspv.App)Application.Current;

            files = app.files
                ?.Where(f => pictureExts.Any(ext => f.EndsWith(ext)))
                ?.ToArray();
            target = app.target;
            if (target == null || files == null || files.Length == 0) { return; }

            index = Array.FindIndex(files, f => f.Equals(target));
            index = index == -1 ? 0 : index;
            load();
        }

        private void load()
        {
            this.image.Source = new BitmapImage(new Uri(files[index]));
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.PageDown))
            {
                index = index + 1 == files.Length ? 0 : index + 1;
                load();
            }
            else if (e.Key.Equals(Key.Left) || e.Key.Equals(Key.PageUp)) {
                index = index == 0 ? files.Length - 1 : index - 1;
                load();
            }
        }
    }
}
