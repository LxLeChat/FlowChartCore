using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;

namespace FlowChartCore
{
    public class ForeachNode : Node
    {
        protected ForEachStatementAst RawAst {get;set;}

        public ForeachNode(ForEachStatementAst _ast, int _depth, int _position, Node _parent)
        {
            name = "ForeachNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            PopulateChildren();
        }

        internal override void PopulateChildren() {

            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = RawAst.GetChildAst();
            int p = 0;
            foreach (var item in Childs)
            {
                // On appelle CreateNode qui est une extension pour AST
                children.Add(item.CreateNode(Depth+1,p,this));
                p++;
            }
        }

    }
}