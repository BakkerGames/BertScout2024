@echo off
adb shell getprop ro.boot.serialno>serialno.txt
set /p SERIALNO=<serialno.txt
adb pull /sdcard/Documents/BertScout2024.db3 "%SERIALNO%.db3"
del serialno.txt