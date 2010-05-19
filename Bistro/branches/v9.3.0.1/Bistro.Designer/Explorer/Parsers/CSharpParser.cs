using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony.Samples.CSharp;

namespace Bistro.Designer.Explorer
{
    class CSharpParser : MetadataParserBase
    {
        public CSharpParser() : base()
        {
            grammar = new CSharpGrammar();
            parser = new Parser(new LanguageData(grammar));

        }
        protected override void AddParseNodeRec(ParseTreeNode nodeInfo, bool skip)
        {
            if (nodeInfo == null || skip) return;
            Token token = nodeInfo.AstNode as Token;
            if (token != null)
            {
                if (token.Terminal.Category != TokenCategory.Content) return;
            }
            try
            {
                bool isClassDef = (String.Compare(nodeInfo.ToString(), "class_declaration") == 0);
                bool isClassField = (String.Compare(nodeInfo.ToString(), "field_declaration") == 0);
                bool isClassProperty = (String.Compare(nodeInfo.ToString(), "property_declaration") == 0);
                if (isClassDef)
                {
                    curCtrl = nodeInfo.ChildNodes[3].ToString();
                    curCtrl = curCtrl.Substring(0, curCtrl.Length - tail.Length);
                    controllerInfo.Add(new ControllerMetadata(curCtrl));
                    curbpi.controller = controllerInfo[controllerInfo.Count - 1];
                    curFieldOrProp = String.Empty;
                }
                if (isClassField)
                {
                    //[0]<member_header>[1]<type_ref>[2]<variable_declarators>
                    ParseTreeNode varDeclarator = nodeInfo.ChildNodes[2];
                    while (varDeclarator.ChildNodes.Count > 0)
                        varDeclarator = varDeclarator.ChildNodes[0];
                    curFieldOrProp = varDeclarator.ToString();
                    curFieldOrProp = curFieldOrProp.Substring(0, curFieldOrProp.Length - tail.Length);
                }
                if (isClassProperty)
                {
                    //[0]<member_header>[1]<type_ref>[2]<qual_name_with_targs>[3]<accessor_declaration>
                    ParseTreeNode qual_name = nodeInfo.ChildNodes[2];
                    if (qual_name.ChildNodes[1].ChildNodes.Count > 0)
                    {
                        //property name is full,like N1.I.PropName
                        //go down until we reach '.<Name>'
                        while (qual_name.ChildNodes.Count > 0)
                            qual_name = qual_name.ChildNodes[qual_name.ChildNodes.Count - 1];
                        curFieldOrProp = qual_name.ToString();
                    }
                    else
                        //property name is identifier_or_builtin -> identifier 
                        curFieldOrProp = qual_name.ChildNodes[0].ChildNodes[0].ToString();
                    curFieldOrProp = curFieldOrProp.Substring(0, curFieldOrProp.Length - tail.Length);
                }
                AnalyzeTree(nodeInfo);
            }
            catch (Exception ex)
            {
            }

        }
        protected override void AnalyzeTree(ParseTreeNode nodeInfo)
        {
            foreach (var child in nodeInfo.ChildNodes)
            {

                ParseTreeNode curChild = child;
                string curVal = String.Empty;
                bool skipBranch = false;
                #region analyze NonTerminals
                switch (child.ToString())
                {
                    case "attribute":
                        //[0]<qual_name_with_targs>=<identifier><qual_name>
                        //[1]<attribute_arguments_par_opt>
                        ParseTreeNode qual_name = child.ChildNodes[0];
                        if (qual_name.ChildNodes[1].ChildNodes.Count > 0)
                        {
                            //attribute name is full,like Bistro.Controllers.Bind
                            //go down until we reach '.<Name>'
                            while (qual_name.ChildNodes.Count > 0)
                                qual_name = qual_name.ChildNodes[qual_name.ChildNodes.Count - 1];
                            curAttr = qual_name.ToString();
                        }
                        else
                            //attribute name is identifier
                            curAttr = qual_name.ChildNodes[0].ChildNodes[0].ToString();

                        curAttr = curAttr.Substring(0, curAttr.Length - tail.Length);
                        switch (curAttr)
                        {
                            case "Request":
                                controllerInfo[controllerInfo.Count - 1].Provides.Add(curFieldOrProp);
                                break;
                            case "Session":
                                controllerInfo[controllerInfo.Count - 1].Provides.Add(curFieldOrProp);
                                break;
                            case "Provides":
                                controllerInfo[controllerInfo.Count - 1].Provides.Add(curFieldOrProp);
                                break;
                            case "DependsOn":
                                controllerInfo[controllerInfo.Count - 1].DependsOn.Add(curFieldOrProp);
                                break;
                            case "Requires":
                                controllerInfo[controllerInfo.Count - 1].Requires.Add(curFieldOrProp);
                                break;

                        }

                        break;


                    case "attr_arg":
                        if (curAttr == "Bind" && String.IsNullOrEmpty(curFieldOrProp))
                        {
                            ParseTreeNode expr;
                            if (child.ChildNodes.Count > 1)
                            {
                                //Priority = 1 : [0]Priority [1] = [2]primary expression->identifier
                                curVal = child.ChildNodes[0].ToString() + "= ";
                                expr = child.ChildNodes[2];
                                while (expr.ChildNodes.Count > 0)
                                    expr = expr.ChildNodes[expr.ChildNodes.Count - 1];
                                //curVal += expr.ToString();
                                if (curVal.Contains("Priority"))
                                    curbpi.priority = Convert.ToInt32(expr);
                                else if (curVal.Contains("BindType"))
                                {
                                    switch (expr.ToString()) 
                                    {
                                        case "After":
                                            curbpi.bindType = Bistro.Controllers.Descriptor.BindType.After;
                                            break;
                                        case "Payload":
                                            curbpi.bindType = Bistro.Controllers.Descriptor.BindType.Payload;
                                            break;
                                        case "Teardown":
                                             curbpi.bindType = Bistro.Controllers.Descriptor.BindType.Teardown;
                                             break;

                                        default:
                                             break;
                                    }
                                }
                                //curVal is  like this: Priority (Identifier)= 1 (Number)
                            }
                            else
                            {
                                //"target" : [0]primary expression -> literal
                                expr = child.ChildNodes[0];
                                while (expr.ChildNodes.Count > 0)
                                    expr = expr.ChildNodes[expr.ChildNodes.Count - 1];
                                curVal = expr.ToString();
                                curbpi.target = curVal;
                                curbpi.bindLen = curbpi.target.Split('\\').Length;
                            }
                            controllerInfo[controllerInfo.Count - 1].Binds.Add(new BindPointInfo(curbpi));

                        }
                        else if (curAttr == "RenderWith" && String.IsNullOrEmpty(curFieldOrProp))
                        {
                            //"target" : [0]primary expression -> literal
                            ParseTreeNode expr = child.ChildNodes[0];
                            while (expr.ChildNodes.Count > 0)
                                expr = expr.ChildNodes[expr.ChildNodes.Count - 1];
                            controllerInfo[controllerInfo.Count - 1].RenderWith.Add(expr.ToString());
                        }

                        break;


                    default:
                        break;

                }
                #endregion
                AddParseNodeRec(child, skipBranch);
            }
        }
        private string curFieldOrProp;//name of the field that is being processed  

    }
}
