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
    $Keywords,
    [Parameter(Mandatory=$false,
    HelpMessage="Write output to specified file.")]
    [Alias("o")]
    [string]$output="messages.pot")


  

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

    $result = '#, fuzzy
msgid ""
msgstr ""
"POT-Creation-Date: ' + $(Get-Date -Format 'yyyy-mm-dd HH:mmK') + '\n"
"MIME-Version: 1.0\n"
"Content-Type: text/plain; charset=utf-8\n"
"Content-Transfer-Encoding: 8bit\n\n"' + [System.Environment]::NewLine + [System.Environment]::NewLine

    $msgids.GetEnumerator() | ForEach-Object {
        $result = $result + $($_.Value.Locations -join [System.Environment]::NewLine) + [System.Environment]::NewLine + "msgid """ + $_.Key + """" + [System.Environment]::NewLine + "msgstr """"" + [System.Environment]::NewLine + [System.Environment]::NewLine
    }

    if ($output -eq '-') {
        Write-Output $result.ToString() 
    }
    else {
        $result -replace "\r", "" | Out-File -Encoding 'ascii' -NoNewline $output   
    }
}
