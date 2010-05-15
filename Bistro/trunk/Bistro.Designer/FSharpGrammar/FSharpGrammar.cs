using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony.Ast;
namespace Irony.Samples.FSharp
{
    [Language("FSharp", "0.1", "Micro-subset of F#, work in progress")]
    public class FSharpGrammar : Irony.Parsing.Grammar
    {
        public FSharpGrammar()
        {
  #region 1. Terminals
            var number = TerminalFactory.CreateCSharpNumber("number");
            var identifier = TerminalFactory.CreateCSharpIdentifier("identifier");
            StringLiteral StringLiteral = TerminalFactory.CreateCSharpString("StringLiteral");
            CommentTerminal SingleLineComment1 = new CommentTerminal("SingleLineComment1", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            CommentTerminal SingleLineComment2 = new CommentTerminal("SingleLineComment2", "#", "\r", "\n", "\u2085", "\u2028", "\u2029");
            CommentTerminal DelimitedComment = new CommentTerminal("DelimitedComment", "(*", "*)");
            NonGrammarTerminals.Add(SingleLineComment1);
            NonGrammarTerminals.Add(SingleLineComment2);
            NonGrammarTerminals.Add(DelimitedComment);
            var comma = ToTerm(",");
            var dot = ToTerm(".");
            var semicolon = ToTerm(";");
  #endregion
  #region 2. Non-terminals
            var Open = new NonTerminal("Open");
            var Expr = new NonTerminal("Expr");
            var Term = new NonTerminal("Term");

            var Literal = new NonTerminal("Literal");
            var BinExpr = new NonTerminal("BinExpr");
            var CondExpr = new NonTerminal("CondExpr");
            var ParExpr = new NonTerminal("ParExpr");
            var UnExpr = new NonTerminal("UnExpr");
            var UnOp = new NonTerminal("UnOp", "operator");
            var BinOp = new NonTerminal("BinOp", "operator");
            var AssignmentStmt = new NonTerminal("AssignmentStmt");
            var Stmt = new NonTerminal("Stmt");
            var ExtStmt = new NonTerminal("ExtStmt");
            var Block = new NonTerminal("Block");
            var StmtList = new NonTerminal("StmtList");

            var Param = new NonTerminal("Param");
            var ParamList = new NonTerminal("ParamList");
            var ParamListExt = new NonTerminal("ParamListExt");
            var SimpleParams1 = new NonTerminal("SimpleParams1");
            var SimpleParams2 = new NonTerminal("SimpleParams2");
            var CombinedParams = new NonTerminal("CombinedParams");
            var ExtParams = new NonTerminal("ExtParams");
            var ArgList = new NonTerminal("ArgList");
            var FunctionCall = new NonTerminal("FunctionCall");
            
            var DummyString = new NonTerminal("DummyString");
            var DummyNested = new NonTerminal("DummyNested");
            var DummyStmt = new NonTerminal("DummyStmt");
            var DummyList = new NonTerminal("DummyList");
            var Named = new NonTerminal("Named");
            var RetValues = new NonTerminal("RetValues");
            var FunctionDef = new NonTerminal("FunctionDef");
            var FuncBody = new NonTerminal("FunctionBody");

            /********************************************************************************/
            //Namespaces,modules,types
            NonTerminal option_opt = new NonTerminal("option_opt");
            NonTerminal type_opt = new NonTerminal("type_opt");
            NonTerminal scope_opt = new NonTerminal("scope_opt"); 

            NonTerminal namespace_declaration = new NonTerminal("namespace_declaration");
            //NonTerminal namespace_declarations_opt = new NonTerminal("namespace_declarations_opt");
            NonTerminal qual_name_segments_opt = new NonTerminal("qual_name_segments_opt");
            NonTerminal namespace_body = new NonTerminal("namespace_body");
            NonTerminal open_directives_opt = new NonTerminal("open_directives_opt");
            NonTerminal in_module = new NonTerminal("in_module");
            NonTerminal modules = new NonTerminal("modules");
            NonTerminal module_declarations_opt = new NonTerminal("module_declarations");
            NonTerminal module_body = new NonTerminal("module_body");
            NonTerminal type_declarations_opt = new NonTerminal("type_declarations");
            NonTerminal type_body = new NonTerminal("type_body");
            NonTerminal block_opt = new NonTerminal("block_opt");
            NonTerminal expr_opt = new NonTerminal("expr_opt");
            NonTerminal else_clause_opt = new NonTerminal("else_clause_opt");
            /*******************************************************************************/
            // Attributes
            NonTerminal attribute_section = new NonTerminal("attribute_section");
            NonTerminal attributes_opt = new NonTerminal("attributes_opt");
            NonTerminal attribute_target_specifier_opt = new NonTerminal("attribute_target_specifier_opt");
            NonTerminal attribute_target = new NonTerminal("attribute_target");
            NonTerminal attribute = new NonTerminal("attribute");
            NonTerminal attribute_list = new NonTerminal("attribute_list");
            NonTerminal attribute_arguments_opt = new NonTerminal("attribute_arguments");
            NonTerminal named_argument = new NonTerminal("named_argument");
            NonTerminal attr_arg = new NonTerminal("attr_arg");
            NonTerminal attribute_arguments_par_opt = new NonTerminal("attribute_arguments_par_opt");
            NonTerminal param_attr_opt = new NonTerminal("param_attr_opt");
  #endregion
  #region 3. BNF rules - base entities
            #region old qual_name_segments
            /* qual_name_segments_opt.Rule can be realized in a  simpler way than:
                qual_name_segments_opt.Rule = MakeStarRule(qual_name_segments_opt, null, qual_name_segment);
                qual_name_segment.Rule = dot + identifier;
                Open.Rule = Empty | "open" + identifier + qual_name_segments_opt + Eos;
                namespace_declaration.Rule = "namespace" + identifier + qual_name_segments_opt + Eos + namespace_body;
                Term.Rule = Literal | ParExpr | identifier | FunctionCall | identifier + qual_name_segments_opt;
            */
            #endregion

            Term.Rule = Literal | qual_name_segments_opt | Empty;
            qual_name_segments_opt.Rule = MakeStarRule(qual_name_segments_opt, dot, Literal);
            Literal.Rule = identifier |number | StringLiteral |"["+StringLiteral+"]"
                | "let" | "[" | "]" | "(" | ")" | "|" | ":" |"&" | "&&" | "+" | "-" | "*" | "/"
                | "::" | "<|" | "|>" | "=" | "->" | "<-" | "'" | ","
                | "<<" | "<<<" | ">>" | ">>>" | "^" | "{" | "}" | Eos;
            Named.Rule = DummyString + "|> named" + StringLiteral;
            RetValues.Rule = MakeStarRule(RetValues, comma, Literal);
            Expr.Rule = Term | UnExpr | BinExpr;
            expr_opt.Rule = Empty | Expr;
            ParExpr.Rule = "(" + expr_opt + ")";
            UnExpr.Rule = UnOp + Term;
            UnOp.Rule = ToTerm("+") | "-";
            BinExpr.Rule = Expr + BinOp + Expr;
            BinOp.Rule = ToTerm("+") | "-" | "*" | "/" | "**"
                            | ">" | "<" | ">=" | "<=" | "<>" | "=" | "compare";
            CondExpr.Rule = "if" + Expr + "then" + Eos + Block + else_clause_opt;
            else_clause_opt.Rule = "else" + Eos + Block | Empty;
            AssignmentStmt.Rule = "let" + identifier + "=" + DummyStmt;
            ArgList.Rule = MakeStarRule(ArgList, comma, Expr);
            Stmt.Rule = AssignmentStmt | Expr;
            ExtStmt.Rule = Stmt + Eos | FunctionDef | module_declarations_opt | type_declarations_opt;
            Block.Rule = Indent + StmtList + Dedent;
            block_opt.Rule = Empty | Eos | Block;
            StmtList.Rule = MakePlusRule(StmtList, ExtStmt);

       #region Functions

            SimpleParams1.Rule = MakeStarRule(SimpleParams1, comma, identifier);
            SimpleParams2.Rule = MakePlusRule(SimpleParams2, identifier);
            param_attr_opt.Rule = "[<param" + ":" + qual_name_segments_opt + ">]" | Empty;
            type_opt.Rule = ":" + qual_name_segments_opt | Empty;
            option_opt.Rule = "option" | Empty;
            scope_opt.Rule = "form" | Empty | "session";
            Param.Rule = param_attr_opt + identifier + type_opt + scope_opt + option_opt;
            ExtParams.Rule = MakePlusRule(ExtParams, Param);
            ParamList.Rule = Empty | SimpleParams1 | SimpleParams2 
                | "(" + MakeStarRule(ParamList, comma, Param) + ")" |"(" + ExtParams + ")";
            ParamListExt.Rule = MakePlusRule(ParamListExt, ParamList);
            
            DummyString.Rule = MakeStarRule(DummyString,Term) | Named;
            DummyStmt.Rule = DummyString | Indent + DummyString | Dedent + DummyString;
            DummyNested.Rule = MakeStarRule(DummyNested,Indent,DummyString);
            DummyList.Rule = Empty | MakeStarRule(DummyList, DummyStmt)| DummyNested;
            FunctionDef.Rule = attributes_opt + "let" + identifier + ParamListExt + "=" + DummyString + Eos + FuncBody;
            FuncBody.Rule = Indent + DummyList + Dedent| Empty;
            FunctionCall.Rule = qual_name_segments_opt + ArgList;
      #endregion

  #endregion

  #region 3. BNF rules -  Namespaces,modules,types,attributes
            Open.Rule = Empty | "open" + qual_name_segments_opt + Eos;
            open_directives_opt.Rule = MakePlusRule(open_directives_opt, Open);
            namespace_declaration.Rule = "namespace" + qual_name_segments_opt + Eos + namespace_body;
            namespace_body.Rule = Indent + open_directives_opt + modules + Dedent;
            modules.Rule = MakeStarRule(modules, module_declarations_opt);
            in_module.Rule = FunctionDef | type_declarations_opt;
            module_declarations_opt.Rule = Empty | "module" + identifier + "=" + Eos + Indent + module_body + Dedent;
            module_body.Rule = MakeStarRule(module_body, in_module);
            //module_body.Rule = MakeStarRule(module_body, FunctionDef);
            type_declarations_opt.Rule = Empty | "type" + identifier + "=" + "{" + Eos + type_body
                | "type" + identifier + "=" + Eos + type_body;
            type_body.Rule = DummyList; 
            attributes_opt.Rule = MakeStarRule(attributes_opt, attribute_section);
            attribute_section.Rule = "[<" + attribute_list + ">]" + Eos;
            attribute_list.Rule = MakePlusRule(attribute_list, semicolon, attribute);
            attribute.Rule = identifier + attribute_arguments_par_opt;
            attribute_arguments_par_opt.Rule = Empty | "(" + attribute_arguments_opt + ")";
            attribute_arguments_opt.Rule = MakeStarRule(attribute_arguments_opt, comma, attr_arg);
            attr_arg.Rule = Expr;
  #endregion

            this.Root = namespace_declaration;       // Set grammar root

            // 4. Token filter
            //we need to add continuation symbol to NonGrammarTerminals because it is not used anywhere in grammar
            NonGrammarTerminals.Add(ToTerm(@"\"));

            // 5. Operators precedence
            RegisterOperators(1, "+", "-");
            RegisterOperators(2, "*", "/");
            RegisterOperators(3, Associativity.Right, "**");

            // 6. Miscellaneous: punctuation, braces, transient nodes
            RegisterPunctuation("(", ")", ":");
            RegisterBracePair("(", ")");
            MarkTransient(Term, Stmt, ExtStmt, UnOp, BinOp, ExtStmt, ParExpr, ParamList);

            // 7. Error recovery rule
            FuncBody.ErrorRule = SyntaxError + Dedent;
            // 8. Syntax error reporting
            AddNoReportGroup("(");
            AddNoReportGroup(Eos);
            AddOperatorReportGroup("operator");

            //9. Initialize console attributes
            ConsoleTitle = "F# Console";
            ConsoleGreeting = @"Irony Sample Console for F#.";
            ConsolePrompt = ">>>";
            ConsolePromptMoreInput = "...";
            //We have to disable scanner-parser link. With TokenFilter sitting between scanner and parser, scanner can no longer 
            // use the expected terminals in current parser state to filter current current candidate terminals
            this.LanguageFlags = LanguageFlags.DisableScannerParserLink | LanguageFlags.CreateAst | LanguageFlags.CanRunSample;


        }//constructor

        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            var outlineFilter = new CodeOutlineFilter(language.GrammarData, OutlineOptions.ProduceIndents, ToTerm(@"\"));
            filters.Add(outlineFilter);

        }

    }

}
