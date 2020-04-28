# FlowChartCore
 FlowChart c# version
 This is an attemp to write my first c# library ...
 This will be used mainly for PowerShell Core. (trying to make it compatible with 5.1 but no succes at the moment)
 The main goal is to generate a dot graph, then it will be consumed by GraphViz, or any tool accepting dot language.
 The result of the dot being consumed, will be a PowerShell Script FlowChart.

# Steps
 - Create Node structure referencing all types for, foreach, if etc..
 - Implement Graph Method to generate a dot file

## Importing the dll
``` powershell
Import-module .\Flowchartcore.dll
```

## Utility Method Implemented:
- ParseScriptBlock
Given the following PowerShell scriptblock:
``` powershell
$a = {
  foreach ($item in $collection) {
      for ($i = 0; $i -lt $array.Count; $i++) {}
      for ($i = 0; $i -lt $array.Count; $i++) {}
      for ($i = 0; $i -lt $array.Count; $i++) {}
  }
}
```
You can do this:
``` powershell
PS \> $x = [FlowChartCore.Utility]::ParseScriptBlock($a)
```
Will return the following:
``` powershell
PS \>  $x
Name     : ForeachNode
Children : {ForNode, ForNode, ForNode}
Parent   :
Position : 1
Depth    : 0

PS \> $x.Children
Name     : ForNode
Children : {}
Parent   : FlowChartCore.ForeachNode
Position : 1
Depth    : 1

Name     : ForNode
Children : {}
Parent   : FlowChartCore.ForeachNode
Position : 2
Depth    : 1

Name     : ForNode
Children : {}
Parent   : FlowChartCore.ForeachNode
Position : 3
Depth    : 1

```

# Done & ToDo:
- [x] Discovering & Implementing Foreach
- [x] Discovering & Implementing For
- [x] Discovering & Implementing If / ElseIf / Else
- [ ] Discovering & Implementing While
- [ ] Discovering & Implementing DoWhile
- [ ] Discovering & Implementing While
- [ ] Discovering & Implementing DoUntil
- [x] Discovering & Implementing Switch / Switch Case / Switch Default
- [ ] Discovering & Implementing Foreach Try / Catch / Finally
- [ ] Discovering & Implementing Loops Labels
- [ ] Discovering & Implementing Keyword: Exit, Return, Break, Continue
