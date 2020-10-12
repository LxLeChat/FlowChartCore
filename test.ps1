## Corrigé
$sb={
    $grade = 92
    Switch ($Grade)
    {
        {$grade -ge 90} { "Grade A";Break}
        {$grade -ge 80} { "Grade B";Break}
        {$grade -ge 70} { "Grade C";Break}
        {$grade -ge 60} { "Grade D";Break}
        default { "Grade F" }
    }
}

$sb={
    $grade = 92
    Switch ($Grade)
    {
        {$grade -ge 90} { "Grade A";Break}
        # {$grade -ge 80} { "Grade B";Break}
        # {$grade -ge 70} { "Grade C";Break}
        # {$grade -ge 60} { "Grade D";Break}
        # default { "Grade F" }
    }
 }


$sb={
    $grade = 92
    Switch ($Grade)
    {
        {$grade -ge 90} { "Grade A";Break}
        # {$grade -ge 80} { "Grade B";Break}
        # {$grade -ge 70} { "Grade C";Break}
        # {$grade -ge 60} { "Grade D";Break}
         default { "Grade F" }
    }
    writelog "suite" #   !!!!!!!!  n'est pas considéré dans le graph  !!!!!!!!!!
 }