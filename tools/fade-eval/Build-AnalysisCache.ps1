<#
.SYNOPSIS
    Step 1 of the fade suggestion harness: analyses every track that has saved fade sections and caches the result.

.DESCRIPTION
    Reads the saved fade sections (ShufflerFolder\Halloumi.Shuffler.ExtendedAttributes.xml) and the library cache
    (for each track's file and genre), runs BeatAnalysis on each track's audio, and writes:

      <CacheFolder>\<n>.bat     the saved analysis (BeatAnalysis.Save)
      <CacheFolder>\tracks.csv  one row per track: saved fade sections, genre, file, analysis file

    Decoding takes about a second per track, so this is done once; Test-FadeSuggestions.ps1 then works from the cache.
    Nothing is written to the music files or to the Shuffler data.

    MUST be run in 32-bit Windows PowerShell (BASS is 32-bit):
      C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File Build-AnalysisCache.ps1

    Build the solution first (Release, x86). To use more than one core, run several copies with -Shard 0..N-1 -Of N.
#>
param(
    [string]$CacheFolder = (Join-Path $env:LOCALAPPDATA 'Halloumi\FadeEval'),
    [string]$ShufflerFolder,
    [string]$LibraryFile = (Join-Path $env:APPDATA 'Halloumi Shuffler\Halloumi.Shuffler.Library.xml'),
    [int]$Shard = 0,
    [int]$Of = 1,
    [switch]$Force
)

$ErrorActionPreference = 'Stop'
if ([IntPtr]::Size -ne 4) { throw 'Run this in 32-bit PowerShell (C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe).' }

$repo = Split-Path (Split-Path $PSScriptRoot)
if (-not $ShufflerFolder) {
    [xml]$settings = Get-Content (Join-Path $env:APPDATA 'Halloumi Shuffler\Halloumi.Shuffler.Settings.xml')
    $ShufflerFolder = $settings.DocumentElement.ShufflerFolder
}

# work from a copy of the build, so that the build output isn't locked while this runs
$bin = Join-Path $CacheFolder 'bin'
New-Item -ItemType Directory -Force $bin | Out-Null
if ($Shard -eq 0) { Copy-Item (Join-Path $repo 'Halloumi.BassEngine\bin\x86\Release\net48\*.dll') $bin -Force }

[Reflection.Assembly]::UnsafeLoadFrom("$bin\Bass.Net.dll") | Out-Null
[Reflection.Assembly]::UnsafeLoadFrom("$bin\Halloumi.Shuffler.AudioEngine.dll") | Out-Null
$helper = Get-Content (Join-Path $repo 'Halloumi.BassEngine\Helpers\AudioEngineHelper.cs') -Raw
if ($helper -match 'BassNet\.Registration\("([^"]+)",\s*"([^"]+)"\)') { [Un4seen.Bass.BassNet]::Registration($Matches[1], $Matches[2]) }
[Un4seen.Bass.Bass]::LoadMe($bin) | Out-Null
[Un4seen.Bass.Bass]::BASS_Init(0, 44100, [Un4seen.Bass.BASSInit]::BASS_DEVICE_DEFAULT, [IntPtr]::Zero) | Out-Null

# file and genre of each library track, by "artist - title"
[xml]$library = Get-Content $LibraryFile -Encoding UTF8
$byName = @{}
foreach ($t in $library.ArrayOfTrack.Track) {
    $byName[("$($t.Artist) - $($t.Title)").ToLowerInvariant()] = $t
}

[xml]$saved = Get-Content (Join-Path $ShufflerFolder 'Halloumi.Shuffler.ExtendedAttributes.xml') -Encoding UTF8
$rows = New-Object Collections.Generic.List[object]
$index = -1
foreach ($node in $saved.ArrayOfExtenedAttributes.ExtenedAttributes) {
    $a = @{}
    foreach ($pair in $node.Attributes.Split(';')) {
        $kv = $pair.Split('=')
        if ($kv.Length -eq 2) { $a[$kv[0]] = $kv[1] }
    }
    if (-not ($a.ContainsKey('FadeIn') -and $a.ContainsKey('FadeInLengthInSeconds') -and $a.ContainsKey('FadeOut') -and $a.ContainsKey('FadeOutLengthInSeconds'))) { continue }
    $index++

    $track = $byName[$node.Track.ToLowerInvariant()]
    if (-not $track -or -not (Test-Path -LiteralPath $track.Filename)) { continue }

    $cacheFile = Join-Path $CacheFolder "$index.bat"
    if ($index % $Of -eq $Shard -and ($Force -or -not (Test-Path $cacheFile))) {
        $analysis = [Halloumi.Shuffler.AudioEngine.Helpers.BeatAnalysis]::Analyse($track.Filename)
        if ($analysis) { $analysis.Save($cacheFile) }
        if ($index % 50 -eq $Shard) { Write-Host "$index  $($node.Track)" }
    }

    $rows.Add([pscustomobject]@{
        Id = $index
        Track = $node.Track
        Genre = $track.Genre
        Duration = $a['Duration']
        FadeIn = $a['FadeIn']
        FadeInLength = $a['FadeInLengthInSeconds']
        FadeOut = $a['FadeOut']
        FadeOutLength = $a['FadeOutLengthInSeconds']
        SkipStart = $a['SkipStart']
        SkipLength = $a['SkipLengthInSeconds']
        StartLoopCount = $a['StartLoopCount']
        EndLoopCount = $a['EndLoopCount']
        Rank = $a['Rank']
        File = $track.Filename
    })
}

if ($Shard -eq 0) {
    $rows | Export-Csv (Join-Path $CacheFolder 'tracks.csv') -NoTypeInformation -Encoding UTF8
    Write-Host "$($rows.Count) tracks listed in $CacheFolder\tracks.csv"
}
Write-Host "Shard $Shard of $Of finished."
