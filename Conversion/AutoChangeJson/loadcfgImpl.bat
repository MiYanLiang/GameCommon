echo off

:: 获取文本的前两行内容并赋值给变量
::(Set /p Line1=&Set /p Line2=)<timeZonesCfg.txt
::set DEFAULT_TIME_ZONE=%Line2%
::set NEW_TIME_ZONE=%Line1%
::call setTimeZone.bat %NEW_TIME_ZONE%

::java -showversion -XX:+PrintCommandLineFlags -XX:+UseG1GC -Xmx6G -classpath lib\kutil.jar;lib\gs.jar;lib\poi-3.9-20121203.jar;lib\poi-ooxml-3.9-20121203.jar;lib\poi-ooxml-schemas-3.9-20121203.jar;lib\log4j-1.2.16.jar;lib\dom4j-1.6.1.jar;lib\xmlbeans-2.3.0.jar i3k.gtool.DataTool --srcdir=datasrc --luadstdir=..\datafiles\lua_scripts\gamedb --dstdir=..\gamedata --server_dstdir=. --ckFile=res.txt --nthread=4
if not errorlevel 0 goto p
pylib\python\python.exe pylib\export.py datasrc ..\..\ThreeKillGame\Assets\Resources\Jsons\
::..\script\gamedb\
::call setTimeZone.bat %DEFAULT_TIME_ZONE%

:p
goto:eof

