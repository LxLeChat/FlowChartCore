using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;

namespace FlowChartCore
{
    public class IfNode : Node
    {
        protected IfStatementAst RawAst { get; set; }

        // Constructor
        public IfNode(IfStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "IfNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

            SetChildren();
            
        }

        internal override void SetChildren()
        {

            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = RawAst.GetChildAst();
            int p = 0;
            foreach (var item in Childs)
            {
                // On appelle CreateNode qui est une extension pour AST
                children.Add(item.CreateNode(Depth + 1, p, this,null));
                p++;
            }

            // Recuperaction des Elsefis
            IEnumerable<StatementBlockAst> ElseIfs = RawAst.GetElseIf();
            if (ElseIfs != null)
            {
                foreach (var item in ElseIfs)
                {
                    // On appelle CreateNode qui est une extension de StatementBlockAST, avec un param enum qui identifie le elseif
                    children.Add(item.CreateNode(Depth + 1, p, this, FlowChartCore.StatementType.ElseIf));
                    p++;
                }
            }

            // Recuperaction du Else
            StatementBlockAst Else = RawAst.GetElse();
            // On appelle CreateNode qui est une extension de StatementBlockAST, avec un param enum qui identifie le else
            if (Else != null)
            {
                children.Add(Else.CreateNode(Depth + 1, p, this, FlowChartCore.StatementType.Else));
            }
        }



    }
}