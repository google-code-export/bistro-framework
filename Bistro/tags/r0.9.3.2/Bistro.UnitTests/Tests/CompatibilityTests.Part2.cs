using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.UnitTests.Tests
{
    public partial class CompatibilityTests
    {
        private void SubSource2()
        {

            #region Imported - GET/hi/...
            NewTestWithUrl(
                "Imported - GET/hi/...",
                Types(
                    Type(
                        "hiController1",
                        BindAttribute("GET /hi/new/world/a")
                    ),
                    Type(
                        "hiController2",
                        BindAttribute("GET /hi/new/*/*/now")
                    ),
                    Type(
                        "hiController3",
                        BindAttribute("GET /hi/*/world/?/now")
                    ),
                    Type(
                        "hiController4",
                        BindAttribute("GET /hi/*/world/*/now")
                    ),
                    Type(
                        "hiController5",
                        BindAttribute("GET /hi/*/world/a/now")
                    ),
                    Type(
                        "hiController6",
                        BindAttribute("GET /hi/*/world/a")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /hi/*/world/a/*")
                    )
                ),
                UrlTest("special - GET /hi/old/world/a/notnow", "GET /hi/new/world/a/notnow", CtrUnOrdGrp("hiController6", "hiController1", "hiController7")),
                UrlTest("special - GET /hi/new/world/b/now", "GET /hi/new/world/b/now", CtrUnOrdGrp("hiController3", "hiController4", "hiController2")),
                UrlTest("special - GET /hi/new/world/a/now", "GET /hi/new/world/a/now", CtrUnOrdGrp("hiController3", "hiController6", "hiController1", "hiController7", "hiController5", "hiController2", "hiController4")),
                UrlTest("GET /hi/new/world/a", "GET /hi/new/world/a", CtrUnOrdGrp("hiController6", "hiController1")),
                UrlTest("GET /hi/new/aaaaa/aaaaa/now", "GET /hi/new/aaaaa/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/aaaaa/abcde/now", "GET /hi/new/aaaaa/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/aaaaa/testvalue/now", "GET /hi/new/aaaaa/testvalue/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/aaaaa/now", "GET /hi/new/abcde/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/abcde/now", "GET /hi/new/abcde/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/testvalue/now", "GET /hi/new/abcde/testvalue/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/aaaaa/now", "GET /hi/new/testvalue/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/abcde/now", "GET /hi/new/testvalue/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/testvalue/now", "GET /hi/new/testvalue/testvalue/now", "hiController2"),
                UrlTest("GET /hi/aaaaa/world/now", "GET /hi/aaaaa/world/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/bbb124/now", "GET /hi/aaaaa/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/aaaa123/bbb124/now", "GET /hi/aaaaa/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/now", "GET /hi/abcde/world/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/bbb124/now", "GET /hi/abcde/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaa123/bbb124/now", "GET /hi/abcde/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/now", "GET /hi/testvalue/world/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/bbb124/now", "GET /hi/testvalue/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaa123/bbb124/now", "GET /hi/testvalue/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/aaaaa/now", "GET /hi/aaaaa/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/abcde/now", "GET /hi/aaaaa/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/testvalue/now", "GET /hi/aaaaa/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaaa/now", "GET /hi/abcde/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/abcde/now", "GET /hi/abcde/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/testvalue/now", "GET /hi/abcde/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaaa/now", "GET /hi/testvalue/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/abcde/now", "GET /hi/testvalue/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/testvalue/now", "GET /hi/testvalue/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/a/now", "GET /hi/aaaaa/world/a/now", CtrUnOrdGrp("hiController6", "hiController3", "hiController7", "hiController4", "hiController5")),
                UrlTest("GET /hi/abcde/world/a/now", "GET /hi/abcde/world/a/now", CtrUnOrdGrp("hiController6", "hiController3", "hiController7", "hiController4", "hiController5")),
                UrlTest("GET /hi/testvalue/world/a/now", "GET /hi/testvalue/world/a/now", CtrUnOrdGrp("hiController6", "hiController3", "hiController7", "hiController4", "hiController5")),
                UrlTest("GET /hi/aaaaa/world/a", "GET /hi/aaaaa/world/a", "hiController6"),
                UrlTest("GET /hi/abcde/world/a", "GET /hi/abcde/world/a", "hiController6"),
                UrlTest("GET /hi/testvalue/world/a", "GET /hi/testvalue/world/a", "hiController6"),
                UrlTest("GET /hi/aaaaa/world/a/aaaaa", "GET /hi/aaaaa/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/abcde", "GET /hi/aaaaa/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/testvalue", "GET /hi/aaaaa/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/aaaaa", "GET /hi/abcde/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/abcde", "GET /hi/abcde/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/testvalue", "GET /hi/abcde/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/aaaaa", "GET /hi/testvalue/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/abcde", "GET /hi/testvalue/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/testvalue", "GET /hi/testvalue/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7"))

                //Node("GET /hi", Controllers(),
                //    Node("/*/world", Controllers(),
                //        Node("/*/now", "hiController4", "hiController3"),
                //        Node("/?/now", "hiController3"),
                //        Node("/a", Controllers("hiController7", "hiController6"),
                //            Node("/*", "hiController7", "hiController6"),
                //            Node("/now", "hiController7", "hiController6", "hiController5", "hiController4", "hiController3")
                //            )
                //        ),
                //    Node("/new", Controllers(),
                //        Node("/*/*/now", "hiController2"),
                //        Node("/world/a", "hiController7", "hiController6", "hiController1")
                //        )
                //    )
                );
            #endregion

            //We need more complicated tests - with complex url AND parameters to sort by.

            #region Imported - GET/hi/... - 0
            NewTestWithUrl(
                "Imported - GET/hi/... - 0",
                Types(
                    Type(
                        "hiController4",
                        BindAttribute("GET /hi/*/world/*/now")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /hi/*/world/a/*")
                    )
                ),
                UrlTest("special - GET /hi/aaaa/world/a/now", "GET /hi/aaaa/world/a/now", CtrUnOrdGrp("hiController4", "hiController7")),
                UrlTest("special - GET /hi/aaaa/world/a/now/aaa", "GET /hi/aaaa/world/a/now/aaa", CtrUnOrdGrp("hiController7", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/aaaaa/now", "GET /hi/aaaaa/world/aaaaa/now", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/abcde/now", "GET /hi/aaaaa/world/abcde/now", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/testvalue/now", "GET /hi/aaaaa/world/testvalue/now", "hiController4"),
                UrlTest("GET /hi/abcde/world/aaaaa/now", "GET /hi/abcde/world/aaaaa/now", "hiController4"),
                UrlTest("GET /hi/abcde/world/abcde/now", "GET /hi/abcde/world/abcde/now", "hiController4"),
                UrlTest("GET /hi/abcde/world/testvalue/now", "GET /hi/abcde/world/testvalue/now", "hiController4"),
                UrlTest("GET /hi/testvalue/world/aaaaa/now", "GET /hi/testvalue/world/aaaaa/now", "hiController4"),
                UrlTest("GET /hi/testvalue/world/abcde/now", "GET /hi/testvalue/world/abcde/now", "hiController4"),
                UrlTest("GET /hi/testvalue/world/testvalue/now", "GET /hi/testvalue/world/testvalue/now", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/a/aaaaa", "GET /hi/aaaaa/world/a/aaaaa", "hiController7"),
                UrlTest("GET /hi/aaaaa/world/a/abcde", "GET /hi/aaaaa/world/a/abcde", "hiController7"),
                UrlTest("GET /hi/aaaaa/world/a/testvalue", "GET /hi/aaaaa/world/a/testvalue", "hiController7"),
                UrlTest("GET /hi/abcde/world/a/aaaaa", "GET /hi/abcde/world/a/aaaaa", "hiController7"),
                UrlTest("GET /hi/abcde/world/a/abcde", "GET /hi/abcde/world/a/abcde", "hiController7"),
                UrlTest("GET /hi/abcde/world/a/testvalue", "GET /hi/abcde/world/a/testvalue", "hiController7"),
                UrlTest("GET /hi/testvalue/world/a/aaaaa", "GET /hi/testvalue/world/a/aaaaa", "hiController7"),
                UrlTest("GET /hi/testvalue/world/a/abcde", "GET /hi/testvalue/world/a/abcde", "hiController7"),
                UrlTest("GET /hi/testvalue/world/a/testvalue", "GET /hi/testvalue/world/a/testvalue", "hiController7")

                //Node("GET /hi/*/world", Controllers(),
                //    Node("/*/now", "hiController4"),
                //    Node("/a/*", "hiController7")
                //    )
                );
            #endregion

            #region Imported - GET/hi/... - 1
            NewTestWithUrl(
                "Imported - GET/hi/... - 1",
                Types(
                    Type(
                        "hiController2",
                        BindAttribute("GET /hi/new/*/*/now")
                    ),
                    Type(
                        "hiController3",
                        BindAttribute("GET /hi/*/world/?/now")
                    ),
                    Type(
                        "hiController4",
                        BindAttribute("GET /hi/*/world/*/now")
                    ),
                    Type(
                        "hiController6",
                        BindAttribute("GET /hi/*/world/a")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /hi/*/world/a/*")
                    )
                ),
                UrlTest("special - GET /hi/new/world/a1/a2/now", "GET /hi/new/world/a1/a2/now", "hiController3"),
                UrlTest("special - GET /hi/new/world/a/now", "GET /hi/new/world/a/now", CtrUnOrdGrp("hiController6", "hiController3", "hiController7", "hiController2", "hiController4")),
                UrlTest("special - GET /hi/new/world/bb/now", "GET /hi/new/world/bb/now", CtrUnOrdGrp("hiController3", "hiController4", "hiController2")),
                UrlTest("GET /hi/new/aaaaa/aaaaa/now", "GET /hi/new/aaaaa/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/aaaaa/abcde/now", "GET /hi/new/aaaaa/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/aaaaa/testvalue/now", "GET /hi/new/aaaaa/testvalue/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/aaaaa/now", "GET /hi/new/abcde/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/abcde/now", "GET /hi/new/abcde/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/testvalue/now", "GET /hi/new/abcde/testvalue/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/aaaaa/now", "GET /hi/new/testvalue/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/abcde/now", "GET /hi/new/testvalue/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/testvalue/now", "GET /hi/new/testvalue/testvalue/now", "hiController2"),
                UrlTest("GET /hi/aaaaa/world/now", "GET /hi/aaaaa/world/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/bbb124/now", "GET /hi/aaaaa/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/aaaa123/bbb124/now", "GET /hi/aaaaa/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/now", "GET /hi/abcde/world/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/bbb124/now", "GET /hi/abcde/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaa123/bbb124/now", "GET /hi/abcde/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/now", "GET /hi/testvalue/world/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/bbb124/now", "GET /hi/testvalue/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaa123/bbb124/now", "GET /hi/testvalue/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/aaaaa/now", "GET /hi/aaaaa/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/abcde/now", "GET /hi/aaaaa/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/testvalue/now", "GET /hi/aaaaa/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaaa/now", "GET /hi/abcde/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/abcde/now", "GET /hi/abcde/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/testvalue/now", "GET /hi/abcde/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaaa/now", "GET /hi/testvalue/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/abcde/now", "GET /hi/testvalue/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/testvalue/now", "GET /hi/testvalue/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/a", "GET /hi/aaaaa/world/a", "hiController6"),
                UrlTest("GET /hi/abcde/world/a", "GET /hi/abcde/world/a", "hiController6"),
                UrlTest("GET /hi/testvalue/world/a", "GET /hi/testvalue/world/a", "hiController6"),
                UrlTest("GET /hi/aaaaa/world/a/aaaaa", "GET /hi/aaaaa/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/abcde", "GET /hi/aaaaa/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/testvalue", "GET /hi/aaaaa/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/aaaaa", "GET /hi/abcde/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/abcde", "GET /hi/abcde/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/testvalue", "GET /hi/abcde/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/aaaaa", "GET /hi/testvalue/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/abcde", "GET /hi/testvalue/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/testvalue", "GET /hi/testvalue/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7"))

                //Node("GET /hi", Controllers(),
                //    Node("/*/world", Controllers(),
                //        Node("/*/now", "hiController4", "hiController3"),
                //        Node("/?/now", "hiController3"),
                //        Node("/a", Controllers("hiController7", "hiController6"),
                //            Node("/*", "hiController7", "hiController6")
                //            )
                //        ),
                //    Node("/new/*/*/now", "hiController2"
                //        )
                //    )
                );
            #endregion

            #region Imported - GET/hi/... - 2
            NewTestWithUrl(
                "Imported - GET/hi/... - 2",
                Types(
                    Type(
                        "hiController3",
                        BindAttribute("GET /hi/*/world/?/now")
                    ),
                    Type(
                        "hiController4",
                        BindAttribute("GET /hi/*/world/*/now")
                    ),
                    Type(
                        "hiController6",
                        BindAttribute("GET /hi/*/world/a")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /hi/*/world/a/*")
                    )
                ),
                UrlTest("special GET /hi/testvalue/world/a/bbb/ccc", "GET /hi/testvalue/world/a/bbb/ccc", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("special GET /hi/testvalue/world/a/bbb", "GET /hi/testvalue/world/a/bbb", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("special GET /hi/testvalue/world/a/now", "GET /hi/testvalue/world/a/now", CtrUnOrdGrp("hiController6", "hiController3", "hiController7", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/now", "GET /hi/aaaaa/world/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/bbb124/now", "GET /hi/aaaaa/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/aaaa123/bbb124/now", "GET /hi/aaaaa/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/now", "GET /hi/abcde/world/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/bbb124/now", "GET /hi/abcde/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaa123/bbb124/now", "GET /hi/abcde/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/now", "GET /hi/testvalue/world/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/bbb124/now", "GET /hi/testvalue/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaa123/bbb124/now", "GET /hi/testvalue/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/aaaaa/now", "GET /hi/aaaaa/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/abcde/now", "GET /hi/aaaaa/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/testvalue/now", "GET /hi/aaaaa/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaaa/now", "GET /hi/abcde/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/abcde/now", "GET /hi/abcde/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/testvalue/now", "GET /hi/abcde/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaaa/now", "GET /hi/testvalue/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/abcde/now", "GET /hi/testvalue/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/testvalue/now", "GET /hi/testvalue/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/a", "GET /hi/aaaaa/world/a", "hiController6"),
                UrlTest("GET /hi/abcde/world/a", "GET /hi/abcde/world/a", "hiController6"),
                UrlTest("GET /hi/testvalue/world/a", "GET /hi/testvalue/world/a", "hiController6"),
                UrlTest("GET /hi/aaaaa/world/a/aaaaa", "GET /hi/aaaaa/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/abcde", "GET /hi/aaaaa/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/testvalue", "GET /hi/aaaaa/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/aaaaa", "GET /hi/abcde/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/abcde", "GET /hi/abcde/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/testvalue", "GET /hi/abcde/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/aaaaa", "GET /hi/testvalue/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/abcde", "GET /hi/testvalue/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/testvalue", "GET /hi/testvalue/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7"))

                //Node("GET /hi/*/world", Controllers(),
                //        Node("/*/now", "hiController4", "hiController3"),
                //        Node("/?/now", "hiController3"),
                //        Node("/a", Controllers("hiController7", "hiController6"),
                //            Node("/*", "hiController7", "hiController6")//,
                //            )
                //    )
                );
            #endregion

            #region Imported - GET/hi/... - 3
            NewTestWithUrl(
                "Imported - GET/hi/... - 3",
                Types(
                    Type(
                        "hiController4",
                        BindAttribute("GET /*/now")
                    ),
                    Type(
                        "hiController6",
                        BindAttribute("GET /a")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /a/*")
                    )
                ),
                UrlTest("special - GET /a/now/aaa", "GET /a/now/aaa", CtrUnOrdGrp("hiController6", "hiController7", "hiController4")),
                UrlTest("special - GET /a/now", "GET /a/now", CtrUnOrdGrp("hiController6", "hiController7", "hiController4")),
                UrlTest("GET /aaaaa/now", "GET /aaaaa/now", "hiController4"),
                UrlTest("GET /abcde/now", "GET /abcde/now", "hiController4"),
                UrlTest("GET /testvalue/now", "GET /testvalue/now", "hiController4"),
                UrlTest("GET /a", "GET /a", "hiController6"),
                UrlTest("GET /a/aaaaa", "GET /a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /a/abcde", "GET /a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /a/testvalue", "GET /a/testvalue", CtrUnOrdGrp("hiController6", "hiController7"))

                //Node("GET /*/now", "hiController4"),
                //Node("GET /a", Controllers("hiController7", "hiController6"),
                //    Node("/*", "hiController7", "hiController6")
                //    )
                );
            #endregion

            #region Imported - GET/hi/... - 4
            NewTestWithUrl(
                "Imported - GET/hi/... - 4",
                Types(
                    Type(
                        "hiController4",
                        BindAttribute("GET /hi/*/world/*/now")
                    ),
                    Type(
                        "hiController6",
                        BindAttribute("GET /hi/*/world/a")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /hi/*/world/a/*")
                    )
                ),
                UrlTest("special - GET /hi/anyvalue/world/a/now/aaa", "GET /hi/anyvalue/world/a/now/aaa", CtrUnOrdGrp("hiController6", "hiController7", "hiController4")),
                UrlTest("special - GET /hi/anyvalue/world/a/now", "GET /hi/anyvalue/world/a/now", CtrUnOrdGrp("hiController6", "hiController7", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/aaaaa/now", "GET /hi/aaaaa/world/aaaaa/now", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/abcde/now", "GET /hi/aaaaa/world/abcde/now", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/testvalue/now", "GET /hi/aaaaa/world/testvalue/now", "hiController4"),
                UrlTest("GET /hi/abcde/world/aaaaa/now", "GET /hi/abcde/world/aaaaa/now", "hiController4"),
                UrlTest("GET /hi/abcde/world/abcde/now", "GET /hi/abcde/world/abcde/now", "hiController4"),
                UrlTest("GET /hi/abcde/world/testvalue/now", "GET /hi/abcde/world/testvalue/now", "hiController4"),
                UrlTest("GET /hi/testvalue/world/aaaaa/now", "GET /hi/testvalue/world/aaaaa/now", "hiController4"),
                UrlTest("GET /hi/testvalue/world/abcde/now", "GET /hi/testvalue/world/abcde/now", "hiController4"),
                UrlTest("GET /hi/testvalue/world/testvalue/now", "GET /hi/testvalue/world/testvalue/now", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/a", "GET /hi/aaaaa/world/a", "hiController6"),
                UrlTest("GET /hi/abcde/world/a", "GET /hi/abcde/world/a", "hiController6"),
                UrlTest("GET /hi/testvalue/world/a", "GET /hi/testvalue/world/a", "hiController6"),
                UrlTest("GET /hi/aaaaa/world/a/aaaaa", "GET /hi/aaaaa/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/abcde", "GET /hi/aaaaa/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/aaaaa/world/a/testvalue", "GET /hi/aaaaa/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/aaaaa", "GET /hi/abcde/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/abcde", "GET /hi/abcde/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/abcde/world/a/testvalue", "GET /hi/abcde/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/aaaaa", "GET /hi/testvalue/world/a/aaaaa", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/abcde", "GET /hi/testvalue/world/a/abcde", CtrUnOrdGrp("hiController6", "hiController7")),
                UrlTest("GET /hi/testvalue/world/a/testvalue", "GET /hi/testvalue/world/a/testvalue", CtrUnOrdGrp("hiController6", "hiController7"))
                //Node("GET /hi/*/world", Controllers(),
                //        Node("/*/now", "hiController4"),
                //        Node("/a", Controllers("hiController7", "hiController6"),
                //            Node("/*", "hiController7", "hiController6")
                //            )
                //    )
                );
            #endregion

            #region Imported - GET/hi/... - 5
            NewTestWithUrl(
                "Imported - GET/hi/... - 5",
                Types(
                    Type(
                        "hiController2",
                        BindAttribute("GET /hi/new/*/*/now")
                    ),
                    Type(
                        "hiController3",
                        BindAttribute("GET /hi/*/world/?/now")
                    ),
                    Type(
                        "hiController4",
                        BindAttribute("GET /hi/*/world/*/now")
                    ),
                    Type(
                        "hiController7",
                        BindAttribute("GET /hi/*/world/a/*")
                    )
                ),
                UrlTest("special - GET /hi/old/world/b/now", "GET /hi/old/world/b/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("special - GET /hi/old/world/a/now", "GET /hi/old/world/a/now", CtrUnOrdGrp("hiController3", "hiController7", "hiController4")),
                UrlTest("special - GET /hi/new/world/a/now", "GET /hi/new/world/a/now", CtrUnOrdGrp("hiController3", "hiController7", "hiController4", "hiController2")),
                UrlTest("special - GET /hi/new/world/testvalue/now", "GET /hi/new/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4", "hiController2")),
                UrlTest("GET /hi/new/aaaaa/aaaaa/now", "GET /hi/new/aaaaa/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/aaaaa/abcde/now", "GET /hi/new/aaaaa/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/aaaaa/testvalue/now", "GET /hi/new/aaaaa/testvalue/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/aaaaa/now", "GET /hi/new/abcde/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/abcde/now", "GET /hi/new/abcde/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/abcde/testvalue/now", "GET /hi/new/abcde/testvalue/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/aaaaa/now", "GET /hi/new/testvalue/aaaaa/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/abcde/now", "GET /hi/new/testvalue/abcde/now", "hiController2"),
                UrlTest("GET /hi/new/testvalue/testvalue/now", "GET /hi/new/testvalue/testvalue/now", "hiController2"),
                UrlTest("GET /hi/aaaaa/world/now", "GET /hi/aaaaa/world/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/aaaaa/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/bbb124/now", "GET /hi/aaaaa/world/bbb124/now", "hiController3", "hiController4"),
                UrlTest("GET /hi/aaaaa/world/aaaa123/bbb124/now", "GET /hi/aaaaa/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/now", "GET /hi/abcde/world/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/abcde/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/abcde/world/bbb124/now", "GET /hi/abcde/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaa123/bbb124/now", "GET /hi/abcde/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/now", "GET /hi/testvalue/world/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "GET /hi/testvalue/world/abcde/edcba/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/testvalue/world/bbb124/now", "GET /hi/testvalue/world/bbb124/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaa123/bbb124/now", "GET /hi/testvalue/world/aaaa123/bbb124/now", "hiController3"),
                UrlTest("GET /hi/aaaaa/world/aaaaa/now", "GET /hi/aaaaa/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/abcde/now", "GET /hi/aaaaa/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/testvalue/now", "GET /hi/aaaaa/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/aaaaa/now", "GET /hi/abcde/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/abcde/now", "GET /hi/abcde/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/abcde/world/testvalue/now", "GET /hi/abcde/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/aaaaa/now", "GET /hi/testvalue/world/aaaaa/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/abcde/now", "GET /hi/testvalue/world/abcde/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/testvalue/world/testvalue/now", "GET /hi/testvalue/world/testvalue/now", CtrUnOrdGrp("hiController3", "hiController4")),
                UrlTest("GET /hi/aaaaa/world/a/aaaaa", "GET /hi/aaaaa/world/a/aaaaa", "hiController7"),
                UrlTest("GET /hi/aaaaa/world/a/abcde", "GET /hi/aaaaa/world/a/abcde", "hiController7"),
                UrlTest("GET /hi/aaaaa/world/a/testvalue", "GET /hi/aaaaa/world/a/testvalue", "hiController7"),
                UrlTest("GET /hi/abcde/world/a/aaaaa", "GET /hi/abcde/world/a/aaaaa", "hiController7"),
                UrlTest("GET /hi/abcde/world/a/abcde", "GET /hi/abcde/world/a/abcde", "hiController7"),
                UrlTest("GET /hi/abcde/world/a/testvalue", "GET /hi/abcde/world/a/testvalue", "hiController7"),
                UrlTest("GET /hi/testvalue/world/a/aaaaa", "GET /hi/testvalue/world/a/aaaaa", "hiController7"),
                UrlTest("GET /hi/testvalue/world/a/abcde", "GET /hi/testvalue/world/a/abcde", "hiController7"),
                UrlTest("GET /hi/testvalue/world/a/testvalue", "GET /hi/testvalue/world/a/testvalue", "hiController7")
                //Node("GET /hi", Controllers(),
                //    Node("/*/world", Controllers(),
                //        Node("/*/now", "hiController4", "hiController3"),
                //        Node("/?/now", "hiController3"),
                //        Node("/a/*", "hiController7")
                //        ),
                //    Node("/new/*/*/now", "hiController2"
                //        )
                //    )
                );
            #endregion

            #region Imported - DependsOn/Requires
            NewTestWithUrl(
                "Imported - DependsOn/Requires",
                Types(
                    Type("DRController2",
                        Attributes(BindAttribute("GET /dependson/requires")),
                        Field("z", "int", RequestAttribute, RequiresAttribute)
                        ),
                    Type("DRController1",
                        Attributes(BindAttribute("GET /dependson/requires")),
                        Field("z", "int", RequestAttribute)
                        )
                    ),
                    UrlTest("GET /dependson/requires", "GET /dependson/requires", "DRController1", "DRController2")
                //Node("GET /dependson/requires", "DRController1", "DRController2") // Check for Verbs???
                );
            #endregion



            #region Imported - Paging
            NewTestWithUrl(
                "Imported - Paging",
                Types(
                    Type("DataRoot",
                        Attributes(BindAttribute("GET /data/?")),
                        Field("dataRoot", "Boolean", RequestAttribute)
                        ),
                    Type("Data14sData",
                        Attributes(
                            BindAttribute("GET /data/Data12/id/{Data12Id}/Data14s/id/{dataId}"),
                            BindAttribute("GET /data/Data12/id/{Data12Id}/Data14s/id/{dataId}/withpaging/{linesPerPage}/{pageNumber}")
                        ),
                        Field("dataRoot", "Boolean", RequestAttribute, RequiresAttribute),
                        Field("dataSource", "Boolean", RequestAttribute)
                        ),
                    Type("BlueCrossData14sData",
                        Attributes(
                            BindAttribute("GET /data/Data12/id/11/Data14s/id/{dataId}"),
                            BindAttribute("GET /data/Data12/id/11/Data14s/id/{dataId}/withpaging/{linesPerPage}/{pageNumber}")
                        ),
                        Field("dataSource", "Boolean", RequestAttribute, RequiresAttribute),
                        Field("dataSourceCustom", "Boolean", RequestAttribute),
                        Field("dataId", "int")
                        ),
                    Type("WithPaging",
                        Attributes(
                            BindAttribute("GET /data/?/withpaging/{linesPerPage}/{pageNumber}")
                        ),
                        Field("dataSource", "Boolean", RequestAttribute, DependsOnAttribute),
                        Field("dataSourceCustom", "Boolean", RequestAttribute, DependsOnAttribute),
                        Field("withPaging", "Boolean", RequestAttribute)
                        ),
                    Type("Data14sRender",
                        Attributes(
                            BindAttribute("GET /data/Data12/id/*/Data14s/id/*")
                        ),
                        Field("dataSource", "Boolean", RequestAttribute, RequiresAttribute),
                        Field("dataSourceCustom", "Boolean", RequestAttribute, DependsOnAttribute),
                        Field("withPaging", "Boolean", RequestAttribute, DependsOnAttribute)
                        )
                    ),
                    UrlTest("GET /data", "GET /data"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124", "GET /data/abcde/edcba/aaaa123/bbb124", "DataRoot"),
                    UrlTest("GET /data/bbb124", "GET /data/bbb124", "DataRoot"),
                    UrlTest("GET /data/aaaa123/bbb124", "GET /data/aaaa123/bbb124", "DataRoot"),
                    UrlTest("GET /data/Data12/id//Data14s/id/", "GET /data/Data12/id//Data14s/id/", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1", "GET /data/Data12/id//Data14s/id/variablevalue1", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423", "GET /data/Data12/id//Data14s/id/123412423", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue", "GET /data/Data12/id//Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/", "GET /data/Data12/id/variablevalue1/Data14s/id/", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/", "GET /data/Data12/id/123412423/Data14s/id/", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423", "GET /data/Data12/id/123412423/Data14s/id/123412423", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue", "GET /data/Data12/id/123412423/Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/", "GET /data/Data12/id/testvalue/Data14s/id/", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423", "GET /data/Data12/id/testvalue/Data14s/id/123412423", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging//", "GET /data/Data12/id//Data14s/id//withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging//variablevalue1", "GET /data/Data12/id//Data14s/id//withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging//123412423", "GET /data/Data12/id//Data14s/id//withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging//testvalue", "GET /data/Data12/id//Data14s/id//withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/", "GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/123412423", "GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/testvalue", "GET /data/Data12/id//Data14s/id//withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/123412423/", "GET /data/Data12/id//Data14s/id//withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/123412423/variablevalue1", "GET /data/Data12/id//Data14s/id//withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/123412423/123412423", "GET /data/Data12/id//Data14s/id//withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/123412423/testvalue", "GET /data/Data12/id//Data14s/id//withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/testvalue/", "GET /data/Data12/id//Data14s/id//withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/testvalue/variablevalue1", "GET /data/Data12/id//Data14s/id//withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/testvalue/123412423", "GET /data/Data12/id//Data14s/id//withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id//withpaging/testvalue/testvalue", "GET /data/Data12/id//Data14s/id//withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//variablevalue1", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//123412423", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//testvalue", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/123412423", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/testvalue", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/123412423", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "GET /data/Data12/id//Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging//", "GET /data/Data12/id//Data14s/id/123412423/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging//variablevalue1", "GET /data/Data12/id//Data14s/id/123412423/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging//123412423", "GET /data/Data12/id//Data14s/id/123412423/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging//testvalue", "GET /data/Data12/id//Data14s/id/123412423/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/", "GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/123412423", "GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/testvalue", "GET /data/Data12/id//Data14s/id/123412423/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/", "GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/variablevalue1", "GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/123412423", "GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/testvalue", "GET /data/Data12/id//Data14s/id/123412423/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/", "GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/variablevalue1", "GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/123412423", "GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/testvalue", "GET /data/Data12/id//Data14s/id/123412423/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging//", "GET /data/Data12/id//Data14s/id/testvalue/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging//variablevalue1", "GET /data/Data12/id//Data14s/id/testvalue/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging//123412423", "GET /data/Data12/id//Data14s/id/testvalue/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging//testvalue", "GET /data/Data12/id//Data14s/id/testvalue/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/123412423", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/variablevalue1", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/123412423", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/testvalue", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/123412423", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/testvalue", "GET /data/Data12/id//Data14s/id/testvalue/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//123412423", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id//withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/123412423/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/123412423", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/testvalue", "GET /data/Data12/id/variablevalue1/Data14s/id/testvalue/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging//", "GET /data/Data12/id/123412423/Data14s/id//withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging//variablevalue1", "GET /data/Data12/id/123412423/Data14s/id//withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging//123412423", "GET /data/Data12/id/123412423/Data14s/id//withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging//testvalue", "GET /data/Data12/id/123412423/Data14s/id//withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/", "GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/123412423", "GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/testvalue", "GET /data/Data12/id/123412423/Data14s/id//withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/", "GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/123412423", "GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/testvalue", "GET /data/Data12/id/123412423/Data14s/id//withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/", "GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/123412423", "GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/testvalue", "GET /data/Data12/id/123412423/Data14s/id//withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//123412423", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//testvalue", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/123412423", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "GET /data/Data12/id/123412423/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//123412423", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//testvalue", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/123412423", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/123412423", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/testvalue", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/123412423", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/testvalue", "GET /data/Data12/id/123412423/Data14s/id/123412423/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//123412423", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//testvalue", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/123412423", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/testvalue", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/123412423", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/testvalue", "GET /data/Data12/id/123412423/Data14s/id/testvalue/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging//", "GET /data/Data12/id/testvalue/Data14s/id//withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging//variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id//withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging//123412423", "GET /data/Data12/id/testvalue/Data14s/id//withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging//testvalue", "GET /data/Data12/id/testvalue/Data14s/id//withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/123412423", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/testvalue", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/123412423", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/testvalue", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/123412423", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/testvalue", "GET /data/Data12/id/testvalue/Data14s/id//withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//123412423", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//testvalue", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/123412423", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//123412423", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//testvalue", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/123412423", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/123412423", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/123412423", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/123412423/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//123412423", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//testvalue", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/123412423", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/123412423", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/testvalue/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/", "GET /data/Data12/id/11/Data14s/id/", "DataRoot", "Data14sData", "BlueCrossData14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1", "GET /data/Data12/id/11/Data14s/id/variablevalue1", "DataRoot", "Data14sData", "BlueCrossData14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423", "GET /data/Data12/id/11/Data14s/id/123412423", "DataRoot", "Data14sData", "BlueCrossData14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue", "GET /data/Data12/id/11/Data14s/id/testvalue", "DataRoot", "Data14sData", "BlueCrossData14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging//", "GET /data/Data12/id/11/Data14s/id//withpaging//", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging//variablevalue1", "GET /data/Data12/id/11/Data14s/id//withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging//123412423", "GET /data/Data12/id/11/Data14s/id//withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging//testvalue", "GET /data/Data12/id/11/Data14s/id//withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/", "GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/123412423", "GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/testvalue", "GET /data/Data12/id/11/Data14s/id//withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/123412423/", "GET /data/Data12/id/11/Data14s/id//withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/123412423/variablevalue1", "GET /data/Data12/id/11/Data14s/id//withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/123412423/123412423", "GET /data/Data12/id/11/Data14s/id//withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/123412423/testvalue", "GET /data/Data12/id/11/Data14s/id//withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/", "GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/variablevalue1", "GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/123412423", "GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/testvalue", "GET /data/Data12/id/11/Data14s/id//withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//variablevalue1", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//123412423", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//testvalue", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/123412423", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "GET /data/Data12/id/11/Data14s/id/variablevalue1/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging//", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging//", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging//variablevalue1", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging//123412423", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging//testvalue", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/123412423", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/variablevalue1", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/123412423", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/testvalue", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/123412423", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/testvalue", "GET /data/Data12/id/11/Data14s/id/123412423/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//variablevalue1", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//123412423", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//testvalue", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging//testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/variablevalue1/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/123412423", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/testvalue", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/123412423/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/variablevalue1", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/123412423", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/123412423", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/testvalue", "GET /data/Data12/id/11/Data14s/id/testvalue/withpaging/testvalue/testvalue", "DataRoot", "Data14sData", "Data14sData", "BlueCrossData14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender"),
                    UrlTest("GET /data/withpaging//", "GET /data/withpaging//", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging//variablevalue1", "GET /data/withpaging//variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging//123412423", "GET /data/withpaging//123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging//testvalue", "GET /data/withpaging//testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/variablevalue1/", "GET /data/withpaging/variablevalue1/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/variablevalue1/variablevalue1", "GET /data/withpaging/variablevalue1/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/variablevalue1/123412423", "GET /data/withpaging/variablevalue1/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/variablevalue1/testvalue", "GET /data/withpaging/variablevalue1/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/123412423/", "GET /data/withpaging/123412423/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/123412423/variablevalue1", "GET /data/withpaging/123412423/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/123412423/123412423", "GET /data/withpaging/123412423/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/123412423/testvalue", "GET /data/withpaging/123412423/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/testvalue/", "GET /data/withpaging/testvalue/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/testvalue/variablevalue1", "GET /data/withpaging/testvalue/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/testvalue/123412423", "GET /data/withpaging/testvalue/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/withpaging/testvalue/testvalue", "GET /data/withpaging/testvalue/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging//", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging//", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging//variablevalue1", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging//variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging//123412423", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging//123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging//testvalue", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging//testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/variablevalue1", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/123412423", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/testvalue", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/variablevalue1/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/variablevalue1", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/123412423", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/testvalue", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/123412423/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/variablevalue1", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/123412423", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/testvalue", "GET /data/abcde/edcba/aaaa123/bbb124/withpaging/testvalue/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging//", "GET /data/bbb124/withpaging//", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging//variablevalue1", "GET /data/bbb124/withpaging//variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging//123412423", "GET /data/bbb124/withpaging//123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging//testvalue", "GET /data/bbb124/withpaging//testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/variablevalue1/", "GET /data/bbb124/withpaging/variablevalue1/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/variablevalue1/variablevalue1", "GET /data/bbb124/withpaging/variablevalue1/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/variablevalue1/123412423", "GET /data/bbb124/withpaging/variablevalue1/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/variablevalue1/testvalue", "GET /data/bbb124/withpaging/variablevalue1/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/123412423/", "GET /data/bbb124/withpaging/123412423/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/123412423/variablevalue1", "GET /data/bbb124/withpaging/123412423/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/123412423/123412423", "GET /data/bbb124/withpaging/123412423/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/123412423/testvalue", "GET /data/bbb124/withpaging/123412423/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/testvalue/", "GET /data/bbb124/withpaging/testvalue/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/testvalue/variablevalue1", "GET /data/bbb124/withpaging/testvalue/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/testvalue/123412423", "GET /data/bbb124/withpaging/testvalue/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/bbb124/withpaging/testvalue/testvalue", "GET /data/bbb124/withpaging/testvalue/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging//", "GET /data/aaaa123/bbb124/withpaging//", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging//variablevalue1", "GET /data/aaaa123/bbb124/withpaging//variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging//123412423", "GET /data/aaaa123/bbb124/withpaging//123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging//testvalue", "GET /data/aaaa123/bbb124/withpaging//testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/variablevalue1/", "GET /data/aaaa123/bbb124/withpaging/variablevalue1/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/variablevalue1/variablevalue1", "GET /data/aaaa123/bbb124/withpaging/variablevalue1/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/variablevalue1/123412423", "GET /data/aaaa123/bbb124/withpaging/variablevalue1/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/variablevalue1/testvalue", "GET /data/aaaa123/bbb124/withpaging/variablevalue1/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/123412423/", "GET /data/aaaa123/bbb124/withpaging/123412423/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/123412423/variablevalue1", "GET /data/aaaa123/bbb124/withpaging/123412423/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/123412423/123412423", "GET /data/aaaa123/bbb124/withpaging/123412423/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/123412423/testvalue", "GET /data/aaaa123/bbb124/withpaging/123412423/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/testvalue/", "GET /data/aaaa123/bbb124/withpaging/testvalue/", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/testvalue/variablevalue1", "GET /data/aaaa123/bbb124/withpaging/testvalue/variablevalue1", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/testvalue/123412423", "GET /data/aaaa123/bbb124/withpaging/testvalue/123412423", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/aaaa123/bbb124/withpaging/testvalue/testvalue", "GET /data/aaaa123/bbb124/withpaging/testvalue/testvalue", "DataRoot", "WithPaging"),
                    UrlTest("GET /data/Data12/id/aaaaa/Data14s/id/aaaaa", "GET /data/Data12/id/aaaaa/Data14s/id/aaaaa", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/aaaaa/Data14s/id/abcde", "GET /data/Data12/id/aaaaa/Data14s/id/abcde", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/aaaaa/Data14s/id/testvalue", "GET /data/Data12/id/aaaaa/Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/abcde/Data14s/id/aaaaa", "GET /data/Data12/id/abcde/Data14s/id/aaaaa", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/abcde/Data14s/id/abcde", "GET /data/Data12/id/abcde/Data14s/id/abcde", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/abcde/Data14s/id/testvalue", "GET /data/Data12/id/abcde/Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/aaaaa", "GET /data/Data12/id/testvalue/Data14s/id/aaaaa", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/abcde", "GET /data/Data12/id/testvalue/Data14s/id/abcde", "DataRoot", "Data14sData", "Data14sRender"),
                    UrlTest("GET /data/Data12/id/testvalue/Data14s/id/testvalue", "GET /data/Data12/id/testvalue/Data14s/id/testvalue", "DataRoot", "Data14sData", "Data14sRender")
                //Node("GET /data", Controllers(),
                //    Node("/?", Controllers("DataRoot"),
                //        Node("/withpaging/{linesPerPage}/{pageNumber}", Controllers("WithPaging", "DataRoot"))),
                //    Node("/Data12/id", Controllers(),
                //        Node("/*/Data14s/id/*", Controllers("DataRoot", "Data14sData", "Data14sRender")),
                //        Node("/{Data12Id}/Data14s/id/{dataId}", Controllers("DataRoot", "Data14sData", "Data14sRender"),
                //            Node("/withpaging/{linesPerPage}/{pageNumber}", Controllers("DataRoot", "Data14sData", "WithPaging", "Data14sRender"))),
                //        Node("/11/Data14s/id/{dataId}", Controllers("DataRoot", "Data14sData", "BlueCrossData14sData", "Data14sRender"),
                //            Node("/withpaging/{linesPerPage}/{pageNumber}", Controllers("DataRoot", "Data14sData", "BlueCrossData14sData", "WithPaging", "Data14sRender")))
                //        )
                //    )
                    );
            #endregion

            #region Imported - Paging-simple
            NewTestWithUrl(
                "Imported - Paging-simple",
                Types(
                    Type("DataRoot",
                        Attributes(BindAttribute("GET /a/?"))
                        ),
                    Type("WithPaging",
                        Attributes(
                            BindAttribute("GET /a/?/c3")
                        )
                        ),
                    Type("Data14sRender",
                        Attributes(
                            BindAttribute("GET /a/b/c2")
                        )
                        ),
                    Type("Data14sRenderBis",
                        Attributes(
                            BindAttribute("GET /a/b/c1")
                        )
                        )
                    ),
                    UrlTest("special - GET /a/b/c2/c3", "GET /a/b/c2/c3", CtrUnOrdGrp("WithPaging", "DataRoot", "Data14sRender")),
                    UrlTest("GET /a", "GET /a"),
                    UrlTest("GET /a/abcde/edcba/aaaa123/bbb124", "GET /a/abcde/edcba/aaaa123/bbb124", "DataRoot"),
                    UrlTest("GET /a/bbb124", "GET /a/bbb124", "DataRoot"),
                    UrlTest("GET /a/aaaa123/bbb124", "GET /a/aaaa123/bbb124", "DataRoot"),
                    UrlTest("GET /a/c3", "GET /a/c3", "WithPaging", "DataRoot"),
                    UrlTest("GET /a/abcde/edcba/aaaa123/bbb124/c3", "GET /a/abcde/edcba/aaaa123/bbb124/c3", "WithPaging", "DataRoot"),
                    UrlTest("GET /a/bbb124/c3", "GET /a/bbb124/c3", "WithPaging", "DataRoot"),
                    UrlTest("GET /a/aaaa123/bbb124/c3", "GET /a/aaaa123/bbb124/c3", "WithPaging", "DataRoot"),
                    UrlTest("GET /a/b/c2", "GET /a/b/c2", "DataRoot", "Data14sRender"),
                    UrlTest("GET /a/b/c1", "GET /a/b/c1", "DataRoot", "Data14sRenderBis")

                    //Node("GET /a", Controllers(),// is empty because it's not a method node
                //    Node("/?", Controllers("DataRoot"),
                //        Node("/c3", Controllers("WithPaging", "DataRoot"))),
                //    Node("/b", Controllers(),// is empty because it's not a method node
                //        Node("/c2", Controllers("Data14sRender", "DataRoot")),
                //        Node("/c1", Controllers("Data14sRenderBis", "DataRoot"))
                //        )
                //    )
                );
            #endregion

            #region tree - single controller
            NewTestWithUrl(
                "tree - single controller",
                Types(Type("Controller1", BindAttribute("/?"))),
                UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "Controller1"),
                UrlTest("GET /bbb124", "GET /bbb124", "Controller1"),
                UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "Controller1"),
                UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "Controller1"),
                UrlTest("POST /bbb124", "POST /bbb124", "Controller1"),
                UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "Controller1"),
                UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "Controller1"),
                UrlTest("PUT /bbb124", "PUT /bbb124", "Controller1"),
                UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "Controller1"),
                UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "Controller1"),
                UrlTest("DELETE /bbb124", "DELETE /bbb124", "Controller1"),
                UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "Controller1"),
                UrlTest("HEAD /abcde/edcba/aaaa123/bbb124", "HEAD /abcde/edcba/aaaa123/bbb124", "Controller1"),
                UrlTest("HEAD /bbb124", "HEAD /bbb124", "Controller1"),
                UrlTest("HEAD /aaaa123/bbb124", "HEAD /aaaa123/bbb124", "Controller1")

//                Node("* /?", Controller("Controller1", 1))
            );
            #endregion

            #region tree - one controller - 3 bindings (flat)
            NewTestWithUrl(
                "tree - one controller - 3 bindings (flat)",
                Types(Type(
                    "Controller1",
                    Attributes(
                        BindAttribute("/default"),
                        BindAttribute("/path1"),
                        BindAttribute("/path2/more"))
                    )),
                    UrlTest("GET /default", "GET /default", "Controller1"),
                    UrlTest("POST /default", "POST /default", "Controller1"),
                    UrlTest("PUT /default", "PUT /default", "Controller1"),
                    UrlTest("DELETE /default", "DELETE /default", "Controller1"),
                    UrlTest("HEAD /default", "HEAD /default", "Controller1"),
                    UrlTest("GET /path1", "GET /path1", "Controller1"),
                    UrlTest("POST /path1", "POST /path1", "Controller1"),
                    UrlTest("PUT /path1", "PUT /path1", "Controller1"),
                    UrlTest("DELETE /path1", "DELETE /path1", "Controller1"),
                    UrlTest("HEAD /path1", "HEAD /path1", "Controller1"),
                    UrlTest("GET /path2/more", "GET /path2/more", "Controller1"),
                    UrlTest("POST /path2/more", "POST /path2/more", "Controller1"),
                    UrlTest("PUT /path2/more", "PUT /path2/more", "Controller1"),
                    UrlTest("DELETE /path2/more", "DELETE /path2/more", "Controller1"),
                    UrlTest("HEAD /path2/more", "HEAD /path2/more", "Controller1")

                //Node("* /default", "Controller1"),
                //Node("* /path1", "Controller1"),
                //Node("* /path2/more", "Controller1")
                );
            #endregion

            #region tree - one controller - 3 bindings (tree) - DUPLICATE CONTROLLERS
            NewTestWithUrl(
				"tree - one controller - 3 bindings (tree) - DUPLICATE CONTROLLERS",
                Types(Type(
                    "Controller1",
                    Attributes(
                        BindAttribute("/default"),
                        BindAttribute("/path2"),
                        BindAttribute("/path2/more"))
                    )),
                    UrlTest("GET /default", "GET /default", "Controller1"),
                    UrlTest("POST /default", "POST /default", "Controller1"),
                    UrlTest("PUT /default", "PUT /default", "Controller1"),
                    UrlTest("DELETE /default", "DELETE /default", "Controller1"),
                    UrlTest("HEAD /default", "HEAD /default", "Controller1"),
                    UrlTest("GET /path2", "GET /path2", "Controller1"),
                    UrlTest("POST /path2", "POST /path2", "Controller1"),
                    UrlTest("PUT /path2", "PUT /path2", "Controller1"),
                    UrlTest("DELETE /path2", "DELETE /path2", "Controller1"),
                    UrlTest("HEAD /path2", "HEAD /path2", "Controller1"),
                    UrlTest("GET /path2/more", "GET /path2/more", "Controller1", "Controller1"),
                    UrlTest("POST /path2/more", "POST /path2/more", "Controller1", "Controller1"),
                    UrlTest("PUT /path2/more", "PUT /path2/more", "Controller1", "Controller1"),
                    UrlTest("DELETE /path2/more", "DELETE /path2/more", "Controller1", "Controller1"),
                    UrlTest("HEAD /path2/more", "HEAD /path2/more", "Controller1", "Controller1")

                //Node("* /default", "Controller1"),
                //Node("* /path2", Controllers("Controller1"),
                //    Node("/more", "Controller1")
                //    )
                );
            #endregion

            #region tree - one generic one specific - DUPLICATE CONTROLLERS
            NewTestWithUrl(
				"tree - one generic one specific - DUPLICATE CONTROLLERS",
                Types(Type(
                    "Controller1",
                    Attributes(
                        BindAttribute("/default"),
                        BindAttribute("/path2"),
                        BindAttribute("/path2/more"))
                        ),
                    Type("Controller2", BindAttribute("/?"))
                    ),
                    UrlTest("GET /default", "GET /default", "Controller2", "Controller1"),
                    UrlTest("POST /default", "POST /default", "Controller2", "Controller1"),
                    UrlTest("PUT /default", "PUT /default", "Controller2", "Controller1"),
                    UrlTest("DELETE /default", "DELETE /default", "Controller2", "Controller1"),
                    UrlTest("HEAD /default", "HEAD /default", "Controller2", "Controller1"),
                    UrlTest("GET /path2", "GET /path2", "Controller2", "Controller1"),
                    UrlTest("POST /path2", "POST /path2", "Controller2", "Controller1"),
                    UrlTest("PUT /path2", "PUT /path2", "Controller2", "Controller1"),
                    UrlTest("DELETE /path2", "DELETE /path2", "Controller2", "Controller1"),
                    UrlTest("HEAD /path2", "HEAD /path2", "Controller2", "Controller1"),
                    UrlTest("GET /path2/more", "GET /path2/more", "Controller2", "Controller1", "Controller1"),
                    UrlTest("POST /path2/more", "POST /path2/more", "Controller2", "Controller1", "Controller1"),
                    UrlTest("PUT /path2/more", "PUT /path2/more", "Controller2", "Controller1", "Controller1"),
                    UrlTest("DELETE /path2/more", "DELETE /path2/more", "Controller2", "Controller1", "Controller1"),
                    UrlTest("HEAD /path2/more", "HEAD /path2/more", "Controller2", "Controller1", "Controller1"),
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
                    UrlTest("HEAD /aaaa123/bbb124", "HEAD /aaaa123/bbb124", "Controller2")
                //Node("* /default", "Controller2", "Controller1"),
                //Node("* /path2", Controllers("Controller2", "Controller1"),
                //    Node("/more", "Controller2", "Controller1")
                //    ),
                //Node("* /?", "Controller2")
                );
            #endregion


            // Note that there's questionmark here, without leading slash
            #region tree - one generic one specific - reversed - DUPLICATE CONTROLLERS
            NewTestWithUrl(
				"tree - one generic one specific - reversed - DUPLICATE CONTROLLERS",
                Types(
                    Type("Controller2", BindAttribute("?")),
                    Type(
                    "Controller1",
                    Attributes(
                        BindAttribute("/default"),
                        BindAttribute("/path2"),
                        BindAttribute("/path2/more"))
                        )
                    ),
                    UrlTest("test special", "GET /anytestpath", "Controller2"),
                    UrlTest("GET /default", "GET /default", "Controller1", "Controller2"),
                    UrlTest("POST /default", "POST /default", "Controller1", "Controller2"),
                    UrlTest("PUT /default", "PUT /default", "Controller1", "Controller2"),
                    UrlTest("DELETE /default", "DELETE /default", "Controller1", "Controller2"),
                    UrlTest("HEAD /default", "HEAD /default", "Controller1", "Controller2"),
                    UrlTest("GET /path2", "GET /path2", "Controller1", "Controller2"),
                    UrlTest("POST /path2", "POST /path2", "Controller1", "Controller2"),
                    UrlTest("PUT /path2", "PUT /path2", "Controller1", "Controller2"),
                    UrlTest("DELETE /path2", "DELETE /path2", "Controller1", "Controller2"),
                    UrlTest("HEAD /path2", "HEAD /path2", "Controller1", "Controller2"),
                    UrlTest("GET /path2/more", "GET /path2/more", "Controller1", "Controller2", "Controller1"),
                    UrlTest("POST /path2/more", "POST /path2/more", "Controller1", "Controller2", "Controller1"),
                    UrlTest("PUT /path2/more", "PUT /path2/more", "Controller1", "Controller2", "Controller1"),
                    UrlTest("DELETE /path2/more", "DELETE /path2/more", "Controller1", "Controller2", "Controller1"),
                    UrlTest("HEAD /path2/more", "HEAD /path2/more", "Controller1", "Controller2", "Controller1")
                //Node("* ?", "Controller2"),
                //Node("* /default", "Controller1", "Controller2"),
                //Node("* /path2", Controllers("Controller1", "Controller2"),
                //    Node("/more", "Controller1", "Controller2")
                //    )
                );
            #endregion

            #region Cross-bindings
            NewTestWithUrl(
                "Cross-bindings",
                Types(
                    Type("Controller1", BindAttribute("/a/*/b/c")),
                    Type("Controller2", BindAttribute("/a/z/*/c"))
                    ),
                    UrlTest("GET /a/bbb/b/c", "GET /a/bbb/b/c", "Controller1"),
                    UrlTest("GET /a/z/bbb/c", "GET /a/z/bbb/c", "Controller2"),
                    UrlTest("GET /a/z/b/c", "GET /a/z/b/c", CtrUnOrdGrp("Controller2", "Controller1")),
                    UrlTest("GET /a/bbb/b/c/tail", "GET /a/bbb/b/c/tail", "Controller1"),
                    UrlTest("GET /a/z/bbb/c/tail", "GET /a/z/bbb/c/tail", "Controller2"),
                    UrlTest("GET /a/z/b/c/tail", "GET /a/z/b/c/tail", CtrUnOrdGrp("Controller2", "Controller1"))
                    );


            #endregion



        }

    }
}
