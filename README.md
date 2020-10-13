# FlowChartCore
The main goal is to generate a dot graph, then it will be consumed by GraphViz, or any tool accepting dot language.
The result of the dot being consumed, will be a PowerShell Script Chart.
You can eventually pass the result to PSGraph to automatically create the graph.

It's the first time i'm writing something in c# ... jumping from Powershell...

# Why ?
First because i had a bad experience at a client, who asked me to add new functiunalities to a HUGE script (actually it was 3 gigantics scripts... anyway... ). Every modification i made had some impacts on other scripts... so i was tired every time to look a all these scripts... and thought ... maybe having a graph representation might make my life easier !

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


## What is not covered ?
Scriptblocks for ``invoke-command``, ``invoke-expression``, ``Foreach-Object {}`` or `` $a = {}``

## What is a node ?
What i call a node is a know AST, like an ``If`` or a ``Foreach`` for example...

## Dot language
To create the dot graph, i use a library called DotNetGraph. https://github.com/vfrz/DotNetGraph

## What can you do? Example
(Add Examples

## Methods
Each node has public methods. One i find useful is the ``FindNodes`` method. It takes a predicat as parameter, and a boolean for recursion.
(add examples)
-
3
