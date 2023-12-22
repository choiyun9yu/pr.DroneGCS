# Drone


## 1. Software Overview

### 1-1. FCU(Flight Control Unit)
CPU와 IMU가 조합된 하드웨어이고 이 컴퓨터에 올라가는 OS는 주로 Ardupilot과 PX4이다. 

드론 하드웨어 및 운영체제
- Ardupilot
- PX4

### 1-2. Companion Computer
드론 응용 프로그램 
- MAVROS
- Pymavlink
- Dronekit
- MAVSDK

### 1-3, GCS, Ground Control Computer
- QGroundControl
- Mission Planner
- MAVproxy

can also use a programming interface
- MAVROS
- Pymavlink
- Dronekit
- MAVSDK

<br>

## 2. 드론 개발 라이브러리

### 2-1. Mavlink 
Mavlink는 프로토콜이다. 프로토콜은 통신 규약이다. 통신 규약은 수신자와 송신자 사이에서 서로 동일한 문법으로 소통가능하게 하는 역할을 한다.

마브 링크는 전문의 내용이 미리 확정이 되어 있는 고정적 전문 방식이다.

![img2.png](..%2Fdata%2Fimg2.png)

MAVLink는 드론과 지상국 간에 데이터 및 명령을 전송하는 데 가장 일반적으로 사용되는 직렬 프로토콜이다. 이 프로토콜은 common.xml 및 ardupilot.xml 에서 찾을 수 있는 대규모 메시지 집합을 정의한다.

- Heart Beat(1hz) 가 왔다갔다 하고
- GCS에서 Request Data Stream을 보내면 Drone에서 Send Data를 보낸다. 
- GCS에서 Send Commands(COMMAND_LONG)를 보내면 Dore에서 ACK를 보낸다. 

## 2-2. MavProxy
[MAVProxy](https://ardupilot.org/mavproxy/)  
가장 원초적인 GCS이다. GCS를 만들기 위해서 반드시 처음 공부해야하는 프로젝트이다. 현재 개발되고 있는 모든 GCS의 모체이다.

## 2-3. MAVSDK
드론 오픈 소스 펌웨어는 Ardupilot과 PX4 펌웨어가 있다. 이 중 PX4 측에서 사용하는 개발툴이 MAVSDK이다. 

## 2-4. MAVROS
MAVLink와 ROS가 결합된 라이브러리이다. (C언어 -> MAVLink -> ROS 순서)


