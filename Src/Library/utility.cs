using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using ExtensionMethods;
using System;
using System.Text.RegularExpressions;
using System.IO;
using DotNetGraph.Core;

namespace FlowChartCore
{
    public abstract class Utility
    {
        // Utility Method to return a list of types
        // that will be searched in the scriptblock,
        // and in every "code block"...
        static public List<Type> GetValidTypes(){

            return new List<Type>() {
                typeof(IfStatementAst),
                typeof(ForEachStatementAst),
                typeof(ForStatementAst),
                typeof(WhileStatementAst),
                typeof(SwitchStatementAst),
                typeof(DoWhileStatementAst),
                typeof(DoUntilStatementAst),
                typeof(TryStatementAst),
                typeof(BreakStatementAst),
                typeof(ContinueStatementAst),
                typeof(ExitStatementAst),
                typeof(ThrowStatementAst),
                typeof(ReturnStatementAst),
                // typeof(FunctionDefinitionAst)
                // typeof(PipelineAst)
            };

        }


        // Static Method to parse a scriptblock
        public static List<Node> ParseScriptBlock(ScriptBlock scriptBlock){

            Ast NamedBlock = scriptBlock.Ast.Find(Args => Args is NamedBlockAst, false);
            // IEnumerable<Ast> enumerable = NamedBlock.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == NamedBlock, false);
            IEnumerable<Ast> enumerable = NamedBlock.FindAll(Args => Args is Ast && Args.Parent == NamedBlock, false);

            int Position = 1;
            List<Node> Nodes = new List<Node>();
            Tree Arbre = new Tree(Nodes,scriptBlock.Ast,NodesOrigin.ScriptBlock);
            bool tmp = true;

            foreach ( var block in enumerable ) {
                if( FlowChartCore.Utility.GetValidTypes().Contains(block.GetType()) )
                {
                    // valid type found
                    Node tmpNode = block.CreateNode(0,Position,null,Arbre);
                    Nodes.Add(tmpNode);
                    // tmpNode.Origin = NodesOrigin.ScriptBlock;

                    Position++;
                    // reset tmp
                    tmp = true;
                } else if ( tmp )
                {
                    // not a valid type, and tmp is false, create code node
                    tmp = false;
                    Node tmpNode = new CodeNode(0,Position,null,Arbre);
                    Nodes.Add(tmpNode);
                    // tmpNode.Origin = NodesOrigin.ScriptBlock;

                    Position++;
                } 
            }

            return Arbre.Nodes;
        }

        // Static Method to parse a script file
        public static List<Node> ParseFile(string file){
            
            string script = File.ReadAllText(file);

            FileInfo ScriptFileInfo = new FileInfo(file);

            ScriptBlock scriptblock = ScriptBlock.Create(script);
            Ast NamedBlock = scriptblock.Ast.Find(Args => Args is NamedBlockAst, false);
            // IEnumerable<Ast> enumerable = NamedBlock.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == NamedBlock, false);
            IEnumerable<Ast> enumerable = NamedBlock.FindAll(Args => Args is Ast && Args.Parent == NamedBlock, false);

            int Position = 1;
            List<Node> Nodes = new List<Node>();
            Tree Arbre = new Tree(Nodes,scriptblock.Ast,NodesOrigin.File,ScriptFileInfo);
            bool tmp = true;

            foreach ( var block in enumerable ) {
                if( FlowChartCore.Utility.GetValidTypes().Contains(block.GetType()) )
                {
                    // valid type found
                    Node tmpNode = block.CreateNode(0,Position,null,Arbre);
                    // tmpNode.Origin = NodesOrigin.File;
                    // tmpNode.FileInfo = ScriptFileInfo;

                    Nodes.Add(tmpNode);
                    Position++;
                    // reset tmp
                    tmp = true;
                } else if ( tmp )
                {
                    // not a valid type, and tmp is false, create code node
                    tmp = false;
                    Node tmpNode = new CodeNode(0,Position,null,Arbre);
                    // tmpNode.Origin = NodesOrigin.File;
                    // tmpNode.FileInfo = ScriptFileInfo;

                    Nodes.Add(tmpNode);
                    Position++;
                }
            }

            return Arbre.Nodes;
        }
    
        // Static method to create dot definition,
        // based on DotnetGraph Library
        public static String CompileDot(List<IDotElement> dotElements ){
            
            
            DotNetGraph.DotGraph g = new DotNetGraph.DotGraph("a",true);
            g.Elements.AddRange(dotElements);
            DotNetGraph.Compiler.DotCompiler compiler = new DotNetGraph.Compiler.DotCompiler(g);
            string compiled = compiler.Compile(true,true);

            // fix issue #51
            // when there is a path  like c:\n\blalal the c:\n was transformed into c:\l
            Regex Rx = new Regex(@"(?<!\\)\\(n|r)");
            string compiledCleaned = Rx.Replace(compiled,"\\l");

            return compiledCleaned;
        }

        public static String CompileDot(List<IDotElement> dotElements, string graphName ){
            
            
            DotNetGraph.DotGraph g = new DotNetGraph.DotGraph($"{graphName}",true);
            g.Elements.AddRange(dotElements);
            DotNetGraph.Compiler.DotCompiler compiler = new DotNetGraph.Compiler.DotCompiler(g);
            string compiled = compiler.Compile(true,true);

            // fix issue #51
            // when there is a path  like c:\n\blalal the c:\n was transformed into c:\l
            Regex Rx = new Regex(@"(?<!\\)\\(n|r)");
            string compiledCleaned = Rx.Replace(compiled,"\\l");

            return compiledCleaned;
        }

        public static List<IDotElement> AddGraph (List<Node> nodes) {
            List<IDotElement> tmp = new List<IDotElement>();
            foreach (var node in nodes)
            {
                tmp.AddRange(node.Graph);
                if (node.children != null)
                {
                    tmp.AddRange(AddGraph(node.children));
                }
            }
            return tmp;
        }
    }
}
