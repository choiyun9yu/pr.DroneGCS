// using DotNetty.Buffers;
// using DotNetty.Transport.Channels.Sockets;
// using MongoDB.Bson.IO;
//
// namespace kisa_gcs_service.Services;
//
// using System;
// using System.IO;
// using System.Net;
// using System.Net.Sockets;
// using System.Threading;
//
// public class MavlinkUdpMessageDecoder
// {
//     private readonly MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();
//     private UdpClient udpClient;
//     private Thread receiveThread;
//
//     public MavlinkUdpMessageDecoder()
//     {
//         udpClient = new UdpClient(14556);
//         receiveThread = new Thread(ReceiveMessages);
//         receiveThread.Start();
//     }
//
//     private void ReceiveMessages()
//     {
//         try
//         {
//             while (true)
//             {
//                 IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
//                 byte[] data = udpClient.Receive(ref remoteEp);  // UDP로 받은 바이트 파일 
//                 
//                 var stream = new ReadOnlyByteBufferStream(data, false);
//
//                 try
//                 {
//                     var decoded = parser.ReadPacket(stream);    // input이 Stream Type
//                     // Console.WriteLine(decoded);
//                     Console.WriteLine(decoded.GetType());
//                 }
//                 catch (Exception e)
//                 {
//                     Console.Error.WriteLine(e.Message);
//                 }
//             }
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e.ToString());
//         }
//         finally
//         {
//             udpClient.Close();
//         }
//     }
//
//     public void Stop()
//     {
//         udpClient.Close();
//         receiveThread.Join(); // 스레드가 완료될 때까지 대기
//     }
// }