using System.Management.Automation.Language;
using ExtensionMethods;

namespace FlowChartCore
{
    public class ForeachNode : Node
    {
        protected ForEachStatementAst RawAst {get;set;}

        public ForeachNode(ForEachStatementAst item, int Depth, int Position, Node ParentNode)
        {
            name = "ForeachNode";
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