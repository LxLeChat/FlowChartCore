using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;

namespace FlowChartCore
{
    public class SwitchNode : Node
    {
        protected SwitchStatementAst RawAst {get;set;}

        // Constructor
        public SwitchNode(SwitchStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "SwitchNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

            

            SetChildren();
            
        }

        internal override void SetChildren() {

            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST

            int p = 0;

            // Recuperation des Case
            IEnumerable<StatementBlockAst> Cases = RawAst.GetCases();
            if (Cases != null)
            {
                foreach (var item in Cases)
                {
                    // On appelle CreateNode qui est une extension pour AST
                    children.Add(item.CreateNode(depth+1,p,this,StatementType.SwitchCase));
                    p++;
                }   
            }

            // Recuperation du Default
            StatementBlockAst DefaultCase = RawAst.GetDefault();
            if (DefaultCase != null)
            {
                children.Add(DefaultCase.CreateNode(depth+1,p,this,StatementType.SwitchDefault));
            }
        }

    }
}