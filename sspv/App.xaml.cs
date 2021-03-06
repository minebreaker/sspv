﻿using System.IO;
using System.Windows;

namespace sspv


{
    public partial class App : Application
    {
        public string[] Files;
        // The picture file to be shown immediately after the app is up
        public string Target;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 0) { return; }
            this.Target = e.Args[0];
            // If the path is invalid, just stop doing anything
            if (!File.Exists(this.Target) && !Directory.Exists(this.Target)) { return; }

            // FIXME: should handle the case when the path is wrong
            var parentDirectory = IsDirectory(this.Target)
                ? this.Target
                : Directory.GetParent(this.Target).FullName;

            this.Files = Directory.GetFiles(parentDirectory);
        }

        private static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}
