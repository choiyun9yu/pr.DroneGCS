# Mission Planing

## 명령 유형 
### Navigation Command
- 이륙, 웨이포인트로 이동, 고도 변경, 착륙 등 차량의 움직임을 제어하는 데 사용된다.
- NAV 명령은 우선순위가 가장 높다. NAV 명령이 로드될 때 실행되지 않은 Do/CONDITION 명령은 거넌 뛴다.
### Do Command 
- 보조 기능을 위한 것이며 차량의 위치에 영향을 주지 않는다.
### Condition Command
- UAV 가 웨이포인트로부터 특정 고도 또는 거리에 도달하는 등 일부 조건이 충족될 때 까지 DO 명령을 지연하는 데 사용 된다.
### !Note
- 임무 중에는 최대 하나의 "Navigation" 명령과 하나의 "Do" 또는 "Condition" 명령이 동시에 실행될 수 있다.
- "Condition" 및 "Do" 는 이전 "NAV" 명령과 연결된다.
- 이러한 명령이 실행되기 전에 UAV 가 다음 웨이포인트에 도달하면 다음 NAV 명령이 로드되도록 건너 뛴다.

## 1. Navigation Commands
- MAV_CMD_NAV_PAYLOAD_PLACE:  
  https://ardupilot.org/copter/docs/common-mavlink-mission-command-messages-mav_cmd.html#mav-cmd-nav-payload-place

## 2. Do Commands
- MAV_CMD_DO_GRIPPER:  
  https://ardupilot.org/copter/docs/common-mavlink-mission-command-messages-mav_cmd.html#mav-cmd-do-gripper

## 3. Condition Commands


## 4. Special Commands
- ARM:  
  https://ardupilot.org/copter/docs/arming_the_motors.html#arming-the-motors
- AUTO_OPTIONS:  
  https://ardupilot.org/copter/docs/parameters.html#auto-options
- MAV_CMD_MISSION_START:  
  https://ardupilot.org/copter/docs/common-mavlink-mission-command-messages-mav_cmd.html#mav-cmd-mission-start
- Auto 모드로 지상에 있을 때 임무를 시작 하는데 사용  
  https://ardupilot.org/copter/docs/common-mavlink-mission-command-messages-mav_cmd.html#mav-cmd-component-arm-disarm
- 착륙 후 임무 계속하기  
  https://ardupilot.org/copter/docs/common-continue-mission.html