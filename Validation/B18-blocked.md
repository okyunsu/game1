START 2026-10-18T23:14:31.3726612+09:00
BASE e140f144e4df8c42f7cf454de11b381c953f3dfb
UNITY_STARTUP exit=-536870911
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.24f1 (4e7b9b5b6244) revision 5143451'; Using compiler version '194234433'; Build Type 'Release'
OS: 'Windows 11  (10.0.26200) Core' Language: 'ko' Physical Memory: 31849 MB
[Licensing::IpcConnector] Connection to channel LicenseClient-gosu3 refused
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
System  architecture: x64
Process architecture: x64
Date: 2026-10-18T14:14:31Z
Assertion failed on expression: 'SUCCEEDED(hr)'
Assertion failed on expression: 'wmiOpened'
Assertion failed on expression: 'ERROR_SUCCESS == status'
[Licensing::Module] Successfully launched the LicensingClient (PId: 29076)

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe
-batchmode
-nographics
-quit
-projectPath
C:\Users\gosu3\OneDrive\문서\ChatGPT\game1
-logFile
C:\Users\gosu3\OneDrive\문서\ChatGPT\game1\Validation\_raw\B18\startup-license.log
Successfully changed project path to: C:\Users\gosu3\OneDrive\문서\ChatGPT\game1
C:/Users/gosu3/OneDrive/문서/ChatGPT/game1
[UnityMemory] Configuration Parameters - Can be set up in boot.config
    "memorysetup-temp-allocator-size-nav-mesh-worker=65536"
    "memorysetup-temp-allocator-size-audio-worker=65536"
    "memorysetup-temp-allocator-size-gfx=262144"
    "memorysetup-allocator-temp-initial-block-size-main=262144"
    "memorysetup-allocator-temp-initial-block-size-worker=262144"
    "memorysetup-temp-allocator-size-background-worker=32768"
    "memorysetup-temp-allocator-size-job-worker=262144"
    "memorysetup-temp-allocator-size-preload-manager=33554432"
    "memorysetup-remapper-allocator-initial-capacity=32768"
    "memorysetup-temp-allocator-size-gi-baking-worker=262144"
    "memorysetup-temp-allocator-size-gi-baking-worker=262144"
    "memorysetup-temp-allocator-size-gi-baking-worker=262144"
    "memorysetup-temp-allocator-size-gi-baking-worker=262144"
    "memorysetup-temp-allocator-size-gi-baking-worker=262144"
    "memorysetup-bucket-allocator-granularity=16"
    "memorysetup-bucket-allocator-bucket-count=8"
    "memorysetup-bucket-allocator-block-size=33554432"
    "memorysetup-bucket-allocator-block-count=8"
    "memorysetup-main-allocator-block-size=16777216"
    "memorysetup-thread-allocator-block-size=16777216"
    "memorysetup-gfx-main-allocator-block-size=16777216"
    "memorysetup-gfx-thread-allocator-block-size=16777216"
    "memorysetup-cache-allocator-block-size=4194304"
    "memorysetup-typetree-allocator-block-size=2097152"
    "memorysetup-profiler-bucket-allocator-granularity=16"
    "memorysetup-profiler-bucket-allocator-bucket-count=8"
    "memorysetup-profiler-bucket-allocator-block-size=33554432"
    "memorysetup-profiler-bucket-allocator-block-count=8"
    "memorysetup-profiler-allocator-block-size=16777216"
    "memorysetup-profiler-editor-allocator-block-size=1048576"
    "memorysetup-temp-allocator-size-main=16777216"
    "memorysetup-job-temp-allocator-block-size=2097152"
    "memorysetup-job-temp-allocator-block-size-background=1048576"
    "memorysetup-job-temp-allocator-reduction-small-platforms=262144"
    "memorysetup-temp-allocator-size-cloud-worker=32768"
Player connection [8160]  Target information:

Player connection [8160]  * "[IP] 172.30.1.93 [Port] 55504 [Flags] 2 [Guid] 2451411042 [EditorId] 2451411042 [Version] 1048832 [Id] WindowsEditor(7,pracro) [Debug] 1 [PackageName] WindowsEditor [ProjectName] Editor" 

