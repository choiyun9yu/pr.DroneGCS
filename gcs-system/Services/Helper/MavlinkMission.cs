using System.Net;
using DotNetty.Transport.Channels;
using gcs_system.Interfaces;
using gcs_system.MAVSDK;

namespace gcs_system.Services.Helper;

public class MavlinkMission()
{
    private IChannelHandlerContext? _context;
    private IPEndPoint? _droneAddress;
    private readonly MavlinkEncoder _encoder = new();
    private readonly MAVLink.MavlinkParse _parser = new();
    
    private bool _isMission = false;
    private bool _isResponse = false;
    private MAVLink.MAVLINK_MSG_ID _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
    private string _messageType = "";  
    List<MAVLink.mavlink_mission_item_int_t> _missionItems = new();
    
    private double WAIT_TIME = 1500;
    private int MAX_RETRY_COUNT = 4;
    
    public async Task WaitforResponseAsync(IChannelHandlerContext ctx, IPEndPoint addr, MAVLink.MAVLinkMessage msg)
    {
        _context = ctx;
        _droneAddress = addr;
        
        // 초기화 
        _isMission = true;
        _isResponse = false;
        _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
        _messageType = "";
        
        // 비동기 작업 완료 대기 
        var waitTaskCompletionSource = new TaskCompletionSource<bool>();
        var timeOutTask = Task.Delay(TimeSpan.FromMilliseconds(WAIT_TIME));

        switch ((MAVLink.MAVLINK_MSG_ID)msg.msgid)
        {
            case MAVLink.MAVLINK_MSG_ID.MISSION_COUNT:
            {
                _messageType = "UploadMissionItems";
                Console.WriteLine("Mission Upload 하기 위해 드론으로 MISSION_COUNT 보내기!");
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT:
            {
                _messageType = "UploadMissionItems";
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_LIST:
            {
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_COUNT;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
            {
                _messageType = "DownloadMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_CLEAR_ALL:
            {
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
                break;
            }
        }

        await _encoder.SendCommandAsync(_context, _droneAddress, msg);
        
        // 재시도 횟수를 고려하여 응답 대기 
        for (int retryCount = 0; retryCount <= MAX_RETRY_COUNT; retryCount++)
        {
            await Task.WhenAny(waitTaskCompletionSource.Task, timeOutTask);

            if (_isResponse)
            {
                waitTaskCompletionSource.TrySetResult(true);
                break;
            }
            if (retryCount >= MAX_RETRY_COUNT)
            {
                Console.WriteLine("MISSION_UPLOAD_FAIL");
                waitTaskCompletionSource.TrySetResult(true);
                break;
            }
            Console.WriteLine(retryCount + " 번 째 재시도");
            await _encoder.SendCommandAsync(_context, _droneAddress, msg); 
        }

    }
    
    public void SetMissionItems(string? droneId, List<DroneLocation> missionTransits, int alt)
    {
        for (int i = 0; i < missionTransits.Count; i++)
        {
            var points = missionTransits[i];
            Console.WriteLine($"미션 요소 {i + 1}: lat: {points.lat}, lng: {points.lng}");
            
            MAVLink.mavlink_mission_item_int_t commandBody = new MAVLink.mavlink_mission_item_int_t
            {
                target_system = byte.Parse(droneId),
                seq = i,
                mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,
                command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
                frame = 6,         
                x = (int)Math.Round(points.lat * 10000000),
                y = (int)Math.Round(points.lng * 10000000),
                z = alt,           
            };
            
            _missionItems.Add(commandBody);
        }

    }
    
    public async void UpdateMissionState(MAVLink.MAVLinkMessage msg)
    {
        var msgid = (MAVLink.MAVLINK_MSG_ID)msg.msgid;
        if (_isMission && msgid == _waitMsgId || msgid == MAVLink.MAVLINK_MSG_ID.MISSION_ACK)
        {
            
            // 재시도 끝내는 트리거
            _isResponse = true;

            switch ((MAVLink.MAVLINK_MSG_ID)msg.msgid)
            {
                case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST:
                {
                    var data = (MAVLink.mavlink_mission_request_t)msg.data;
                    Console.WriteLine($"receive: mission_request ({data.seq})");
                    
                    // TODO: 여기서 제대로 보냈으면 변화가 있어야하는데 제대로 보내지 않은 듯
                    await SendMavMissionSeq(data.seq);
                    
                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
                {
                    var data = (MAVLink.mavlink_mission_request_int_t)msg.data;
                    Console.WriteLine($"receive: mission_request_int ({data.seq})");
                    await SendMavMissionSeq(data.seq);
                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_ACK:
                {
                    var data = (MAVLink.mavlink_mission_ack_t)msg.data;
                    HandleMissionAct(data);
                    break;
                }
            }
            
        }
        
    }

    private async Task SendMavMissionSeq(ushort seq)
    {
        var missionItemMsg = _parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, _missionItems[seq]);
        Console.WriteLine($"{seq} 번 째 Mission_ITEM 전송");
        await _encoder.SendCommandAsync(_context, _droneAddress, new MAVLink.MAVLinkMessage(missionItemMsg));
    }

    private void HandleMissionAct(MAVLink.mavlink_mission_ack_t act)
    {
        if ((MAVLink.MAV_MISSION_RESULT)act.type == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
        {
            Console.WriteLine("임무 성공");
            _isMission = false;
            _messageType = "";
        }
        
        if ((MAVLink.MAV_MISSION_RESULT)act.type == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ERROR)
        {
            Console.WriteLine("임무 에러");
            _isMission = false;
            _messageType = "";
        }
        
        if ((MAVLink.MAV_MISSION_RESULT)act.type == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_OPERATION_CANCELLED)
        {
            Console.WriteLine("임무 취소");
            _isMission = false;
            _messageType = "";
        }
        
    }
    
}