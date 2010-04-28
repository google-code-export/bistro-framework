#pragma warning disable 0169, 0649, 0618
// -------------------------------------------------------------------------------
//
//		Generated document definition classes
//		Workflow Server code generator v.2.3.2.0
//		All modifications to the content of this file will be lost
//		when file will be overwritten by the code generator
//
//		Generated from the Xml schema located at 
//			Config\ServerConfig.xml
//			as of 8/30/2008 2:45 PM
//
// -------------------------------------------------------------------------------
namespace WorkflowServer.GeneratedCode.Documents {
    using WorkflowServer.Foundation.Documents;
    using WorkflowServer.Foundation.Documents.MetaInfo;
    
    
    [System.SerializableAttribute()]
    public partial class message_queue : WorkflowServer.Foundation.Documents.DocumentMessageQueue {
        
        // Contains names of metanodes. Used at run time by the ElementMetaNode.Build method
        static string[] WS_metanode_names = new string[] {
                "docID",
                "message"};
        
        // Local metanode index. Populated runtime by the ElmentMetaNode.Build method
        static WorkflowServer.Foundation.Documents.MetaInfo.DocumentMetaNode[] WS_metanodes;
        
        /// WS_documentDependency WorkflowServer.Foundation.Documents.DocumentMessageQueue=Unknown
        public static System.Guid WS_documentVersion = new System.Guid("61546c1f-1f88-428c-9e39-4473b2c32f37");
        
        public message_queue(WorkflowServer.Foundation.Documents.DocumentFactory factory, System.Xml.XmlReader content) : 
                base(factory, content) {
            return;
        }
        
        public message_queue(WorkflowServer.Foundation.Documents.DocumentFactory factory) : 
                base(factory) {
            return;
        }
        
        public static WorkflowServer.Foundation.Documents.FactoryEnvelope<message_queue> DefaultFactory {
            get {
                return ((WorkflowServer.Foundation.Documents.FactoryEnvelope<message_queue>)(WorkflowServer.Foundation.Documents.DocumentFactory.GetFactoryByName("message-queue")));
            }
        }
    }
}
