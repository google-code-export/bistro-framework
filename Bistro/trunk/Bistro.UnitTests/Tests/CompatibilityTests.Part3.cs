using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.UnitTests.Tests
{
	public partial class CompatibilityTests
	{
		private void SubSource3()
		{
			#region tree - long path
			NewTestWithUrl(
				"tree - long path",
				Types(
					Type("Controller2", BindAttribute("/?")),
					Type(
					"Controller1",
					Attributes(
						BindAttribute("/default"),
						BindAttribute("/path2/more/more2"))
						)
					),
					UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("GET /bbb124", "GET /bbb124", "Controller2"),
					UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Controller2"),
					UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("POST /bbb124", "POST /bbb124", "Controller2"),
					UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Controller2"),
					UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("PUT /bbb124", "PUT /bbb124", "Controller2"),
					UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Controller2"),
					UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("DELETE /bbb124", "DELETE /bbb124", "Controller2"),
					UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Controller2"),
					UrlTest("HEAD /abcde/edcba/aaaa123/bbb124", "HEAD /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("HEAD /bbb124", "HEAD /bbb124", "Controller2"),
					UrlTest("HEAD /aaaa123/bbb124", "HEAD /aaaa123/bbb124", "Controller2"),
					UrlTest("GET /default", "GET /default", "Controller1", "Controller2"),
					UrlTest("POST /default", "POST /default", "Controller1", "Controller2"),
					UrlTest("PUT /default", "PUT /default", "Controller1", "Controller2"),
					UrlTest("DELETE /default", "DELETE /default", "Controller1", "Controller2"),
					UrlTest("HEAD /default", "HEAD /default", "Controller1", "Controller2"),
					UrlTest("GET /path2/more/more2", "GET /path2/more/more2", "Controller2", "Controller1"),
					UrlTest("POST /path2/more/more2", "POST /path2/more/more2", "Controller2", "Controller1"),
					UrlTest("PUT /path2/more/more2", "PUT /path2/more/more2", "Controller2", "Controller1"),
					UrlTest("DELETE /path2/more/more2", "DELETE /path2/more/more2", "Controller2", "Controller1"),
					UrlTest("HEAD /path2/more/more2", "HEAD /path2/more/more2", "Controller2", "Controller1")
				//Node("* /?", "Controller2"),
				//Node("* /default", "Controller1", "Controller2"),
				//Node("* /path2/more/more2", "Controller1", "Controller2")
				);
			#endregion

			#region tree - long path break in
			NewTestWithUrl(
				"tree - long path break in",
				Types(
					Type("Controller2", BindAttribute("/?")),
					Type(
					"Controller1",
					Attributes(
						BindAttribute("/default"),
						BindAttribute("/path2/more/more2"))
						),
					Type("Controller3", BindAttribute("/path2/more"))
					),
					UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("GET /bbb124", "GET /bbb124", "Controller2"),
					UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Controller2"),
					UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("POST /bbb124", "POST /bbb124", "Controller2"),
					UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Controller2"),
					UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("PUT /bbb124", "PUT /bbb124", "Controller2"),
					UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Controller2"),
					UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("DELETE /bbb124", "DELETE /bbb124", "Controller2"),
					UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Controller2"),
					UrlTest("HEAD /abcde/edcba/aaaa123/bbb124", "HEAD /abcde/edcba/aaaa123/bbb124", "Controller2"),
					UrlTest("HEAD /bbb124", "HEAD /bbb124", "Controller2"),
					UrlTest("HEAD /aaaa123/bbb124", "HEAD /aaaa123/bbb124", "Controller2"),
					UrlTest("GET /default", "GET /default", "Controller1", "Controller2"),
					UrlTest("POST /default", "POST /default", "Controller1", "Controller2"),
					UrlTest("PUT /default", "PUT /default", "Controller1", "Controller2"),
					UrlTest("DELETE /default", "DELETE /default", "Controller1", "Controller2"),
					UrlTest("HEAD /default", "HEAD /default", "Controller1", "Controller2"),
					UrlTest("GET /path2/more/more2", "GET /path2/more/more2", "Controller2", "Controller3", "Controller1"),
					UrlTest("POST /path2/more/more2", "POST /path2/more/more2", "Controller2", "Controller3", "Controller1"),
					UrlTest("PUT /path2/more/more2", "PUT /path2/more/more2", "Controller2", "Controller3", "Controller1"),
					UrlTest("DELETE /path2/more/more2", "DELETE /path2/more/more2", "Controller2", "Controller3", "Controller1"),
					UrlTest("HEAD /path2/more/more2", "HEAD /path2/more/more2", "Controller2", "Controller3", "Controller1"),
					UrlTest("GET /path2/more", "GET /path2/more", "Controller2", "Controller3"),
					UrlTest("POST /path2/more", "POST /path2/more", "Controller2", "Controller3"),
					UrlTest("PUT /path2/more", "PUT /path2/more", "Controller2", "Controller3"),
					UrlTest("DELETE /path2/more", "DELETE /path2/more", "Controller2", "Controller3"),
					UrlTest("HEAD /path2/more", "HEAD /path2/more", "Controller2", "Controller3")
				//Node("* /?", "Controller2"),
				//Node("* /default", "Controller1", "Controller2"),
				//Node("* /path2/more", Controllers("Controller3", "Controller2"),
				//Node("/more2", "Controller3", "Controller1", "Controller2")
				//)
				);
			#endregion

            #region controller ordering - 1
            NewTestWithUrl(
                "controller ordering - 1",
                Types(
                    Type("Controller1", BindAttribute("/default")),
                    Type("Controller2", BindAttribute("/default"))
                    ),
                    UrlTest("GET /default", "GET /default", CtrUnOrdGrp("Controller2", "Controller1")),
                    UrlTest("POST /default", "POST /default", CtrUnOrdGrp("Controller2", "Controller1")),
                    UrlTest("PUT /default", "PUT /default", CtrUnOrdGrp("Controller2", "Controller1")),
                    UrlTest("DELETE /default", "DELETE /default", CtrUnOrdGrp("Controller2", "Controller1")),
                    UrlTest("HEAD /default", "HEAD /default", CtrUnOrdGrp("Controller2", "Controller1"))
                //Node("* /default", "Controller2", "Controller1")
                );
            #endregion

			#region controller ordering - 2
			NewTestWithUrl(
				"controller ordering - 2",
				Types(
					Type(
						"Controller1",
						Attributes(BindAttribute("/default")),
						Field("f1", "string", RequestAttribute)),
					Type("Controller2", Attributes(BindAttribute("/default")),
						Field("f1", "string", RequiresAttribute))
					),
					UrlTest("GET /default", "GET /default", "Controller1", "Controller2"),
					UrlTest("POST /default", "POST /default", "Controller1", "Controller2"),
					UrlTest("PUT /default", "PUT /default", "Controller1", "Controller2"),
					UrlTest("DELETE /default", "DELETE /default", "Controller1", "Controller2"),
					UrlTest("HEAD /default", "HEAD /default", "Controller1", "Controller2")
				//Node("* /default", "Controller1", "Controller2")
				);
			#endregion

			#region controller ordering - 3
			NewTestWithUrl(
				"controller ordering - 3",
				Types(
					Type(
						"Controller1",
						Attributes(BindAttribute("/default")),
						Field("f1", "string", RequiresAttribute)),
					Type("Controller2", Attributes(BindAttribute("/default")),
						Field("f1", "string", RequestAttribute))
					),
					UrlTest("GET /default", "GET /default", "Controller2", "Controller1"),
					UrlTest("POST /default", "POST /default", "Controller2", "Controller1"),
					UrlTest("PUT /default", "PUT /default", "Controller2", "Controller1"),
					UrlTest("DELETE /default", "DELETE /default", "Controller2", "Controller1"),
					UrlTest("HEAD /default", "HEAD /default", "Controller2", "Controller1")
				//Node("* /default", "Controller2", "Controller1")
				);
			#endregion

			#region controller ordering - 4
			NewTestWithUrl(
				"controller ordering - 4", // c1 -(f2)-> c3 ; c3 -(f1)-> c2 ; c4 -(f1)-> c2
				Types(
					Type("Controller1", Attributes(BindAttribute("/default")),
						Field("f2", "string", RequiresAttribute)),
					Type("Controller2", Attributes(BindAttribute("/default")),
						Field("f1", "string", RequestAttribute)),
					Type("Controller3", Attributes(BindAttribute("/default")),
						Field("f2", "string", RequestAttribute),
						Field("f1", "string", RequiresAttribute)
						),
					Type("Controller4", Attributes(BindAttribute("/default")),
						Field("f1", "string", RequiresAttribute))
					),
					UrlTest("GET /default", "GET /default", "Controller2", "Controller3", "Controller1", "Controller4"),
					UrlTest("POST /default", "POST /default", "Controller2", "Controller3", "Controller1", "Controller4"),
					UrlTest("PUT /default", "PUT /default", "Controller2", "Controller3", "Controller1", "Controller4"),
					UrlTest("DELETE /default", "DELETE /default", "Controller2", "Controller3", "Controller1", "Controller4"),
					UrlTest("HEAD /default", "HEAD /default", "Controller2", "Controller3", "Controller1", "Controller4")
				//Node("* /default", "Controller2", "Controller4", "Controller3", "Controller1")
				);
			#endregion

			#region controller root test
			NewTestWithUrl(
				"controller root test",
				Types(
					Type("Controller1", Attributes(BindAttribute("GET/"))),
					Type("Controller2", Attributes(BindAttribute("GET/?"))),
					Type("Controller3", Attributes(BindAttribute("GET/aaa")))
					),
					UrlTest("GET /", "GET /", CtrUnOrdGrp("Controller1","Controller2")),
					UrlTest("GET /bbb", "GET /bbb", "Controller2"),
					UrlTest("GET /aaa", "GET /aaa", CtrUnOrdGrp("Controller2", "Controller3"))

				);
			#endregion

			#region controller root test
			NewTestWithUrl(
				"controller root test - 2",
				Types(
					Type("Controller1", Attributes(BindAttribute("GET/data"))),
					Type("Controller2", Attributes(BindAttribute("GET/data/?"))),
					Type("Controller3", Attributes(BindAttribute("GET/data/aaa")))
					),
					UrlTest("GET /data", "GET /data", "Controller1"),
					UrlTest("GET /data/bbb", "GET /data/bbb", CtrUnOrdGrp("Controller1", "Controller2")),
					UrlTest("GET /data/aaa", "GET /data/aaa", CtrUnOrdGrp("Controller1", "Controller2", "Controller3"))

				);
			#endregion


        }
    }
}
