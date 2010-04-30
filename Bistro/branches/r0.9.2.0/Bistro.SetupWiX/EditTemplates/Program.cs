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
        /// </summary>
        /// <param name="args">only one argument - template path</param>
        static void Main(string[] args)
        {
            try
            {
                String FILE_PATH = args[0];

                String bistroPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\Bistro").GetValue("InstallDir");
                String ndjangoPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\NDjango").GetValue("InstallDir");
                String extenderPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\ProjectExtender").GetValue("InstallDir");

                FILE_PATH = FILE_PATH.Replace("\"", "");

                if (bistroPath != null)
                {
                    if (!bistroPath.EndsWith("\\"))
                        bistroPath += "\\";
                    bistroPath = bistroPath.Replace("\"", "");
                }

                if (ndjangoPath != null)
                {
                    if (!ndjangoPath.EndsWith("\\"))
                        ndjangoPath += "\\";
                    ndjangoPath = ndjangoPath.Replace("\"", "");
                }

                if (extenderPath != null)
                {
                    if (!extenderPath.EndsWith("\\"))
                        extenderPath += "\\";
                    extenderPath = extenderPath.Replace("\"", "");
                }

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

        /// <summary>
        /// process zip file
        /// </summary>
        /// <param name="zipfile_PATH"></param>
        /// <param name="bistroPath"></param>
        /// <param name="ndjangoPath"></param>
        /// <param name="extenderPath"></param>
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

                    content = content.Replace(BISTRODIR, (bistroPath == null) ? BISTRODIR : Path.GetFullPath(bistroPath)).
                        Replace(NDJANGODIR, (ndjangoPath == null) ? NDJANGODIR : Path.GetFullPath(ndjangoPath)).
                        Replace(EXTENDERDIR, (extenderPath == null) ? EXTENDERDIR : Path.GetFullPath(extenderPath));

                    zipf.RemoveEntry(entry);
                    zipf.AddFileFromString(entry.FileName, "", content);

                    zipf.Save();
                    File.Delete(Path.GetTempPath() + entry.FileName);
                }
        }

        /// <summary>
        /// process file
        /// </summary>
        /// <param name="FILE_PATH"></param>
        /// <param name="bistroPath"></param>
        /// <param name="ndjangoPath"></param>
        /// <param name="extenderPath"></param>
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
