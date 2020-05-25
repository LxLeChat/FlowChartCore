using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class FinallyNode : Node
    {
        protected StatementBlockAst RawAst {get;set;}
        internal override int OffSetScriptBlockStart {get => RawAst.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Extent.EndOffset-OffSetToRemove-1;}

        public FinallyNode(StatementBlockAst _ast, int _depth, int _position, Node _parent)
        {
            name = "FinallyNode";
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
                    p++;
                }
                else if(FlowChartCore.Utility.GetValidTypes().Contains(item.GetType())){
                    children.Add(item.CreateNode(depth+1,p,this,null));
                    tmp = true;
                    p++;

                    // Because i introduced PipelineAST as a valid AST
                    // stuff like Write-Host will be present in Childs array ...
                    // Maybe it's not such a good idea to create node for Foreach-Object...
                    // Node childnode = item.CreateNode(depth+1,p,this,null); 
                    // if ( null != childnode)
                    // {
                    //     children.Add(childnode);
                    //     tmp = true;
                    //     p++;
                    // } else if(tmp) {
                    //     children.Add(new CodeNode(depth+1,p,this,null));
                    //     tmp = false;
                    //     p++;
                    // }
                }
            }
        }

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.FinallyBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

    }
}