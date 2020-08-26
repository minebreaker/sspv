using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace sspv


{
    public partial class App : Application
    {
        public string[] files;
        public string target;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 0) { return; }
            target = e.Args[0];
            var parentDirectory = IsDirectory(target)
                ? target
                : Directory.GetParent(target).FullName;

            files = Directory.GetFiles(parentDirectory);
        }

        private static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}
