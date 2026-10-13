$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskUnity='C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe'
$taskDir=Join-Path $taskRoot 'Logs/N5-TF';New-Item -ItemType Directory -Path $taskDir -Force|Out-Null
Add-Content "$PSScriptRoot/N5-progress.txt" "$([datetimeoffset]::UtcNow.ToString('o')) TF START"
foreach($scene in @('Menu','A04','A02','B04','C06','Ending')){
 $tag='N5-TF-'+$scene
 $p=Start-Process $taskUnity -ArgumentList @('-batchmode','-projectPath',('"'+$taskRoot+'"'),'-executeMethod','N5Build.Run','-n5-build',$tag,'-n5-start-scene',$scene,'-logFile',('Logs/N5-TF/build-'+$scene+'.log')) -WindowStyle Hidden -PassThru
 if(!$p.WaitForExit(240000)){Stop-Process -Id $p.Id;throw 'build timeout'}
 if(!(Test-Path "$PSScriptRoot/$tag-build.txt")){throw "Build failed $scene"}
}
$taskRows=[System.Collections.Generic.List[string]]::new()
foreach($resolution in @(@(1280,720),@(1600,900),@(1920,1080),@(2560,1440))){foreach($fullscreen in @(0,1)){foreach($scenario in @('start','hit','abilities','death','B04','boss','ending')){
 $id="$($resolution[0])x$($resolution[1])-$fullscreen-$scenario-D3D11"
 $scene=switch($scenario){'hit'{'A04'}'death'{'A02'}'B04'{'B04'}'boss'{'C06'}'ending'{'Ending'}default{'Menu'}}
 $slot=Join-Path $taskDir ($id+'.json');if($scenario -eq 'abilities'){[IO.File]::WriteAllText($slot,'{"version":1,"dash":true,"doubleJump":true,"bossDefeated":false,"endingReached":false,"checkpointId":"CP-A01"}')}
 $exe=Join-Path $taskRoot "Builds/N5-TF-$scene/Afterglow-N5-TF-$scene.exe"
 $out=Join-Path $PSScriptRoot ('N5-TF-'+$id+'.txt')
 $p=Start-Process $exe -ArgumentList @('-force-d3d11','-screen-width',$resolution[0],'-screen-height',$resolution[1],'-screen-fullscreen',$fullscreen,'-verify-display',$scenario,'-save-path',('"'+$slot+'"'),'-verify-output',('"'+$out+'"'),'-verify-shot',('Validation/N5-TF-shots/'+$id+'.png'),'-logFile',('Logs/N5-TF/'+$id+'.log')) -WindowStyle Normal -PassThru
 if(!$p.WaitForExit(60000)){Stop-Process -Id $p.Id;$taskRows.Add("KNOWN-LIMIT timeout $id")}elseif(Test-Path $out){$taskRows.Add("$id $(Get-Content $out -First 1)")}else{$taskRows.Add("FAIL no evidence $id exit=$($p.ExitCode)")}
}}}
# A second graphics backend is an independent normal-window capture attempt.
$p=Start-Process (Join-Path $taskRoot 'Builds/N5-TF-Menu/Afterglow-N5-TF-Menu.exe') -ArgumentList @('-force-d3d12','-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-verify-display','start','-save-path','Logs/N5-TF/d3d12-save.json','-verify-output','Validation/N5-TF-D3D12.txt','-verify-shot','Validation/N5-TF-shots/1280x720-0-start-D3D12.png','-logFile','Logs/N5-TF/d3d12.log') -WindowStyle Normal -PassThru
 if(!$p.WaitForExit(60000)){Stop-Process -Id $p.Id;$taskRows.Add('KNOWN-LIMIT D3D12 timeout')}elseif(Test-Path "$PSScriptRoot/N5-TF-D3D12.txt"){$taskRows.Add("D3D12 $(Get-Content "$PSScriptRoot/N5-TF-D3D12.txt" -First 1)")}
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'N5-TF-display.txt'),$taskRows)
