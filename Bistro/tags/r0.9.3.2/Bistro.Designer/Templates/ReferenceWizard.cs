using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TemplateWizard;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;


namespace BistroTemplates
{
    public class ReferenceWizard: IWizard
    {
        #region IWizard Members
#if VS2008
        private const string NDJANGODIR = "[NDJANGODIR35]";
#elif VS2010
        private const string NDJANGODIR = "[NDJANGODIR40]";
#endif
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
            String bistroPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\Bistro").GetValue("InstallDir");
#if VS2008
            String ndjangoPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\NDjango").GetValue("InstallDir2008");
#elif VS2010
            String ndjangoPath = (String)Registry.LocalMachine.CreateSubKey(@"Software\Hill30\NDjango").GetValue("InstallDir2010");
#endif
            StreamReader reader = new StreamReader(fileName);
            string content = reader.ReadToEnd();
            reader.Close();
            content = content.Replace(BISTRODIR, (bistroPath == null) ? BISTRODIR : Path.GetFullPath(bistroPath)).
                Replace(NDJANGODIR, (ndjangoPath == null) ? NDJANGODIR : Path.GetFullPath(ndjangoPath));
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
