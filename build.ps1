#======================================================================================================#
#                                               SETTINGS                                               #
#======================================================================================================#
$rids = 'win-x64', 'win-arm64', 'linux-x64', 'linux-arm64', 'osx-x64', 'osx-arm64'
$versions = 
     @{ Id='sc'; Params='-p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true'; },
     @{ Id='fd'; Params='-p:PublishSingleFile=true --self-contained false'; }
#    @{ Id='aot'; Params='-p:PublishTrimmed=true -p:PublishAot=true --self-contained true'; }

$project = './BiomeParser.csproj'
$outDir = 'publish'

# Bring some color
function Hl {
    param ([Parameter(Mandatory=$True, Position=1, ValueFromPipeline=$True)][String]$text)

    $fg = $Host.UI.RawUI.ForegroundColor
    $Host.UI.RawUI.ForegroundColor = [ConsoleColor]::Blue
    Write-Output $text
    $Host.UI.RawUI.ForegroundColor = $fg
}

# We'd like to know how long it takes
$stopwatch = [System.Diagnostics.Stopwatch]::new()
$stopwatch.Start()

$count = 1
$total = $rids.Length * $versions.Length
foreach ($rid in $rids) {
    foreach ($version in $versions) {
        $id = $version.Id
        $params = $version.Params
        
        echo "`r`n🧱 Building ${rid}-${id} | ${count}/${total}" | Hl
        $count++
        
        # Build
        $cmd = "dotnet publish ${project} -c Release -r ${rid} -o ${outDir}/${rid}-${id} --p:DebugType=none ${params}"
        echo "`r`n⚡ $cmd" | Hl
        Invoke-Expression $cmd

        # Zip it up
        $zip = "7z a ${outDir}/${rid}-${id}.zip ./${outDir}/${rid}-${id}/* -bb0 -bd"
        echo "`r`n📦 $zip" | Hl
        Invoke-Expression $zip
    }
}

$stopwatch.Stop()
echo "`r`n——————————" | Hl
echo ("⏱ Completed in " + $stopwatch.Elapsed.ToString('hh\:mm\:ss\.fff')) | Hl