using System;
using System.Net;
using System.Net.Sockets;
using Mavsdk;

namespace kisa_gcs_service.MAVSDK;

public class DroneServiceMAVLink
{
    public static void ProcessMavLinkMessage()
    {
        UdpClient udpClient = new UdpClient(14556); // ArduPilot SITL에서 데이터 받을 포트 지정
        try
        {
            while (true)
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref remoteEP);
                Console.WriteLine(data);
                // data 배열에는 수신한 바이트 데이터가 들어있다.
                // 여기서 MAVLink 메시지를 해석하고 처리하는 로직을 추가하면 된다.
                // 예를 들면 MAVLink 라이브러리를 사용할 수 있다.
                // https://github.com/mavlink/c_library_v2
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