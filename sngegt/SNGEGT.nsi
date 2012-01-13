; example2.nsi
;
; This script is based on example1.nsi, but it remember the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

RequestExecutionLevel admin

; The name of the installer
Name "SNGEGT"

; The file to write
OutFile "SNGEGT-setup.exe"

; The default installation directory
InstallDir $PROGRAMFILES\SNGEGT

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\SNGEGT" "Install_Dir"

;--------------------------------

; Pages

;Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles


; The stuff to install
Section "SNGEGT (required)"
  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  SetOverwrite on  
     
  
  File "paketti\\blinds.xml"
  File "paketti\\SNGEGT.exe"
  File "paketti\\SNG Quiz.exe"
  File "paketti\\Heads-Up Trainer.exe"
  File "paketti\\DealerControl.gif"
  File "paketti\\Cards.dll"  
  ;File "paketti\\GT.license" ;preinstalled license key for people with file copying problems

  
  ; Delete icons
  Delete "$SMPROGRAMS\SNGEGT\*.*"
  Delete $INSTDIR\SNGEGT-test.exe
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\SNGEGT "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SNGEGT" "DisplayName" "SNGEGT"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SNGEGT" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SNGEGT" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SNGEGT" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
  ;Create start menu shortcuts
  CreateDirectory "$SMPROGRAMS\SNGEGT"  
  CreateShortCut "$SMPROGRAMS\SNGEGT\SNGEGT.lnk" "$INSTDIR\SNGEGT.exe" "" "$INSTDIR\SNGEGT.exe" 
  CreateShortCut "$SMPROGRAMS\SNGEGT\SNGEGT test.lnk" "$INSTDIR\SNGEGT.exe" "TEST" "$INSTDIR\SNGEGT.exe" 
  CreateShortCut "$SMPROGRAMS\SNGEGT\SNG Quiz.lnk" "$INSTDIR\SNG Quiz.exe" 
  CreateShortCut "$SMPROGRAMS\SNGEGT\Heads-Up Trainer.lnk" "$INSTDIR\Heads-Up Trainer.exe"
  ;CreateShortCut "$SMPROGRAMS\SNGEGT\Rubin.lnk" "$INSTDIR\Rubin.bat"
SectionEnd



; Uninstaller
Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SNGEGT"
  DeleteRegKey HKLM SOFTWARE\SNGEGT

  ; Remove files and uninstaller
  Delete $INSTDIR\SNGEGT.exe
  Delete $INSTDIR\update.exe
  Delete "$INSTDIR\SNG Quiz.exe"
  Delete "$INSTDIR\Heads-Up Trainer.exe"
  Delete $INSTDIR\Cards.dll
  Delete $INSTDIR\blinds.xml
  Delete $INSTDIR\DealerControl.gif
  Delete $INSTDIR\uninstall.exe
  Delete $INSTDIR\ICSharpCode.SharpZipLib.dll
  ;Delete $INSTDIR\Rubin.bat
  

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\SNGEGT\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\SNGEGT"
  RMDir "$INSTDIR"

SectionEnd


;Function FileSize 
;  Exch $0
;  Push $1
;  FileOpen $1 $0 "r"
;  FileSeek $1 0 END $0
;  FileClose $1
;  Pop $1
;  Exch $0 
;FunctionEnd