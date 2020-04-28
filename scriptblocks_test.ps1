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
        do {
            foreach ($item in $collection) {
                
            }
        } while ($a)
    
    }