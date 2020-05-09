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

        public string discovercode(){
            // il faut pouvoir partir du root ... ! HOW ???
            // .ShowAst.Extent.Text.Substring(50,46).Replace('} else {','') : 50 offset du parent, 46 offset du nextnode par exemple,
            // et on enleve le "label" du nextnode.. !
            // si c'est pas le 1° noeud, il faut le getprevious, pour recup l offset de end du noeud précendent
            // si ce 

            // tet créer des extensions pour avoir le offset qui nous interesse parce que
            // dans extent pour un foreach le startoffset ç'est la ligne foreach(...) {}
            // mais on trouve le bon truc dans le body
            // if ( IsFirst && Depth > 0 && !IsLast) {
            //     // int offsetstart = parent.ast.Extent.StartScriptPosition.
            // }

            // if ( IsLast && Depth > 0 && !IsFirst ) {
                
            // }
            // GetStartOffset,GetBodyStartOffset
            // int fora = ForEachStatementExtensions.GetStartOffset((ForEachStatementAst)parent.ast,GetRootNode().parentroot.Ast.Extent.StartOffset);
            // int forb = ForEachStatementExtensions.GetBodyStartOffset((ForEachStatementAst)parent.ast,GetRootNode().parentroot.Ast.Extent.StartOffset);
            // int ifa = IfStatementExtensions.GetStartOffset((IfStatementAst)GetNextNode().ast,GetRootNode().parentroot.Ast.Extent.StartOffset);
            // int ifb = IfStatementExtensions.GetBodyStartOffset((IfStatementAst)GetNextNode().ast,GetRootNode().parentroot.Ast.Extent.StartOffset);
            // Console.WriteLine($"foreach:{fora},{forb}\nif:{ifa},{ifb}");
            return GetRootNode().parentroot.Ast.Extent.Text.Substring(parent.OffSetScriptBlockStart,GetNextNode().OffSetStatementStart-parent.OffSetScriptBlockStart);

            // $y = $v[0].parentroot.ast.extent.text
            // $y.Substring(forb+1,ifa-forb+1) et ça nous donne le code... \o/

            // return 1;
            // return IfStatementExtensions.GetBodyOffsetStartBef((IfStatementAst)GetNextNode().ast);

            //ensuite il faut faire :
            // GetRootNode().parentroot.Ast.Extent.Text.Substring(0,53) et la ça nous retourne le code jusqu a avant le if !
            // maintenant y a plus qu a trouver le offset du parentnode avant le code ! 
        }

    }
}