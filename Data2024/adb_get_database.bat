@echo off
"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe" shell getprop ro.boot.serialno>serialno.txt
set /p SERIALNO=<serialno.txt
"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe" pull /sdcard/Documents/BertScout2024.db3 "%SERIALNO%.db3"
del serialno.txt
echo Copied to %SERIALNO%.db3
pause