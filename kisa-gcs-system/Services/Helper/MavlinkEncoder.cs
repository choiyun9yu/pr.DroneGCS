using MAVSDK;

namespace kisa_gcs_service.Services.Helper;

public class MavlinkEncoder : MessageToByteEncoder<MAVLink.MAVLinkMessage>
{
    private readonly MAVLink.MavlinkParse _parser = new ();
    
    protected override void Encode(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg, IByteBuffer output)
    {
        var encodeMsg = _parser.GenerateMAVLinkPacket20(
            (MAVLink.MAVLINK_MSG_ID)
            msg.msgid,
            msg.data,
            sign: false,
            msg.sysid,
            msg.compid,
            msg.seq
        );
        output.WriteBytes(encodeMsg);
    }
    
}