Player connection [8160] Host joined multi-casting on [225.0.0.222:54997]...
Player connection [8160] Host joined alternative multi-casting on [225.0.0.222:34997]...
Input System module state changed to: Initialized.
[Physics::Module] Initialized fallback backend.
[Physics::Module] Id: 0xdecafbad
[Package Manager] Connected to IPC stream "Upm-29156" after 0.1 seconds.
[Licensing::Module] Licensing is not yet initialized.
[Licensing::Module] Error: Timed-out after 60.01s, waiting for Licensing to initialize
Library Redirect Path: Library/
[Physics::Module] Selected backend.
[Physics::Module] Name: PhysX
[Physics::Module] Id: 0xf2b8ea05
[Physics::Module] SDK Version: 4.1.2
[Physics::Module] Integration Version: 1.0.0
[Physics::Module] Threading Mode: Multi-Threaded
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Initialize engine version: 6000.3.24f1 (4e7b9b5b6244)
[Subsystems] Discovering subsystems at path C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data/Resources/UnitySubsystems
[Subsystems] Discovering subsystems at path C:/Users/gosu3/OneDrive/문서/ChatGPT/game1/Assets
Forcing GfxDevice: Null
GfxDevice: creating device client; kGfxThreadingModeNonThreaded
NullGfxDevice:
    Version:  NULL 1.0 [1.0]
    Renderer: Null Device
    Vendor:   Unity Technologies
Initialize mono
Mono path[0] = 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data/Managed'
Mono path[1] = 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data/MonoBleedingEdge/lib/mono/unityjit-win32'
Mono config path = 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data/MonoBleedingEdge/etc'
Using monoOptions --debugger-agent=transport=dt_socket,embedding=1,server=y,suspend=n,address=127.0.0.1:56156
Using cacheserver namespaces - metadata:defaultmetadata, artifacts:defaultartifacts
Using cacheserver namespaces - metadata:defaultmetadata, artifacts:defaultartifacts
ImportWorker Server TCP listen port: 0
AcceleratorClientConnectionCallback - disconnected - :0
Begin MonoManager ReloadAssembly
Registering precompiled unity dll's ...
Register platform support module: C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data/PlaybackEngines/WindowsStandaloneSupport/UnityEditor.WindowsStandalone.Extensions.dll
Registered in 0.175518 seconds.
- Loaded All Assemblies, in  6.789 seconds
Native extension for WindowsStandalone target not found
Package Manager log level set to [2]
ScheduleIndexationOnStartup MainProcess:False IndexOnStartup:True
Mono: successfully reloaded assembly
- Finished resetting the current domain, in  0.317 seconds
Domain Reload Profiling: 7100ms
	BeginReloadAssembly (5394ms)
		ExecutionOrderSort (0ms)
		DisableScriptedObjects (0ms)
		BackupInstance (0ms)
		ReleaseScriptingObjects (0ms)
		CreateAndSetChildDomain (11ms)
	RebuildCommonClasses (330ms)
	RebuildNativeTypeToScriptingClass (10ms)
	initialDomainReloadingComplete (231ms)
	LoadAllAssembliesAndSetupDomain (817ms)
		LoadAssemblies (5374ms)
		RebuildTransferFunctionScriptingTraits (0ms)
		AnalyzeDomain (814ms)
			TypeCache.Refresh (811ms)
				TypeCache.ScanAssembly (621ms)
			BuildScriptInfoCaches (1ms)
			ResolveRequiredComponents (1ms)
	FinalizeReload (318ms)
		ReleaseScriptCaches (0ms)
		RebuildScriptCaches (0ms)
		SetupLoadedEditorAssemblies (289ms)
			LogAssemblyErrors (0ms)
			InitializePlatformSupportModulesInManaged (37ms)
			SetLoadedEditorAssemblies (2ms)
			BeforeProcessingInitializeOnLoad (59ms)
			ProcessInitializeOnLoadAttributes (113ms)
			ProcessInitializeOnLoadMethodAttributes (77ms)
			AfterProcessingInitializeOnLoad (0ms)
			EditorAssembliesLoaded (0ms)
		ExecutionOrderSort2 (0ms)
		AwakeInstancesAfterBackupRestoration (0ms)
