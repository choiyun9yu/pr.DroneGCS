using MAVSDK;

namespace kisa_gcs_system.Services.Helper;


public class MavlinkEncoder : MessageToByteEncoder<MAVLink.MAVLinkMessage>
{
    private IChannelHandlerContext? _context;
    private IPEndPoint? _droneAddress;
    private readonly MAVLink.MavlinkParse _parser = new ();
    
    protected override void Encode(IChannelHandlerContext context, MAVLink.MAVLinkMessage message, IByteBuffer output)
    {
        var encodedMsg = _parser.GenerateMAVLinkPacket20(
            (MAVLink.MAVLINK_MSG_ID)message.msgid,
            message.data,
            sign: false,
            message.sysid,
            message.compid,
            message.seq
        );
        output.WriteBytes(encodedMsg);
    }
}
