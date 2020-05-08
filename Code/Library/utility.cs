using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using ExtensionMethods;
using System;

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
            Tree Arbre = new Tree(Nodes);

            foreach ( var block in enumerable ) {
                var tmpNode = block.CreateNode(0,Position,null,Arbre);
                Nodes.Add(tmpNode);
                Position++;
            }

            return Arbre.Nodes;
        }
    
        public static String CompileDot(Node plop ){
            foreach (var item in plop.children)
            {
                Utility.CompileDot(item);
                plop.Graph.AddRange(item.Graph);
            }
            
            DotNetGraph.DotGraph g = new DotNetGraph.DotGraph("a",true);
            g.Elements.AddRange(plop.Graph);
            DotNetGraph.Compiler.DotCompiler compiler = new DotNetGraph.Compiler.DotCompiler(g);
            
            return compiler.Compile(true);
        }
    }
}
