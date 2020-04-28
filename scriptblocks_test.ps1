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
        try {
            foreach ($item in $collection) {
                
            }
        }
        catch {
            for ($i = 0; $i -lt $array.Count; $i++) {
                
            }
        }
        finally {
            if ($a) {
                
            }
        }
    
    }