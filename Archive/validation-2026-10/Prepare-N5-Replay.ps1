param([Parameter(Mandatory=$true)][string]$Recording,[Parameter(Mandatory=$true)][string]$SavePath)
$ErrorActionPreference='Stop'
if(Test-Path -LiteralPath $SavePath){throw 'Use a new isolated replay slot; existing save is preserved'}
$data=Get-Content -LiteralPath $Recording -Raw|ConvertFrom-Json
$folder=Split-Path ([IO.Path]::GetFullPath($SavePath)) -Parent;New-Item -ItemType Directory -Path $folder -Force|Out-Null
if($data.checkpoint){@{version=1;dash=$data.dash;doubleJump=$data.doubleJump;bossDefeated=$false;endingReached=$false;checkpointId=$data.checkpoint}|ConvertTo-Json|Set-Content -LiteralPath $SavePath -Encoding utf8}
else{'Recording starts New Game; leave this save path absent.'}
