// namespace kisa_gcs_service.Service;
//
// // 드론 메시지의 속성을 정의하고 메시지를 생성하고 파싱하는데 사용되는 클래스
// public class DroneMessage   
// {
//   public ushort StartCode { get; }
//   public byte ProtocolVersion { get; }
//   public COMMAND_CODE CommandID { get; }
//   public uint MessageID { get; }
//   public ushort DataLength { get; set; }
//
//   private System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
//
//   private string _rawContent;
//
//   public string RawContent
//   {
//     get { return this._rawContent; }
//     set
//     {
//       this._rawContent = value;
//       this.DataLength = (ushort)enc.GetByteCount(value);
//     }
//   }
//
//   // 생성자들은 각 속성을 초기화 한다.
//   public DroneMessage(
//     ushort startCode,
//     byte protocolVersion,
//     COMMAND_CODE commandID,
//     uint messageID,
//     ushort dataLength,
//     string rawContent
//   ) : this(startCode, protocolVersion, commandID, messageID)
//   {
//     this.DataLength = dataLength;
//     this.RawContent = rawContent;
//   }
//
//   public DroneMessage(ushort startCode, byte protocolVersion, COMMAND_CODE commandID, uint messageID)
//   {
//     this.StartCode = startCode;
//     this.ProtocolVersion = protocolVersion;
//     this.CommandID = commandID;
//     this.MessageID = messageID;
//     this.DataLength = 0;
//     this._rawContent = String.Empty;
//   }
//
//   public DroneMessage(COMMAND_CODE commandID, uint messageID)
//   {
//     this.StartCode = 0x7777;
//     this.ProtocolVersion = 0;
//     this.CommandID = commandID;
//     this.MessageID = messageID;
//     this.DataLength = 0;
//     this._rawContent = String.Empty;
//   }
//   
//   // 드론 메시지를 IByteBuffer로 변환한다.
//   public IByteBuffer ToByteBuffer(IByteBufferAllocator allocator)
//   {
//     var dataToByte = Encoding.ASCII.GetBytes(RawContent);
//
//     var bufSend = allocator.Buffer(11 + dataToByte.Length);
//
//     bufSend.WriteUnsignedShort(StartCode);
//     bufSend.WriteByte(ProtocolVersion);
//     bufSend.WriteUnsignedShort((ushort)CommandID);
//     bufSend.WriteInt((int)MessageID);
//     bufSend.WriteUnsignedShort(DataLength);
//     bufSend.WriteBytes(dataToByte);
//
//     return bufSend;
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
