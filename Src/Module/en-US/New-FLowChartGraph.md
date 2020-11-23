---
external help file: FlowchartCore.dll-Help.xml
Module Name: FlowChartCore
online version:
schema: 2.0.0
---

# New-FLowChartGraph

## SYNOPSIS
Génére un fichier .dot (Graphviz) à partir d'un graph construit à l'aide Find-FLowChartNodes

## SYNTAX

```
New-FLowChartGraph [-Nodes] <System.Collections.Generic.List`1[FlowChartCore.Node]> [-CodeAsText]
 [<CommonParameters>]
```

## DESCRIPTION
des

## EXAMPLES

### basic
@{paragraph=PS C:\\\>}

```
Find-FlowChartNodes -Scriptblock { Get-Childitem } |
 New-FLowChartGraph
```

desc

exemple output

## PARAMETERS

### -Nodes
Listes de noeuds issue d'une analyse par Find-FlowChartNodes

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
Le contenu d'un noeud correspond au code analysé.Par defaut le contenu d'un noeud de code est'Code block'.

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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### intput type
## OUTPUTS

### System.String[]
## NOTES

## RELATED LINKS
