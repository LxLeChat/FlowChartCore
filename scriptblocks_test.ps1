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
        if ($a) {
            
        } elseif ($a) {
            for ($i = 0; $i -lt $array.Count; $i++) {
                
            }
        } elseif ($b) {
            foreach ($item in $collection) {
                
            }
        } else {
            "prout"
        }
    }