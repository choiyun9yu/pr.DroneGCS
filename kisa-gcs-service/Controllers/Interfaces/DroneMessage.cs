// #nullable disable
//
// using DotNetty.Buffers;
// using System;
// using System.Text;
// using DotNetty.Codecs;
// using DotNetty.Transport.Channels;
// using System.Collections.Generic;
//
// namespace kisa_gcs_service.Controllers
// {
//   public class DroneMessage
//   {
//     public ushort StartCode { get; }
//     public byte ProtocolVersion { get; }
//     public COMMAND_CODE CommandID { get; }
//     public uint MessageID { get; }
//     public ushort DataLength { get; set; }
//
//     private System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
//
//     private string _rawContent;
//
//     public string RawContent
//     {
//       get
//       {
//         return this._rawContent;
//       }
//       set
//       {
//         this._rawContent = value;
//         this.DataLength = (ushort)enc.GetByteCount(value);
//       }
//     }
//
//     public DroneMessage(
//       ushort startCode,
//       byte protocolVersion,
//       COMMAND_CODE commandID,
//       uint messageID,
//       ushort dataLength,
//       string rawContent
//     ) : this(startCode, protocolVersion, commandID, messageID)
//     {
//       this.DataLength = dataLength;
//       this.RawContent = rawContent;
//     }
//
//     public DroneMessage(ushort startCode, byte protocolVersion, COMMAND_CODE commandID, uint messageID)
//     {
//       this.StartCode = startCode;
//       this.ProtocolVersion = protocolVersion;
//       this.CommandID = commandID;
//       this.MessageID = messageID;
//       this.DataLength = 0;
//       this._rawContent = String.Empty;
//     }
//
//     public DroneMessage(COMMAND_CODE commandID, uint messageID)
//     {
//       this.StartCode = 0x7777;
//       this.ProtocolVersion = 0;
//       this.CommandID = commandID;
//       this.MessageID = messageID;
//       this.DataLength = 0;
//       this._rawContent = String.Empty;
//     }
//
//     public static DroneMessage Parse(IByteBuffer buffer)
//     {
//       return new DroneMessage(
//         buffer.ReadUnsignedShort(),
//         buffer.ReadByte(),
//         (COMMAND_CODE)buffer.ReadUnsignedShort(),
//         buffer.ReadUnsignedInt(),
//         buffer.ReadUnsignedShort(),
//         buffer.ReadString(buffer.ReadableBytes, Encoding.ASCII)
//       );
//     }
//
//     public IByteBuffer ToByteBuffer(IByteBufferAllocator allocator)
//     {
//       var dataToByte = Encoding.ASCII.GetBytes(RawContent);
//
//       var bufSend = allocator.Buffer(11 + dataToByte.Length);
//
//       bufSend.WriteUnsignedShort(StartCode);
//       bufSend.WriteByte(ProtocolVersion);
//       bufSend.WriteUnsignedShort((ushort)CommandID);
//       bufSend.WriteInt((int)MessageID);
//       bufSend.WriteUnsignedShort(DataLength);
//       bufSend.WriteBytes(dataToByte);
//
//       return bufSend;
//     }
//
//     public override string ToString()
//     {
//       return String.Format(
//         "Start code: {0}\n" +
//         "Protocol version: {1}\n" +
//         "Command ID: {2}\n" +
//         "Message ID: {3}\n" +
//         "Data length: {4}\n" +
//         "Raw content: {5}",
//         StartCode, ProtocolVersion, CommandID, MessageID, DataLength, RawContent);
//     }
//   }
//   public class DroneMessageDecoder : MessageToMessageDecoder<IByteBuffer>
//   {
//     protected override void Decode(IChannelHandlerContext context, IByteBuffer buffer, List<object> output)
//     {
//       output.Add(DroneMessage.Parse(buffer));
//     }
//   }
//
//   public class DroneMessageEncoder : MessageToMessageEncoder<DroneMessage>
//   {
//     protected override void Encode(IChannelHandlerContext context, DroneMessage message, List<object> output)
//     {
//       output.Add(message.ToByteBuffer(context.Allocator));
//     }
//   }
// }