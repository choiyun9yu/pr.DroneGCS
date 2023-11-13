using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using MavLink;

namespace kisa_gcs_service;

public class DroneServiceMavlink
{
    public static void MavLinkMessage()
    {
        UdpClient udpClient = new UdpClient(14556); // ArduPilot SITL에서 데이터 받을 포트 지정
        try
        {
            // 시리얼 포트 설정
            var serialPort = new SerialPort("COM1", 57600); // 시리얼 포트 이름: COM1, 시리얼 통신 보드레이트(통신 속도): 57600
            serialPort.Open();  // 시리얼 포트 열기
            
            // MAVLink 커넥션 설정, 앞에서 설정한 시리얼 포트를 기반으로 MAVLink 연결을 만듬
            var mavlinkConnection = new MAVLinkConnection(serialPort.BaseStream);
            
            // MAVLink 메시지 수신 이벤트 핸들러 등록, 커넥션을 통해 메시지가 수신되면 해당 이벤트 핸들러가 호출됨
            mavlinkConnection.MessageReceived += (sender, e) =>
            {
                // 메시지 처리
                var message = e.Message as MavLinkMessage;
                if (message != null)
                {
                    Console.WriteLine($"Received MAVLink Message: {message.MessageName}");  // 수신된 메시지의 이름을 콘솔에 출력
                }
            };

            // MAVLink 메시지를 보내려면 해당 시메지 객체를 만들고 mavlinkConnection을 통해 전송
            var heartbeat = new msg_heartbeat
            {
                type = (byte)MAV_AUTOPILOT.ARDUPILOTMEGA,
                autopilot = (byte)MAV_AUTOPILOT.ARDUPILOTMEGA,
                base_mode = (byte)MAV_MODE_FLAG.MAV_MODE_GUIDED_DISARMED,
                custom_mode = 0,
                system_status = (byte)MAV_STATE.STANDBY
            };

            mavlinkConnection.Send(heartbeat);

            
            while (true)
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref remoteEp);  // data 배열에는 수신한 바이트 데이터가 들어있다.
                // 여기서 MAVLink 메시지를 해석하고 처리하는 로직을 추가하면 된다.
                
                Console.WriteLine(data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            udpClient.Close();
        }
    }
}

