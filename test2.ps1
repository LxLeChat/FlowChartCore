$x="azeazeazeazeaze"
foreach ($item in $collection) {
    
    ForEach-Object -InputObject $x {"ahahah"}
    if ($a) {
        "if"
    } elseif ($b) {
        "elseif"
        for ($i = 0; $i -lt $array.Count; $i++) {
         "for"
     }
    } else {
        "else"
    }

    while ($a) {
        "while"
    }

    do {
        "dowhile"
    } while ($c)

    do {
        "dountil"
    } until ($c)

    try {
        "try"
        "prout"
    }
    catch {
        "catch"
        foreach ($item in $collection) {
            
        }
    } finally {
        "finally"
    }
"x"}