using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Bistro.UnitTests.Support.Reflection;
using Bistro.UnitTests.Tests.Compatibility;
using Bistro.Controllers;
using Bistro.Controllers.Dispatch;
using Bistro.Configuration;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers.Descriptor.Data;
using Bistro.UnitTests.Support.CustomManager;
using System.Configuration;

namespace Bistro.UnitTests.Tests
{
    [TestFixture]
    public partial class CompatibilityTests
    {
        #region Test creation stuff
        #region TestDescriptor
        internal class TestDescriptor
        {
            public string Name { get; set; }
            public TestTypeInfo[] Controllers { get; set; }
            //            public IErrorDescriptor[] Errors { get; set; }
            public UrlControllersTest[] UrlTests { get; set; }

            public override string ToString()
            {
                return Name;
            }

            public TestDescriptor(string name, TestTypeInfo[] controllers, UrlControllersTest[] urlTests)
            {
                Name = name;
                Controllers = controllers;
                //Errors = errors ?? new IErrorDescriptor[0];
                UrlTests = urlTests ?? new UrlControllersTest[0];
                //BindingTree = bindingTree;
            }


        }
        #endregion

        #region Types and Type creation methods
        private TestTypeInfo[] Types(params TestTypeInfo[] types) { return types; }

        private TestTypeInfo Type(string type, params TestTypeInfo.TestAttributeInfo[] attributes)
        {

            return Type(type, attributes, new TestTypeInfo.TestFieldInfo[] { });
        }

        private TestTypeInfo Type(string type, TestTypeInfo.TestAttributeInfo[] attributes, params TestTypeInfo.TestFieldInfo[] fields)
        {
            return Type(type, attributes, fields, new TestTypeInfo.TestPropertyInfo[] { });
        }

        private TestTypeInfo Type(string type, TestTypeInfo.TestAttributeInfo[] attributes, params TestTypeInfo.TestPropertyInfo[] properties)
        {
            return Type(type, attributes, new TestTypeInfo.TestFieldInfo[] { }, properties);
        }

        private TestTypeInfo Type(string type, TestTypeInfo.TestAttributeInfo[] attributes, TestTypeInfo.TestFieldInfo[] fields, params TestTypeInfo.TestPropertyInfo[] properties)
        {
            return new TestTypeInfo(type, attributes, fields, properties);
        }
        #endregion

        #region Bind attributes creation methods

        private TestTypeInfo.TestAttributeInfo[] Attributes(params TestTypeInfo.TestAttributeInfo[] attributes) { return attributes; }

        private TestTypeInfo.TestFieldInfo Field(string name, string type, params TestTypeInfo.TestAttributeInfo[] attributes)
        {
            return new TestTypeInfo.TestFieldInfo(name, type, attributes);
        }

        private TestTypeInfo.TestAttributeInfo BindAttribute(string binding)
        {
            return new TestTypeInfo.TestAttributeInfo(typeof(BindAttribute), new Parameter("Target", binding), new Parameter("ControllerBindType", BindType.Before), new Parameter("Priority", -1));
        }

        private TestTypeInfo.TestAttributeInfo RequestAttribute { get { return new TestTypeInfo.TestAttributeInfo(typeof(RequestAttribute)); } }

        private TestTypeInfo.TestAttributeInfo FormFieldAttribute { get { return new TestTypeInfo.TestAttributeInfo(typeof(FormFieldAttribute)); } }

        private TestTypeInfo.TestAttributeInfo RequiresAttribute { get { return new TestTypeInfo.TestAttributeInfo(typeof(RequiresAttribute)); } }

        private TestTypeInfo.TestAttributeInfo ProvidesAttribute { get { return new TestTypeInfo.TestAttributeInfo(typeof(ProvidesAttribute)); } }

        private TestTypeInfo.TestAttributeInfo DependsOnAttribute { get { return new TestTypeInfo.TestAttributeInfo(typeof(DependsOnAttribute)); } }

        private TestTypeInfo.TestAttributeInfo SessionAttribute { get { return new TestTypeInfo.TestAttributeInfo(typeof(SessionAttribute)); } }
        #endregion

        #region Url tests creation methods

