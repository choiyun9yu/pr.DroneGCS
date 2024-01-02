using MAVSDK;

namespace kisa_gcs_system.Services;

public class MavlinkEncoder : MessageToByteEncoder<MAVLink.MAVLinkMessage>
{
    private readonly MAVLink.MavlinkParse _parse = new MAVLink.MavlinkParse();
    
    protected override void Encode(IChannelHandlerContext context, MAVLink.MAVLinkMessage message, IByteBuffer output)
    {
        var encoded = _parse.GenerateMAVLinkPacket20(
            (MAVLink.MAVLINK_MSG_ID)message.msgid,
            message.data,
            sign: false,
            message.sysid,
            message.compid,
            message.seq
        );
        output.WriteBytes(encoded);
    }
}