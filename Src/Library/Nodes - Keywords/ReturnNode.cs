using System.Management.Automation.Language;
using System.Management.Automation;
using System;

namespace FlowChartCore
{
    public class ReturnNode : Node
    {
        protected ReturnStatementAst RawAst {get;set;}
        private string pipeline;
        public string Pipeline
        {
            get { return pipeline; }
            set {}
        }
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}

        public ReturnNode(ReturnStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "ReturnNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;
            SetOffToRemove();
            SetPipeline();
            
        }

        // Fix Issue #17, throw if return pipeline is empty
        internal void SetPipeline(){
            if ( RawAst.Pipeline != null ){
                pipeline = RawAst.Pipeline.Extent.Text;
            }
        }

        public override String GetEndId() {
            return Id;
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ReturnBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ReturnBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText, PowerShell PSinstance){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ReturnBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        // return the rawast
        public override Ast GetAst() {
            return RawAst;
        }


    }
}