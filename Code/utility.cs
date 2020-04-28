using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public abstract class Utility
    {
        static public List<Type> GetValidTypes(){

            var ValidTypes = new List<Type>() {
                typeof(IfStatementAst),
                typeof(ForEachStatementAst),
                typeof(ForStatementAst),
                typeof(DoWhileStatementAst),
                typeof(WhileStatementAst),
                typeof(SwitchStatementAst),
                typeof(WhileStatementAst),
            };
            
            return ValidTypes;
        }

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
