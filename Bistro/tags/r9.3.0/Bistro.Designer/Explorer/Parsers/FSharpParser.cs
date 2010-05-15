using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Irony.Parsing;
using Irony.Samples.FSharp;

namespace Bistro.Designer.Explorer
{
    class FSharpParser : MetadataParserBase
    {
        public FSharpParser()
            : base()
        {
            grammar = new FSharpGrammar();
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
                //bool isFuncDef = (String.Compare(nodeInfo.ToString(), "FunctionDef") == 0);
                if (String.Compare(nodeInfo.ToString(), "FunctionDef") == 0)
                {
                    //Trace.WriteLine(txt + " of " + nodeInfo.ChildNodes[2] + " :");
                    curCtrl = nodeInfo.ChildNodes[2].ToString();
                    curCtrl = curCtrl.Substring(0, curCtrl.Length - tail.Length);
                    controllerInfo.Add(new ControllerMetadata(curCtrl));
                    curbpi.controller = controllerInfo[controllerInfo.Count - 1];

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

                            while (curChild.ChildNodes.Count > 0)
                            {
                                curChild = curChild.ChildNodes[0];
                            }
                            Trace.WriteLine("<attr name>:" + curChild.ToString());
                            curAttr = curChild.ToString();
                            curAttr = curAttr.Substring(0, curAttr.Length - tail.Length);
                            break;

                        case "attr_arg":
                            bool isFullName = false;
                            /// TODO : refactor this branch - not readable
                            /// 
                            if (curAttr == "Bind")
                            {
                                curbpi.controller = controllerInfo[controllerInfo.Count - 1];
                                //Bind("target") - arg is Literal NT is processed as other common attr_args
                                //Bind("target",Priority=1,ControllerBindType = BindType.After)- args are Literal,BinExpr,BinExpr
                                while (curChild.ChildNodes.Count > 0)
                                {
                                    curChild = curChild.ChildNodes[0];
                                    //Processing of named parameters
                                    if (String.Compare(curChild.ToString(), "BinExpr") == 0)
                                    {
                                        if (curChild.ChildNodes[0].ToString() == "Expr")
                                        {
                                            //process Expr-Literal = Expr->Literal
                                            ParseTreeNode node = (curChild.ChildNodes[0]).ChildNodes[0];
                                            if (node.ToString() == "Literal")
                                            {
                                                curVal = node.ChildNodes[0].ToString() + "= ";//Expr -> Literal->""
                                                curChild = curChild.ChildNodes[2];//right Expr
                                                if (curChild.ChildNodes[0].ToString() == "qual_name_segments_opt")
                                                {
                                                    curChild = (curChild.ChildNodes[0]).ChildNodes[node.ChildNodes.Count - 1];//Literal
                                                    //curVal += curChild.ChildNodes[0].ToString();
                                                }
                                                else
                                                {
                                                    //curVal += (curChild.ChildNodes[0]).ChildNodes[0].ToString();

                                                }
                                                if (curVal.Contains("Priority"))
                                                {
                                                }
                                                else if (curVal.Contains("BindType"))
                                                {
                                                }
                                                else
                                                {
                                                }
                                            }
                                            //TODO switch target,bindType,priority and etc!!!
                                            Trace.WriteLine("<val>:" + curChild.ToString());
                                            controllerInfo[controllerInfo.Count-1].Binds.Add(new BindPointInfo(curbpi));
                                            break;
                                        }

                                    }
                                    else if (String.Compare(curChild.ToString(), "Literal") == 0)
                                    {
                                        while (curChild.ChildNodes.Count > 0)
                                        {
                                            curChild = (isFullName) ? curChild.ChildNodes[curChild.ChildNodes.Count - 1] : curChild.ChildNodes[0];
                                        }
                                        Trace.WriteLine("<val>:" + curChild.ToString());
                                        curVal += curChild.ToString();
                                        //TODO switch target,bindType,priority and etc!!!
                                        curbpi.target = curVal;
                                        controllerInfo[controllerInfo.Count - 1].Binds.Add(new BindPointInfo(curbpi));

                                    }
                                }
                            }
                            else
                            {
                                while (curChild.ChildNodes.Count > 0)
                                {
                                    curChild = (isFullName) ? curChild.ChildNodes[curChild.ChildNodes.Count - 1] : curChild.ChildNodes[0];
                                }
                                Trace.WriteLine("<val>:" + curChild.ToString());
                                curVal += curChild.ToString();
                                //TODO : need to add analyze other attrs ? or do not need
                                //(controllerInfo[curCtrl][curAttr]).Add(curVal);
                            }
                            break;

                        case "SimpleParams1":
                            for (int i = 0; i < curChild.ChildNodes.Count; i++)
                            {
                                curVal = curChild.ChildNodes[i].ToString();
                                curVal = curVal.Substring(0, curVal.Length - tail.Length);
                                controllerInfo[controllerInfo.Count - 1].Requires.Add(curVal);
                            }
                            break;

                        case "SimpleParams2":
                            curVal = curChild.ChildNodes[0].ToString();
                            curVal = curVal.Substring(0, curVal.Length - tail.Length);
                            controllerInfo[controllerInfo.Count - 1].Requires.Add(curVal);
                            break;

                        case "Param":
                            curVal = curChild.ChildNodes[1].ToString();//parameter's name
                            curVal = curVal.Substring(0, curVal.Length - tail.Length);//remove tail (identifier)
                            if (curChild.ChildNodes.Count > 2 && curChild.ChildNodes[4].ChildNodes.Count > 0)//that's option parameter 
                                controllerInfo[controllerInfo.Count - 1].DependsOn.Add(curVal);
                            else
                                controllerInfo[controllerInfo.Count - 1].Requires.Add(curVal);

                            break;

                        /*Note : it works until this NT occurs only inside FunctionBody
                         * so track changes in the FSharpGrammar.cs
                         */
                        case "DummyList":

                            //go down until we reach DummyString - select always last child in branch
                            while (curChild.ChildNodes.Count > 0)
                            {
                                if (String.Compare(curChild.ToString(), "DummyString") == 0)
                                    break;
                                curChild = curChild.ChildNodes[curChild.ChildNodes.Count - 1];
                            }

                            //analysis of last  DummyString in function's body
                            if (curChild.ChildNodes.Count > 0)
                            {
                                if (curChild.ChildNodes[0].ToString() == "Named")
                                {
                                    //{Dummystring} {|> named} {resource}
                                    ParseTreeNode named = curChild.ChildNodes[0];
                                    curVal = named.ChildNodes[2].ToString();
                                    curVal = curVal.Substring(0, curVal.Length - toCut.Length);
                                    controllerInfo[controllerInfo.Count - 1].Provides.Add(curVal);
                                    ParseTreeNode nestedNamed = named.ChildNodes.FindLast(NamedNode);
                                    if (nestedNamed != null)
                                    {
                                        curVal = nestedNamed.ChildNodes[2].ToString();
                                        curVal = curVal.Substring(0, curVal.Length - toCut.Length);
                                        controllerInfo[controllerInfo.Count - 1].Provides.Add(curVal);
                                    }
                                    /*if there is named construction,so it is not likely there would be return values
                                     * like str1,str2,...
                                     * try to skip another DummyStrings
                                    */
                                    skipBranch = true;
                                    break;
                                }
                                else
                                {
                                    for (int i = 1; i < curChild.ChildNodes.Count - 1; i++)
                                    {
                                        ParseTreeNode elem = curChild.ChildNodes[i];
                                        if (elem.ToString() == "Literal" && elem.ChildNodes.Count > 0
                                                 && elem.ChildNodes[0].ToString() == ", (Key symbol)")
                                        {
                                            /*if there are simple identifiers before and after comma then add them to Provides values*/
                                            if (curChild.ChildNodes[i - 1].ToString() == "Literal" &&
                                                 curChild.ChildNodes[i - 1].ChildNodes.Count == 1 &&
                                                 curChild.ChildNodes[i + 1].ToString() == "Literal" &&
                                                 curChild.ChildNodes[i + 1].ChildNodes.Count == 1)//Literal->identifier
                                            {
                                                string param1 = curChild.ChildNodes[i - 1].ChildNodes[0].ToString();
                                                string rem = (param1.Contains(toCut)) ? toCut : tail; //parameter can be either identifier or StringLiteral
                                                param1 = param1.Substring(0, param1.Length - rem.Length);
                                                string param2 = curChild.ChildNodes[i + 1].ChildNodes[0].ToString();
                                                rem = (param2.Contains(toCut)) ? toCut : tail;
                                                param2 = param2.Substring(0, param2.Length - rem.Length);

                                                if (!controllerInfo[controllerInfo.Count - 1].Provides.Contains(param1))
                                                    controllerInfo[controllerInfo.Count - 1].Provides.Add(param1);

                                                if (!controllerInfo[controllerInfo.Count - 1].Provides.Contains(param2))
                                                    controllerInfo[controllerInfo.Count - 1].Provides.Add(param2);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;

                    }
                    #endregion
                    AddParseNodeRec(child, skipBranch);
                }
        }
        /// <summary>
        /// Predicate for FindLast nested Named  node
        /// </summary>
        /// <param name="node">current node of the iteration</param>
        /// <returns>true if current node is Named node,false - otherwise</returns>
        private static bool NamedNode(ParseTreeNode node)
        {
            return String.Compare(node.ToString(), "Named") == 0;

        }

    }
}
