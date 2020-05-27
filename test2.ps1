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
        $LicenseUrl = Read-Host -Prompt   '[OPTIONAL] Enter the license URL'
$AppMoniker = Read-Host -Prompt   '[OPTIONAL] Enter the AppMoniker (friendly name). For example: vscode'
$Tags = Read-Host -Prompt   '[OPTIONAL] Enter any tags that would be useful to discover this tool. For example: zip, c++'
$Homepage = Read-Host -Prompt   '[OPTIONAL] Enter the Url to the homepage of the application'
$Description = Read-Host -Prompt '[OPTIONAL] Enter a description of the application'



##########################################
# Write  metadata
##########################################

$OFS
$string = "Id: " + $id
write-output $string | out-file $filename
write-host "Id: "  -ForeGroundColor Blue -NoNewLine
write-host $id  -ForeGroundColor White  
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
            
##########################################
# Read in metadata
##########################################

While ($id.Length -eq 0) {
    write-host  'Enter the package Id, in the following format <Publisher.Appname>' 
    $id = Read-Host -Prompt 'For example: Microsoft.Excel'
    }
        }
    } finally {
        "finally"
    }
"x"}