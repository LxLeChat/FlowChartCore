using System.Management.Automation.Language;
using ExtensionMethods;

namespace FlowChartCore
{
    public class ForNode : Node
    {
        protected ForStatementAst RawAst {get;set;}

        public ForNode(ForStatementAst item, int Depth, int Position, Node ParentNode)
        {
            name = "ForNode";
            position = Position;
            depth = Depth;
            RawAst = item;
            parent = ParentNode;

            FindChildren();
        }
        
        public void FindChildren () {
            children = RawAst.CreateChildNodes(Depth+1,this);
        }
    }
}