using System.Collections.Generic;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public interface IBuilder
    {
        List<IDotElement> DotDefinition {get;set;}
        void CreateNode();
        void CreateEdgeToNextSibling();
        void CreateEdgeToFirstChildren();
        void CreateEndNode();
    }

    public interface IBuilderIf
    {
        void CreateTrueEdge();
        void CreateFalseEdge();
    }

    public interface IBuilderLoops
    {
        void CreateLoopEdge();
    }

    public interface IBuilderTry
    {
        void CreateCatchEdge();
        void CreateFinallyEdge(Node FinallyNode);
    }

    public interface IBuilderFinally {
        void CreateEndTryEdge ();
    }
    
    public interface IBuilderKeyWords
    {
        void CreateSpecialEdge();
    }

}