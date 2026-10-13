param([ValidateSet('MoveA2','MoveA3','Commit','Regression','Editor','Launch','Finish')][string]$Operation,[string]$Tag,[string]$Method,[string[]]$Files,[string[]]$Arguments,[switch]$Quiet)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
Set-Location -LiteralPath $taskRoot
switch($Operation){
 'MoveA2'{foreach($taskMove in (Get-Content Validation/_raw/B13/A2-map.json -Raw | ConvertFrom-Json)){if(!(Test-Path -LiteralPath $taskMove.Source)){continue};if(Test-Path -LiteralPath $taskMove.Dest){throw "Preserve $($taskMove.Dest)"};$taskTracked=@(git ls-files -- $taskMove.Source);if($taskTracked.Count){git mv -- $taskMove.Source $taskMove.Dest;if($LASTEXITCODE){throw 'git mv failed'}}else{Move-Item -LiteralPath $taskMove.Source -Destination $taskMove.Dest}}}
 'MoveA3'{New-Item -ItemType Directory -Force Archive/tools-b10 | Out-Null;foreach($taskFile in $Files){git mv -- ('Tools/'+$taskFile) ('Archive/tools-b10/'+$taskFile);if($LASTEXITCODE){throw 'git mv failed'}}}
 'Commit'{git add -- $Files;if($LASTEXITCODE){throw 'git add failed'};git commit -m $Tag;if($LASTEXITCODE){throw 'git commit failed'}}
 'Regression'{& ./Tools/Run-Regression.ps1 -Tag $Tag -Quiet:$Quiet;exit $LASTEXITCODE}
 'Editor'{$taskLog="Validation/_raw/B13/$Tag.log";$taskArgs=@('-batchmode','-nographics','-projectPath',('"'+$taskRoot+'"'),'-executeMethod',$Method,'-logFile',('"'+(Join-Path $taskRoot $taskLog)+'"'))+$Arguments;$taskProcess=Start-Process 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe' -ArgumentList $taskArgs -WindowStyle Hidden -PassThru;if(!$taskProcess.WaitForExit(900000)){throw 'Unity timeout'};Write-Output "Unity $Tag exit=$($taskProcess.ExitCode)";exit $taskProcess.ExitCode}
 'Launch'{& ./Launchers/current/Start-V4.ps1 @Arguments;exit $LASTEXITCODE}
 'Finish'{if((git rev-parse HEAD) -eq '12892b9e23904443261f23c891082c0ce535361b'){throw 'No batch commits'};git reset --soft 12892b9e23904443261f23c891082c0ce535361b;git commit --file Validation/_raw/B13/commit-message.txt;git tag B13}
}
