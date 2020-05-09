using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;

namespace FlowChartCore
{
    public class DoWhileNode : Node
    {
        protected DoWhileStatementAst RawAst {get;set;}
        public string Label { get => label;}
        public override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}
        public override int OffSetScriptBlockStart {get => RawAst.Body.Extent.StartOffset-OffSetToRemove+1;}

        public DoWhileNode(DoWhileStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "WhileNode";
            position = _position;
            depth = _depth;
            RawAst = _ast;
            parent = _parent;
            parentroot = _tree;

            SetOffToRemove();
            SetLabel();
            SetChildren();
            CreateCodeNode(0);
            
        }

        // Set Label
        internal void SetLabel () {
            label = RawAst.Label;
        }

        internal override void SetChildren() {

            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = RawAst.GetChildAst();
            
            int p = 0;
            bool tmp = true;

            foreach (var item in Childs)
            {
                if (tmp && !FlowChartCore.Utility.GetValidTypes().Contains(item.GetType()) ) {
                    children.Add(new CodeNode(depth+1,p,this,null));
                    tmp = false;
                }
                else if(FlowChartCore.Utility.GetValidTypes().Contains(item.GetType())){
                    // On appelle CreateNode qui est une extension pour AST
                    children.Add(item.CreateNode(depth+1,p,this,null));
                    tmp = true;
                }
                p++;
            }
        }
    }
}