using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class BreakNode : Node
    {
        protected BreakStatementAst RawAst {get;set;}
        public string Label { get => label;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}

        public BreakNode(BreakStatementAst _ast, int _depth, int _position, Node _parent)
        {
            name = "BreakNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            SetLabel();
            
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
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.BreakBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.BreakBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }


    }
}