using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;

namespace FlowChartCore
{
    public class CodeNode : Node
    {
        public CodeNode(int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "CodeNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            parentroot = _tree;
            
        }

    }
}