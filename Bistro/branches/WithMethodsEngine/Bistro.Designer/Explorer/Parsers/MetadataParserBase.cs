using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;
using Irony.Parsing;
using Irony.Samples.CSharp;
using Irony.Samples.FSharp;
using Bistro.MethodsEngine;
using Bistro.MethodsEngine.Reflection;
using Bistro.Interfaces;
using Bistro.Controllers;
using Bistro.Controllers.Descriptor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace Bistro.Designer.Explorer
{

    public class BindPointInfo : IMethodsBindPointDesc
    {
        /// <summary>
        /// BindAttribute Syntax Example [<Bind("target",Priority = 1,ControllerBindType = BindType.After,Target="target")>]
        /// </summary>
        internal int priority = -1;
        internal BindType bindType = BindType.Before;
        internal string target;
        internal string verb = "get";
        internal int bindLen;
        internal ControllerMetadata controller;
        public BindPointInfo() { }
        public BindPointInfo(BindPointInfo info)
        {
            
            this.target = info.target;
            this.controller = info.controller;
            this.bindType = info.bindType;
            this.bindLen = info.bindLen;
            this.priority = info.priority;
        }
        /*public BindPointInfo(string target, int priority, ControllerMetadata controller)
        {
            //BindPointInfo(target,"all",-1,BindType.Before,controller);

        }*/
        /*public BindPointInfo(string target, string verb, int priority, BindType type, ControllerMetadata controller)
        {
            this.target = target;
            this.verb = verb;
            this.priority = priority;
            this.bindType = type;
            this.controller = controller;
        }*/

        #region IMethodsBindPointDesc Members

        int IMethodsBindPointDesc.BindLength
        {
            get { return bindLen; }
        }

        IMethodsControllerDesc IMethodsBindPointDesc.Controller
        {
            get { return controller; }
        }

        BindType IMethodsBindPointDesc.ControllerBindType
        {
            get { return bindType; }
        }

        int IMethodsBindPointDesc.Priority
        {
            get { return priority; }
        }

        string IMethodsBindPointDesc.Target
        {
            get { return target; }
        }

        #endregion
    }
    public class ControllerMetadata : IMethodsControllerDesc
    {
        string name;
        public string FileName{get;set;}
        public int Line{get;set;}
        internal bool isSecurity;
        internal List<BindPointInfo> Binds;
        internal List<string> RenderWith;
        internal List<string> DependsOn;
        internal List<string> Provides;
        internal List<string> Requires;
        public ControllerMetadata(string name)
        {
            this.name = name;
            Binds = new List<BindPointInfo>();
            RenderWith = new List<string>();
            DependsOn = new List<string>();
            Provides = new List<string>();
            Requires = new List<string>();
        }

        #region IMethodsControllerDesc Members

        string IMethodsControllerDesc.ControllerTypeName
        {
            get { return name; }
        }

        List<string> IMethodsControllerDesc.DependsOn
        {
            get { return DependsOn; }
        }

        string IMethodsControllerDesc.GetResourceType(string resourceName)
        {
            return "type";
        }

        bool IMethodsControllerDesc.IsSecurity
        {
            get { return isSecurity; }
        }

        List<string> IMethodsControllerDesc.Provides
        {
            get { return Provides; }
        }

        List<string> IMethodsControllerDesc.Requires
        {
            get { return Requires; }
        }

        IEnumerable<IMethodsBindPointDesc> IMethodsControllerDesc.Targets
        {
            get { return (IEnumerable<IMethodsBindPointDesc>)Binds; }
        }

        #endregion
    }
    public class MetadataParserBase
    {

        /// <summary>
        ///info about controllers is stored here after parser finished parsing current file
        /// </summary>
        public List<ControllerMetadata> controllerInfo;
        /// <summary>
        /// format key = absolute src filename,value={controllerInfo(see above)}
        /// </summary>
        public Dictionary<string, List<ControllerMetadata>> infobyFiles;
        public MetadataParserBase()
        {
            parseTree = null;
            controllerInfo = new List<ControllerMetadata>();
            infobyFiles = new Dictionary<string, List<ControllerMetadata>>();
        }
        public string FileName
        {
            set { srcFilename = value; }
            get { return srcFilename; }
        }
        /// <summary>
        /// Method is used to extract controllers' data from specified source file into ControllersTable 
        /// </summary>
        /// <returns>true if essential changes in controllers' declarations were detected,
        /// else - otherwise</returns>
        public bool FillControllerInfo()
        {

            try
            {
                TextReader rd = new StreamReader(srcFilename);
                controllerInfo.Clear();
                Extract(rd.ReadToEnd(), "<source>");
                if (parseTree == null
                    || parseTree.Status == ParseTreeStatus.Error) return false;
                AddParseNodeRec(parseTree.Root, false);
                //if source file has been recently added to the project,add new row into ControllersTable
                //else if there were any changes in source file,update corresponding row in ControllersTable
                if (!infobyFiles.ContainsKey(srcFilename))
                {
                    List<ControllerMetadata> curtbl = new List<ControllerMetadata>(controllerInfo);
                    infobyFiles.Add(srcFilename, curtbl);
                    return true;
                }
                else
                {
                    //TODO : compare controllerInfo with infobyFiles[FileName] and smart merge changes 
                    return false;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(srcFilename + " is not a valid filename");
                return false;
            }
        }

        protected ParseTree parseTree;
        protected Parser parser;
        protected Grammar grammar;
        private string srcFilename;
        protected const string tail = " (identifier)";
        protected const string toCut = " (StringLiteral)";

        protected string curCtrl;//name of the controller that is being processed
        protected string curAttr;//name of the attribute that is being processed
        protected BindPointInfo curbpi = new BindPointInfo();

        private void Extract(string srcText, string fname)
        {
            try
            {
                parser.Parse(srcText, fname);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                parseTree = parser.Context.CurrentParseTree;
            }
        }
        protected virtual void AddParseNodeRec(ParseTreeNode nodeInfo, bool skip)
        {
        }
        protected virtual void AnalyzeTree(ParseTreeNode nodeInfo)
        {
        }

    }
    public static class MetadataFromDll
    {
        
        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="assm">The assm.</param>
        public static bool LoadAssemblies(List<Assembly> assmlist,EngineControllerDispatcher engine)
        {
            bool controllerFound = false;
            int k = 0;
            try
            {
                foreach (Assembly assm in assmlist)
                {
                    var aaa = assm.GetTypes();
                    int i = 0;
                    int j = aaa.Length;
                    foreach (Type t in aaa)
                    {
                        if (t.GetInterface(typeof(IController).Name) != null)
                        {
                            controllerFound = true;
                            if (!t.IsAbstract)
                            {
                                IControllerDescriptor descriptor = ControllerDescriptor.CreateDescriptor(t, null);
                                engine.RegisterController(descriptor);
                            }
                        }
                        i++;
                    }
                    k++;
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                for (int i = 0; i < ex.Types.Length; i++)
                    if (ex.Types[i] != null)
                        sb.AppendFormat("\t{0} loaded\r\n", ex.Types[i].Name);

                for (int i = 0; i < ex.LoaderExceptions.Length; i++)
                    if (ex.LoaderExceptions[i] != null)
                        sb.AppendFormat("\texception {0}\r\n", ex.LoaderExceptions[i].Message);

                Debug.WriteLine("ExceptionLoadingAssembly " + assmlist[k].FullName + " " + sb.ToString());
            }
            return controllerFound;
        }
    }
   
}