		private UrlControllersTest UrlTest(string name, string url, params object[] controllersGroups)
		{
			url = url.Replace(" ", "");
			return new UrlControllersTest(name, url, controllersGroups);
		}
		/// <summary>
		/// UnOrdered group
		/// </summary>
		/// <param name="groups"></param>
		/// <returns></returns>
		private CtrGroupUnordered CtrUnOrdGrp(params object[] groups)
		{
			return new CtrGroupUnordered(groups);
		}

		/// <summary>
		/// Ordered group
		/// </summary>
		/// <param name="groups"></param>
		/// <returns></returns>
		private CtrGroupOrdered CtrOrdGrp(params object[] groups)
		{
			return new CtrGroupOrdered(groups);
		}


        #endregion


        List<TestDescriptor> tests = new List<TestDescriptor>();

        #region Test Creation Methods

        private void NewTestWithUrl(string name, TestTypeInfo[] types, params UrlControllersTest[] urlTests)//, IErrorDescriptor[] errors, params BindingTest[] nodes)
        {
            tests.Add(new TestDescriptor(name, types, urlTests));
        }


        #endregion



        #endregion

        /// <summary>
        /// Configures section handler.
        /// </summary>
        [TestFixtureSetUp]
        public void setup()
        {

            initSh = new SectionHandler();

            initSh.ControllerManagerFactory = "Bistro.UnitTests.Support.CustomManager.TestControllerManagerFactory, Bistro.UnitTests";
            initSh.Application = "Bistro.UnitTests.Support.CustomManager.TestApplication, Bistro.UnitTests";




        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestFixtureTearDown]
        public void Cleanup() 
        {
            TestApplication app = Application.Instance as TestApplication;
            app.ResetApp();

        }


        #region private fields
        private Application application;
        private IControllerManager manager;
        private IControllerDispatcher dispatcher;
        private SectionHandler initSh;
        #endregion

        /// <summary>
        /// Test execution.
        /// </summary>
        /// <param name="test"></param>
        void realTest(object test)
        {
            #region Load part
            TestDescriptor descriptor = (TestDescriptor)test;

            Application.Initialize(initSh);

            application = Application.Instance;
            manager = application.ManagerFactory.GetManagerInstance();
            dispatcher = application.DispatcherFactory.GetDispatcherInstance();

            TestControllerManager testMgr = manager as TestControllerManager;
            Assert.IsNotNull(testMgr, "Invalid TestControllerManager");

            testMgr.LoadSpecial(descriptor.Controllers);
            #endregion


            #region Test part



            foreach (UrlControllersTest urlTest in descriptor.UrlTests)
            {
                urlTest.Validate(dispatcher);
            }

            #endregion
            
        }


        [Test, TestCaseSource("TestSource")]
        public void run(object test)
        {
            realTest(test);
        }


