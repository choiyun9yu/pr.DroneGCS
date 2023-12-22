# MAVSDK
[PX4 Toolchain](https://docs.px4.io/master/en/dev_setup/dev_env_linux_ubuntu.html)

MAVSDK는 Dronecode 재단에서 개발한 드론, 카메라 또는 지상 시스템과 같은 MAVLink 시스템과 인터페이스 하기 위한 다양한 프로그래밍 언어용 오픈소스 라이브러리 모음이다. 

MAVSDK의 기본이 되는 프로그래밍 언어는 C++이다. 다른 프로그래밍 언어에 대해서는 Wrapper를 제공한다. 
 
[API Reference](https://mavsdk.mavlink.io/main/en/cpp/api_reference/)
- Mavsdk: 드론을 검색하고 연결한다
- System: 연결된 드론을 나타낸다. 
- Info: 시스템의 하드웨어 또는 스포트웨어에 대한 기본 버전 정보이다.
- Telemetry: 차량 원격 측정 및 상태 정보를 가져오고 원격 측정 업데이트 속도를 설정한다
- Action: 무장, 이륙, 착륙을 포함한 간단한 드론 액션
- Mission: Waypoint 미션 생성 및 업로드/다운로드 Mission은 Missionitem 개체에서 생성된다.
- Offboard: 속도 명령으로 드론을 제어한다.
- Geofence: 지오펜스를 지정한다.
- Gimbal: 짐벌을 제어한다.
- Camera: 카메라를 제어한다.
- FollowMe: Drone 위치 추적
- Calibration: 센서 보정
- LogFiles: 드론에서 로그 파일ㅇ르 다운로드 한다. 
