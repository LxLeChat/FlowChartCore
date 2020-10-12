using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class CatchNode : Node
    {
        protected CatchClauseAst RawAst {get;set;}
        internal override int OffSetScriptBlockStart {get => RawAst.Body.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Body.Extent.EndOffset-OffSetToRemove-1;}

        public CatchNode(CatchClauseAst _ast, int _depth, int _position, Node _parent)
        {
            name = "CatchNode";
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
                    // On appelle CreateNode qui est une extension pour AST
                    // children.Add(item.CreateNode(depth+1,p,this,null));
                    // tmp = true;
                    // p++;


                    // Because i introduced PipelineAST as a valid AST
                    // stuff like Write-Host will be present in Childs array ...
                    // Maybe it's not such a good idea to create node for Foreach-Object...
                    Node childnode = item.CreateNode(depth+1,p,this,null); 
                    if ( null != childnode)
                    {
                        children.Add(childnode);
                        tmp = true;
                        p++;
                    } else if(tmp) {
                        children.Add(new CodeNode(depth+1,p,this,null));
                        tmp = false;
                        p++;
                    }
                }
                // p++;
            }
        }

        public override String GetEndId() {
            // si le try a un finally node, alors le end id devient le finally id
            // Node finallyNode = parent.children.Find(x => x.GetType() == typeof(FinallyNode)) ?? null;
            // if(finallyNode != null ) {
            //     return finallyNode.Id;
            // } else {
                return parent.GetEndId();
            // }
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.CatchBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.CatchBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive,codeAsText);
                }
            }
        }

    }
}