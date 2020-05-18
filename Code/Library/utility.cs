using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using ExtensionMethods;
using System;
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
                typeof(DoWhileStatementAst),
                typeof(WhileStatementAst),
                typeof(SwitchStatementAst),
                typeof(WhileStatementAst),
                typeof(DoWhileStatementAst),
                typeof(DoUntilStatementAst),
                typeof(TryStatementAst),
                typeof(BreakStatementAst),
                typeof(ContinueStatementAst),
                typeof(ExitStatementAst),
                typeof(PipelineAst)
            };

        }


        public static List<Node> ParseScriptBlock(ScriptBlock scriptBlock){

            // List<Node> Nodes = new List<Node>();
            // i'm not sure having a linkedlist with the full object is a good idea ...
            // maybe something lighter ? removing some properties maybe ...
            // LinkedList<Node> LList = new LinkedList<Node>();
            Ast NamedBlock = scriptBlock.Ast.Find(Args => Args is NamedBlockAst, false);
            IEnumerable<Ast> enumerable = NamedBlock.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == NamedBlock, false);

            int Position = 1;
            List<Node> Nodes = new List<Node>();
            Tree Arbre = new Tree(Nodes,scriptBlock.Ast);

            foreach ( var block in enumerable ) {
                var tmpNode = block.CreateNode(0,Position,null,Arbre);

                // added if .. because of pipelineAST handinling...
                if (null != tmpNode) {
                    Nodes.Add(tmpNode);
                    Position++;
                }
            }

            return Arbre.Nodes;
        }
    
        public static String CompileDot(List<IDotElement> dotElements ){
            
            
            DotNetGraph.DotGraph g = new DotNetGraph.DotGraph("a",true);
            g.Elements.AddRange(dotElements);
            DotNetGraph.Compiler.DotCompiler compiler = new DotNetGraph.Compiler.DotCompiler(g);
            return compiler.Compile(true);
        }

        public static List<IDotElement> Plop (List<Node> nodes) {
            List<IDotElement> tmp = new List<IDotElement>();
            foreach (var node in nodes)
            {
                tmp.AddRange(node.Graph);
                if (node.children != null)
                {
                    tmp.AddRange(Plop(node.children));
                }
            }
            return tmp;
        }
    }
}
