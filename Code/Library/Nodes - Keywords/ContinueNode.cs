using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class ContinueNode : Node
    {
        protected ContinueStatementAst RawAst {get;set;}
        public string Label { get => label;}

        public ContinueNode(ContinueStatementAst _ast, int _depth, int _position, Node _parent)
        {
            name = "ContinueNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            SetLabel();
            
        }

        // Set Label
        internal void SetLabel () {
            label = RawAst.Label.ToString();
        }

    }
}