function XGetText-Xaml {
    param(
    [Parameter(Mandatory=$true,
               Position=0,
               ParameterSetName="sourceFiles",
               HelpMessage="XAML files to extract msgids from.")]
    [ValidateNotNullOrEmpty()]
    [string[]]$sourceFiles,
    [Parameter(Mandatory=$true,
    HelpMessage="Additional keywords that match MarkupExtensions enclosing msgids to be extracted.")]
    [Alias("k")]
    [string[]]
    $Keywords)

  

    $msgids = @{}
    $sourceFiles | ForEach-Object {
        Select-String $_ -Pattern $("""{[a-z]?[a-z0-9]*:"+$Keywords[0]+ " ([^}]*)}""") | ForEach-Object {
            $msgid = $_.Matches.Groups[1].ToString()
            if (-Not $msgids.ContainsKey($msgid))
            {
               $msgids.Add($msgid, @{Locations = New-Object System.Collections.ArrayList})
            }
            [void] $msgids[$msgid].Locations.Add('#: ' + $_.Filename + ':' + $_.LineNumber)
        } 
    }

    $result = ""
    $msgids.GetEnumerator() | ForEach-Object {
        $result = $result + $($_.Value.Locations -join [System.Environment]::NewLine) + [System.Environment]::NewLine + "msgid """ + $_.Key + """" + [System.Environment]::NewLine + "msgstr """"" + [System.Environment]::NewLine + [System.Environment]::NewLine 
    }

    Write-Output $result.ToString()
}
