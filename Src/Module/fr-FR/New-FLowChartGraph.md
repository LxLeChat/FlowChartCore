---
external help file: FlowchartCore.dll-Help.xml
Module Name: FlowChartCore
online version:
schema: 2.0.0
---

# New-FLowChartGraph

## SYNOPSIS
Génére un fichier .dot (Graphviz) à partir d'un graph construit à l'aide Find-FLowChartNodes.

## SYNTAX

```
New-FLowChartGraph [-Nodes] <System.Collections.Generic.List`1[FlowChartCore.Node]> [-CodeAsText]
 [<CommonParameters>]
```

## DESCRIPTION
La visualisation d'un graph construit par Find-FLowChartNodes se fait à l'aide de Graphviz.
Cet outil tier nécessite un fichier au format .dot

## EXAMPLES

### Genération d'un graph au format .dot
@{paragraph=PS C:\\\>}

```
$Sb={
 If ( $x ) 
 {
    Foreach ( $y in $x ) 
    { "Do Something" }
 }
}
Find-FlowChartNodes -Scriptblock $Sb|
 New-FLowChartGraph

#digraph "a" {
#        "01"[label="If $x",shape=diamond];
#        "end_01"[shape=mdiamond,label="End If"];
#        "01" -> "0110"[label="True"];
#        "01" -> "end_01"[label="False"];
#        "end_01" -> "end_of_script";
#        "0110"[label="CodeBlock"];
#        "0110" -> "end_01";
#}
```

Description

Dans cette exemple le résultat est une chaîne de caractères contenant une déclaration de graph interprété par l'outil Dot.exe de Graphviz.

.
### Genération d'un graph détaillé au format .dot
@{paragraph=PS C:\\\>}

```
$Sb={
 If ( $x ) 
 {
    "Do Something" 
 }
}
Find-FlowChartNodes -Scriptblock $Sb|
 New-FLowChartGraph  -CodeAsText

#digraph "a" {
#        "01"[label="If $x",shape=diamond];
#        "end_01"[shape=mdiamond,label="End If"];
#        "01" -> "0110"[label="True"];
#        "01" -> "end_01"[label="False"];
#        "end_01" -> "end_of_script";
#        "0110"[label="\"Do Something\"",shape=box];
#        "0110" -> "end_01";
#}
```

Description

Dans cette exemple le label associé au code liée au If contient [label="\"Do Something\"] et plus le label par défaut [label="CodeBlock"].

.
### Genération d'un graph et affichage d'un graphique au format png
@{paragraph=PS C:\\\>}

```
$Sb={
 If ( $x ) 
 {
    "Do Something" 
 }
}

$Path=$Env:Temp
$FileName='Test'

$FullFileName=Join-Path $Path $FileName
$GraphFilename="$FullFileName.graph"

$Result=Find-FLowChartNodes -ScriptBlock $Sb|New-FLowChartGraph -CodeAsText:$AsText
if ($null -ne $Result) 
{ 
    $GraphFilename="$FullFileName.graph"
    $Result|Set-Content $GraphFilename
    if (Test-Path Env:GRAPHVIZ_DOT)
    {
    if ((Test-Path $GraphFilename) -and ((Get-Item $GraphFilename).length -ne 0))
    {
        &"$Env:GRAPHVIZ_DOT\dot.exe" -Tpng $GraphFilename -o "$FullFileName.png"
        if (! $NoDisplay.IsPresent)
        { Invoke-Item "$FullFileName.png" }
    }
    else 
    { Write-Warning "No file or zero size file." }
    }
}
```

Description

Dans cette exemple on analyse un scriptblock, puis on génére le .dot représentant le graph, enfin on génére l'image représentant le logigramme (l'enchaînement des opérations) du code Powershell.

.
## PARAMETERS

### -Nodes
Listes de noeuds issue d'une analyse par Find-FlowChartNodes.

```yaml
Type: System.Collections.Generic.List`1[FlowChartCore.Node]
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -CodeAsText
Le contenu d'un noeud correspond au code analysé. Par défaut le contenu d'un noeud de code est 'Codeblock'.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
Ce cmdlet supporte les paramètres communs suivant : -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. Pour plus d'information, voir [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### [System.Collections.Generic.List[[FlowChartCore.Node]]
## OUTPUTS

### System.String
## NOTES

## RELATED LINKS