[Licensing::Module] Licensing is not yet initialized.
Crash!!!
SymInit: Symbol-SearchPath: 'C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Data/Mono;.;C:\Users\gosu3\OneDrive\����\ChatGPT\game1;C:\Users\gosu3\OneDrive\����\ChatGPT\game1\Library\BurstCache\JIT;C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor;C:\Windows;C:\Windows\syste', symOptions: 534, UserName: 'gosu3'
OS-Version: 10.0.0
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe:Unity.exe (00007FF6886E0000), size: 290816 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 6000.3.24.31643
C:\Windows\SYSTEM32\ntdll.dll:ntdll.dll (00007FFEC42A0000), size: 2523136 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\KERNEL32.DLL:KERNEL32.DLL (00007FFEC3480000), size: 831488 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\KERNELBASE.dll:KERNELBASE.dll (00007FFEC19B0000), size: 4198400 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\USER32.dll:USER32.dll (00007FFEC29A0000), size: 1929216 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\System32\win32u.dll:win32u.dll (00007FFEC1680000), size: 159744 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\GDI32.dll:GDI32.dll (00007FFEC33A0000), size: 176128 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.8328
C:\Windows\System32\gdi32full.dll:gdi32full.dll (00007FFEC1490000), size: 1220608 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\msvcp_win.dll:msvcp_win.dll (00007FFEC16B0000), size: 667648 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\ucrtbase.dll:ucrtbase.dll (00007FFEC1E50000), size: 1359872 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\IMM32.DLL:IMM32.DLL (00007FFEC3650000), size: 200704 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9278
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.dll:Unity.dll (00007FFE02BA0000), size: 92389376 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 6000.3.24.31643
C:\Windows\System32\CRYPT32.dll:CRYPT32.dll (00007FFEC1760000), size: 1568768 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\System32\ADVAPI32.dll:ADVAPI32.dll (00007FFEC32D0000), size: 770048 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\msvcrt.dll:msvcrt.dll (00007FFEC37D0000), size: 692224 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 7.0.26100.9711
C:\Windows\System32\sechost.dll:sechost.dll (00007FFEC35A0000), size: 688128 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\RPCRT4.dll:RPCRT4.dll (00007FFEC3880000), size: 1146880 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\s3tcompress.dll:s3tcompress.dll (00007FFE60B40000), size: 180224 (result: 0), SymType: '-deferred-', PDB: ''
C:\Windows\System32\ole32.dll:ole32.dll (00007FFEC3F40000), size: 1679360 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\etccompress.dll:etccompress.dll (00007FFE01F10000), size: 5070848 (result: 0), SymType: '-deferred-', PDB: ''
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\libfbxsdk.dll:libfbxsdk.dll (00007FFE023F0000), size: 8019968 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 2020.3.7.0
C:\Windows\System32\combase.dll:combase.dll (00007FFEC3AA0000), size: 3715072 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\SETUPAPI.dll:SETUPAPI.dll (00007FFEC2E40000), size: 4747264 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\System32\SHELL32.dll:SHELL32.dll (00007FFEC2070000), size: 7979008 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\SHLWAPI.dll:SHLWAPI.dll (00007FFEC3410000), size: 417792 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\WS2_32.dll:WS2_32.dll (00007FFEC3E30000), size: 528384 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\ispc_texcomp.dll:ispc_texcomp.dll (00007FFE01D50000), size: 1826816 (result: 0), SymType: '-deferred-', PDB: ''
C:\Windows\System32\OLEAUT32.dll:OLEAUT32.dll (00007FFEC2D60000), size: 888832 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\compress_bc7e.dll:compress_bc7e.dll (00007FFE01BF0000), size: 1433600 (result: 0), SymType: '-deferred-', PDB: ''
C:\Windows\System32\WINTRUST.dll:WINTRUST.dll (00007FFEC1DC0000), size: 532480 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\WinPixEventRuntime.dll:WinPixEventRuntime.dll (00007FFE71460000), size: 57344 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 1.0.2403.8001
C:\Windows\WinSxS\amd64_microsoft.windows.common-controls_6595b64144ccf1df_6.0.26100.9711_none_3e07b8b0e334e825\COMCTL32.dll:COMCTL32.dll (00007FFEAEE70000), size: 2691072 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 6.10.26100.9711
C:\Windows\SYSTEM32\bcrypt.dll:bcrypt.dll (00007FFEC1370000), size: 172032 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\d3d11.dll:d3d11.dll (00007FFEBD6D0000), size: 2486272 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\d3d12.dll:d3d12.dll (00007FFE5DEE0000), size: 143360 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\dwmapi.dll:dwmapi.dll (00007FFEBE8D0000), size: 188416 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\dxgi.dll:dxgi.dll (00007FFEBE670000), size: 1290240 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\HID.DLL:HID.DLL (00007FFEBF770000), size: 61440 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.1150
C:\Windows\SYSTEM32\IPHLPAPI.DLL:IPHLPAPI.DLL (00007FFEBFAA0000), size: 208896 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9278
C:\Windows\SYSTEM32\MSVCP140.dll:MSVCP140.dll (00007FFEB1FB0000), size: 643072 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 14.51.36247.0
C:\Windows\SYSTEM32\VCRUNTIME140.dll:VCRUNTIME140.dll (00007FFEB1F80000), size: 180224 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 14.51.36247.0
C:\Windows\SYSTEM32\VCRUNTIME140_1.dll:VCRUNTIME140_1.dll (00007FFEB2490000), size: 49152 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 14.51.36247.0
C:\Windows\SYSTEM32\OPENGL32.dll:OPENGL32.dll (00007FFE97E60000), size: 1097728 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9278
C:\Windows\SYSTEM32\WINHTTP.dll:WINHTTP.dll (00007FFEB7B70000), size: 1220608 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\WINMM.dll:WINMM.dll (00007FFEB7CA0000), size: 225280 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\GLU32.dll:GLU32.dll (00007FFE97DA0000), size: 233472 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\SketchUpAPI.dll:SketchUpAPI.dll (00007FFE01350000), size: 8990720 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 23.0.0.0
C:\Windows\SYSTEM32\Secur32.dll:Secur32.dll (00007FFEBE410000), size: 53248 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.7705
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\NativeProxyHelper.dll:NativeProxyHelper.dll (00007FFE01190000), size: 1789952 (result: 0), SymType: '-deferred-', PDB: ''
C:\Windows\SYSTEM32\VERSION.dll:VERSION.dll (00007FFEB5D00000), size: 45056 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.1150
C:\Windows\SYSTEM32\WSOCK32.dll:WSOCK32.dll (00007FFEBC0E0000), size: 40960 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.1
C:\Windows\SYSTEM32\powrprof.dll:powrprof.dll (00007FFEC12E0000), size: 385024 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.3912
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\SketchUpCommonPreferences.dll:SketchUpCommonPreferences.dll (00007FFE60190000), size: 499712 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 23.0.0.0
C:\Windows\SYSTEM32\dxcore.dll:dxcore.dll (00007FFEBE620000), size: 278528 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\SSPICLI.DLL:SSPICLI.DLL (00007FFEC03F0000), size: 303104 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\umbraoptimizer64.dll:umbraoptimizer64.dll (00007FFE01060000), size: 1187840 (result: 0), SymType: '-deferred-', PDB: ''
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\FreeImage.dll:FreeImage.dll (00000223C1ED0000), size: 5332992 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 3.18.0.0
C:\Windows\SYSTEM32\cfgmgr32.DLL:cfgmgr32.DLL (00007FFEC0FA0000), size: 335872 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\MSWSOCK.DLL:MSWSOCK.DLL (00007FFEC06F0000), size: 442368 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\UMPDC.dll:UMPDC.dll (00007FFEC12C0000), size: 86016 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.8972
C:\Windows\SYSTEM32\MSASN1.dll:MSASN1.dll (00007FFEC0A60000), size: 77824 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\bcryptprimitives.dll:bcryptprimitives.dll (00007FFEC18F0000), size: 704512 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\kernel.appcore.dll:kernel.appcore.dll (00007FFEC0110000), size: 110592 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9278
C:\Windows\system32\uxtheme.dll:uxtheme.dll (00007FFEBE540000), size: 684032 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\windows.storage.dll:windows.storage.dll (00007FFEBEEB0000), size: 9101312 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\SHCORE.dll:SHCORE.dll (00007FFEC39A0000), size: 1019904 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\profapi.dll:profapi.dll (00007FFEC13A0000), size: 167936 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\system32\IconCodecService.dll:IconCodecService.dll (00007FFE93960000), size: 45056 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.1
C:\Windows\system32\wbem\wbemprox.dll:wbemprox.dll (00007FFE9E260000), size: 73728 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\wbemcomn.dll:wbemcomn.dll (00007FFEB2400000), size: 548864 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\WindowsCodecs.dll:WindowsCodecs.dll (00007FFEBCBE0000), size: 2326528 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\netprofm.dll:netprofm.dll (00007FFEBB8B0000), size: 450560 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\npmproxy.dll:npmproxy.dll (00007FFEB77B0000), size: 102400 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\NSI.dll:NSI.dll (00007FFEC33D0000), size: 40960 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\dhcpcsvc6.DLL:dhcpcsvc6.DLL (00007FFEB7B40000), size: 122880 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\dhcpcsvc.DLL:dhcpcsvc.DLL (00007FFEB7B10000), size: 151552 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\DNSAPI.dll:DNSAPI.dll (00007FFEBFB40000), size: 1409024 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\DSPARSE.dll:DSPARSE.dll (00007FFEC04D0000), size: 53248 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.1
C:\Windows\system32\napinsp.dll:napinsp.dll (00007FFE93C00000), size: 98304 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.1150
C:\Windows\System32\winrnr.dll:winrnr.dll (00007FFE93BE0000), size: 77824 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9278
C:\Windows\system32\nlansp_c.dll:nlansp_c.dll (00007FFE5DCC0000), size: 159744 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\system32\wshbth.dll:wshbth.dll (00007FFEA41F0000), size: 90112 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\System32\fwpuclnt.dll:fwpuclnt.dll (00007FFEB7920000), size: 548864 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\rasadhlp.dll:rasadhlp.dll (00007FFEB79B0000), size: 45056 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\AMDUnityPlugin.DLL:AMDUnityPlugin.DLL (00007FFDFF1F0000), size: 9895936 (result: 0), SymType: '-deferred-', PDB: ''
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\NVUnityPlugin.DLL:NVUnityPlugin.DLL (00007FFDFF050000), size: 1646592 (result: 0), SymType: '-deferred-', PDB: ''
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Data\Tools\astcenc-avx2.dll:astcenc-avx2.dll (00007FFDFEF30000), size: 352256 (result: 0), SymType: '-deferred-', PDB: ''
C:\Windows\SYSTEM32\mfplat.dll:mfplat.dll (00007FFEA8BA0000), size: 2256896 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\RTWorkQ.DLL:RTWorkQ.DLL (00007FFEB3720000), size: 241664 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.8972
C:\Windows\SYSTEM32\mfreadwrite.dll:mfreadwrite.dll (00007FFE94D40000), size: 872448 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\Mf.dll:Mf.dll (00007FFEA9890000), size: 622592 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\MFCORE.DLL:MFCORE.DLL (00007FFEA8EC0000), size: 4304896 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\CompPkgSup.DLL:CompPkgSup.DLL (00007FFEB4A50000), size: 204800 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.8521
C:\Windows\System32\Windows.StateRepositoryPS.dll:Windows.StateRepositoryPS.dll (00007FFEB4350000), size: 802816 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\wintypes.dll:wintypes.dll (00007FFEBB450000), size: 1482752 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\windows.staterepositoryclient.dll:windows.staterepositoryclient.dll (00007FFEAF8E0000), size: 286720 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\SYSTEM32\windows.staterepositorycore.dll:windows.staterepositorycore.dll (00007FFEAF990000), size: 122880 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\Windows.Media.dll:Windows.Media.dll (00007FFE9DB70000), size: 6922240 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\System32\MMDevApi.dll:MMDevApi.dll (00007FFEB52E0000), size: 679936 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9711
C:\Windows\System32\DEVOBJ.dll:DEVOBJ.dll (00007FFEC1000000), size: 192512 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\AUDIOSES.DLL:AUDIOSES.DLL (00007FFEB4B90000), size: 1810432 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549
C:\Windows\SYSTEM32\resourcepolicyclient.dll:resourcepolicyclient.dll (00007FFEBEA60000), size: 81920 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.8115
C:\Windows\SYSTEM32\opencl.dll:opencl.dll (00007FFE3B200000), size: 237568 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 3.0.6.0
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\radeonrays.dll:radeonrays.dll (00007FFE60130000), size: 344064 (result: 0), SymType: '-deferred-', PDB: ''
C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Data\MonoBleedingEdge\EmbedRuntime\mono-2.0-bdwgc.dll:mono-2.0-bdwgc.dll (00007FFE00610000), size: 10756096 (result: 0), SymType: '-deferred-', PDB: ''
C:\Windows\SYSTEM32\dbghelp.dll:dbghelp.dll (00007FFEB5E30000), size: 2408448 (result: 0), SymType: '-deferred-', PDB: '', fileVersion: 10.0.26100.9549

