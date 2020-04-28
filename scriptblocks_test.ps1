$f = { foreach ($item in $collection) { for ($i = 0; $i -lt $array.Count; $i++) {} };for ($i = 0; $i -lt $array.Count; $i++) {} }

$a = {
    foreach ($item in $collection) {
        for ($i = 0; $i -lt $array.Count; $i++) { foreach ($item in $collection) {} }
        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
        for ($i = 0; $i -lt $array.Count; $i++) {}
    }
}

    $b = {
        foreach ($item in $collection) {
            switch ($a) {
                1 { for ($i = 0; $i -lt $array.Count; $i++) {
                    
                } }
                2 { for ($i = 0; $i -lt $array.Count; $i++) {
                    
                } }
                Default { foreach ($item in $collection) {
                    
                }}
            }
        }
    
    }