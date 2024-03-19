using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using gcs_system.MAVSDK;

namespace gcs_system.Services.Helper;

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