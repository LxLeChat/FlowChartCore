// using System.Management.Automation.Language;
using System.Collections.Generic;
using FlowChartCore;
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
        // void CreateLoopNode();
    }

    public interface IBuilderTry
    {
        void CreateCatchEdge();
        void CreateFinallyEdge(Node FinallyNode);
    }

    public interface IBuilderBreak
    {
        void CreateBreakEdge();
    }

}