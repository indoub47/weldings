using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    internal static class DbBackupCreator
    {
        private static string targetFileName()
        {
            string source = Properties.Settings.Default.AccessDbPath;
            string extension = Path.GetExtension(source);
            string fileName = string.Format(Properties.Settings.Default.DBBackupFilenameFormat, DateTime.Now) + extension;
            return Path.Combine(Properties.Settings.Default.OutputPath, fileName);
        }

        private static void Create()
        {
            File.Copy(Properties.Settings.Default.AccessDbPath, targetFileName());
        }
    }
}
