using System.Management.Automation.Language;
using System;

namespace FlowChartCore
{
    public class FunctionNode : Node
    {
        protected FunctionDefinitionAst RawAst {get;set;}
        private string funcname;
        public string FuncName
        {
            get { return funcname; }
            set {}
        }
        
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}
        internal override int OffSetScriptBlockStart {get => RawAst.Body.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Body.Extent.EndOffset-OffSetToRemove-1;}

        public FunctionNode(FunctionDefinitionAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "FunctionNode";
            position = _position;
            depth = _depth;
            RawAst = _ast;
            parent = _parent;
            parentroot = _tree;

            SetFuncName();
            SetOffToRemove();
            
        }

        // Set FuncName
        internal void SetFuncName(){
            funcname = RawAst.Name;
        }
        
        // public override void GenerateGraph(bool recursive){
        //     Graph.Clear();
        //     FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ForBuilder(this);
        //     Graph.AddRange(x.DotDefinition);

        //     if(recursive) {
        //         foreach (var child in Children) {
        //             child.GenerateGraph(recursive);
        //         }
        //     }
        // }

        // public override void GenerateGraph(bool recursive, bool codeAsText){
        //     Graph.Clear();
        //     FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ForBuilder(this);
        //     Graph.AddRange(x.DotDefinition);

        //     if(recursive) {
        //         foreach (var child in Children) {
        //             child.GenerateGraph(recursive,codeAsText);
        //         }
        //     }
        // }

        public override String GetEndId() {
            return $"end_{Id}";
        }

        public override Ast GetAst() {
            return RawAst;
        }

    }
}