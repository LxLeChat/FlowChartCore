using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;

namespace FlowChartCore
{
    public class SwitchNode : Node
    {
        protected SwitchStatementAst RawAst {get;set;}
        public IEnumerable<StatementBlockAst> Cases {get;set;}
        public StatementBlockAst Default {get;set;}
        public SwitchStatementAst MyProperty { get => RawAst; }

        // Constructor
        public SwitchNode(SwitchStatementAst _ast, int _depth, int _position, Node _parent)
        {
            name = "SwitchNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            

            PopulateChildren();
        }

        internal override void PopulateChildren() {

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
                    children.Add(item.CreateNode(Depth+1,p,this,StatementType.SwitchCase));
                    p++;
                }   
            }

            // Recuperation du Default
            StatementBlockAst DefaultCase = RawAst.GetDefault();
            if (DefaultCase != null)
            {
                children.Add(DefaultCase.CreateNode(Depth+1,p,this,StatementType.SwitchDefault));
            }
        }

    }
}