using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.UnitTests.Tests
{
    public partial class CompatibilityTests
    {
        private void SubSource1()
        {
            #region NoRecruiters-WFS - Big Test
            NewTestWithUrl("NoRecruiters-WFS - lacks currentUser",
                Types(
                    Type(
                        "RegisterDisplay",
                        BindAttribute("GET /auth/register")
                        ),
                    Type(
                        "Register",
                        Attributes(
                            BindAttribute("POST /auth/register")
                        ),
                        Field("username", "string", FormFieldAttribute, RequestAttribute),
                        Field("email", "string", FormFieldAttribute, RequestAttribute),
                        Field("firstname", "string", FormFieldAttribute, RequestAttribute),
                        Field("lastname", "string", FormFieldAttribute, RequestAttribute),
                        Field("password", "string", FormFieldAttribute, RequestAttribute),

                        Field("defaultContentType", "string", RequestAttribute, RequiresAttribute),

                        Field("errors", "Dictionary<string,string>", RequestAttribute)//Inherited

                        ),
                    Type(
                        "SignInDisplay",
                        BindAttribute("GET /auth/signin?{originalRequest}")
                        ),
                    Type(
                        "SignIn",
                        Attributes(
                            BindAttribute("POST /auth/signin")
                        ),
                        Field("username", "string", FormFieldAttribute, RequestAttribute),
                        Field("originalRequest", "string", FormFieldAttribute, RequestAttribute),

                        Field("password", "string", FormFieldAttribute),
                        Field("defaultContentType", "string", RequestAttribute),

                        Field("errors", "Dictionary<string,string>", RequestAttribute)//Inherited
                        ),
                    Type(
                        "SignOut",
                        BindAttribute("GET /auth/signout")
                        ),
                    Type(
                        "DataAccessControl",
                        Attributes(
                            BindAttribute("/?/byname/{shortName}"),
                            BindAttribute("/?/byId/{postingId}")
                        ),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),

                            Field("posting", "PostingStub", RequestAttribute)
                        ),
                    Type(
                        "CompanyFunctionAccessControl",
                        BindAttribute("GET /posting/manage"),
                        BindAttribute("GET /posting/ad/applicants/byId/{adId}")
                        ),
                    Type("GeneralFunctionAccessControl",
                        Attributes(
                            BindAttribute("/posting")
                        ),
                        Field("currentUser", "UserProfileStub", RequestAttribute)
                        ),

                    Type("DefaultController",
                        Attributes(
                            BindAttribute("/?")
                        ),
                        Field("userType", "string", RequestAttribute),
                        Field("root", "string", RequestAttribute),
                        Field("defaultContentType", "string", RequestAttribute)
                        ),

                    Type("ClearPreferences",
                        Attributes(
                            BindAttribute("/default/{preferenceReset}")
                        ),
                        Field("preferenceReset", "bool", RequestAttribute),
                        Field("defaultContentType", "string", RequestAttribute, RequiresAttribute)
                        ),

                    Type("Home",
                        Attributes(
                            BindAttribute("/default")
                        ),
                        Field("preferenceReset", "bool", RequestAttribute, DependsOnAttribute),
                        Field("defaultContentType", "string", RequestAttribute, RequiresAttribute)
                        ),

                    Type("Static",
                        Attributes(
                            BindAttribute("/static/{contentId}")
                        ),
                        Field("contentId", "string")
                        ),

                    Type("View",
                        Attributes(
                            BindAttribute("GET /ad/{shortName}"),
                            BindAttribute("GET /resume/{shortName}")
                        ),
                        Field("contentType", "string", RequestAttribute, DependsOnAttribute),
                        Field("posting", "PostingStub", RequestAttribute),
                        Field("defaultContentType", "string", RequestAttribute, RequiresAttribute),
                        Field("shortName", "string")
                        ),

                    Type("Untag",
                        Attributes(
                            BindAttribute("GET /?/without-tag/{tag}")
                        ),
                        Field("tag", "string"),
                        Field("currentTags", "List<string>", SessionAttribute)
                        ),

                    Type("Tag",
                        Attributes(
                            BindAttribute("GET /?/with-tag/{tagList}")
                        ),
                        Field("currentTags", "List<string>", SessionAttribute),
                        Field("tagList", "string")
                        ),
                    Type("FirstTimeSearch",
                        Attributes(
                            BindAttribute("GET /postings/{contentType}?{firstTime}")
                        ),
                        Field("contentType", "string"),
                        Field("firstTime", "bool"),
                        Field("defaultContentType", "string", RequestAttribute, RequiresAttribute)
                        ),
                    Type("Search",
                        Attributes(
                            BindAttribute("/postings/{contentType}")
                        ),
                        Field("txtQuery", "string", FormFieldAttribute),
                        Field("currentTags", "List<string>", SessionAttribute, DependsOnAttribute),
                        Field("popularTags", "List<string>", RequestAttribute),
                        Field("searchResults", "List<string>", RequestAttribute),
                        Field("contentType", "string", RequestAttribute)
                        ),

                    Type("Flag",
                        Attributes(
                            BindAttribute("/posting/flag/{contentType}/{flagType}/{shortName}")
                        ),
                        Field("flagType", "string"),
                        Field("contentType", "string"),
                        Field("shortName", "string")//,
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute)
                        ),

                    Type("ApplyDisplay",
                        Attributes(
                            BindAttribute("GET /posting/apply/{appContentType}/{shortName}")
                        ),
                        Field("appContentType", "string", RequestAttribute),
                        Field("shortName", "string", RequestAttribute)
                        ),

                    Type("ApplyController",
                        Attributes(
                            BindAttribute("POST /posting/apply/{appContentType}/{shortName}")
                        ),
                        Field("appContentType", "string", RequestAttribute),
                        Field("shortName", "string", RequestAttribute),
                        Field("comment", "string", RequestAttribute, FormFieldAttribute)//,
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute)
                        ),

                    Type("ResumeDisplay",
                        Attributes(
                            BindAttribute("GET /posting/resume/byname/{shortName}")
                        ),
                        Field("shortName", "string"),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),
                        Field("posting", "PostingStub", RequestAttribute, RequiresAttribute)
                        ),
                    Type("ResumeUpdate",
                        Attributes(
                            BindAttribute("POST /posting/resume/byname/{shortName}")
                        ),
                        Field("shortName", "string"),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),
                        Field("posting", "PostingStub", RequestAttribute, RequiresAttribute),

                        Field("heading", "string", FormFieldAttribute, RequestAttribute),
                        Field("tags", "string", FormFieldAttribute, RequestAttribute),
                        Field("detail", "string", FormFieldAttribute, RequestAttribute),
                        Field("published", "string", FormFieldAttribute, RequestAttribute)
                        ),
                    Type("PreviewDisplay",
                        Attributes(
                            BindAttribute("GET /posting/resume/preview/byname/{shortName}")
                        ),
                        Field("shortName", "string"),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),
                        Field("posting", "PostingStub", RequestAttribute, RequiresAttribute)
                        ),
                    Type("Manage",
                        Attributes(
                            BindAttribute("GET /posting/manage")
                        ),
                        Field("unpublished", "List<string>", RequestAttribute),
                        Field("published", "List<string>", RequestAttribute)//,

//                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute)
                        ),
                    Type("ViewAllApplicatints",
                        Attributes(
                            BindAttribute("GET /posting/ad/applicants/byId/{adId}")
                        ),
                        Field("adId", "string", RequestAttribute),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),
                        Field("applicants", "List<string>", RequestAttribute)
                        ),
                    Type("AdDisplay",
                        Attributes(
                            BindAttribute("GET /posting/ad/byname/{shortName}")
                        ),
                        Field("shortName", "string"),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),
                        Field("posting", "PostingStub", RequestAttribute, RequiresAttribute)
                        ),
                    Type("AdUpdate",
                        Attributes(
                            BindAttribute("POST /posting/ad/byname/{shortName}")
                        ),
                        Field("shortName", "string"),
                //                        Field("currentUser", "UserProfileStub", RequestAttribute, RequiresAttribute),
                        Field("posting", "PostingStub", RequestAttribute, RequiresAttribute),

                        Field("heading", "string", FormFieldAttribute, RequestAttribute),
                        Field("tags", "string", FormFieldAttribute, RequestAttribute),
                        Field("detail", "string", FormFieldAttribute, RequestAttribute),
                        Field("published", "string", FormFieldAttribute, RequestAttribute)
                        )

                    ),
                    UrlTest("badTest - GET /auth/signin/byname/ShortNameValue/without-tag/tagValue/with-tag/tagListValue/postings/ContentTypeValue?originalRequest=origReqValue&firstTime=true", "GET /auth/signin/byname/ShortNameValue/without-tag/tagValue/with-tag/tagListValue/postings/ContentTypeValue?originalRequest=origReqValue&firstTime=true", "DefaultController", CtrUnOrdGrp("SignInDisplay", "DataAccessControl"), CtrUnOrdGrp("Untag", "Tag")),
                    UrlTest("GET /auth/signin/byname/ShortNameValue?originalRequest=origReqValue", "GET /auth/signin/byname/ShortNameValue?originalRequest=origReqValue", "DefaultController", "SignInDisplay", "DataAccessControl"),
                    UrlTest("GET /auth/signin/byname?originalRequest=origReqValue", "GET /auth/signin/byname?originalRequest=origReqValue", "DefaultController", "DataAccessControl", "SignInDisplay"),

                    UrlTest("GET /postings/ContentTypeValue/byname/ShortNameValue?firstTime=true", "GET /postings/ContentTypeValue/byname/ShortNameValue?firstTime=true", "DefaultController", "FirstTimeSearch", "Search", "DataAccessControl"),
                    UrlTest("GET /postings/ContentTypeValue/byname?firstTime=true", "GET /postings/ContentTypeValue/byname?firstTime=true", "DefaultController", "DataAccessControl", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/ContentTypeValue/byname/?firstTime=true", "GET /postings/ContentTypeValue/byname/?firstTime=true", "DefaultController", "FirstTimeSearch", "Search", "DataAccessControl"),
                    UrlTest("GET /postings/byname/ShortNameValue?firstTime=true", "GET /postings/byname/ShortNameValue?firstTime=true", "DefaultController", "FirstTimeSearch", "Search", "DataAccessControl"),
                    UrlTest("GET /postings/byname?firstTime=true", "GET /postings/byname?firstTime=true", "DefaultController", "DataAccessControl", "Search", "FirstTimeSearch"),


                    UrlTest("special - GET /postings/contTypeValue?firstTime=true", "GET /postings/contTypeValue?firstTime=true", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("special - GET /postings/?firstTime=true", "GET /postings/?firstTime=true", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("special - GET /auth/signin?originalRequest=aaa", "GET /auth/signin?originalRequest=aaa", "DefaultController", "SignInDisplay"),
                    UrlTest("special - GET /auth/signin/aaa?originalRequest=aaa", "GET /auth/signin/aaa?originalRequest=aaa", "DefaultController", "SignInDisplay"),
                    UrlTest("GET /auth/register", "GET /auth/register", "DefaultController", "RegisterDisplay"),
                    UrlTest("POST /auth/register", "POST /auth/register", "DefaultController", "Register"),
                    UrlTest("GET /auth/signin?{originalRequest}", "GET /auth/signin?{originalRequest}", "DefaultController", "SignInDisplay"),
                    UrlTest("POST /auth/signin", "POST /auth/signin", "DefaultController", "SignIn"),
                    UrlTest("GET /auth/signout", "GET /auth/signout", "DefaultController", "SignOut"),
                    UrlTest("GET /byname/", "GET /byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byname/variablevalue1", "GET /byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byname/123412423", "GET /byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byname/testvalue", "GET /byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byname/", "GET /abcde/edcba/aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "GET /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byname/123412423", "GET /abcde/edcba/aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byname/testvalue", "GET /abcde/edcba/aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byname/", "GET /bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byname/variablevalue1", "GET /bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byname/123412423", "GET /bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byname/testvalue", "GET /bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byname/", "GET /aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byname/variablevalue1", "GET /aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byname/123412423", "GET /aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byname/testvalue", "GET /aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byname/", "POST /byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byname/variablevalue1", "POST /byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byname/123412423", "POST /byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byname/testvalue", "POST /byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byname/", "POST /abcde/edcba/aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "POST /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byname/123412423", "POST /abcde/edcba/aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byname/testvalue", "POST /abcde/edcba/aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byname/", "POST /bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byname/variablevalue1", "POST /bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byname/123412423", "POST /bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byname/testvalue", "POST /bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byname/", "POST /aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byname/variablevalue1", "POST /aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byname/123412423", "POST /aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byname/testvalue", "POST /aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byname/", "PUT /byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byname/variablevalue1", "PUT /byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byname/123412423", "PUT /byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byname/testvalue", "PUT /byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byname/", "PUT /abcde/edcba/aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "PUT /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byname/123412423", "PUT /abcde/edcba/aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byname/testvalue", "PUT /abcde/edcba/aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byname/", "PUT /bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byname/variablevalue1", "PUT /bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byname/123412423", "PUT /bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byname/testvalue", "PUT /bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byname/", "PUT /aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byname/variablevalue1", "PUT /aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byname/123412423", "PUT /aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byname/testvalue", "PUT /aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byname/", "DELETE /byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byname/variablevalue1", "DELETE /byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byname/123412423", "DELETE /byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byname/testvalue", "DELETE /byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byname/", "DELETE /abcde/edcba/aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "DELETE /abcde/edcba/aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byname/123412423", "DELETE /abcde/edcba/aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byname/testvalue", "DELETE /abcde/edcba/aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byname/", "DELETE /bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byname/variablevalue1", "DELETE /bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byname/123412423", "DELETE /bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byname/testvalue", "DELETE /bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byname/", "DELETE /aaaa123/bbb124/byname/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byname/variablevalue1", "DELETE /aaaa123/bbb124/byname/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byname/123412423", "DELETE /aaaa123/bbb124/byname/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byname/testvalue", "DELETE /aaaa123/bbb124/byname/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byId/", "GET /byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byId/variablevalue1", "GET /byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byId/123412423", "GET /byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /byId/testvalue", "GET /byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byId/", "GET /abcde/edcba/aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "GET /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byId/123412423", "GET /abcde/edcba/aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/byId/testvalue", "GET /abcde/edcba/aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byId/", "GET /bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byId/variablevalue1", "GET /bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byId/123412423", "GET /bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /bbb124/byId/testvalue", "GET /bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byId/", "GET /aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byId/variablevalue1", "GET /aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byId/123412423", "GET /aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /aaaa123/bbb124/byId/testvalue", "GET /aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byId/", "POST /byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byId/variablevalue1", "POST /byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byId/123412423", "POST /byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /byId/testvalue", "POST /byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byId/", "POST /abcde/edcba/aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "POST /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byId/123412423", "POST /abcde/edcba/aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124/byId/testvalue", "POST /abcde/edcba/aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byId/", "POST /bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byId/variablevalue1", "POST /bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byId/123412423", "POST /bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /bbb124/byId/testvalue", "POST /bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byId/", "POST /aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byId/variablevalue1", "POST /aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byId/123412423", "POST /aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("POST /aaaa123/bbb124/byId/testvalue", "POST /aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byId/", "PUT /byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byId/variablevalue1", "PUT /byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byId/123412423", "PUT /byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /byId/testvalue", "PUT /byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byId/", "PUT /abcde/edcba/aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "PUT /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byId/123412423", "PUT /abcde/edcba/aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124/byId/testvalue", "PUT /abcde/edcba/aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byId/", "PUT /bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byId/variablevalue1", "PUT /bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byId/123412423", "PUT /bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /bbb124/byId/testvalue", "PUT /bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byId/", "PUT /aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byId/variablevalue1", "PUT /aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byId/123412423", "PUT /aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("PUT /aaaa123/bbb124/byId/testvalue", "PUT /aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byId/", "DELETE /byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byId/variablevalue1", "DELETE /byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byId/123412423", "DELETE /byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /byId/testvalue", "DELETE /byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byId/", "DELETE /abcde/edcba/aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "DELETE /abcde/edcba/aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byId/123412423", "DELETE /abcde/edcba/aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124/byId/testvalue", "DELETE /abcde/edcba/aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byId/", "DELETE /bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byId/variablevalue1", "DELETE /bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byId/123412423", "DELETE /bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /bbb124/byId/testvalue", "DELETE /bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byId/", "DELETE /aaaa123/bbb124/byId/", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byId/variablevalue1", "DELETE /aaaa123/bbb124/byId/variablevalue1", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byId/123412423", "DELETE /aaaa123/bbb124/byId/123412423", "DefaultController", "DataAccessControl"),
                    UrlTest("DELETE /aaaa123/bbb124/byId/testvalue", "DELETE /aaaa123/bbb124/byId/testvalue", "DefaultController", "DataAccessControl"),
                    UrlTest("GET /posting/manage", "GET /posting/manage", "GeneralFunctionAccessControl", "DefaultController", "CompanyFunctionAccessControl", "Manage"),
                    UrlTest("GET /posting/ad/applicants/byId/", "GET /posting/ad/applicants/byId/", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/applicants/byId/variablevalue1", "GET /posting/ad/applicants/byId/variablevalue1", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/applicants/byId/123412423", "GET /posting/ad/applicants/byId/123412423", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/applicants/byId/testvalue", "GET /posting/ad/applicants/byId/testvalue", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting", "GET /posting", "DefaultController", "GeneralFunctionAccessControl"),
                    UrlTest("POST /posting", "POST /posting", "DefaultController", "GeneralFunctionAccessControl"),
                    UrlTest("PUT /posting", "PUT /posting", "DefaultController", "GeneralFunctionAccessControl"),
                    UrlTest("DELETE /posting", "DELETE /posting", "DefaultController", "GeneralFunctionAccessControl"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", "DefaultController"),
                    UrlTest("GET /bbb124", "GET /bbb124", "DefaultController"),
                    UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", "DefaultController"),
                    UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", "DefaultController"),
                    UrlTest("POST /bbb124", "POST /bbb124", "DefaultController"),
                    UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", "DefaultController"),
                    UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", "DefaultController"),
                    UrlTest("PUT /bbb124", "PUT /bbb124", "DefaultController"),
                    UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", "DefaultController"),
                    UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", "DefaultController"),
                    UrlTest("DELETE /bbb124", "DELETE /bbb124", "DefaultController"),
                    UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", "DefaultController"),
                    UrlTest("GET /default/", "GET /default/", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("GET /default/variablevalue1", "GET /default/variablevalue1", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("GET /default/123412423", "GET /default/123412423", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("GET /default/testvalue", "GET /default/testvalue", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("POST /default/", "POST /default/", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("POST /default/variablevalue1", "POST /default/variablevalue1", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("POST /default/123412423", "POST /default/123412423", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("POST /default/testvalue", "POST /default/testvalue", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("PUT /default/", "PUT /default/", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("PUT /default/variablevalue1", "PUT /default/variablevalue1", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("PUT /default/123412423", "PUT /default/123412423", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("PUT /default/testvalue", "PUT /default/testvalue", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("DELETE /default/", "DELETE /default/", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("DELETE /default/variablevalue1", "DELETE /default/variablevalue1", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("DELETE /default/123412423", "DELETE /default/123412423", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("DELETE /default/testvalue", "DELETE /default/testvalue", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("GET /default", "GET /default", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("POST /default", "POST /default", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("PUT /default", "PUT /default", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("DELETE /default", "DELETE /default", "DefaultController", "ClearPreferences", "Home"),
                    UrlTest("GET /static/", "GET /static/", "DefaultController", "Static"),
                    UrlTest("GET /static/variablevalue1", "GET /static/variablevalue1", "DefaultController", "Static"),
                    UrlTest("GET /static/123412423", "GET /static/123412423", "DefaultController", "Static"),
                    UrlTest("GET /static/testvalue", "GET /static/testvalue", "DefaultController", "Static"),
                    UrlTest("POST /static/", "POST /static/", "DefaultController", "Static"),
                    UrlTest("POST /static/variablevalue1", "POST /static/variablevalue1", "DefaultController", "Static"),
                    UrlTest("POST /static/123412423", "POST /static/123412423", "DefaultController", "Static"),
                    UrlTest("POST /static/testvalue", "POST /static/testvalue", "DefaultController", "Static"),
                    UrlTest("PUT /static/", "PUT /static/", "DefaultController", "Static"),
                    UrlTest("PUT /static/variablevalue1", "PUT /static/variablevalue1", "DefaultController", "Static"),
                    UrlTest("PUT /static/123412423", "PUT /static/123412423", "DefaultController", "Static"),
                    UrlTest("PUT /static/testvalue", "PUT /static/testvalue", "DefaultController", "Static"),
                    UrlTest("DELETE /static/", "DELETE /static/", "DefaultController", "Static"),
                    UrlTest("DELETE /static/variablevalue1", "DELETE /static/variablevalue1", "DefaultController", "Static"),
                    UrlTest("DELETE /static/123412423", "DELETE /static/123412423", "DefaultController", "Static"),
                    UrlTest("DELETE /static/testvalue", "DELETE /static/testvalue", "DefaultController", "Static"),
                    UrlTest("GET /ad/", "GET /ad/", "DefaultController", "View"),
                    UrlTest("GET /ad/variablevalue1", "GET /ad/variablevalue1", "DefaultController", "View"),
                    UrlTest("GET /ad/123412423", "GET /ad/123412423", "DefaultController", "View"),
                    UrlTest("GET /ad/testvalue", "GET /ad/testvalue", "DefaultController", "View"),
                    UrlTest("GET /resume/", "GET /resume/", "DefaultController", "View"),
                    UrlTest("GET /resume/variablevalue1", "GET /resume/variablevalue1", "DefaultController", "View"),
                    UrlTest("GET /resume/123412423", "GET /resume/123412423", "DefaultController", "View"),
                    UrlTest("GET /resume/testvalue", "GET /resume/testvalue", "DefaultController", "View"),
                    UrlTest("GET /without-tag/", "GET /without-tag/", "DefaultController", "Untag"),
                    UrlTest("GET /without-tag/variablevalue1", "GET /without-tag/variablevalue1", "DefaultController", "Untag"),
                    UrlTest("GET /without-tag/123412423", "GET /without-tag/123412423", "DefaultController", "Untag"),
                    UrlTest("GET /without-tag/testvalue", "GET /without-tag/testvalue", "DefaultController", "Untag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/without-tag/", "GET /abcde/edcba/aaaa123/bbb124/without-tag/", "DefaultController", "Untag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/without-tag/variablevalue1", "GET /abcde/edcba/aaaa123/bbb124/without-tag/variablevalue1", "DefaultController", "Untag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/without-tag/123412423", "GET /abcde/edcba/aaaa123/bbb124/without-tag/123412423", "DefaultController", "Untag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/without-tag/testvalue", "GET /abcde/edcba/aaaa123/bbb124/without-tag/testvalue", "DefaultController", "Untag"),
                    UrlTest("GET /bbb124/without-tag/", "GET /bbb124/without-tag/", "DefaultController", "Untag"),
                    UrlTest("GET /bbb124/without-tag/variablevalue1", "GET /bbb124/without-tag/variablevalue1", "DefaultController", "Untag"),
                    UrlTest("GET /bbb124/without-tag/123412423", "GET /bbb124/without-tag/123412423", "DefaultController", "Untag"),
                    UrlTest("GET /bbb124/without-tag/testvalue", "GET /bbb124/without-tag/testvalue", "DefaultController", "Untag"),
                    UrlTest("GET /aaaa123/bbb124/without-tag/", "GET /aaaa123/bbb124/without-tag/", "DefaultController", "Untag"),
                    UrlTest("GET /aaaa123/bbb124/without-tag/variablevalue1", "GET /aaaa123/bbb124/without-tag/variablevalue1", "DefaultController", "Untag"),
                    UrlTest("GET /aaaa123/bbb124/without-tag/123412423", "GET /aaaa123/bbb124/without-tag/123412423", "DefaultController", "Untag"),
                    UrlTest("GET /aaaa123/bbb124/without-tag/testvalue", "GET /aaaa123/bbb124/without-tag/testvalue", "DefaultController", "Untag"),
                    UrlTest("GET /with-tag/", "GET /with-tag/", "DefaultController", "Tag"),
                    UrlTest("GET /with-tag/variablevalue1", "GET /with-tag/variablevalue1", "DefaultController", "Tag"),
                    UrlTest("GET /with-tag/123412423", "GET /with-tag/123412423", "DefaultController", "Tag"),
                    UrlTest("GET /with-tag/testvalue", "GET /with-tag/testvalue", "DefaultController", "Tag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/with-tag/", "GET /abcde/edcba/aaaa123/bbb124/with-tag/", "DefaultController", "Tag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/with-tag/variablevalue1", "GET /abcde/edcba/aaaa123/bbb124/with-tag/variablevalue1", "DefaultController", "Tag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/with-tag/123412423", "GET /abcde/edcba/aaaa123/bbb124/with-tag/123412423", "DefaultController", "Tag"),
                    UrlTest("GET /abcde/edcba/aaaa123/bbb124/with-tag/testvalue", "GET /abcde/edcba/aaaa123/bbb124/with-tag/testvalue", "DefaultController", "Tag"),
                    UrlTest("GET /bbb124/with-tag/", "GET /bbb124/with-tag/", "DefaultController", "Tag"),
                    UrlTest("GET /bbb124/with-tag/variablevalue1", "GET /bbb124/with-tag/variablevalue1", "DefaultController", "Tag"),
                    UrlTest("GET /bbb124/with-tag/123412423", "GET /bbb124/with-tag/123412423", "DefaultController", "Tag"),
                    UrlTest("GET /bbb124/with-tag/testvalue", "GET /bbb124/with-tag/testvalue", "DefaultController", "Tag"),
                    UrlTest("GET /aaaa123/bbb124/with-tag/", "GET /aaaa123/bbb124/with-tag/", "DefaultController", "Tag"),
                    UrlTest("GET /aaaa123/bbb124/with-tag/variablevalue1", "GET /aaaa123/bbb124/with-tag/variablevalue1", "DefaultController", "Tag"),
                    UrlTest("GET /aaaa123/bbb124/with-tag/123412423", "GET /aaaa123/bbb124/with-tag/123412423", "DefaultController", "Tag"),
                    UrlTest("GET /aaaa123/bbb124/with-tag/testvalue", "GET /aaaa123/bbb124/with-tag/testvalue", "DefaultController", "Tag"),
                    UrlTest("GET /postings{firstTime}", "GET /postings{firstTime}", "DefaultController"),
                    UrlTest("GET /postings/abcde/edcba/aaaa123/bbb124{firstTime}", "GET /postings/abcde/edcba/aaaa123/bbb124{firstTime}", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/bbb124{firstTime}", "GET /postings/bbb124{firstTime}", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/aaaa123/bbb124{firstTime}", "GET /postings/aaaa123/bbb124{firstTime}", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/variablevalue1?{firstTime}", "GET /postings/variablevalue1?{firstTime}", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/123412423?{firstTime}", "GET /postings/123412423?{firstTime}", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/testvalue?{firstTime}", "GET /postings/testvalue?{firstTime}", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/", "GET /postings/", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/variablevalue1", "GET /postings/variablevalue1", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/123412423", "GET /postings/123412423", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("GET /postings/testvalue", "GET /postings/testvalue", "DefaultController", "Search", "FirstTimeSearch"),
                    UrlTest("POST /postings/", "POST /postings/", "DefaultController", "Search"),
                    UrlTest("POST /postings/variablevalue1", "POST /postings/variablevalue1", "DefaultController", "Search"),
                    UrlTest("POST /postings/123412423", "POST /postings/123412423", "DefaultController", "Search"),
                    UrlTest("POST /postings/testvalue", "POST /postings/testvalue", "DefaultController", "Search"),
                    UrlTest("PUT /postings/", "PUT /postings/", "DefaultController", "Search"),
                    UrlTest("PUT /postings/variablevalue1", "PUT /postings/variablevalue1", "DefaultController", "Search"),
                    UrlTest("PUT /postings/123412423", "PUT /postings/123412423", "DefaultController", "Search"),
                    UrlTest("PUT /postings/testvalue", "PUT /postings/testvalue", "DefaultController", "Search"),
                    UrlTest("DELETE /postings/", "DELETE /postings/", "DefaultController", "Search"),
                    UrlTest("DELETE /postings/variablevalue1", "DELETE /postings/variablevalue1", "DefaultController", "Search"),
                    UrlTest("DELETE /postings/123412423", "DELETE /postings/123412423", "DefaultController", "Search"),
                    UrlTest("DELETE /postings/testvalue", "DELETE /postings/testvalue", "DefaultController", "Search"),
                    UrlTest("GET /posting/flag///", "GET /posting/flag///", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag///variablevalue1", "GET /posting/flag///variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag///123412423", "GET /posting/flag///123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag///testvalue", "GET /posting/flag///testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//variablevalue1/", "GET /posting/flag//variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//variablevalue1/variablevalue1", "GET /posting/flag//variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//variablevalue1/123412423", "GET /posting/flag//variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//variablevalue1/testvalue", "GET /posting/flag//variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//123412423/", "GET /posting/flag//123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//123412423/variablevalue1", "GET /posting/flag//123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//123412423/123412423", "GET /posting/flag//123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//123412423/testvalue", "GET /posting/flag//123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//testvalue/", "GET /posting/flag//testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//testvalue/variablevalue1", "GET /posting/flag//testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//testvalue/123412423", "GET /posting/flag//testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag//testvalue/testvalue", "GET /posting/flag//testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1//", "GET /posting/flag/variablevalue1//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1//variablevalue1", "GET /posting/flag/variablevalue1//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1//123412423", "GET /posting/flag/variablevalue1//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1//testvalue", "GET /posting/flag/variablevalue1//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/variablevalue1/", "GET /posting/flag/variablevalue1/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/variablevalue1/variablevalue1", "GET /posting/flag/variablevalue1/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/variablevalue1/123412423", "GET /posting/flag/variablevalue1/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/variablevalue1/testvalue", "GET /posting/flag/variablevalue1/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/123412423/", "GET /posting/flag/variablevalue1/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/123412423/variablevalue1", "GET /posting/flag/variablevalue1/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/123412423/123412423", "GET /posting/flag/variablevalue1/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/123412423/testvalue", "GET /posting/flag/variablevalue1/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/testvalue/", "GET /posting/flag/variablevalue1/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/testvalue/variablevalue1", "GET /posting/flag/variablevalue1/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/testvalue/123412423", "GET /posting/flag/variablevalue1/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/variablevalue1/testvalue/testvalue", "GET /posting/flag/variablevalue1/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423//", "GET /posting/flag/123412423//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423//variablevalue1", "GET /posting/flag/123412423//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423//123412423", "GET /posting/flag/123412423//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423//testvalue", "GET /posting/flag/123412423//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/variablevalue1/", "GET /posting/flag/123412423/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/variablevalue1/variablevalue1", "GET /posting/flag/123412423/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/variablevalue1/123412423", "GET /posting/flag/123412423/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/variablevalue1/testvalue", "GET /posting/flag/123412423/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/123412423/", "GET /posting/flag/123412423/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/123412423/variablevalue1", "GET /posting/flag/123412423/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/123412423/123412423", "GET /posting/flag/123412423/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/123412423/testvalue", "GET /posting/flag/123412423/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/testvalue/", "GET /posting/flag/123412423/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/testvalue/variablevalue1", "GET /posting/flag/123412423/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/testvalue/123412423", "GET /posting/flag/123412423/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/123412423/testvalue/testvalue", "GET /posting/flag/123412423/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue//", "GET /posting/flag/testvalue//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue//variablevalue1", "GET /posting/flag/testvalue//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue//123412423", "GET /posting/flag/testvalue//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue//testvalue", "GET /posting/flag/testvalue//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/variablevalue1/", "GET /posting/flag/testvalue/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/variablevalue1/variablevalue1", "GET /posting/flag/testvalue/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/variablevalue1/123412423", "GET /posting/flag/testvalue/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/variablevalue1/testvalue", "GET /posting/flag/testvalue/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/123412423/", "GET /posting/flag/testvalue/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/123412423/variablevalue1", "GET /posting/flag/testvalue/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/123412423/123412423", "GET /posting/flag/testvalue/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/123412423/testvalue", "GET /posting/flag/testvalue/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/testvalue/", "GET /posting/flag/testvalue/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/testvalue/variablevalue1", "GET /posting/flag/testvalue/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/testvalue/123412423", "GET /posting/flag/testvalue/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/flag/testvalue/testvalue/testvalue", "GET /posting/flag/testvalue/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag///", "POST /posting/flag///", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag///variablevalue1", "POST /posting/flag///variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag///123412423", "POST /posting/flag///123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag///testvalue", "POST /posting/flag///testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//variablevalue1/", "POST /posting/flag//variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//variablevalue1/variablevalue1", "POST /posting/flag//variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//variablevalue1/123412423", "POST /posting/flag//variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//variablevalue1/testvalue", "POST /posting/flag//variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//123412423/", "POST /posting/flag//123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//123412423/variablevalue1", "POST /posting/flag//123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//123412423/123412423", "POST /posting/flag//123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//123412423/testvalue", "POST /posting/flag//123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//testvalue/", "POST /posting/flag//testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//testvalue/variablevalue1", "POST /posting/flag//testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//testvalue/123412423", "POST /posting/flag//testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag//testvalue/testvalue", "POST /posting/flag//testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1//", "POST /posting/flag/variablevalue1//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1//variablevalue1", "POST /posting/flag/variablevalue1//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1//123412423", "POST /posting/flag/variablevalue1//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1//testvalue", "POST /posting/flag/variablevalue1//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/variablevalue1/", "POST /posting/flag/variablevalue1/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/variablevalue1/variablevalue1", "POST /posting/flag/variablevalue1/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/variablevalue1/123412423", "POST /posting/flag/variablevalue1/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/variablevalue1/testvalue", "POST /posting/flag/variablevalue1/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/123412423/", "POST /posting/flag/variablevalue1/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/123412423/variablevalue1", "POST /posting/flag/variablevalue1/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/123412423/123412423", "POST /posting/flag/variablevalue1/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/123412423/testvalue", "POST /posting/flag/variablevalue1/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/testvalue/", "POST /posting/flag/variablevalue1/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/testvalue/variablevalue1", "POST /posting/flag/variablevalue1/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/testvalue/123412423", "POST /posting/flag/variablevalue1/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/variablevalue1/testvalue/testvalue", "POST /posting/flag/variablevalue1/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423//", "POST /posting/flag/123412423//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423//variablevalue1", "POST /posting/flag/123412423//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423//123412423", "POST /posting/flag/123412423//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423//testvalue", "POST /posting/flag/123412423//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/variablevalue1/", "POST /posting/flag/123412423/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/variablevalue1/variablevalue1", "POST /posting/flag/123412423/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/variablevalue1/123412423", "POST /posting/flag/123412423/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/variablevalue1/testvalue", "POST /posting/flag/123412423/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/123412423/", "POST /posting/flag/123412423/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/123412423/variablevalue1", "POST /posting/flag/123412423/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/123412423/123412423", "POST /posting/flag/123412423/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/123412423/testvalue", "POST /posting/flag/123412423/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/testvalue/", "POST /posting/flag/123412423/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/testvalue/variablevalue1", "POST /posting/flag/123412423/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/testvalue/123412423", "POST /posting/flag/123412423/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/123412423/testvalue/testvalue", "POST /posting/flag/123412423/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue//", "POST /posting/flag/testvalue//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue//variablevalue1", "POST /posting/flag/testvalue//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue//123412423", "POST /posting/flag/testvalue//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue//testvalue", "POST /posting/flag/testvalue//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/variablevalue1/", "POST /posting/flag/testvalue/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/variablevalue1/variablevalue1", "POST /posting/flag/testvalue/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/variablevalue1/123412423", "POST /posting/flag/testvalue/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/variablevalue1/testvalue", "POST /posting/flag/testvalue/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/123412423/", "POST /posting/flag/testvalue/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/123412423/variablevalue1", "POST /posting/flag/testvalue/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/123412423/123412423", "POST /posting/flag/testvalue/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/123412423/testvalue", "POST /posting/flag/testvalue/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/testvalue/", "POST /posting/flag/testvalue/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/testvalue/variablevalue1", "POST /posting/flag/testvalue/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/testvalue/123412423", "POST /posting/flag/testvalue/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("POST /posting/flag/testvalue/testvalue/testvalue", "POST /posting/flag/testvalue/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag///", "PUT /posting/flag///", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag///variablevalue1", "PUT /posting/flag///variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag///123412423", "PUT /posting/flag///123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag///testvalue", "PUT /posting/flag///testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//variablevalue1/", "PUT /posting/flag//variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//variablevalue1/variablevalue1", "PUT /posting/flag//variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//variablevalue1/123412423", "PUT /posting/flag//variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//variablevalue1/testvalue", "PUT /posting/flag//variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//123412423/", "PUT /posting/flag//123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//123412423/variablevalue1", "PUT /posting/flag//123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//123412423/123412423", "PUT /posting/flag//123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//123412423/testvalue", "PUT /posting/flag//123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//testvalue/", "PUT /posting/flag//testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//testvalue/variablevalue1", "PUT /posting/flag//testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//testvalue/123412423", "PUT /posting/flag//testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag//testvalue/testvalue", "PUT /posting/flag//testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1//", "PUT /posting/flag/variablevalue1//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1//variablevalue1", "PUT /posting/flag/variablevalue1//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1//123412423", "PUT /posting/flag/variablevalue1//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1//testvalue", "PUT /posting/flag/variablevalue1//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/variablevalue1/", "PUT /posting/flag/variablevalue1/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/variablevalue1/variablevalue1", "PUT /posting/flag/variablevalue1/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/variablevalue1/123412423", "PUT /posting/flag/variablevalue1/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/variablevalue1/testvalue", "PUT /posting/flag/variablevalue1/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/123412423/", "PUT /posting/flag/variablevalue1/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/123412423/variablevalue1", "PUT /posting/flag/variablevalue1/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/123412423/123412423", "PUT /posting/flag/variablevalue1/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/123412423/testvalue", "PUT /posting/flag/variablevalue1/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/testvalue/", "PUT /posting/flag/variablevalue1/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/testvalue/variablevalue1", "PUT /posting/flag/variablevalue1/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/testvalue/123412423", "PUT /posting/flag/variablevalue1/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/variablevalue1/testvalue/testvalue", "PUT /posting/flag/variablevalue1/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423//", "PUT /posting/flag/123412423//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423//variablevalue1", "PUT /posting/flag/123412423//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423//123412423", "PUT /posting/flag/123412423//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423//testvalue", "PUT /posting/flag/123412423//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/variablevalue1/", "PUT /posting/flag/123412423/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/variablevalue1/variablevalue1", "PUT /posting/flag/123412423/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/variablevalue1/123412423", "PUT /posting/flag/123412423/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/variablevalue1/testvalue", "PUT /posting/flag/123412423/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/123412423/", "PUT /posting/flag/123412423/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/123412423/variablevalue1", "PUT /posting/flag/123412423/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/123412423/123412423", "PUT /posting/flag/123412423/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/123412423/testvalue", "PUT /posting/flag/123412423/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/testvalue/", "PUT /posting/flag/123412423/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/testvalue/variablevalue1", "PUT /posting/flag/123412423/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/testvalue/123412423", "PUT /posting/flag/123412423/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/123412423/testvalue/testvalue", "PUT /posting/flag/123412423/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue//", "PUT /posting/flag/testvalue//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue//variablevalue1", "PUT /posting/flag/testvalue//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue//123412423", "PUT /posting/flag/testvalue//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue//testvalue", "PUT /posting/flag/testvalue//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/variablevalue1/", "PUT /posting/flag/testvalue/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/variablevalue1/variablevalue1", "PUT /posting/flag/testvalue/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/variablevalue1/123412423", "PUT /posting/flag/testvalue/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/variablevalue1/testvalue", "PUT /posting/flag/testvalue/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/123412423/", "PUT /posting/flag/testvalue/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/123412423/variablevalue1", "PUT /posting/flag/testvalue/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/123412423/123412423", "PUT /posting/flag/testvalue/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/123412423/testvalue", "PUT /posting/flag/testvalue/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/testvalue/", "PUT /posting/flag/testvalue/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/testvalue/variablevalue1", "PUT /posting/flag/testvalue/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/testvalue/123412423", "PUT /posting/flag/testvalue/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("PUT /posting/flag/testvalue/testvalue/testvalue", "PUT /posting/flag/testvalue/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag///", "DELETE /posting/flag///", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag///variablevalue1", "DELETE /posting/flag///variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag///123412423", "DELETE /posting/flag///123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag///testvalue", "DELETE /posting/flag///testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//variablevalue1/", "DELETE /posting/flag//variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//variablevalue1/variablevalue1", "DELETE /posting/flag//variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//variablevalue1/123412423", "DELETE /posting/flag//variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//variablevalue1/testvalue", "DELETE /posting/flag//variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//123412423/", "DELETE /posting/flag//123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//123412423/variablevalue1", "DELETE /posting/flag//123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//123412423/123412423", "DELETE /posting/flag//123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//123412423/testvalue", "DELETE /posting/flag//123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//testvalue/", "DELETE /posting/flag//testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//testvalue/variablevalue1", "DELETE /posting/flag//testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//testvalue/123412423", "DELETE /posting/flag//testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag//testvalue/testvalue", "DELETE /posting/flag//testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1//", "DELETE /posting/flag/variablevalue1//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1//variablevalue1", "DELETE /posting/flag/variablevalue1//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1//123412423", "DELETE /posting/flag/variablevalue1//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1//testvalue", "DELETE /posting/flag/variablevalue1//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/variablevalue1/", "DELETE /posting/flag/variablevalue1/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/variablevalue1/variablevalue1", "DELETE /posting/flag/variablevalue1/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/variablevalue1/123412423", "DELETE /posting/flag/variablevalue1/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/variablevalue1/testvalue", "DELETE /posting/flag/variablevalue1/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/123412423/", "DELETE /posting/flag/variablevalue1/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/123412423/variablevalue1", "DELETE /posting/flag/variablevalue1/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/123412423/123412423", "DELETE /posting/flag/variablevalue1/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/123412423/testvalue", "DELETE /posting/flag/variablevalue1/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/testvalue/", "DELETE /posting/flag/variablevalue1/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/testvalue/variablevalue1", "DELETE /posting/flag/variablevalue1/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/testvalue/123412423", "DELETE /posting/flag/variablevalue1/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/variablevalue1/testvalue/testvalue", "DELETE /posting/flag/variablevalue1/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423//", "DELETE /posting/flag/123412423//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423//variablevalue1", "DELETE /posting/flag/123412423//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423//123412423", "DELETE /posting/flag/123412423//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423//testvalue", "DELETE /posting/flag/123412423//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/variablevalue1/", "DELETE /posting/flag/123412423/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/variablevalue1/variablevalue1", "DELETE /posting/flag/123412423/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/variablevalue1/123412423", "DELETE /posting/flag/123412423/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/variablevalue1/testvalue", "DELETE /posting/flag/123412423/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/123412423/", "DELETE /posting/flag/123412423/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/123412423/variablevalue1", "DELETE /posting/flag/123412423/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/123412423/123412423", "DELETE /posting/flag/123412423/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/123412423/testvalue", "DELETE /posting/flag/123412423/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/testvalue/", "DELETE /posting/flag/123412423/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/testvalue/variablevalue1", "DELETE /posting/flag/123412423/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/testvalue/123412423", "DELETE /posting/flag/123412423/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/123412423/testvalue/testvalue", "DELETE /posting/flag/123412423/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue//", "DELETE /posting/flag/testvalue//", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue//variablevalue1", "DELETE /posting/flag/testvalue//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue//123412423", "DELETE /posting/flag/testvalue//123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue//testvalue", "DELETE /posting/flag/testvalue//testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/variablevalue1/", "DELETE /posting/flag/testvalue/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/variablevalue1/variablevalue1", "DELETE /posting/flag/testvalue/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/variablevalue1/123412423", "DELETE /posting/flag/testvalue/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/variablevalue1/testvalue", "DELETE /posting/flag/testvalue/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/123412423/", "DELETE /posting/flag/testvalue/123412423/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/123412423/variablevalue1", "DELETE /posting/flag/testvalue/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/123412423/123412423", "DELETE /posting/flag/testvalue/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/123412423/testvalue", "DELETE /posting/flag/testvalue/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/testvalue/", "DELETE /posting/flag/testvalue/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/testvalue/variablevalue1", "DELETE /posting/flag/testvalue/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/testvalue/123412423", "DELETE /posting/flag/testvalue/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("DELETE /posting/flag/testvalue/testvalue/testvalue", "DELETE /posting/flag/testvalue/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "Flag"),
                    UrlTest("GET /posting/apply//", "GET /posting/apply//", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply//variablevalue1", "GET /posting/apply//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply//123412423", "GET /posting/apply//123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply//testvalue", "GET /posting/apply//testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/variablevalue1/", "GET /posting/apply/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/variablevalue1/variablevalue1", "GET /posting/apply/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/variablevalue1/123412423", "GET /posting/apply/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/variablevalue1/testvalue", "GET /posting/apply/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/123412423/", "GET /posting/apply/123412423/", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/123412423/variablevalue1", "GET /posting/apply/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/123412423/123412423", "GET /posting/apply/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/123412423/testvalue", "GET /posting/apply/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/testvalue/", "GET /posting/apply/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/testvalue/variablevalue1", "GET /posting/apply/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/testvalue/123412423", "GET /posting/apply/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("GET /posting/apply/testvalue/testvalue", "GET /posting/apply/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyDisplay"),
                    UrlTest("POST /posting/apply//", "POST /posting/apply//", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply//variablevalue1", "POST /posting/apply//variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply//123412423", "POST /posting/apply//123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply//testvalue", "POST /posting/apply//testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/variablevalue1/", "POST /posting/apply/variablevalue1/", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/variablevalue1/variablevalue1", "POST /posting/apply/variablevalue1/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/variablevalue1/123412423", "POST /posting/apply/variablevalue1/123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/variablevalue1/testvalue", "POST /posting/apply/variablevalue1/testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/123412423/", "POST /posting/apply/123412423/", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/123412423/variablevalue1", "POST /posting/apply/123412423/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/123412423/123412423", "POST /posting/apply/123412423/123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/123412423/testvalue", "POST /posting/apply/123412423/testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/testvalue/", "POST /posting/apply/testvalue/", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/testvalue/variablevalue1", "POST /posting/apply/testvalue/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/testvalue/123412423", "POST /posting/apply/testvalue/123412423", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("POST /posting/apply/testvalue/testvalue", "POST /posting/apply/testvalue/testvalue", "DefaultController", "GeneralFunctionAccessControl", "ApplyController"),
                    UrlTest("GET /posting/resume/byname/", "GET /posting/resume/byname/", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeDisplay"),
                    UrlTest("GET /posting/resume/byname/variablevalue1", "GET /posting/resume/byname/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeDisplay"),
                    UrlTest("GET /posting/resume/byname/123412423", "GET /posting/resume/byname/123412423", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeDisplay"),
                    UrlTest("GET /posting/resume/byname/testvalue", "GET /posting/resume/byname/testvalue", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeDisplay"),
                    UrlTest("POST /posting/resume/byname/", "POST /posting/resume/byname/", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeUpdate"),
                    UrlTest("POST /posting/resume/byname/variablevalue1", "POST /posting/resume/byname/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeUpdate"),
                    UrlTest("POST /posting/resume/byname/123412423", "POST /posting/resume/byname/123412423", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeUpdate"),
                    UrlTest("POST /posting/resume/byname/testvalue", "POST /posting/resume/byname/testvalue", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "ResumeUpdate"),
                    UrlTest("GET /posting/resume/preview/byname/", "GET /posting/resume/preview/byname/", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "PreviewDisplay"),
                    UrlTest("GET /posting/resume/preview/byname/variablevalue1", "GET /posting/resume/preview/byname/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "PreviewDisplay"),
                    UrlTest("GET /posting/resume/preview/byname/123412423", "GET /posting/resume/preview/byname/123412423", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "PreviewDisplay"),
                    UrlTest("GET /posting/resume/preview/byname/testvalue", "GET /posting/resume/preview/byname/testvalue", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "PreviewDisplay"),
                    UrlTest("GET /posting/manage", "GET /posting/manage", "GeneralFunctionAccessControl", "DefaultController", "CompanyFunctionAccessControl", "Manage"),
                    UrlTest("GET /posting/ad/applicants/byId/", "GET /posting/ad/applicants/byId/", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/applicants/byId/variablevalue1", "GET /posting/ad/applicants/byId/variablevalue1", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/applicants/byId/123412423", "GET /posting/ad/applicants/byId/123412423", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/applicants/byId/testvalue", "GET /posting/ad/applicants/byId/testvalue", "GeneralFunctionAccessControl", "DefaultController", "DataAccessControl", "CompanyFunctionAccessControl", "ViewAllApplicatints"),
                    UrlTest("GET /posting/ad/byname/", "GET /posting/ad/byname/", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdDisplay"),
                    UrlTest("GET /posting/ad/byname/variablevalue1", "GET /posting/ad/byname/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdDisplay"),
                    UrlTest("GET /posting/ad/byname/123412423", "GET /posting/ad/byname/123412423", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdDisplay"),
                    UrlTest("GET /posting/ad/byname/testvalue", "GET /posting/ad/byname/testvalue", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdDisplay"),
                    UrlTest("POST /posting/ad/byname/", "POST /posting/ad/byname/", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdUpdate"),
                    UrlTest("POST /posting/ad/byname/variablevalue1", "POST /posting/ad/byname/variablevalue1", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdUpdate"),
                    UrlTest("POST /posting/ad/byname/123412423", "POST /posting/ad/byname/123412423", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdUpdate"),
                    UrlTest("POST /posting/ad/byname/testvalue", "POST /posting/ad/byname/testvalue", "DefaultController", "GeneralFunctionAccessControl", "DataAccessControl", "AdUpdate")
                    );


            #endregion

            #region Branching
            NewTestWithUrl("Branching",
               Types(
                   Type(
                       "C1",
                       BindAttribute("/a/b1/c1")
                       ),
                   Type(
                       "C2",
                       BindAttribute("/a/b1/c2")
                       ),
                   Type(
                       "C3",
                       BindAttribute("/a/b2/c1")
                       ),
                   Type(
                       "C4",
                       BindAttribute("/a/b2/c2")
                       )
                   ),
                   UrlTest("special - GET /a", "GET /a"),
                   UrlTest("GET /a/b1/c1", "GET /a/b1/c1", "C1"),
                   UrlTest("POST /a/b1/c1", "POST /a/b1/c1", "C1"),
                   UrlTest("PUT /a/b1/c1", "PUT /a/b1/c1", "C1"),
                   UrlTest("DELETE /a/b1/c1", "DELETE /a/b1/c1", "C1"),
                   UrlTest("HEAD /a/b1/c1", "HEAD /a/b1/c1", "C1"),
                   UrlTest("GET /a/b1/c2", "GET /a/b1/c2", "C2"),
                   UrlTest("POST /a/b1/c2", "POST /a/b1/c2", "C2"),
                   UrlTest("PUT /a/b1/c2", "PUT /a/b1/c2", "C2"),
                   UrlTest("DELETE /a/b1/c2", "DELETE /a/b1/c2", "C2"),
                   UrlTest("HEAD /a/b1/c2", "HEAD /a/b1/c2", "C2"),
                   UrlTest("GET /a/b2/c1", "GET /a/b2/c1", "C3"),
                   UrlTest("POST /a/b2/c1", "POST /a/b2/c1", "C3"),
                   UrlTest("PUT /a/b2/c1", "PUT /a/b2/c1", "C3"),
                   UrlTest("DELETE /a/b2/c1", "DELETE /a/b2/c1", "C3"),
                   UrlTest("HEAD /a/b2/c1", "HEAD /a/b2/c1", "C3"),
                   UrlTest("GET /a/b2/c2", "GET /a/b2/c2", "C4"),
                   UrlTest("POST /a/b2/c2", "POST /a/b2/c2", "C4"),
                   UrlTest("PUT /a/b2/c2", "PUT /a/b2/c2", "C4"),
                   UrlTest("DELETE /a/b2/c2", "DELETE /a/b2/c2", "C4"),
                   UrlTest("HEAD /a/b2/c2", "HEAD /a/b2/c2", "C4")
                //Node("* /a", Controllers(),
                //    Node("/b1", Controllers(),
                //        Node("/c1", "C1"),
                //        Node("/c2", "C2")
                //        ),
                //    Node("/b2", Controllers(),
                //        Node("/c1", "C3"),
                //        Node("/c2", "C4")
                //        )
                //    )

               );
            #endregion

            #region tree - one controller - 2 bindings (flat)
            NewTestWithUrl(
                "tree - one controller - 2 bindings (flat)",
                Types(Type(
                    "Controller1",
                    Attributes(
                        BindAttribute("/"),
                        BindAttribute("/path1"))
                    )),
                //Node("* /", "Controller1"),
                //Node("* /path1", "Controller1")
                UrlTest("GET /path1/anotherpath", "GET /path1/anotherpath", "Controller1"),
                UrlTest("POST /path1/anotherpath", "POST /path1/anotherpath", "Controller1"),
                UrlTest("PUT /path1/anotherpath", "PUT /path1/anotherpath", "Controller1"),
                UrlTest("DELETE /path1/anotherpath", "DELETE /path1/anotherpath", "Controller1"),
                UrlTest("HEAD /path1/anotherpath", "HEAD /path1/anotherpath", "Controller1"),
                UrlTest("GET /anotherpath", "GET /anotherpath"),
                UrlTest("POST /anotherpath", "POST /anotherpath"),
                UrlTest("PUT /anotherpath", "PUT /anotherpath"),
                UrlTest("DELETE /anotherpath", "DELETE /anotherpath"),
                UrlTest("HEAD /anotherpath", "HEAD /anotherpath"),
                UrlTest("GET /", "GET /", "Controller1"),
                UrlTest("POST /", "POST /", "Controller1"),
                UrlTest("PUT /", "PUT /", "Controller1"),
                UrlTest("DELETE /", "DELETE /", "Controller1"),
                UrlTest("HEAD /", "HEAD /", "Controller1"),
                UrlTest("GET /path1", "GET /path1", "Controller1"),
                UrlTest("POST /path1", "POST /path1", "Controller1"),
                UrlTest("PUT /path1", "PUT /path1", "Controller1"),
                UrlTest("DELETE /path1", "DELETE /path1", "Controller1"),
                UrlTest("HEAD /path1", "HEAD /path1", "Controller1")

            );
            #endregion

            #region Imported - home/root
            NewTestWithUrl(
                "Imported - home/root",
                Types(
                    Type(
                        "HomeUrlController1",
                        BindAttribute("/?")
                    ),
                    Type(
                        "HomeUrlController2",
                        BindAttribute("/?")
                    )
                ),
                UrlTest("GET /abcde/edcba/aaaa123/bbb124", "GET /abcde/edcba/aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("GET /bbb124", "GET /bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("GET /aaaa123/bbb124", "GET /aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("POST /abcde/edcba/aaaa123/bbb124", "POST /abcde/edcba/aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("POST /bbb124", "POST /bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("POST /aaaa123/bbb124", "POST /aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("PUT /abcde/edcba/aaaa123/bbb124", "PUT /abcde/edcba/aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("PUT /bbb124", "PUT /bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("PUT /aaaa123/bbb124", "PUT /aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("DELETE /abcde/edcba/aaaa123/bbb124", "DELETE /abcde/edcba/aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("DELETE /bbb124", "DELETE /bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("DELETE /aaaa123/bbb124", "DELETE /aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("HEAD /abcde/edcba/aaaa123/bbb124", "HEAD /abcde/edcba/aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("HEAD /bbb124", "HEAD /bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1")),
                UrlTest("HEAD /aaaa123/bbb124", "HEAD /aaaa123/bbb124", CtrUnOrdGrp("HomeUrlController2", "HomeUrlController1"))

//                Node("* /?", "HomeUrlController2", "HomeUrlController1") // Actually that's not so good - controllers may come in any order here.
                );
            #endregion

            #region Imported - /hello/...
            NewTestWithUrl("Imported - /hello/...",
                Types(
                    Type("HelloYouController1", BindAttribute("/hello/?/you")),
                    Type("HelloYouController2", BindAttribute("/hello/*/you"))
                ),
                UrlTest("GET /hello/you", "GET /hello/you", "HelloYouController1"),
                UrlTest("GET /hello/abcde/edcba/aaaa123/bbb124/you", "GET /hello/abcde/edcba/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("GET /hello/bbb124/you", "GET /hello/bbb124/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("GET /hello/aaaa123/bbb124/you", "GET /hello/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("POST /hello/you", "POST /hello/you", "HelloYouController1"),
                UrlTest("POST /hello/abcde/edcba/aaaa123/bbb124/you", "POST /hello/abcde/edcba/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("POST /hello/bbb124/you", "POST /hello/bbb124/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("POST /hello/aaaa123/bbb124/you", "POST /hello/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("PUT /hello/you", "PUT /hello/you", "HelloYouController1"),
                UrlTest("PUT /hello/abcde/edcba/aaaa123/bbb124/you", "PUT /hello/abcde/edcba/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("PUT /hello/bbb124/you", "PUT /hello/bbb124/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("PUT /hello/aaaa123/bbb124/you", "PUT /hello/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("DELETE /hello/you", "DELETE /hello/you", "HelloYouController1"),
                UrlTest("DELETE /hello/abcde/edcba/aaaa123/bbb124/you", "DELETE /hello/abcde/edcba/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("DELETE /hello/bbb124/you", "DELETE /hello/bbb124/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("DELETE /hello/aaaa123/bbb124/you", "DELETE /hello/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("HEAD /hello/you", "HEAD /hello/you", "HelloYouController1"),
                UrlTest("HEAD /hello/abcde/edcba/aaaa123/bbb124/you", "HEAD /hello/abcde/edcba/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("HEAD /hello/bbb124/you", "HEAD /hello/bbb124/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("HEAD /hello/aaaa123/bbb124/you", "HEAD /hello/aaaa123/bbb124/you", "HelloYouController1"),
                UrlTest("GET /hello/aaaaa/you", "GET /hello/aaaaa/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("GET /hello/abcde/you", "GET /hello/abcde/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("GET /hello/testvalue/you", "GET /hello/testvalue/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("POST /hello/aaaaa/you", "POST /hello/aaaaa/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("POST /hello/abcde/you", "POST /hello/abcde/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("POST /hello/testvalue/you", "POST /hello/testvalue/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("PUT /hello/aaaaa/you", "PUT /hello/aaaaa/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("PUT /hello/abcde/you", "PUT /hello/abcde/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("PUT /hello/testvalue/you", "PUT /hello/testvalue/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("DELETE /hello/aaaaa/you", "DELETE /hello/aaaaa/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("DELETE /hello/abcde/you", "DELETE /hello/abcde/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("DELETE /hello/testvalue/you", "DELETE /hello/testvalue/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("HEAD /hello/aaaaa/you", "HEAD /hello/aaaaa/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("HEAD /hello/abcde/you", "HEAD /hello/abcde/you", "HelloYouController1", "HelloYouController2"),
                UrlTest("HEAD /hello/testvalue/you", "HEAD /hello/testvalue/you", "HelloYouController1", "HelloYouController2")

                //Node("* /hello", Controllers(),
                //    Node("/*/you", "HelloYouController2", "HelloYouController1"),
                //    Node("/?/you", "HelloYouController1")
                //    )
                );
            #endregion

            #region Imported - /order/world/new
            NewTestWithUrl("Imported - /order/world/new",
                Types(
                    Type(
                        "OrderController1",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c1", "string", SessionAttribute),
                        Field("c2", "string", SessionAttribute, RequiresAttribute)
                    ),
                    Type(
                        "OrderController2",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c2", "string", SessionAttribute),
                        Field("c5", "string", SessionAttribute, RequiresAttribute)
                    ),
                    Type(
                        "OrderController3",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c3", "string", SessionAttribute),
                        Field("c2", "string", SessionAttribute, RequiresAttribute),
                        Field("c4", "string", SessionAttribute, RequiresAttribute),
                        Field("c5", "string", SessionAttribute, RequiresAttribute)
                    ),
                    Type(
                        "OrderController4",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c4", "string", SessionAttribute),
                        Field("c1", "string", SessionAttribute, RequiresAttribute),
                        Field("c2", "string", SessionAttribute, RequiresAttribute),
                        Field("c5", "string", SessionAttribute, RequiresAttribute)
                    ),
                    Type(
                        "OrderController5",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c5", "string", SessionAttribute)
                    ),
                    Type(
                        "OrderController6",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c6", "string", SessionAttribute),
                        Field("c3", "string", SessionAttribute, RequiresAttribute),
                        Field("c7", "string", SessionAttribute, RequiresAttribute)
                    ),
                    Type(
                        "OrderController7",
                        Attributes(
                            BindAttribute("/order/world/new")
                            ),
                        Field("c7", "string", SessionAttribute)
                    )
                ),
                UrlTest("GET /order/world/new", "GET /order/world/new", "OrderController5", CtrUnOrdGrp("OrderController7", CtrOrdGrp("OrderController2", "OrderController1", "OrderController4", "OrderController3")), "OrderController6"),
				UrlTest("POST /order/world/new", "POST /order/world/new", "OrderController5", CtrUnOrdGrp("OrderController7", CtrOrdGrp("OrderController2", "OrderController1", "OrderController4", "OrderController3")), "OrderController6"),
				UrlTest("PUT /order/world/new", "PUT /order/world/new", "OrderController5", CtrUnOrdGrp("OrderController7", CtrOrdGrp("OrderController2", "OrderController1", "OrderController4", "OrderController3")), "OrderController6"),
				UrlTest("DELETE /order/world/new", "DELETE /order/world/new", "OrderController5", CtrUnOrdGrp("OrderController7", CtrOrdGrp("OrderController2", "OrderController1", "OrderController4", "OrderController3")), "OrderController6"),
                UrlTest("HEAD /order/world/new", "HEAD /order/world/new", "OrderController5", CtrUnOrdGrp("OrderController7", CtrOrdGrp("OrderController2", "OrderController1", "OrderController4", "OrderController3")), "OrderController6")
                //Node("* /order/world/new", "OrderController7", "OrderController5", "OrderController2", "OrderController1", "OrderController4", "OrderController3", "OrderController6")
            );
            #endregion

            #region Imported - /one_little_url
            NewTestWithUrl(
                "Imported - /one_little_url",
                Types(
                    Type("littleController1",
                        Attributes(
                            BindAttribute("/one_little_url")
                        ),
                        Field("l1", "string", RequestAttribute),
                        Field("l2", "string", RequestAttribute, RequiresAttribute)
                    ),
                    Type("littleController2",
                        Attributes(
                            BindAttribute("/one_little_url")
                        ),
                        Field("l2", "string", RequestAttribute)
                    )
                ),
                UrlTest("GET /one_little_url", "GET /one_little_url", "littleController2", "littleController1"),
                UrlTest("POST /one_little_url", "POST /one_little_url", "littleController2", "littleController1"),
                UrlTest("PUT /one_little_url", "PUT /one_little_url", "littleController2", "littleController1"),
                UrlTest("DELETE /one_little_url", "DELETE /one_little_url", "littleController2", "littleController1"),
                UrlTest("HEAD /one_little_url", "HEAD /one_little_url", "littleController2", "littleController1")
                //Node("* /one_little_url", "littleController2", "littleController1")
            );
            #endregion

            #region Imported - /little_url/more
            NewTestWithUrl(
                "Imported - /little_url/more",
                Types(
                    Type(
                        "littleController3",
                        Attributes(
                            BindAttribute("/little_url/more")
                        ),
                        Field("l3", "string", RequestAttribute)
                    ),
                    Type(
                        "littleController4",
                        Attributes(
                            BindAttribute("/little_url/more")
                        ),
                        Field("l4", "string", RequestAttribute),
                        Field("l3", "string", RequestAttribute, RequiresAttribute),
                        Field("l5", "string", RequestAttribute, RequiresAttribute)
                    ),
                    Type(
                        "littleController5",
                        Attributes(
                            BindAttribute("/little_url/more")
                        ),
                        Field("l5", "string", RequestAttribute),
                        Field("l3", "string", RequestAttribute, RequiresAttribute)
                    )
                ),
                UrlTest("GET /little_url/more", "GET /little_url/more", "littleController3", "littleController5", "littleController4"),
                UrlTest("POST /little_url/more", "POST /little_url/more", "littleController3", "littleController5", "littleController4"),
                UrlTest("PUT /little_url/more", "PUT /little_url/more", "littleController3", "littleController5", "littleController4"),
                UrlTest("DELETE /little_url/more", "DELETE /little_url/more", "littleController3", "littleController5", "littleController4"),
                UrlTest("HEAD /little_url/more", "HEAD /little_url/more", "littleController3", "littleController5", "littleController4")
                //Node("* /little_url/more", "littleController3", "littleController5", "littleController4")
            );
            #endregion


        }

    }
}
