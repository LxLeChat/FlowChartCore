using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

// Faudrait essayer de recup le code justement... !
// par exemple:
// $a = {
//    foreach ($item in $collection) {
//        $item.whatevermethod()
//        $plop = "aaa"
//        if($x){
           
//        }
//    }
// }
//  la propriété code serait uniquement ça:
//        $item.whatevermethod()
//        $plop = "aaa"
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

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.CodeNodeBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override String GetEndId() {
            return Id;
        }

    }
}