# FlowChartCore
The main goal is to generate a dot graph, then it will be consumed by GraphViz, or any tool accepting dot language.
The result of the dot being consumed, will be a PowerShell Script Chart.
You can eventually pass the result to PSGraph to automatically create the graph.

It's the first time i'm writing something in c# ... jumping from Powershell...
mdlet with C#

## Importing the dll
The Dll can be found in ``Code\bin\debug\netstandard2.0\FlowChartCore.dll``
``` powershell
Import-module .\Flowchartcore.dll
```

## Available cmdlets
- ``Find-FlowChartNodes``
- ``New-FlowChartGraph``

# Find-FlowChartNodes
Returns a List of nodes.
The cmdlet will find nodes in a file (a ps1) or a scriptblock.
(pipeline support not implemented yet)

# New-FlowChartGraph
Returns a graph definition based on dot language.


## What is a node ?
What i call a node is a know AST, like an ``If`` or a ``Foreach`` for example...

## Dot language
To create the dot graph, i use a library called DotNetGraph. https://github.com/vfrz/DotNetGraph

## What can you do? Example
(Add Examples

## Methods
Each node has public methods. One i find useful is the ``FindNodes`` method. It takes a predicat as parameter, and a boolean for recursion.
(add examples)