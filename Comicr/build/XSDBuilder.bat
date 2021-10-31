@echo off
cd %1
call :treeProcess %2 "ComicBookInfo"
cd ..
goto :eof

:treeProcess
rem From http://stackoverflow.com/a/8398621/298754
echo Processing %2
for %%f in (*.xsd) do call :buildXSD %%f %1 %2 %%~nf
for /D %%d in (*) do (
    cd %%d
    call :treeProcess %1 %2.%%d
    cd ..
)
exit /b

:buildXSD
%2 %1 /c /n:%3