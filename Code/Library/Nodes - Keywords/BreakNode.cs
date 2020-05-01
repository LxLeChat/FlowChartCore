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

        public BreakNode(BreakStatementAst _ast, int _depth, int _position, Node _parent)
        {
            name = "BreakNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            
        }

        // Set Label
        internal void SetLabel () {
            label = RawAst.Label.ToString();
        }

    }
}