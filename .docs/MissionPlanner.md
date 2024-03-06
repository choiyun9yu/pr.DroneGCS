# Mission Planner

## 1. Install

### 1-1. mono 
[Download](https://firmware.ardupilot.org/Tools/MissionPlanner/)  
QGroundControl 과 달리 Mission Planner 의 경우 Linux 환경에서 구동할 수 있는 형식으로 배포되지 않는다.
따라서 exe 실행 파일을 구동하기 위한 mono 라는 프로그램을 설치해야 한다.

    % sudo apt install mono-runtime libmono-system-windows-forms4.0-cil libmono-system-core4.0-cil

    % sudo apt install mono-complete

### 1-2. Mission Planner
[Download](https://firmware.ardupilot.org/Tools/MissionPlanner/MissionPlanner-latest.zip)  
다운 받은 압축 파일을 원하는 경로에서 압축 해제 하고 해당 경로에서 아래 명령을 실행한다.

    % mono MissionPlanner.exe

## 2. Setting
### 2-1. Connect Ardupilot SITL
