using System.Net;
using DotNetty.Transport.Channels;
using gcs_system.Interfaces;
using gcs_system.MAVSDK;
using Newtonsoft.Json;

namespace gcs_system.Services.Helper;

public class MavlinkMission()
{
    private IChannelHandlerContext? _context;
    private IPEndPoint? _droneAddress;
    private readonly MavlinkEncoder _encoder = new();
    private readonly MAVLink.MavlinkParse _parser = new();

    private List<MAVLink.mavlink_mission_item_int_t> _missionItems = new();
    private DroneState _droneState;
    
    private bool _isMission = false;
    private bool _isResponse = false;
    private MAVLink.MAVLINK_MSG_ID _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
    private string _messageType = "";  

    
    private double WAIT_TIME = 1500;
    private int MAX_RETRY_COUNT = 4;
    
    public async Task WaitforResponseAsync(IChannelHandlerContext ctx, IPEndPoint addr, MAVLink.MAVLinkMessage msg, int count)
    {
        _context = ctx;
        _droneAddress = addr;
        _isMission = true;
        _isResponse = false;
        _messageType = "UploadMissionItems";
        _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST;

        // 메시지 보내기
        // Console.WriteLine($"Send to Drone about MISSION_COUNT({count})");
        await _encoder.SendCommandAsync(_context, _droneAddress, msg);
        
        // 비동기 작업 완료 대기 객체 설정
        var waitTaskCompletionSource = new TaskCompletionSource<bool>();
        var timeOutTask = Task.Delay(TimeSpan.FromMilliseconds(WAIT_TIME));
        
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
            Console.WriteLine(retryCount + 1 + " 번 째 재시도");
            await _encoder.SendCommandAsync(_context, _droneAddress, msg); 
        }

    }
    
     public async Task WaitforResponseAsync(IChannelHandlerContext ctx, IPEndPoint addr, MAVLink.MAVLinkMessage msg)
    {
        _context = ctx;
        _droneAddress = addr;
        
        // 초기화 
        _isMission = true;
        _isResponse = false;
        _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
        _messageType = "";

        // 보내는 메시지에 따라 설정
        switch ((MAVLink.MAVLINK_MSG_ID)msg.msgid)
        {
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_LIST:
            {
                Console.WriteLine("Send to Drone about MISSION_REQUEST_LIST!");
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_COUNT;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
            {
                Console.WriteLine("Send to Drone about MISSION_REQUEST_INT!");
                _messageType = "DownloadMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_ACK:
            {
                Console.WriteLine("Send to Drone about MISSION_ACK!");
                _messageType = "UploadMissionAck";
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_CLEAR_ALL:
            {
                // Console.WriteLine("Send to Drone about MISSION_CLEAR_ALL!");
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT:
            {
                Console.WriteLine("Send to Drone about MISSION_ITEM_INT!");
                _messageType = "UploadMissionItems";
                break;
            }

        }

        // 메시지 보내기
        await _encoder.SendCommandAsync(_context, _droneAddress, msg);
        
        // 비동기 작업 완료 대기 객체 설정
        var waitTaskCompletionSource = new TaskCompletionSource<bool>();
        var timeOutTask = Task.Delay(TimeSpan.FromMilliseconds(WAIT_TIME));
        
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
            Console.WriteLine(retryCount + 1 + " 번 째 재시도");
            await _encoder.SendCommandAsync(_context, _droneAddress, msg); 
        }

    }
     
    public async Task WaitforResponseAsync(MAVLink.MAVLinkMessage msg)
    {
        _isMission = true;
        _isResponse = false;
        _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
        _messageType = "";
        
        switch ((MAVLink.MAVLINK_MSG_ID)msg.msgid)
        {
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_LIST:
            {
                // Console.WriteLine("Send to Drone about MISSION_REQUEST_LIST!");
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_COUNT;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
            {
                // Console.WriteLine("Send to Drone about MISSION_REQUEST_INT!");
                _messageType = "DownloadMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_ACK:
            {
                // Console.WriteLine("Send to Drone about MISSION_ACK!");
                _messageType = "UploadMissionAck";
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_CLEAR_ALL:
            {
                // Console.WriteLine("Send to Drone about MISSION_CLEAR_ALL!");
                _messageType = "ClearMissionItems";
                _waitMsgId = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
                break;
            }
            case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT:
            {
                // Console.WriteLine("Send to Drone about MISSION_ITEM_INT!");
                _messageType = "UploadMissionItems";
                break;
            }

        }
        
        await _encoder.SendCommandAsync(_context, _droneAddress, msg);
        
        // 비동기 작업 완료 대기 객체 설정
        var waitTaskCompletionSource = new TaskCompletionSource<bool>();
        var timeOutTask = Task.Delay(TimeSpan.FromMilliseconds(WAIT_TIME));
        
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
            Console.WriteLine(retryCount + 1 + " 번 째 재시도");
            await _encoder.SendCommandAsync(_context, _droneAddress, msg); 
        }

    }
    
    public void SetMissionItems(string? droneId, double x, double y, List<DroneLocation> missionTransits, int alt)
    {
        MAVLink.mavlink_mission_item_int_t firstBody = new MAVLink.mavlink_mission_item_int_t
        {
            seq = 0,
            target_system = byte.Parse(droneId),
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,
            command = 16,
            autocontinue = 1,
            frame = 3,         
            x = (int)Math.Round(x * 10000000),
            y = (int)Math.Round(y * 10000000),
            z = alt,           
        };
        _missionItems.Add(firstBody);
        
        MAVLink.mavlink_mission_item_int_t takeoffBody = new MAVLink.mavlink_mission_item_int_t
        {
            seq = 1,
            target_system = byte.Parse(droneId),
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,
            command = 22,
            autocontinue = 1,
            frame = 3,
            x = (int)Math.Round(x * 10000000),
            y = (int)Math.Round(y * 10000000),
            z = alt,           
        };
        _missionItems.Add(takeoffBody);
        
        for (int i = 0; i < missionTransits.Count; i++)
        {
            var points = missionTransits[i];
            // Console.WriteLine($"Waypoint {i + 1}: (lat: {points.lat}, lng: {points.lng})");
            
            MAVLink.mavlink_mission_item_int_t waypointBody = new MAVLink.mavlink_mission_item_int_t
            {
                seq = (ushort)(i+2),
                // seq = (ushort)i,
                target_system = byte.Parse(droneId),
                mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,
                command = 16,
                autocontinue = 1,
                frame = 3,         
                x = (int)Math.Round(points.lat * 10000000),
                y = (int)Math.Round(points.lng * 10000000),
                z = alt,           
            };
            _missionItems.Add(waypointBody);
        }

        // To Check MissionItems
        // foreach (var e in _missionItems)
        // {
        //     Console.Write("seq: " + JsonConvert.SerializeObject(e.seq));
        //     Console.Write(", command: " + JsonConvert.SerializeObject(e.command));
        //     Console.Write(", x: " + JsonConvert.SerializeObject(e.x));
        //     Console.Write(", y: " + JsonConvert.SerializeObject(e.y));
        //     Console.Write(", z: " + JsonConvert.SerializeObject(e.z));
        //     Console.WriteLine(", frame: " + JsonConvert.SerializeObject(e.frame));
        // }

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
                    await SendMavMissionSeq(data.seq);
                    // Console.WriteLine("-------------------------------------");
                    // Console.WriteLine($"Receive mission_request({data.seq})");
                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
                {
                    var data = (MAVLink.mavlink_mission_request_int_t)msg.data;
                    await SendMavMissionSeq(data.seq);
                    // Console.WriteLine("-------------------------------------");
                    // Console.WriteLine($"Receive mission_request_int ({data.seq})");
                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_ACK:
                {
                    var data = (MAVLink.mavlink_mission_ack_t)msg.data;
                    HandleMissionAct(data);
                    // Console.WriteLine("-------------------------------------");
                    // Console.WriteLine("Receive mission_ack");

                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_COUNT:
                {
                    var data = (MAVLink.mavlink_mission_count_t)msg.data;
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine($"Receive mavlink_mission_count({data.count})");
                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM:
                {
                    var data = (MAVLink.mavlink_mission_item_t)msg.data;
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine($"Receive mavlink_mission_item");
                    break;
                }
                case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT:
                {
                    var data = (MAVLink.mavlink_mission_item_int_t)msg.data;
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine($"Receive mavlink_mission_item_int");
                    break;
                }
            }
            
        }
        
    }

    private async Task SendMavMissionSeq(ushort seq)
    {
        var missionItemMsg =  new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, _missionItems[seq]));

        // To Check Mission Item Params
        // Console.WriteLine($"Send mission_item_int({seq})");
        // Console.WriteLine("-------------------------------------");
        // Console.WriteLine($"seq: {_missionItems[seq].seq}");
        // Console.WriteLine($"command: {_missionItems[seq].command}");
        // Console.WriteLine($"target_sys: {_missionItems[seq].target_system}");
        // Console.WriteLine($"target_component: {_missionItems[seq].target_component}");
        // Console.WriteLine($"mission_type: {_missionItems[seq].mission_type}");
        // Console.WriteLine($"auto_continue: {_missionItems[seq].autocontinue}");
        // Console.WriteLine($"current: {_missionItems[seq].current}");
        // Console.WriteLine($"frame: {_missionItems[seq].frame}");
        // Console.WriteLine($"x: {_missionItems[seq].x}");
        // Console.WriteLine($"y: {_missionItems[seq].y}");
        // Console.WriteLine($"z: {_missionItems[seq].z}");
        // Console.WriteLine($"pram1: {_missionItems[seq].param1}");
        // Console.WriteLine($"pram2: {_missionItems[seq].param2}");
        // Console.WriteLine($"pram3: {_missionItems[seq].param3}");
        // Console.WriteLine($"pram4: {_missionItems[seq].param4}");
        
        await _encoder.SendCommandAsync(_context, _droneAddress, missionItemMsg);
    }

    private void HandleMissionAct(MAVLink.mavlink_mission_ack_t act)
    {
        if ((MAVLink.MAV_MISSION_RESULT)act.type == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
        {
            Console.WriteLine("임무 수락");
            _isResponse = true;
            _isMission = false;
            _messageType = "";
            _missionItems = new();
        }
        
        if ((MAVLink.MAV_MISSION_RESULT)act.type == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ERROR)
        {
            Console.WriteLine("임무 에러");
            _isResponse = true;
            _isMission = false;
            _messageType = "";
            _missionItems = new();
        }
        
        if ((MAVLink.MAV_MISSION_RESULT)act.type == MAVLink.MAV_MISSION_RESULT.MAV_MISSION_OPERATION_CANCELLED)
        {
            Console.WriteLine("임무 취소");
            _isResponse = true;
            _isMission = false;
            _messageType = "";
            _missionItems = new();
        }
        
    }
    
}