========== OUTPUTTING STACK TRACE ==================

0x00007FFEC1A7408A (KERNELBASE) RaiseException
0x00007FFE0068120C (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\utils\mono-log-common.c:143] mono_log_write_logfile 
0x00007FFE0066CE12 (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\eglib\goutput.c:172] monoeg_g_logv_nofree 
0x00007FFE0066CE78 (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\eglib\goutput.c:187] monoeg_g_log 
0x00007FFE0081DE7B (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\threadpool-io.c:551] wakeup_pipes_init 
0x00007FFE0081E04E (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\threadpool-io.c:593] initialize 
0x00007FFE0081E4AC (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\threadpool-io.c:636] ves_icall_System_IOSelector_Add 
0x00007FFE0072E004 (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\icall-def.h:475] ves_icall_System_IOSelector_Add_raw 
0x000002258FC738F9 (Mono JIT Code) (wrapper managed-to-native) System.IOSelector:Add (intptr,System.IOSelectorJob)
0x000002258FC72FEB (Mono JIT Code) System.Net.Sockets.Socket:QueueIOSelectorJob (System.Threading.SemaphoreSlim,intptr,System.IOSelectorJob)
0x000002258FC724FB (Mono JIT Code) System.Net.Sockets.Socket:AcceptAsync (System.Net.Sockets.SocketAsyncEventArgs)
0x000002258FC72213 (Mono JIT Code) System.Net.EndPointListener:Accept (System.Net.Sockets.Socket,System.Net.Sockets.SocketAsyncEventArgs,System.Net.Sockets.Socket&)
0x000002258EC9F3E3 (Mono JIT Code) System.Net.EndPointListener:.ctor (System.Net.HttpListener,System.Net.IPAddress,int,bool)
0x000002258EC5BBEB (Mono JIT Code) System.Net.EndPointManager:GetEPListener (string,int,System.Net.HttpListener,bool)
0x000002258EC56753 (Mono JIT Code) System.Net.EndPointManager:AddPrefixInternal (string,System.Net.HttpListener)
0x000002258EC55A7B (Mono JIT Code) System.Net.EndPointManager:AddListener (System.Net.HttpListener)
0x000002258EC55603 (Mono JIT Code) System.Net.HttpListener:Start ()
0x000002258EC515DB (Mono JIT Code) UnityEditor.BuildReportRestService:RunServer ()
0x000002258EC505BB (Mono JIT Code) UnityEditor.BuildReportRestService:RunServerWithRetries (int)
0x000002258EC50413 (Mono JIT Code) UnityEditor.BuildReportRestService:<OnEnable>b__13_0 ()
0x000002258EC50356 (Mono JIT Code) System.Threading.ThreadHelper:ThreadStart_Context (object)
0x000002258EC4FD7E (Mono JIT Code) System.Threading.ExecutionContext:RunInternal (System.Threading.ExecutionContext,System.Threading.ContextCallback,object,bool)
0x000002258EC4F54B (Mono JIT Code) System.Threading.ExecutionContext:Run (System.Threading.ExecutionContext,System.Threading.ContextCallback,object,bool)
0x000002258EC4F3AB (Mono JIT Code) System.Threading.ExecutionContext:Run (System.Threading.ExecutionContext,System.Threading.ContextCallback,object)
0x000002258EC4F1B3 (Mono JIT Code) System.Threading.ThreadHelper:ThreadStart ()
0x000002258E7AFDF6 (Mono JIT Code) (wrapper runtime-invoke) object:runtime_invoke_void__this__ (object,intptr,intptr,intptr)
0x00007FFE00896C0E (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\mini\mini-runtime.c:3445] mono_jit_runtime_invoke 
0x00007FFE007D8984 (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\object.c:3068] do_runtime_invoke 
0x00007FFE007DBF1A (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\object.c:4456] mono_runtime_delegate_try_invoke 
0x00007FFE0080834D (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\threads.c:1296] start_wrapper_internal 
0x00007FFE0080850E (mono-2.0-bdwgc) [C:\build\output\Unity-Technologies\mono\mono\metadata\threads.c:1348] start_wrapper 
0x00007FFEC34ADDA7 (KERNEL32) BaseThreadInitThunk
0x00007FFEC431EE4C (ntdll) RtlUserThreadStart

========== END OF STACKTRACE ===========

A crash has been intercepted by the crash handler. For call stack and other details, see the latest crash report generated in:
 * C:/Users/gosu3/AppData/Local/Packages/sandbox.{a25ecf77-dd17-4ea8-bf9c-7d9f2b74f4dc}/AC/Temp/Unity/Editor/Crashes
