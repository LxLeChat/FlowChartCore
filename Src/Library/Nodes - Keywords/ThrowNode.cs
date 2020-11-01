using System.Management.Automation.Language;
using System;

namespace FlowChartCore
{
    public class ThrowNode : Node
    {
        protected ThrowStatementAst RawAst {get;set;}
        public string Label { get => label;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}

        public ThrowNode(ThrowStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "ThrowNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;
            SetOffToRemove();
            
        }

        public override String GetEndId() {
            return Id;
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ThrowBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ThrowBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }


        // return the rawast
        public override Ast GetAst() {
            return RawAst;
        }


    }
}