$a={
   foreach ($item in $collection) {
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
}

    $a={
        :prout foreach ($item in $collection) {
            if ($a) {
                "if"
            } elseif ($b) {
                "elseif"
                for ($i = 0; $i -lt $array.Count; $i++) {
                "for"
                if ( $i -eq 1 ) {
                    continue
                }
            }
            } else {
                "else"
                break
            }
        }
    }