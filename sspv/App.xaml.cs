using System.IO;
using System.Windows;

namespace sspv


{
    public partial class App : Application
    {
        public string[] Files;
        public string Target;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 0) { return; }
            this.Target = e.Args[0];
            var parentDirectory = IsDirectory(Target)
                ? Target
                : Directory.GetParent(Target).FullName;

            this.Files = Directory.GetFiles(parentDirectory);
        }

        private static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}
