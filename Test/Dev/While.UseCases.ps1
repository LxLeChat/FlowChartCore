New-CodeUseCase 'While true' {
    While ($true)
    {
        Faitqqchose
    }
}
New-CodeUseCase 'While variable' {
    While ($test)
    {
        Faitqqchose
    }
}

New-CodeUseCase 'While Cmdlet' {
    While (Test-Path Env:Test )
    {
        Faitqqchose
    }
}

New-CodeUseCase 'While Pipeline' {
    While ($Env:Test|ForEach-Object {Test-Path $_} )
    {
        Faitqqchose
    }
}

New-CodeUseCase 'While eq operator' {
    $i = 1
    while ($i -le 5) # loop 5 times
    {
        "{0,1}`t{1,2}" -f $i, ($i*$i)
        ++$i
    }
}

New-CodeUseCase 'While true and Break' {
    While ($true)
    {
        Break
    }
}

New-CodeUseCase 'While true and switch' {
    While ($true)
    {
        $grade = 92
        Switch ($Grade)
        {
            {$grade -ge 90} { "Grade A";Break}
            {$grade -ge 80} { "Grade B"}
            {$grade -ge 70} { "Grade C"}
            {$grade -ge 60} { "Grade D";Break}
        }
    }
}

New-CodeUseCase 'While true , if and Break' {
    While ($true)
    {
        if ($a -eq 1) 
        {
            While ($true)
            {
                Break
            }
        }
    }
}

New-CodeUseCase 'While true , if elseif' {
    While ($true)
    {
        $grade = 92
        if ($grade -ge 90) { "Grade A" }
        elseif ($grade -ge 60) { "Grade D" }
        else { "Grade F"}
        writeLog "Suite"
        break
    }
}

New-CodeUseCase 'While true , if, Break and continue' {
    While ($true)
    {
        if ($a -eq 1) 
        {
            While ($true)
            {
               Continue
            }
        }
    }
}
