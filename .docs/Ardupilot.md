# Ardupilot
[Ardupilot SITL](https://ardupilot.org/dev/docs/sitl-simulator-software-in-the-loop.html)
- [Setting](https://ardupilot.org/dev/docs/building-setup-linux.html#building-setup-linux)
- [Running](https://ardupilot.org/dev/docs/sim-on-hardware.html)
- [Using](https://ardupilot.org/dev/docs/using-sitl-for-ardupilot-testing.html)

## 1. SITL(Software-In-The-Loop) 개요
- SITL은 Ardupilot 코드를 PC에서 빌드 및 테스트 할 수 있는 시뮬레이션 프로그램이다.  
- SITL을 이용하면 Hardware에 종속되지 않고 Vehicle specific 코드를 테스트 할 수 있다.
- Physical 하지 않은 function들을 안전하게 테스트하고 디버깅할 수 있다.   
  즉, 실제 드론이 없어도 Ardupilot을 빌드하고 실행되는 모습을 볼 수 있으므로 편리하다.
  ![img.png](../gcs-system/data/img.png)


## 2. SITL 설치

    // 필수 패키지 설치
    % sudo apt-get update
    % sudo apt-get install git gitk git-gui
    % sudo apt-get install python3-pip python3-dev python3-opencv python3-pygame python3-wxgtk4.0 python-wxtools ccache gawk gcc-arm-none-eabi
    % sudo pip3 install MAVProxy pymavlink empy pexpect future PyYAML mavproxy --user

    % echo "export PATH=$PATH:$HOME/.local/bin" >> ~/.bashrc

    // 설치
    % git clone --recurse-submodules https://github.com/ArduPilot/ardupilot
    % cd ardupilot
    % Tools/environment_install/install-prereqs-ubuntu.sh -y
    % . ~/.profile

    // Waf 컴파일러 사용해서 빌드 
    % ./waf configure --board sitl
    % ./waf copter or ./waf -j6

    // 실행
    % cd Tools/autotest
    % python sim_vehicle.py --console --map -v ArduCopter

## 3. STL 사용

    % ./sim_vehicle.py {파라미터 입력}
| 옵션             | 설명                    | 예시                                                       |
|----------------|-----------------------|----------------------------------------------------------|
| --help         | 도움말 보기                | sim_vehicle.py --help                                    |
| -w             | 파라미터 초기화              | sim_vehicle.py -w                                        |
| --console      | 콘솔 창 보기               | sim_vehicle.py --console                                 |
| --map          | 지도 보기                 | sim_vehicle.py --map                                     |
| --list-vehicle | 기체 타입 보기              | sim_vehicle.py --list-vehicle                            |
| --list-frame   | 프레임 타입 보기             | sim_vehicle.py --list-frame                              |
| -v / -f        | 기체 / 프레임 타입 설정(기본 드론) | sim_vehicle.py -v ArduPlane -f quadplane --console --map |
| -L             | 지도 위치 변경              | sim_vehicle.py -L Seoul --console --map                  |
| --osd          | OSD 화면 보기             | sim_vehicle.py -v ArduPlane --console --map --osd        |

#### [드론 조작 명령어](https://ardupilot.org/dev/docs/copter-sitl-mavproxy-tutorial.html)

    # 시뮬레이터 실행시킨 터미널 창에 입력
    > mode auto     // 자동 모드
    > mode stabiliz // 스테빌라이즈 모드 (RC 조종기 입력에 직접적으로 응답하여 기체를 수평으로 유지)
    > mode guided   // 가이드 모드
    > mode land     // 착륙 모드
    > mode rtl      // 이륙 지점으로 돌아가기
    > mode poshold  // 포스홀드 모드 (기체가 현재 위치를 유지하면서 안정화)
    > mode Loiter   // 로이터 모드 (GPS 좌표를 정확하게 유지하면서 고도 및 방향도 유지)

    > arm throttle  // 시동 거는 명령
    > takeoff 40    // 고도 40
    > guided -35.3621741 149.16511256 40  // 위도 경도 고도 로 이동

    > param set RC1_MIN 1100
    > param set RC1_MAX 1900

    단축키 alt + g [36.377 127.385] enter

    mode guided
    arm throttle
    takeoff 40
    guided -35.3621741 149.16511256 10
    guided -35.3621740 149.16511255 10

    % python sim_vehicle.py -v ArduCopter -f hexa --out 127.0.0.1:14556       
    % python sim_vehicle.py -v ArduCopter -I 0 -n 3 --auto-sysid --out=udp:127.0.0.1:14556      // 다중 드론 

### 4-0. 파라미터 초기화

    % sim_vehicle.py -w

### 4-1. 드론 시작 위치 조정 
- ardupilot/Tools/autotest/localpoints.txt 파일에서 좌표를 입력  
  (위도.소수점8번째자리까지,경도,절대고도,머리방향) 
- -L 옵션으로 실행하면 된다.


    % python sim_vehicle.py -L ETRI -v ArduCopter

### 4-2. 드론 주변 장치 추가
[가상 짐벌 추가](https://ardupilot.org/dev/docs/adding_simulated_devices.html#adding-simulated-devices)

가상 짐벌 추가는 우선 시뮬레이터를 실행하고 

    MAV> param set MNT1_TYPE 1
    MAV> param set SERVO6_FUNCTION 6
    MAV> param set SERVO7_FUNCTION 8

종료 후 재시작할 때 -M 플래그 추가 

    % python sim_vehicle.py -L ETRI -v ArduCopter -M --out=udp:127.0.0.1:14556

GIMBAL_DEVICE_ATTITUDE_STATUS 메시지는 전송되는데 GIMBAL_CONTROL 는 전송해주지 않음

<br>
<hr>

# Gazebo

## 1. Ardupilot vs Gazebo

### 1-1. Ardupilot
- ArduPilot은 오픈소스 드론 비행 컨트롤러 소프트웨어 스택이다.
- 다양한 드론 유형과 로봇을 위한 통합 솔루션을 제공하며, APM:Copter, APM:Plane, APM:Rover 등과 같은 다양한 비행 모드를 지원한다.
- 실제 드론에 탑재되어 실제 비행에서 사용될 수 있으며, 드론의 자동 비행, GPS 기반의 내비게이션, 임무 계획 및 제어, 센서 통합 등의 기능을 제공한다.
- 시뮬레이션은 실제 하드웨어에 의존하여 실시간 비행을 시뮬레이션 하거나, 시뮬레이션 도구를 사용하여 가상 환경에서 테스트할 수 있다.

### 1-2. Gazebo
- Gazebo는 로봇 및 드론 시뮬레이션을 위한 오픈 소스 시뮬레이션 환경이다.
- 여러 종류의 로봇 및 드론 모델을 시뮬레이션하고, 다양한 센서와 환경 요소를 추가하여 테스트할 수 있다.
- Gazebo는 플러그인 아키텍처를 지원하여 다양한 컨트롤러, 센서, 알고리즘을 통합할 수 있다.
- PX4, ROS(로봇 운영체제), OpenAiI Gym 등 다양한 로봇 및 드론 프로젝트에서 Gazebo를 사용하여 시뮬레이션을 수행한다.

####
ArduPilot은 주로 실제 드론에서 사용되는 비행 컨트롤러 및 소프트웨어 스택이며,  
Gazebo는 시뮬레이션 환경으로서 로봇 및 드론의 행동을 가상으로 테스트하고 분석하는 데 사용된다.  
일반적으로 ArduPilot은 드론의 실제 비행에 사용되는 반면, Gazebo는 개발 및 테스트 단계에서 드론의 행동을 예측하고 확인하는데 활용된다.