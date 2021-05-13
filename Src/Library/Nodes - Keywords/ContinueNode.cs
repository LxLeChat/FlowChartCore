using System.Management.Automation.Language;
using System.Management.Automation;
using System;

namespace FlowChartCore
{
    public class ContinueNode : Node
    {
        protected ContinueStatementAst RawAst {get;set;}
        public string Label { get => label;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}

        public ContinueNode(ContinueStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "ContinueNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

            SetLabel();
            SetOffToRemove();
            
        }

        // Set Label
        internal void SetLabel () {
            if (RawAst.Label != null)
            {
                label = RawAst.Label.ToString();   
            }
        }

        public override String GetEndId() {
            return Id;
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ContinueBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ContinueBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText, PowerShell PSinstance){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ContinueBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

    }
}