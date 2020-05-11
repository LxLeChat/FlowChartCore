using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class SwitchDefaultNode : Node
    {
        protected StatementBlockAst RawAst {get;set;}
        public override int OffSetScriptBlockStart {get => RawAst.Extent.StartOffset-OffSetToRemove+1;}
        public override int OffSetScriptBlockEnd {get => RawAst.Extent.EndOffset-OffSetToRemove-1;}

        public SwitchDefaultNode(StatementBlockAst _ast, int _depth, int _position, Node _parent)
        {
            name = "SwitchDefaultNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            SetOffToRemove();
            SetChildren();
            CreateCodeNode(0);
            
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