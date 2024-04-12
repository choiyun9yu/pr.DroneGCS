using gcs_system.MAVSDK;

namespace gcs_system.Services.Helper;

public class MavlinkMission
{
    private bool _isMission = false;
    private bool _isResponse = false;
    private MAVLink.MAVLINK_MSG_ID _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
    private string _messageType = "";    
    
    private double WAIT_TIME = 1500;
    private int MAX_RETRY_COUNT = 4;
    
    
    public async Task WaitforResponseAsync(MAVLink.MAVLinkMessage missionMsg)
    {
        // 초기화 
        _isMission = true;
        _isResponse = false;
        _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
        _messageType = "";
        
        // 비동기 작업 완료 대기 
        var waitTaskCompletionSource = new TaskCompletionSource<bool>();
        var timeOutTask = Task.Delay(TimeSpan.FromMilliseconds(WAIT_TIME));

        switch ((MAVLink.MAVLINK_MSG_ID)missionMsg.msgid)
        {
            // GCS 에서 Drone 으로 미션의 총 개수를 보내는 부분
            case MAVLink.MAVLINK_MSG_ID.MISSION_COUNT:
            {
                _messageType = "UploadMissionItems";
                Console.WriteLine("Mission Upload 하기 위해 드론으로 MISSION_COUNT 보내기!");
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST;
                break;
            }
            // Mission Item 
            case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT:
            {
                _messageType = "UploadMissionItems";
                // TODO:
                break;
            }
            // Download Mission 부분 GCS에서 미션 LIST 처음 요청하는 부분 
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_LIST:
            {
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_COUNT;
                break;
            }
            // Download Mission 부분 GCS에서 미션 보내달라 요청하는 부분
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
            {
                _messageType = "DownloadMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT;
                break;
            }
            // Clear Mission
            case MAVLink.MAVLINK_MSG_ID.MISSION_CLEAR_ALL:
            {
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
                break;
            }
        }

        // await SendMavlinkMsg();

    }
}