        internal IList<TestDescriptor> TestSource()
        {
            #region ValidUrls1 test
            NewTestWithUrl("ValidUrls",
                Types(
                    Type(
                        "TestController",
                        BindAttribute("/auth/signin"),
                        BindAttribute("/postings/{contentType}")
                        )
                    ),
                    UrlTest("test1", "GET /auth/signin", "TestController"),
                    UrlTest("GET /auth/signin", "GET /auth/signin", "TestController"),
                    UrlTest("POST /auth/signin", "POST /auth/signin", "TestController"),
                    UrlTest("PUT /auth/signin", "PUT /auth/signin", "TestController"),
                    UrlTest("DELETE /auth/signin", "DELETE /auth/signin", "TestController"),
                    UrlTest("GET /postings/", "GET /postings/", "TestController"),
                    UrlTest("GET /postings/variablevalue1", "GET /postings/variablevalue1", "TestController"),
                    UrlTest("GET /postings/123412423", "GET /postings/123412423", "TestController"),
                    UrlTest("GET /postings/testvalue", "GET /postings/testvalue", "TestController"),
                    UrlTest("POST /postings/", "POST /postings/", "TestController"),
                    UrlTest("POST /postings/variablevalue1", "POST /postings/variablevalue1", "TestController"),
                    UrlTest("POST /postings/123412423", "POST /postings/123412423", "TestController"),
                    UrlTest("POST /postings/testvalue", "POST /postings/testvalue", "TestController"),
                    UrlTest("PUT /postings/", "PUT /postings/", "TestController"),
                    UrlTest("PUT /postings/variablevalue1", "PUT /postings/variablevalue1", "TestController"),
                    UrlTest("PUT /postings/123412423", "PUT /postings/123412423", "TestController"),
                    UrlTest("PUT /postings/testvalue", "PUT /postings/testvalue", "TestController"),
                    UrlTest("DELETE /postings/", "DELETE /postings/", "TestController"),
                    UrlTest("DELETE /postings/variablevalue1", "DELETE /postings/variablevalue1", "TestController"),
                    UrlTest("DELETE /postings/123412423", "DELETE /postings/123412423", "TestController"),
                    UrlTest("DELETE /postings/testvalue", "DELETE /postings/testvalue", "TestController")
                    );
            #endregion

            #region ValidUrls2 test
            NewTestWithUrl("ValidUrls2",
                Types(
                    Type(
                        "TestController",
                        BindAttribute("/A/B"),
                        BindAttribute("/B/C")
                        ),
                    Type(
                        "CommonController",
                        BindAttribute("/A/*")
                        )
                    ),
                    UrlTest("test1", "GET /A/B", "CommonController", "TestController"),
                    UrlTest("GET /A/B", "GET /A/B", "CommonController", "TestController"),
                    UrlTest("POST /A/B", "POST /A/B", "CommonController", "TestController"),
                    UrlTest("PUT /A/B", "PUT /A/B", "CommonController", "TestController"),
                    UrlTest("DELETE /A/B", "DELETE /A/B", "CommonController", "TestController"),
                    UrlTest("GET /B/C", "GET /B/C", "TestController"),
                    UrlTest("POST /B/C", "POST /B/C", "TestController"),
                    UrlTest("PUT /B/C", "PUT /B/C", "TestController"),
                    UrlTest("DELETE /B/C", "DELETE /B/C", "TestController"),
                    UrlTest("GET /A/aaaaa", "GET /A/aaaaa", "CommonController"),
                    UrlTest("GET /A/abcde", "GET /A/abcde", "CommonController"),
                    UrlTest("GET /A/testvalue", "GET /A/testvalue", "CommonController"),
                    UrlTest("POST /A/aaaaa", "POST /A/aaaaa", "CommonController"),
                    UrlTest("POST /A/abcde", "POST /A/abcde", "CommonController"),
                    UrlTest("POST /A/testvalue", "POST /A/testvalue", "CommonController"),
                    UrlTest("PUT /A/aaaaa", "PUT /A/aaaaa", "CommonController"),
                    UrlTest("PUT /A/abcde", "PUT /A/abcde", "CommonController"),
                    UrlTest("PUT /A/testvalue", "PUT /A/testvalue", "CommonController"),
                    UrlTest("DELETE /A/aaaaa", "DELETE /A/aaaaa", "CommonController"),
                    UrlTest("DELETE /A/abcde", "DELETE /A/abcde", "CommonController"),
                    UrlTest("DELETE /A/testvalue", "DELETE /A/testvalue", "CommonController")
                    );
            #endregion

            #region Big Test
            NewTestWithUrl("Big Test",
                Types(
                    Type("Pageable",
                        Attributes(
                            BindAttribute("GET /?/Pageable?{PageNumber}&{PageSize}")
                        ),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("PageNumber", "int", ProvidesAttribute, RequestAttribute),
                        Field("PageSize", "int", ProvidesAttribute, RequestAttribute),
                        Field("TotalItemCount", "int", ProvidesAttribute, RequestAttribute),
                        Field("HasPreviousPage", "bool", ProvidesAttribute, RequestAttribute),
                        Field("HasNextPage", "bool", ProvidesAttribute, RequestAttribute),
                        Field("PageCount", "int", ProvidesAttribute, RequestAttribute),
                        Field("IsPaged", "bool", ProvidesAttribute, RequestAttribute),
                        Field("PageNumbers", "ArrayList", ProvidesAttribute, RequestAttribute),
                        Field("PagingInfo", "string", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", DependsOnAttribute, RequestAttribute)
                        ),

                    Type("Sortable",
                        Attributes(
                            BindAttribute("GET /?/Sortable?{OrderBy}&{Direction}")
                        ),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("OrderBy", "string", ProvidesAttribute, RequestAttribute),
                        Field("Direction", "string", ProvidesAttribute, RequestAttribute),
                        Field("IsSorted", "bool", ProvidesAttribute, RequestAttribute),
                        Field("PagingInfo", "string", ProvidesAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("Authenticator",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("User", "AIMUserStub", ProvidesAttribute, SessionAttribute),
                        Field("PermissionManager", "object", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("Bar",
                        Attributes(
                            BindAttribute("GET /Bar/{BarID}")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("BarID", "string", ProvidesAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("BarDelete",
                        Attributes(
                            BindAttribute("DELETE /Bar/{BarID}")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("BarID", "string", ProvidesAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("Bars",
                        Attributes(
                            BindAttribute("GET /Bars")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("Data12Id", "short", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("GetNextStep",
                        Attributes(
                            BindAttribute("GET /Clinical/GetNextStep?{TransactionId}&{Data17isId}")
                        ),
                        Field("TransactionId", "string", RequestAttribute),
                        Field("Data17isId", "string", RequestAttribute),
                        Field("Questions", "List<ClinicalQuestionStub>", RequestAttribute),
                        Field("Actions", "List<ClinicalAction>", RequestAttribute)
                        ),
                    Type("InitializeClinical",
                        Attributes(
                            BindAttribute("GET /Clinical/Initialize")
                        ),
                        Field("TransactionId", "string", ProvidesAttribute, RequestAttribute),
                        Field("Data17es", "List<Data17is>", ProvidesAttribute, RequestAttribute),
                        Field("ApplicationId", "int", RequestAttribute),
                        Field("BarId", "int", RequestAttribute),
                        Field("ExamId", "int", RequestAttribute),
                        Field("Data15Group", "short", RequestAttribute),
                        Field("Date", "DateTime", RequestAttribute),
                        Field("Data12Id", "short", RequestAttribute),
                        Field("FooDob", "DateTime", RequestAttribute),
                        Field("FooGender", "GenderStub", RequestAttribute),
                        Field("TransactionType", "TransactionTypeStub", RequestAttribute),
                        Field("UserId", "int", RequestAttribute),
                        Field("IsOverwrite", "bool", RequestAttribute),
                        Field("BarData15Groups", "string", RequestAttribute),
                        Field("RevisionType", "RevisionTypeStub", RequestAttribute),
                        Field("ClinicalProductId", "byte", RequestAttribute),
                        Field("DateOfData16", "DateTime", RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("Comments",
                        Attributes(
                            BindAttribute("GET /Comments")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("CommentsData16", "CommentsData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Categories", "object", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("CommentsSend",
                        Attributes(
                            BindAttribute("GET /Comments/Get")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("CommentsData16", "CommentsData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("UserComments", "object", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("DrugCategories",
                        Attributes(
                            BindAttribute("GET /DrugCategories")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("DrugData16", "DrugData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("Categories", "object", ProvidesAttribute, SessionAttribute)
                        ),
                    Type("Drugs",
                        Attributes(
                            BindAttribute("GET /Drugs")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("DrugData16", "DrugData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute)
                        ),
                    Type("Enrollments",
                        Attributes(
                            BindAttribute("GET /Enrollments")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("FooData16", "FooData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("RenderFooEligibilityList", "bool", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("HomeController",
                        Attributes(
                            BindAttribute("GET /default")
                        ),
                        Field("HelpUrl", "string", ProvidesAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("Foo",
                        Attributes(
                            BindAttribute("GET /EligibilityRouter")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("FooData16", "FooData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("FooRequest", "object", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("Foos",
                        Attributes(
                            BindAttribute("GET /Foos")
                        ),
                        Field("FooData16", "FooData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("HelpUrl", "string", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("OrderInquiry",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderInquiry")
                        ),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("UIState", "UIStateStub", SessionAttribute),
                        Field("Data12Data16", "Data12Data16Stub", DependsOnAttribute, RequestAttribute),
                        Field("HealthPlanListHolder", "object", ProvidesAttribute, RequestAttribute),
                        Field("HelpUrl", "string", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("OrderInquiryListBySite",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderInquiry/OrderListBySite")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("Data12Id", "short", DependsOnAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("OrderInquiryList", "object", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("DeleteBar",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest/DeleteBar")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("ProcessBarDeletion",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest/ProcessBarDeletion")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("DrugSelector",
                        Attributes(
                            BindAttribute("GET /DrugSelector?{CategoryId}&{DrugId}")/////////////////////////////////////////////////////////////
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("vtExam", "ExamStub", DependsOnAttribute, SessionAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("DrugData16", "DrugData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("CategoryId", "string", ProvidesAttribute, RequestAttribute),
                        Field("DrugId", "string", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("EligibilityRouter",
                        Attributes(
                            BindAttribute("GET /EligibilityRouter")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("FooData16", "FooData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("RenderFooEligibilityList", "bool", ProvidesAttribute, RequestAttribute),
                        Field("RenderFooInformation", "bool", ProvidesAttribute, RequestAttribute),
                        Field("RenderFooHistoryList", "bool", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("FooHistoryList",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest/FooHistoryList")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("FooData16", "FooData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("PagedDataHolder", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("RenderFooHistoryList", "bool", DependsOnAttribute, RequestAttribute),
                        Field("RenderPhysicianListForRequest", "bool", ProvidesAttribute, RequestAttribute),
                        Field("FooPlanID", "byte?", ProvidesAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("StepNumber", "int", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("FooInformation",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest/FooInformation")
                        ),
                        Field("RenderFooInformation", "bool", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("OrderRequest",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest")
                        ),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Data12Data16", "Data12Data16Stub", DependsOnAttribute, RequestAttribute),
                        Field("UIState", "UIStateStub", SessionAttribute),
                        Field("HealthPlanListHolder", "object", ProvidesAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("CurrentDate", "string", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("OrderRequestQueue",
                        Attributes(
                            BindAttribute("GET /OrderRequestQueue")
                        ),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("PhysicianSelector",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest/PhysicianSelector?{PhysicianId}")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("BarData16", "BarData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("Data14Data16", "Data14Data16Stub", DependsOnAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("Physician", "object", ProvidesAttribute, RequestAttribute),
                        Field("PhysicianId", "int", ProvidesAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("StepNumber", "int", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("StepWizard",
                        Attributes(
                            BindAttribute("GET /OrderManager/OrderRequest")/////////////////////////////////////////////////////
                        ),
                        Field("StepNumber", "int", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("Physicians",
                        Attributes(
                            BindAttribute("GET /Physicians")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("Data14Data16", "Data14Data16Stub", DependsOnAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("FooPlanID", "byte?", DependsOnAttribute, RequestAttribute),
                        Field("RenderPhysicianListForRequest", "bool", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("Foos",
                        Attributes(
                            BindAttribute("GET /Data14s")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("QueryStringCollection", "NameValueCollection", DependsOnAttribute, RequestAttribute),
                        Field("User", "AIMUserStub", DependsOnAttribute, SessionAttribute),
                        Field("Data12Id", "short", DependsOnAttribute, RequestAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute),
                        Field("HelpUrl", "string", ProvidesAttribute, RequestAttribute),
                        Field("Data14Data16", "Data14Data16Stub", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("QUnit",
                        Attributes(
                            BindAttribute("GET /QUnit")
                        )
                        ),
                    Type("RequestItems",
                        Attributes(
                            BindAttribute("GET /RequestItems")
                        ),
                        Field("QueryString", "string", DependsOnAttribute, RequestAttribute),
                        Field("DrugData16", "DrugData16Stub", DependsOnAttribute, RequestAttribute),
                        Field("vtBar", "BarStub", DependsOnAttribute, SessionAttribute),
                        Field("Data", "DataHolderStub", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("AjaxDeterminer",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("IsAjaxRequest", "bool", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("AntiForgeryRequestHandler",
                        Attributes(
                            BindAttribute("POST /?")
                        ),
                        Field("AntiForgeryRequestTokenCookie", "string", DependsOnAttribute, RequestAttribute),
                        Field("AntiForgeryRequestToken", "string", DependsOnAttribute, RequestAttribute, FormFieldAttribute)
                        ),
                    Type("AntiForgeryRequestSetter",
                        Attributes(
                            BindAttribute("GET /?")
                        ),
                        Field("AntiForgeryRequestTokenCookie", "string", DependsOnAttribute, RequestAttribute),
                        Field("AntiForgeryRequestToken", "string", DependsOnAttribute, RequestAttribute, FormFieldAttribute)
                        ),
                    Type("Fake",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("UserID", "int", ProvidesAttribute, RequestAttribute),
                        Field("Data12Id", "short", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("Messenger",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("MessageList", "object", ProvidesAttribute, RequestAttribute),
                        Field("MessageID", "string", DependsOnAttribute, RequestAttribute)
                        ),
                    Type("QueryStringDeterminer",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("QueryStringCollection", "NameValueCollection", ProvidesAttribute, RequestAttribute),
                        Field("QueryString", "string", ProvidesAttribute, RequestAttribute)
                        ),
                    Type("RootRedirect",
                        Attributes(
                            BindAttribute("/")
                        )
                        ),
                    Type("DefaultController",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("BarData16", "BarData16Stub", ProvidesAttribute, RequestAttribute),
                        Field("FooData16", "FooData16Stub", ProvidesAttribute, RequestAttribute),
                        Field("Data12Data16", "Data12Data16Stub", ProvidesAttribute, RequestAttribute),
                        Field("UserData16", "UserData16Stub", ProvidesAttribute, RequestAttribute),
                        Field("Data14Data16", "Data14Data16Stub", ProvidesAttribute, RequestAttribute),
                        Field("UIState", "UIStateStub", ProvidesAttribute, SessionAttribute),
                        Field("CommentsData16", "CommentsData16Stub", ProvidesAttribute, RequestAttribute),
                        Field("Root", "string", ProvidesAttribute, RequestAttribute)
                        )
                    ),
                    UrlTest("GET /Pageable?{PageNumber}&{PageSize}", "GET /Pageable?{PageNumber}&{PageSize}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Pageable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/Pageable?{PageNumber}&{PageSize}", "GET /abcde/edcba/aaaa123/bbb124/Pageable?{PageNumber}&{PageSize}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Pageable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /bbb124/Pageable?{PageNumber}&{PageSize}", "GET /bbb124/Pageable?{PageNumber}&{PageSize}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Pageable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /aaaa123/bbb124/Pageable?{PageNumber}&{PageSize}", "GET /aaaa123/bbb124/Pageable?{PageNumber}&{PageSize}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Pageable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /Sortable?{OrderBy}&{Direction}", "GET /Sortable?{OrderBy}&{Direction}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Sortable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/Sortable?{OrderBy}&{Direction}", "GET /abcde/edcba/aaaa123/bbb124/Sortable?{OrderBy}&{Direction}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Sortable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /bbb124/Sortable?{OrderBy}&{Direction}", "GET /bbb124/Sortable?{OrderBy}&{Direction}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Sortable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /aaaa123/bbb124/Sortable?{OrderBy}&{Direction}", "GET /aaaa123/bbb124/Sortable?{OrderBy}&{Direction}", "Messenger", "Fake", "DefaultController", "QueryStringDeterminer", "Authenticator", "Sortable", "AntiForgeryRequestSetter", "AjaxDeterminer"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("GET /Bar/", "GET /Bar/", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "Bar"),
                    UrlTest("GET /Bar/variablevalue1", "GET /Bar/variablevalue1", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "Bar"),
                    UrlTest("GET /Bar/123412423", "GET /Bar/123412423", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "Bar"),
                    UrlTest("GET /Bar/testvalue", "GET /Bar/testvalue", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "Bar"),
                    UrlTest("DELETE /Bar/", "DELETE /Bar/", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake", "BarDelete"),
                    UrlTest("DELETE /Bar/variablevalue1", "DELETE /Bar/variablevalue1", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake", "BarDelete"),
                    UrlTest("DELETE /Bar/123412423", "DELETE /Bar/123412423", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake", "BarDelete"),
                    UrlTest("DELETE /Bar/testvalue", "DELETE /Bar/testvalue", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake", "BarDelete"),
                    UrlTest("GET /Bars", "GET /Bars", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "Bars", "AntiForgeryRequestSetter"),
                    UrlTest("GET /Clinical/GetNextStep?{TransactionId}&{Data17isId}", "GET /Clinical/GetNextStep?{TransactionId}&{Data17isId}", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "GetNextStep"),
                    UrlTest("GET /Clinical/Initialize", "GET /Clinical/Initialize", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "InitializeClinical"),
                    UrlTest("GET /Comments", "GET /Comments", "QueryStringDeterminer", "Messenger", "DefaultController", "Comments", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /Comments/Get", "GET /Comments/Get", "QueryStringDeterminer", "Messenger", "DefaultController", "Comments", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter", "CommentsSend"),
                    UrlTest("GET /DrugCategories", "GET /DrugCategories", "QueryStringDeterminer", "Messenger", "DrugCategories", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /Drugs", "GET /Drugs", "QueryStringDeterminer", "Messenger", "Drugs", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /Enrollments", "GET /Enrollments", "QueryStringDeterminer", "Messenger", "DefaultController", "Enrollments", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /default", "GET /default", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "HomeController", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /EligibilityRouter", "GET /EligibilityRouter", "DefaultController", "QueryStringDeterminer", "EligibilityRouter", "Foo", "Messenger", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /Foos", "GET /Foos", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "Foos", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /OrderManager/OrderInquiry", "GET /OrderManager/OrderInquiry", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderInquiry"),
                    UrlTest("GET /OrderManager/OrderInquiry/OrderListBySite", "GET /OrderManager/OrderInquiry/OrderListBySite", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderInquiry", "OrderInquiryListBySite"),
                    UrlTest("GET /OrderManager/OrderRequest/DeleteBar", "GET /OrderManager/OrderRequest/DeleteBar", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderRequest", "StepWizard", "DeleteBar"),
                    UrlTest("GET /OrderManager/OrderRequest/ProcessBarDeletion", "GET /OrderManager/OrderRequest/ProcessBarDeletion", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderRequest", "StepWizard", "ProcessBarDeletion"),
                    UrlTest("GET /DrugSelector?{CategoryId}&{DrugId}", "GET /DrugSelector?{CategoryId}&{DrugId}", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "DrugSelector", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /EligibilityRouter", "GET /EligibilityRouter", "DefaultController", "QueryStringDeterminer", "EligibilityRouter", "Foo", "Messenger", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /OrderManager/OrderRequest/FooHistoryList", "GET /OrderManager/OrderRequest/FooHistoryList", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderRequest", "FooHistoryList", "StepWizard"),
                    UrlTest("GET /OrderManager/OrderRequest/FooInformation", "GET /OrderManager/OrderRequest/FooInformation", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderRequest", "StepWizard", "FooInformation"),
                    UrlTest("GET /OrderManager/OrderRequest", "GET /OrderManager/OrderRequest", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "StepWizard", "OrderRequest"),
                    UrlTest("GET /OrderRequestQueue", "GET /OrderRequestQueue", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "OrderRequestQueue", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /OrderManager/OrderRequest/PhysicianSelector?{PhysicianId}", "GET /OrderManager/OrderRequest/PhysicianSelector?{PhysicianId}", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "OrderRequest", "PhysicianSelector", "StepWizard"),
                    UrlTest("GET /OrderManager/OrderRequest", "GET /OrderManager/OrderRequest", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter", "StepWizard", "OrderRequest"),
                    UrlTest("GET /Physicians", "GET /Physicians", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "Physicians", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /Data14s", "GET /Data14s", "QueryStringDeterminer", "Messenger", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "Foos", "AntiForgeryRequestSetter"),
                    UrlTest("GET /QUnit", "GET /QUnit", "QueryStringDeterminer", "Messenger", "QUnit", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /RequestItems", "GET /RequestItems", "QueryStringDeterminer", "Messenger", "RequestItems", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("GET /", "GET /", "QueryStringDeterminer", "Messenger", "RootRedirect", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestSetter"),
                    UrlTest("POST /", "POST /", "QueryStringDeterminer", "Messenger", "RootRedirect", "DefaultController", "AjaxDeterminer", "Authenticator", "Fake", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /", "PUT /", "QueryStringDeterminer", "DefaultController", "RootRedirect", "Messenger", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /", "DELETE /", "QueryStringDeterminer", "DefaultController", "RootRedirect", "Messenger", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /bbb124", "GET /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestSetter"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /bbb124", "POST /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Fake", "Authenticator", "AjaxDeterminer", "AntiForgeryRequestHandler"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Messenger", "QueryStringDeterminer", "DefaultController", "Authenticator", "AjaxDeterminer", "Fake")

                    );
            #endregion

            SubSource1();

            SubSource2();

            SubSource3();

            return tests;
        }




    }
}
