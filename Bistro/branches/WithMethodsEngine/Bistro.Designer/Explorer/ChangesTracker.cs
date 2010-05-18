using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Bistro.Configuration;
using Bistro.MethodsEngine;
using Bistro.MethodsEngine.Reflection;
using Bistro.MethodsEngine.Subsets;
using Bistro.Controllers;
using Bistro.Interfaces;

namespace Bistro.Designer.Explorer
{
    using ControllersTable = Dictionary<string, Dictionary<string, List<string>>>;
    //using Microsoft.Build.BuildEngine;
    public delegate void UpdateTreeDelegate(object sender, UpdateEventArgs e);
    public struct Node
    {
        public string oldtext;
        public string newtext;
        public object tag;
        public string parent;
    }
    public class UpdateEventArgs : EventArgs
    {

        public UpdateEventArgs(Node node)
        {
            NewNode = node;
        }
        public Node NewNode { get; set; }

    }
    internal class ChangesTracker
    {
        internal ChangesTracker(string parserType)
        {

            //SectionHandler sh = new SectionHandler();
            //sh.Application = "Bistro.Application";
            //sh.LoggerFactory = "Bistro.Logging.DefaultLoggerFactory";
            //Bistro.Application.Initialize(sh);
            this.engine = new Bistro.MethodsEngine.EngineControllerDispatcher(Bistro.Application.Instance);
            this.projExt = parserType;
            if (parserType == "csproj")
            {
                this.parser = new CSharpParser();
            }
            else
            {
               this.parser =  new FSharpParser();
            }
        }
        //public event UpdateTreeDelegate NodesChanged;
        /// <summary>
        /// There will be always one observer for this subject - that's why no observer collection is needed
        /// </summary>
        /// <param name="observer"></param>

        internal void OnProjectOpened(List<string> files)
        {
            foreach (string file in files)
            {
                parser.FileName = file;
                parser.FillControllerInfo();
            }

        }
        internal void RebuildNodes(List<string> files)
        {
            foreach (string file in files)
            {
                if (file.StartsWith("-"))
                {
                    string fileName = file.Substring(1);
                    //1. remove controllers defined in that file from engine
                    
                    //2. delete corresponding entry in parser.infobyFiles
                    if (parser.infobyFiles.ContainsKey(fileName))
                        parser.infobyFiles.Remove(fileName);
                }
                else if (file.StartsWith("*"))
                {
                    if (file.EndsWith(projExt))
                    {
                        //call OnProjectRenamed - update root node
                        return;
                    }
                    parser.FileName = file.Substring(1);
                    if (parser.FillControllerInfo())
                    {
                        /*//engine.Clean();
                        foreach (KeyValuePair<string, Dictionary<string, List<string>>> kvp in parser.controllerInfo)
                        {
                           //engine.UpdateControllerData - the controllers that are defined in other files
                         *  should not be clened out
                           engine.RegisterController(new Explorer.ControllerDescription(kvp.Key, kvp.Value));
                        }
                        engine.ForceUpdateBindPoints();*/
                        AnalyzeDependencies();

                    }
                }
            }

        }
        private void AnalyzeDependencies()
        {
           //necessary to find difference and update only nodes where smth has changed
        }
        internal ExplorerWindow Explorer { set;get;}
        private MetadataParserBase parser;
        private EngineControllerDispatcher engine;
        private string projExt;
    }
}

