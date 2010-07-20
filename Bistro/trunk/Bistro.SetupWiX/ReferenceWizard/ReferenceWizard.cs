using System;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TemplateWizard;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;


namespace Templates
{
    public class ReferenceWizard: IWizard
    {
        #region IWizard Members
        private const string NDJANGODIR35 = "[NDJANGODIR35]";
        private const string NDJANGODIR40 = "[NDJANGODIR40]";
        private const string BISTRODIR = "[BISTRODIR]";
        IVsSolution sln;
        string fileName;
        EnvDTE.DTE dte;
        
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
            fileName = project.FullName;
            
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            sln = ((IVsSolution)Package.GetGlobalService(typeof(SVsSolution)));
            ServiceProvider serviceProvider = new ServiceProvider(dte as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            string projectGuid = null;
            using (XmlReader projectReader = XmlReader.Create(fileName))
            {
                projectReader.MoveToContent();
                object nodeName = projectReader.NameTable.Add("ProjectGuid");
                while (projectReader.Read())
                {
                    if (Object.Equals(projectReader.LocalName, nodeName))
                    {
                        projectGuid = projectReader.ReadElementContentAsString();
                        break;
                    }
                }
            }
            IVsHierarchy hier = VsShellUtilities.GetHierarchy(serviceProvider, new Guid(projectGuid));
            sln.CloseSolutionElement((uint)__VSSLNCLOSEOPTIONS.SLNCLOSEOPT_UnloadProject, hier, 0);
            String bistroPath = (String)Registry.LocalMachine.OpenSubKey(@"Software\Hill30\Bistro").GetValue("InstallDir");
            String ndjangoPath35 = (String)Registry.LocalMachine.OpenSubKey(@"Software\Hill30\NDjango").GetValue("InstallDir2008");
            String ndjangoPath40 = (String)Registry.LocalMachine.OpenSubKey(@"Software\Hill30\NDjango").GetValue("InstallDir2010");
            StreamReader reader = new StreamReader(fileName);
            string content = reader.ReadToEnd();
            reader.Close();
            content = content.Replace(BISTRODIR, (bistroPath == null) ? BISTRODIR : Path.GetFullPath(bistroPath)).
                Replace(NDJANGODIR35, (ndjangoPath35 == null) ? NDJANGODIR35 : Path.GetFullPath(ndjangoPath35)).
                Replace(NDJANGODIR40, (ndjangoPath40 == null) ? NDJANGODIR40 : Path.GetFullPath(ndjangoPath40));
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(content);
            sw.Close();
            IntPtr ppProject = IntPtr.Zero;
            Guid guid1 = new Guid();
            Guid guid2 = new Guid();
            sln.CreateProject(ref guid1, fileName, null, null, (uint)__VSCREATEPROJFLAGS.CPF_OPENFILE, ref guid2, out ppProject);


        }

        public void RunStarted(object automationObject, System.Collections.Generic.Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            dte = automationObject as EnvDTE.DTE;
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        #endregion
    }
}
