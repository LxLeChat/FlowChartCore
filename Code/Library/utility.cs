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
            };

        }

        // Utility Method to parse a PowerShell Scriptblock
        // It will return a List of Nodes
        public static List<Node> ParseScriptBlock(ScriptBlock scriptBlock){

            List<Node> Nodes = new List<Node>();
            Ast NamedBlock = scriptBlock.Ast.Find(Args => Args is NamedBlockAst, false);
            IEnumerable<Ast> enumerable = NamedBlock.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == NamedBlock, false);

            int Position = 1;

            foreach ( var block in enumerable ) {
                Nodes.Add( block.CreateNode(0,Position,null) );
                Position++;
            }

            return Nodes;
        }
    }
}
