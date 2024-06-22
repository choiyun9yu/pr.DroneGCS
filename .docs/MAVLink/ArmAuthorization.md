# Arm Authorization
[url](https://mavlink.io/en/services/arm_authorization.html)

## 1. auto matically takeoff?
- https://ardupilot.org/copter/docs/auto-mode.html
- https://ardupilot.org/copter/docs/parameters.html
- https://ardupilot.org/copter/docs/parameters-Copter-stable-V4.2.2.html#auto-options-auto-mode-options

## 2. HEARTBEAT/Connection Protocol
- 하트비트 프로토콜은 시스템 및 구성요소 ID, 차량 유형, 비행 스택, 구성 요소 유형 및 비행 모드와 함께 MAVLink 네트워크에 시스템의 존재를 알리는데 사용 된다.
- 하트비트를 통해 다음을 수행할 수 있다.
  1) 네트워크에 연결된 시스템을 검색하고 연결이 끊어진 시점을 추론할 수 있다.  
     HEARTBEAT 메시지가 정기적으로 수신 되면 구성 요소가 네트워크에 연결된 것으로 간주되고,  
     예상되는 메시지 수를 수신하지 않으면 끊어진 것을 간주 된다.
  2) 구성 요소 유형 및 기타 속성(예: 차량 유형에 따라 GCS 인터페이스 레이아웃)을 기반으로 구성 요소의 다른 메시지를 적절히 처리할 수 있다.
  3) 다른 인터페이스의 시스템으로 메시지를 라우팅 할 수 있다.

