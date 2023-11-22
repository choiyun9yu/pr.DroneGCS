namespace kisa_gcs_service.Services;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class MavlinkUdpMessageDecoder
{
    private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
    private UdpClient udpClient;
    private Thread receiveThread;

    public MavlinkUdpMessageDecoder(int port)
    {
        udpClient = new UdpClient(port);
        receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();
    }

    private void ReceiveMessages()
    {
        try
        {
            while (true)
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref remoteEp);

                // 여기서 MAVLink 메시지를 해석하고 처리하는 로직을 추가하면 됩니다.
                var stream = new ReadOnlyByteBufferStream(data, false);

                try
                {
                    var decoded = parser.ReadPacket(stream);
                    Console.WriteLine(decoded.GetType());
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
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

    public void Stop()
    {
        udpClient.Close();
        receiveThread.Join(); // 스레드가 완료될 때까지 대기
    }
}