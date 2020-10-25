New-CodeUseCase 'Switch simple without Default' {
    Switch ($Grade)
    {
        90 { "Grade A"}
        80 { "Grade B"}
    }
}

New-CodeUseCase 'Switch simple withDefault' {
    Switch ($Grade)
    {
        90 { "Grade A"}
        80 { "Grade B"}
        default { "Grade F" }
    }
}

New-CodeUseCase 'Switch scriptblock with default' {
        Switch ($Grade)
        {
            {$grade -ge 90} { "Grade A"}
            {$grade -ge 80} { "Grade B"}
            {$grade -ge 70} { "Grade C"}
            {$grade -ge 60} { "Grade D"}
            default { "Grade F" }
        }
}

New-CodeUseCase 'Switch scriptblock with default and break' {
    Switch ($Grade)
    {
        {$grade -ge 90} { "Grade A";Break}
        {$grade -ge 80} { "Grade B";Break}
        {$grade -ge 70} { "Grade C";Break}
        {$grade -ge 60} { "Grade D";Break}
        default { "Grade F" }
    }
}

New-CodeUseCase 'Switch -Exact' {
    $test = @{
        Test  = 'test'
        Test2 = 'test2'
    }

    $test.ToString()

    switch -Exact ($test)
    {
        'System.Collections.Hashtable'
        {
            'Hashtable string coercion'
        }
        'test'
        {
            'Hashtable value'
        }
    }
}

New-CodeUseCase 'Switch -Regex' {
    $target = 'https://bing.com'
    switch -Regex ($target)
    {
        '^ftp\://.*$' { "$_ is an ftp address"; Break }
        '^\w+@\w+\.com|edu|org$' { "$_ is an email address"; Break }
        '^(http[s]?)\://.*$' { "$_ is a web address that uses $($matches[1])"; Break }
    }
}

New-CodeUseCase 'Switch -WildCard' {
    switch -wildcard ("c:\data5\archive.zip") 
    {
        '?:\data?\*' {"In some data folder."}
        '*.zip'      {"File is a ZIP."}
    }
}

New-CodeUseCase 'Switch -CaseSensitive' {
    $Grade='A'
    Switch -CaseSensitive ($Grade)
    {
        a { "Grade a"}
        A { "Grade A"}
    }
}

New-CodeUseCase 'Switch -File' {
    $IsXml=$False
    #$FileName=(get-process -pid $pid).Path #test de syntaxe
    switch -file $FileName
    {
        #Début de la section XML 
       "<?xml version=`"1.0`" encoding=`"ibm850`"?>"  {$IsXml=$True;$_;continue}
        #Fin de la section XML 
       "</response>"                                  {$IsXml=$False;$_;break}
        
       default {if ($IsXml)
                  #On traite les lignes si on se trouve dans la section XML 
                 {$_}
               }
    }
}

New-CodeUseCase 'Switch Array' {
    switch (1,4,-1,3,"Hello",2,1)
    {
        {$_ -lt 0} { Continue }
        {$_ -isnot [Int32]} { Break }
        {$_ % 2} {
            "$_ is Odd"
        }
        {-not ($_ % 2)} {
            "$_ is Even"
        }
    }
}


