# sensor_anomaly_detection

I aim to perform anomaly detection on the sensor values obtained from the drone.

## 1. Virtual environment

I deal with this project's virtual environment by miniconda but if you use other tools, It doesn't matter.  
You can check the library version [here](https://github.com/choiyun9yu/sensor_anomaly_detection/blob/main/requirements.txt).

## 2. Anomaly Detection

I used Classification based Anomlay Detection because Classification Models are easy to use and powerful.

## 3. Dataset

### 3-1) feature data

xacc_RAW_IMU : 가속도 x축
zacc_RAW_IMU : 가속도 y축
yacc_RAW_IMU : 가속도 z축

xgyro_RAW_IMU : 자이로 x축
ygyro_RAW_IMU : 자이로 y축
zgyro_RAW_IMU : 자이로 z축

xmag_RAW_IMU : 지자기 x축
ymag_RAW_IMU : 지자기 y축
zmag_RAW_IMU : 지자기 z축

vibration_x_VIBRATION : 진동 x축
vibration_y_VIBRATION : 진동 y축
vibration_z_VIBRATION : 진동 z축

accel_cal_x_SENSOR_OFFSETS :
accel_cal_y_SENSOR_OFFSETS :
accel_cal_z_SENSOR_OFFSETS :

mag_ofs_x_SENSOR_OFFSETS :
mag_ofs_y_SENSOR_OFFSETS :

chan12_raw_RC_CHANNELS : 12번 채널 입력 값
chan13_raw_RC_CHANNELS : 13번 채널 입력 값
chan14_raw_RC_CHANNELS : 14번 채널 입력 값
chan15_raw_RC_CHANNELS : 15번 채널 입력 값
chan16_raw_RC_CHANNELS : 16번 채널 입력 값
chancount_RC_CHANNELS : 입력채널 카운트

roll_ATTITUDE : 양 옆 회전, 선회를 위해 기동하는 방식(롤을 어느정도 한 후 피치를 주면 선회한다.)
pitch_ATTITUDE : 위아래 회전, 고도를 변경하기 위해 기동하는 방식
yaw_ATTITUDE : 좌우 회전, (선회 시 진행방향과 항공기의 기수가 일치하지 않는 현상을 수정할 때 사용)

x_LOCAL_POSITION_NED :
vx_LOCAL_POSITION_NED : 지역 위치 x값
vy_LOCAL_POSITION_NED : 지역 위치 y값

vx_GLOBAL_POSITION_INT : 전역 위치 x값
vy_GLOBAL_POSITION_INT : 전역 위치 y값

airspeed_VFR_HUD : 대기 속도, 항공기가 실제로 느끼는 속도
groundspeed_VFR_HUD : 지상 속도, 지표면에서 봤을 때 항공기 속도

servo3_raw_SERVO_OUTPUT_RAW : 3번 서보모터 출력 값
servo8_raw_SERVO_OUTPUT_RAW : 8번 서보모터 출력 값

nav_bearing_NAV_CONTROLLER_OUTPUT : 항법 컨트롤러 출력값(bearing)
nav_pitch_NAV_CONTROLLER_OUTPUT : 항법 컨트롤러 출력값(pitch)

Vservo_POWER_STATUS : 서보모터 전원 관련 데이터
voltages1_BATTERY_STATUS : 베터리 상태 관련 데이터

press_abs_SCALED_PRESSURE :

### 3-2) label data

loginfo : binary class (normal,fail)

<hr>

## Constraint, 한계점

- 각 독립 변수들이 의미하는 바를 알 수 없다. 머신러닝의 기본 중 하나인 독립변수 속성에 대한 domain knowlege가 전무한 상황에서 모델 개발에 들어가야한다는 점이 매우 큰 제약사항으로 느껴졌다.
