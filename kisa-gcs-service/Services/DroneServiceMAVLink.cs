using System;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace kisa_gcs_service;

public class DroneServiceMAVLink
{
    public static void MavLinkMessage()
    {
        UdpClient udpClient = new UdpClient(14556); // ArduPilot SITL에서 데이터 받을 포트 지정
        try
        {
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