@echo off
:: 设置时区。
::set CN_TIME="China Standard Time"

set NEW_TIME_ZONE=%1

:: show help
::tzutil /?

tzutil /l > timeZonesLists.txt
::echo 更多时区选项，请参考timeZonesLists.txt


tzutil /s %NEW_TIME_ZONE%
set "err=%errorlevel%"
if %err% == 0 (

echo 修改时区成功：%NEW_TIME_ZONE%
) else (
color 04
echo 修改时区失败！
pause  
 )

::pause

