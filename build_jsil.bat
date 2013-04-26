@echo off
setlocal
set CWD=%CD%
set PROJECT=LD26

cd C:\Development\libraries\jsil\JSIL\bin

del %CWD%\%PROJECT%Jsil\jsil\*.* /s /q
jsilc -o %CWD%\%PROJECT%Jsil\jsil %CWD%\%PROJECT%.sln 1> %CWD%\build_jsil.log 2>&1 

endlocal