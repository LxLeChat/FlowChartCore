$f = { foreach ($item in $collection) { for ($i = 0; $i -lt $array.Count; $i++) {} };for ($i = 0; $i -lt $array.Count; $i++) {} }

$a = {
    if ($a) {
        foreach ($item in $collection) {
            
        }
    } else {

    }
}

    $b = {
        :testlabel for ($i = 0; $i -lt $array.Count; $i++) {
            if ($a) {
                if ($b) {
                    for ($i = 0; $i -lt $array.Count; $i++) {
                        if ($x) {
                            
                        }
                    }
                }    
            }
            
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {}
        }

        for ($y = 0; $y -lt $array.Count; $y++) {
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {}
        }

        foreach ($item in $collection) {
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {
                foreach ($item in $collection) {
                    
                }
            }
        }

        if ($a) {
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {}
            for ($i = 0; $i -lt $array.Count; $i++) {}
        }

        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
    }


 dir *.dll | Import-Module
$v=[FlowChartCore.Utility]::ParseScriptBlock($a)
$v.GenerateGraph($true)
$v.Children[0].Children.graph | %{$v.Children[0].Graph.Add($_)}
$v.Children[0].Graph | %{$v[0].Graph.Add($_)}
$v.Children[1].Children.graph | %{$v.Children[1].Graph.Add($_)}
$v.Children[1].Graph | %{$v[1].Graph.Add($_)}
$g=[DotNetGraph.DotGraph]::new("a",$true)
$g.Elements.AddRange($v[0].Graph)
$compiler=[DotNetGraph.Compiler.DotCompiler]::new($g)
$compiler.Compile($true)