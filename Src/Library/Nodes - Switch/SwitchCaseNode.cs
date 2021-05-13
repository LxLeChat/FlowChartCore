using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using ExtensionMethods;
using System;
using System.Linq;

namespace FlowChartCore
{
    public class SwitchCaseNode : Node
    {
        protected StatementBlockAst RawAst {get;set;}
        protected internal string condition;
        public string Condition { get => condition; }
        internal override int OffSetScriptBlockStart {get => RawAst.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Extent.EndOffset-OffSetToRemove-1;}

        public SwitchCaseNode(StatementBlockAst _ast, int _depth, int _position, Node _parent)
        {
            name = "SwitchCaseNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            SetCondition();
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

                    // Because i introduced PipelineAST as a valid AST
                    // stuff like Write-Host will be present in Childs array ...
                    // Maybe it's not such a good idea to create node for Foreach-Object...
                    // Node childnode = item.CreateNode(depth+1,p,this,null); 
                    // if ( null != childnode)
                    // {
                    //     children.Add(childnode);
                    //     tmp = true;
                    //     // p++;
                    // } else if(tmp) {
                    //     children.Add(new CodeNode(depth+1,p,this,null));
                    //     tmp = false;
                    //     // p++;
                    // }
                }
                p++;
            }
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.SwitchCaseBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

        public override void GenerateGraph(bool recursive, bool codeAstText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.SwitchCaseBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive,codeAstText);
                }
            }
        }

        public override void GenerateGraph(bool recursive, bool codeAstText, PowerShell PSinstance){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.SwitchCaseBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive,codeAstText,PSinstance);
                }
            }
        }

        public override String GetEndId() {
            return parent.GetEndId();
        }

        internal override void SetCondition(){
            SwitchStatementAst Truc = (SwitchStatementAst)RawAst.Parent;
            condition = Truc.Clauses.Where(x=> x.Item2 == RawAst).Select(x=>x.Item1.Extent.Text).First();
        }

    }
}