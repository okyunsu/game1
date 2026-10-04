$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
$taskDir=Join-Path $taskRoot 'Logs/N5-TA-r2'
New-Item -ItemType Directory -Path $taskDir -Force | Out-Null
Add-Content "$PSScriptRoot/N5-progress.txt" "$([datetimeoffset]::UtcNow.ToString('o')) TA START"
function Wait-Marker($path,$process){$end=[datetime]::UtcNow.AddSeconds(60);while(!(Test-Path $path)){if($process.HasExited -or [datetime]::UtcNow -gt $end){throw "Marker timeout $path"};Start-Sleep -Milliseconds 200}}
foreach($case in @('readonly','folder','ending')){
 $folder=Join-Path $taskDir $case;New-Item -ItemType Directory -Path $folder -Force|Out-Null
 $slot=Join-Path $folder 'save.json';$marker=Join-Path $taskDir $case
 $data=@{version=1;dash=$true;doubleJump=$true;bossDefeated=($case -eq 'ending');endingReached=($case -eq 'ending');checkpointId='CP-A01'}|ConvertTo-Json -Compress
 [IO.File]::WriteAllText($slot,$data)
 $out=Join-Path $PSScriptRoot "N5-TA-r2-$case.txt"
 $p=Start-Process (Join-Path $taskRoot 'Builds/N5-AB-r2/Afterglow-N5-AB-r2.exe') -ArgumentList @('-screen-fullscreen','0','-save-path',('"'+$slot+'"'),'-verify-save-failure',$case,'-verify-marker',('"'+$marker+'"'),'-verify-output',('"'+$out+'"'),'-logFile',('"'+(Join-Path $folder 'player.log')+'"')) -WindowStyle Normal -PassThru
 $acl=$null
 try{
  if($case -ne 'ending'){
   Wait-Marker ($marker+'.ready') $p
   if($case -eq 'readonly'){(Get-Item $slot).IsReadOnly=$true}else{$acl=Get-Acl -LiteralPath $folder;$restricted=Get-Acl -LiteralPath $folder;$identity=[Security.Principal.WindowsIdentity]::GetCurrent().Name;$rule=[Security.AccessControl.FileSystemAccessRule]::new($identity,[Security.AccessControl.FileSystemRights]::Write,[Security.AccessControl.InheritanceFlags]'ContainerInherit,ObjectInherit',[Security.AccessControl.PropagationFlags]::None,[Security.AccessControl.AccessControlType]::Deny);$restricted.AddAccessRule($rule);Set-Acl -LiteralPath $folder -AclObject $restricted}
   [IO.File]::WriteAllText($marker+'.protected','protected')
   Wait-Marker ($marker+'.failed') $p
   if($case -eq 'readonly'){(Get-Item $slot).IsReadOnly=$false}else{Set-Acl -LiteralPath $folder -AclObject $acl;$acl=$null}
   [IO.File]::WriteAllText($marker+'.restored','restored')
  }
  if(!$p.WaitForExit(90000)){throw "$case timeout"}
 }finally{if($acl){Set-Acl -LiteralPath $folder -AclObject $acl};if(Test-Path $slot){(Get-Item $slot).IsReadOnly=$false};if(!$p.HasExited){Stop-Process -Id $p.Id}}
 Get-Content $out
}
