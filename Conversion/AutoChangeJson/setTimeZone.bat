@echo off
:: ����ʱ����
::set CN_TIME="China Standard Time"

set NEW_TIME_ZONE=%1

:: show help
::tzutil /?

tzutil /l > timeZonesLists.txt
::echo ����ʱ��ѡ���ο�timeZonesLists.txt


tzutil /s %NEW_TIME_ZONE%
set "err=%errorlevel%"
if %err% == 0 (

echo �޸�ʱ���ɹ���%NEW_TIME_ZONE%
) else (
color 04
echo �޸�ʱ��ʧ�ܣ�
pause  
 )

::pause

