using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using Ionic.Utils.Zip;
using Microsoft.Win32;
using System.Threading;

namespace Hill30.Tools
{
    class Program
    {
        private const string NDJANGODIR = "[NDJANGODIR]";
        private const string BISTRODIR = "[BISTRODIR]";
        private const string EXTENDERDIR = "[EXTENDERDIR]";
        private const string NDJANGOVER = "0.9.7";
        private const string BISTROVER = "0.9.2";


        /// <summary>
        /// The program search through the registry for specified value to get the corresponding installation path
        /// Once found, it replaces the special values like [BISTRODIR] in project files with retrieved paths.
        /// Works on 32 bit systems and on 64 bit as well (though there is smth to do about retrieving 64bit keys)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                String bistroPath = String.Empty;
                String ndjangoPath = String.Empty;
                String extenderPath = String.Empty;
                String FILE_PATH = args[0];
                
               
                bistroPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\Bistro").GetValue("InstallDir") ?? @"C:\Program Files\Hill30\Bistro\";
                ndjangoPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\NDjango").GetValue("InstallDir") ?? @"C:\Program Files\Hill30\NDjango\.NET35\";
                extenderPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\ProjectExtender").GetValue("InstallDir") ?? @"C:\Program Files\Hill30\ProjectExtender\";
                //64-bit
                //else if (IntPtr.Size == 8)
                //{
                //    //TODO: not to use WOW63432Node directly from the code.
                //    //See also: http://blogs.msdn.com/cumgranosalis/archive/2005/12/09/Win64RegistryPart1.aspx
                //    bistroPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Hill30\Bistro").GetValue("InstallDir") ?? @"C:\Program Files (x86)\Hill30\Bistro\";
                //    ndjangoPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Hill30\NDjango").GetValue("InstallDir") ?? @"C:\Program Files (x86)\Hill30\NDjango\.NET35\";
                //    extenderPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Hill30\ProjectExtender").GetValue("InstallDir") ?? @"C:\Program Files (x86)\Hill30\ProjectExtender\";
                //}

                if (!bistroPath.EndsWith("\\"))
                    bistroPath += "\\";
                if (!ndjangoPath.EndsWith("\\"))
                    ndjangoPath += "\\";
                if (!extenderPath.EndsWith("\\"))
                    extenderPath += "\\";
                FILE_PATH = FILE_PATH.Replace("\"", "");
                bistroPath = bistroPath.Replace("\"", "");
                ndjangoPath = ndjangoPath.Replace("\"", "");
                extenderPath = extenderPath.Replace("\"", "");

                if (FILE_PATH.EndsWith(".zip"))
                    replaceDirZip(FILE_PATH, bistroPath, ndjangoPath, extenderPath);
                else replaceDirFile(FILE_PATH, bistroPath, ndjangoPath, extenderPath);
            }
            catch (Exception ex)
            {
                File.AppendAllText(Path.GetTempPath()+@"WFSInstallError.log","Message:"+ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        private static void replaceDirZip(string zipfile_PATH, string bistroPath, string ndjangoPath, string extenderPath)
        {
            
                ZipFile zipf = ZipFile.Read(Path.GetFullPath(zipfile_PATH));
                List<ZipEntry> filesToChange = new List<ZipEntry>();
                foreach (ZipEntry entry in zipf.Entries)
                {
                    if (entry.FileName.Contains(".csproj") || entry.FileName.Contains(".fsproj"))
                        filesToChange.Add(entry);
                }

                foreach (ZipEntry entry in filesToChange)
                {
                    entry.Extract(Path.GetTempPath(), true);
                    StreamReader reader = new StreamReader(Path.GetTempPath() + entry.FileName);
                    string content = reader.ReadToEnd();
                    reader.Close();

                    content = content.Replace(BISTRODIR, Path.GetFullPath(bistroPath)).
                        Replace(NDJANGODIR, Path.GetFullPath(ndjangoPath)).
                        Replace(EXTENDERDIR, Path.GetFullPath(extenderPath));

                    zipf.RemoveEntry(entry);
                    zipf.AddFileFromString(entry.FileName, "", content);

                    zipf.Save();
                    File.Delete(Path.GetTempPath() + entry.FileName);
                }
        }
        private static void replaceDirFile(string FILE_PATH, string bistroPath, string ndjangoPath, string extenderPath)
        {
            StreamReader reader = new StreamReader(FILE_PATH);
            string content = reader.ReadToEnd();
            reader.Close();

            //bistroPath = Path.GetFullPath(bistroPath).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            //ndjangoPath = Path.GetFullPath(ndjangoPath).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            content = content.Replace(BISTRODIR, bistroPath).Replace(NDJANGODIR, ndjangoPath).Replace(EXTENDERDIR, extenderPath);
            File.WriteAllText(FILE_PATH, content);
        }

    }
}
