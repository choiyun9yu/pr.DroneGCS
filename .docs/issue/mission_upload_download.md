
# Mission Start 
- React 에서 SignalRContainer/handleAutoMissionStart( )를 사용해서 GCS 의 DroneMonitorManager/SendMavCommand( )를 호출한다.
- SendMavCommand( )는 droneId 를 사용하여 MAVLinkDroneState 객체를 가져와 MAVLinkDroneState/SendMavCommand( )를 호출한다.

#### MAVLinkDroneState.cs
      public async Task SendMavCommand(DeliveryCmdType cmdType)
      {
        // 1) cmdType 이 AutoMission 인 경우 
        if (cmdType == DeliveryCmdType.AutoMissionStart)
        { 
          1-1) 처음에 arm 을 우선 날린다.
          var armCommand = _composeMavCommandMessage(DeliveryCmdType.Arm);
          var armCommandRes = await _sendMavCommand(armCommand);
          if (_deliveryMissionService != null) await _deliveryMissionService.SendNotificationToClient(armCommandRes.Message));
      
          1-2) arm 이 수락된 경우 mission start 를 날린다.
          if (armComposeRes.result is (byte)MAVLink.MAV_RESULT.ACCEPTED)
          {
            var missionStartRes = await _sendMavCommand(_composeMavCommandmessage(DeliveryCmdType.MissionStart));
            if (_deliveryMissionService != null) await _deliveryMissionService.SendNotificationToClient(missionStartRes.Message);
          }
      
        }
        // cmdType 이 AutoMission 이 아닌 경우 들어온 타입을 그대로 날린다.
        else 
        {
          var commandBdoy = _composeMavCommandMessage(cmdType);
          var res = await _sendMavCommand(commandBody);
          if (_deliveryMissionService != null)
          {
            await _deliveryMissionService.SendNotificationToClient(res.Message);
          }
        }
      }

#### _composeMavCommandMessage
      private MAVLink.mavlink_command_long_t _composeMavCommandMessage(DeliveryCmdType cmdType) 
      {
        var targetSystem = Sysid;
      
        // 2) cmdType 에 따라 MAVLink.mavlink_command_long_t 생성하여 반환
        switch (cmdType)
        {
          case DelvieryCmdType.Arm
          {
            return new MAVLink.mavlink_command_long_t
            {
              target_system = targetSystem,
              command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM,
              param1 = 1
            };
          }
          case DelvieryCmdType.MissionStart
          {
            return new MAVLink.mavlink_command_long_t
            {
              target_system = targetSystem,
              command = (ushort)MAVLink.MAV_CMD.MISSION_START,
              param1 = 0,
              param2 = 0,
            };
          }
          case DelvieryCmdType.DoSetServoHigh
          {
            return new MAVLink.mavlink_command_long_t
            {
              target_system = targetSystem,
              command = (ushort)MAVLink.MAV_CMD.DO_SET_SERVO,
              param1 = 10,
              param2 = 1900
            };
          }
          case DelvieryCmdType.DoSetServoLow
          {
            return new MAVLink.mavlink_command_long_t
            {
              target_system = targetSystem,
              command = (ushort)MAVLink.MAV_CMD.DO_SET_SERVO,
              param1 = 10,
              param2 = 1100
            };
          }
          default:
            throw new Exception("Unsupported");
        }
      }

#### _sendMavConmmand
      private async Task(string Message, byte? result)> _sendMavCommand(MAVLink.mavlink_command_log_t commandBody)
      {
        string printMsg = "";
        // 미션을 가지고 잇는 경우(?)
        if (MavMission != null) 
        { 
          try
          {
            // 3)
            var res = await _mavCommandMicroservice.SendMavCommandAndWaitForAck(commandBody);
      
            // 3-1) command 타입에 따라 PrintMsg 다르게 설정
            switch (commandBody.command)
            {
              case (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM:
              {
                printMsg = "COMPONENT_ARM";
                break;
              }
              case (ushort)MAVLink.MAV_CMD.MISSION_START:
              {
                printMsg = "MISSION_START";
                break;
              }
              case (ushort)MAVLink.MAV_CMD.DO_SET_SERVO:
              {
                printMsg = "DO_SET_SERVO";
                break;
              }
              case (ushort)MAVLink.MAV_CMD.DO_SET_RELAY:
              {
                printMsg = "DO_SET_RELAY";
                break;
              }
            }
      
            // 3-2) 메시지가 수락된 경우 
            if (res.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
            {
              return (Message: printMsg + " Accepted", res.result);
            }
            
            return (Message: printMsg + " Failed", res.result);
          }
          // 3-3) 시간 초과한 경우
          catch (TimeoutException) 
          {
            return (Message: printMsg + " timed out", result: null);
          }
        } 
        // 3-4) MavMission 이 null 인 경우(?)
        return (Message: "Sorrt unexcepted request", result: null)
      }
      ...
  
  }

#### MavCommandMicroservice
    public class MavCommandMicroservice
    {
      private readonly MAVLinkDroneState _mavLinkDroneState;
      private readonly MAVLink.MavlinkParse _mavlinkParser = new();
      private readonly List<CommnadQueueItem> _messageQueue = [];
      private const int Timeout = 2000;  //ms
    
      // 생성자
      pulic MavCommandMicroservice(MAVLinkDroneState mavLinkDroneState)
      {
        _mavLinkDroneState = mavLinkDroneState;
        // OnNewMavlinkMessage 이벤트를 구독하여 새로운 마브링크 메시지가 들어올 때마다 _hanleMavMessage( )가 실행되어 7) 과정 수행 가능
        _mavLinkDroneState.OnNewMavlinkMessage += _handleMavMessage;
      }
      ...

      struct CommandQueueItem
      {
        public MavLink.MAVLinkMessage Message;
        public TaskCompletionSource<MAVLink.mavlink_command_ack_t> Ack;
      }

#### SendMavCommandAndWaitForAck(mavlink_command_log_t)
      public Task<MAVLink.mavlink_command_ack_t> SendMavCommandAndWaitForAck(MAVLink.mavlink_command_long_t data)
      {
        // 4) mavlink message 생성 
        var msg = new MAVLink.MAVLinkMessage(_mavlinkParser.GenerateMAVLinkPacket20(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, data));
        // 5) 생성된 mavlink message .... 전송
        return SendMavCommandAndWaitForAck(msg);
      }

#### SendMavCommandAndWairForAck(MAVLink.MAVLinkMessage)
      public Sync Task<MAVLink.mavlink_command_ack_t> SendMavCommandAndWaitForAck(MAVLink.MAVLinkMesasge msg)
      {
        // 6) mavlink message 를 전송하고 그에 mavlink command ack 를 기다린다.
        // 6-1) 드론으로 메시지 전송
        await _mavLinkDroneState.SendMavlinkMsg(msg);
      
        // 6-2) 명령 확인 응답을 받을 때까지 기다리는 task 객체 생성 (TaskCompletionSource 는 비동기 작업의 완료를 외부에서 제어할 수 있는 메커니즘을 제공)
        var task = new TaskCompletionSource<MAVLink.mavlink_command_ack_t>();
      
        // 6-3) 메시지와 이 메시지에 대한 응답을 기다리는 TaskCompletionSource 를 큐에 추가 (이 큐는 나중에 명령 확인 응답을 처리할 때 사용)
        _messageQueue.Add(new CommandQueueItem { Message = msg, Ack = task });
        
        // 6-4) CancellationTokenSource 를 사용하여 타움아웃을 설정 (Task.Delay(Timeout, cts,Token)는 설정된 타임아웃 후에 완료되는  timeoutTask 를 생성한다.)
        using var cts = new CancellationTokenSource();
        var timeoutTask = Task.Delay(Timeout, cts.Token);
      
        // 10) Task.WhenAny(task.Task, timeoutTask)를 통해 명령 응답(task.Task) 또는 타임아웃(timeoutTask) 중 먼저 완료되는 것을 기다린다.
        var resultTask = await Task.WhenAny(task.Task, timeoutTask);
        // 10-1) 타임 아웃인 경우 TimeoutException 에러 던지기
        if (resultTask == timeoutTask)
        {
          task.TrySetException(new TimeoutException());
          throw new TimeoutException();
        }
        // 10-2) 성공적으로 task 를 완료한 경우 
        await cts.CancelAsync();
        return await task.Task;
      }
      
      
#### _handleMavMessage()
      private void _handlemavMessage(MAVLink.MAVLinkMessage message)
      {
        // 7) 요청한 COMMAND_LONG or INT 의 응답을 기다렸다가 처리 
        // 7-1) 필터링, COMMAND_ACK 응답이 아니면 메서드 종료
        if (message.msgid != (byte)MAVLink.MAVLINK_MSG_ID.COMMAND_ACK) return;
      
        // 7-2) 응답 데이터 추출
        var responseData = (MAVLink.mavlink_command_ack_t)message.data;
      
        // 7-3) 대기중인 메시지와 비교하기 위해 foreach 로 waitMsg 에 하나씩 받기
        foreach (var waitMsg in _messageQueue)
        {
          // 기다리고 있는 메시지 큐 객체의 Message 필드의 msgid 로 요청을 보냈을 때 COMMAND 확인
          switch (waitMsg.Message.msgid)
          {
            // 7-2) COMMAND_LONG 으로 보낸 경우 전송한 command 와 수신한 command 가 같지 않으면 continue 를 사용하여 다음 반복으로 넘어가기
            case (byte)MAVLink.MAVLink_MSG_ID.COMMAND_LONG:
              if (((MAVLink.mavlink_command_long_t)waitMsg.Message.data).command != responseData.command)
              {
                continue;
              }
              // 7-3) 일치하면 break 로 foreach 루프문 종료
              break;
            // COMMAND_INT 로 명령했던 경우
            case (byte)MAVLink.MAVLink_MSG_ID.COMMAND_INT:
              if (((MAVLink.mavlink_command_long_t)waitMsg.Message.data).command != responseData.command)
              {
                continue;
              }
              break;
            default:
              countinue;
          }
      
          // 8) 루프문을 나왔다는 것은 해당 명령과 일치하는 응답을 수신했다는 의미이므로 messageQueue 에서 삭제
          _messageQueue.Remove(waitMsg);
      
          // 9) 기다리는 메시지 완료처리
          waitMsg.Ack.SetResult(responseData);
        }
        
      }

    }

<br>

# Mission Download


<br>

# Mission Upload
![image](https://github.com/user-attachments/assets/ea4655da-40f1-4c3f-a870-40f278b62b24)

## Mission upload process in our source code 
- React 가 mission item 들을 MongoDB 로부터 가져와 GCS 로 보낸다.  
  (React 가 SignalR 로 GCS 의 DroneMonitorManager/SendMavlinkMission( )호출)
- SendMavlinkMission( )는 missionItems 를 생성하고 _sendMavlinkMission( )를 호출한다.
- _sendMavlinkMission( )는 droneId 를 사용하여 _droneStateMap 에서 selectedDroneState를 가져온다.
- 해당 드론 객체의 StartMAVMission( )를 통해 MavMissionMicroservice/StartUploadMAVMission( )를 호출한다.

### MavMissionMicroservice.cs 
    public class MavMissionMicroservice
    {
      private readonly MAVLinkDroneState _mavLinkDroneState;
      private readonly string _droneId;
      private readonly MAVLink.MavlinkParse _mavlinkParser = new();
      private MavlinkMissionStatus? _mavMissionSession;
      private TaskCompletionSource<bool>? _missionDownloadAckTask;
      private const double WaitTime = 250;
      private const int MaxRetryCount = 5;
      private bool _isMission;
      public MAVLink.mavlink_mission_item_int_t[]? MavMission;
      public double TotalDistance;
      ...

      // 생성자 
      public MavMissionMicroservice(MAVLinkDroneState mavLinkDroneState, string droneId)
      {
        // 자기 참조 패턴을 사용하였다. MavLinkDroneState 생성자에서 MavMissionMicroservice 를 생성하며 자기 자신을 인자로 전달했다.
        this._mavLinkDroneState = mavLinkDroneState;
        this._droneId = droneId;
        // 이벤트 구독, OnNewMavlinkMessage 는 MAVLinkDroneState 클래스의 이벤트로, 새로운 MAVLink 메시지가 수신될 때 트리거 된다. 이벤트는 특정 조건이 발생했을 때 외부에서 제공한 콜백함수를 호출하는 매커니즘이다.우리 코드에서는 MAVLinkDroneState.cs 의 HandleMavlinkmesasge( ) 에서 OnNewMavLinkMessage?.Invoke(drone Message)형태로 호출되었다.
        this._mavLinkDroneState.OnNewMavlinkMessage += async (message) => await this._handleMavMessage(message);
      }

      public struct MavlinkMissionStatus
      {
        public DateTime StartedAt;
        public int Seq;
        public DateTime LastMsgReceivedAt;
        public DateTime? FinishedAt;
        public bool[] MissionItemReceivingStatus;
        public MAVLink.mavlink_mission_item_int_t[] MissionItems;
        public MAVLink.MAV_MISSION_TYPE MissionType;
      }
      ...
      
#### StartUploadMAVMission( ), 드론으로 미션을 보내는 부분
      /**
       * Send a `MISSION_COUNT` to the drone, create `MavMissionSession` and wait for `MISSION_REQUEST_INT` msg from drone
       */
      public async Task StartUploadMAVMission(MAVLink.mavlink_mission_item_int_t[] mavMission, MAVLink.MAV_MISSION_TYPE missionType = MAVLink.MAV_MISSION_TYPE.MISSION)
      {
        // 1. 드론으로 보낼 미션 카운트 메시지 생성 
        var missionCountMsg = this._mavlinkParser.GenerateMAVLinkPacket20(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT,
          new MAVLink.mavlink_mission_count_t()
          {
            count = (ushort)mavMission.Length,  // 미션을 담은 배열의 길이로 카운트
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,
            target_system = byte.Parse(this._droneId)
          });

        // 2. 미션 세션을 생성
        this._mavMissionSession = new MavlinkMissionStatus()
        {
          MissionItems = mavMission,
          Seq = -1,
          StartedAt = DateTime.Now,
          LastMsgReceivedAt = DateTime.Now,
          MissionType = missionType,
        };
        
        // 3. 메소드가 호출되어 실행하는 도중 이전의 미션 다운로드 비동기처리가 완료되지 않은 경우(null이 아니고 false 인 경우), 이전의 미션 다운로드 Task 는 취소 상태로 설정, TaskCanceledException 발생시킴
        if (this._missionDownloadAckTask is { Task.IsCompleted: false })
        {
          this._missionDownloadAckTask.SetCanceled();
        }

        // 4. 보낼 메시지 유형과 기다릴 메시지 유형을 설정하고 메시지를 전송
        await WaitforResponseAsync(new MAVLink.MAVLinkMessage(missionCountMsg)); //재전송 체크 

        // ?. 드론의 상태를 관리하는 _mavLinkDroneState 객체에서 HandleProgressEvent( ) 메서드가 ProgressEvent 객체를 생성하여 미션 업로드 진행 상태를 0으로 초기화?
        this._mavLinkDroneState.HandleProgressEvent(
          new ProgressEvent()
          {
            Type = "UploadMissionItems",
            Current = 0,
            Total = MavMission?.Length ?? 0,
          });
      }
      ...
      
#### WaitforResponseAsync
      private async Task WaitforResponseAsync(MAVLink.MAVLinkMessage missionReqMsg, int retryCount = 0)
      {
        // recursive(재귀) break poin, 재시도 횟수가 MaxRetryCount 이상이면 ProgressEvent 실패 타입을 생성하고 메소드 종료
        if (retryCount >= MaxRetryCount)
        {
          this._mavLinkDroneState.HandleProgressEvent(new ProgressEvent()
          {
            Type = $"{_mesTpye}_fail",
          });
          return;  // issue: 여기서 return 을 반환하지 않아 136만 번 이상 재시도하는 버그 발생
        }
        // 재시도 횟수가 0보다 높으면 콘솔에 기록 
        if (retryCount > 0)
        {
          Console.WriteLine($"한번 더 시도합니다. retry  {_waitmsgid} : {retryCount} ");
        }

        // 5. 기다릴 메시지 아이디, 기다릴 시간 등을 설정
        _mesTpye = "";
        _isMission = true;
        _waitmsgid = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
        _missionDownloadAckTask = new TaskCompletionSource<bool>();
        var timeoutTask = Task.Delay(TimeSpan.FromMilliseconds(WaitTime));
        
        // 6. switch 문을 사용하여 입력된 메시지 아이디에 따라 메시지 타입과 기다릴 메시지 아이디를 재설정
        switch ((MAVLink.MAVLINK_MSG_ID)missionReqMsg.msgid)
        {
          // upload Mission 부분 GCS에서 드론으로 미션의 총 갯수 보낼 때
          case MAVLink.MAVLINK_MSG_ID.MISSION_COUNT:
          {
            _mesTpye = "UploadMissionItems";
            _waitmsgid = MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST;
            break;
          }
          // upload Mission 부분 GCS에서 미션 아이템을 보낼 때
          case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT:
          {
            _mesTpye = "UploadMissionItems";
            var data = (MAVLink.mavlink_mission_item_int_t)missionReqMsg.data;
            var seq = data.seq;
            _waitmsgid = seq < this._mavMissionSession?.MissionItems.Length - 1 
              ? MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST : MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
            Console.WriteLine($"보내는 미션 번호 : {data.seq} 총 갯수 {this._mavMissionSession?.MissionItems.Length ?? 0}");
            break;
          }
          // download Mission 부분 GCS에서 미션LIST 처음 요청할 때
          case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_LIST:
          {
            _mesTpye = "DownloadMissionItems";
            _waitmsgid = MAVLink.MAVLINK_MSG_ID.MISSION_COUNT;
            break;
          }
          // download Mission 부분 GCS에서 미션 보내달라 요청할 때
          case MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_INT:
          {
            _mesTpye = "DownloadMissionItems";
            _waitmsgid = MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT;
            break;
          }
          // clear Mission 부분 GCS 드론에 업로드된 미션을 정리할 때
          case MAVLink.MAVLINK_MSG_ID.MISSION_CLEAR_ALL:
          {
            _mesTpye = "ClearMissionItems";
            _waitmsgid = MAVLink.MAVLINK_MSG_ID.MISSION_ACK;
            break;
          }
        }
  
        // 7. 드론으로 Mav 메세지 전송
        await this._mavLinkDroneState.SendMavlinkMsg(missionReqMsg);
  
        // 17. Task.When( ) 메서드를 사용하여 작업 완료 or 시간 초과 중 어느 하나라도 먼저 완료되는 Task 를 반환한다.
        var resultTask = await Task.WhenAny(this._missionDownloadAckTask.Task, timeoutTask);
        
        // 17-1. 응답 대기 시간 초과로 완료된 경우 재시도
        if (resultTask == timeoutTask)
        {
          // retryCount 를 하나 더 추가하고 재귀
          Console.WriteLine("재시도 횟수 : " + retryCount + " responseReceived false");
          await this.WaitforResponseAsync(missionReqMsg, retryCount + 1);
          return;
        }
  
        // 17-2. 시간 초과로 완료되지 않은 경우 
        Console.WriteLine("재시도 횟수 : " + retryCount + " responseReceived true");
        
      }
      ...

#### _handleMavMessage( ) 드론에서 응답을 받는 부분
      private async Task _handleMavMessage(MAVLink.MAVLinkMesasge message)
      {
        var msgid = (MAVLink.MAVLINK_MSG_ID)message.msgid;
    
        // 8. 미션이 있고 수신된 메시지 아이디와 기다리는 메시지 아이디가 같거나, 수신된 메시지 아이디가 mission_ack 인 경우 
        if (_isMissin && msgid == _waitmsgid || msgid == MAVLink.MAVLINK_MSG_ID.MISSION_ACK)
        {
          _missionDownloadAckTask?.TrySetResult(true);
  
          // 메시지 아이디에 따라 작업 수행 
          switch ((MAVLink.MAVLINK_MSG_ID)message.msgid)
          {
            // 9. 미션 업로드 시 (이미 mission_count 를 보냈고 request_int 를 기다리고 있는 상황) SendMavMissionSeq( ) 메소드를 사용하여 requset_int 에 맞는 mission_item 을 전송 
            case MAVLink.MAVINK_MSG_ID.MISSION_REQUEST_INT:
            {
              var data = (MAVLink.mavlink_mission_request_int_t)message.data;
              await SendMavMissionSeq(data.seq);
              break;
            }
            case MAVLINK_MSG_ID.MISSION_REQUEST:
            {
              var data = (MAVLink.mavlink_mission_request_t)message.data;
              await SendMavMissionSeq(data.seq);
              break;
            }
            // 12. 미션 업로드 또는 클리어가 성공적으로 끝난 경우 _handleMissionAck( ) 메소드 호출
            case MAVLink.MAVINK_MSG_ID.MISSION_ACK:
            {
              var data = (MAVLink.mavlink_mission_ack_t)message.data;
              _handleMissionAck(data);
              break;
            }
            // 미션 다운로드 시 (gcs 가 이미 mission_request_list 를 전송하여 mission_count 를 기다리고 있는 상황)
            case MAVLink.MAVINK_MSG_ID.MISSION_COUNT:
            {
              var data = (MAVLink.mavlink_mission_count_t)message.data
              await InitMavMissionDownloadSession(data.count, (MAVLink.MAV_MISSION_TYPE)data.mission_type);
              break;
            }
            case MAVLink.MAVINK_MSG_ID.MISSION_ITEM_INT
            {
              var data = (MAVLink.mavlink_mission_item_int_t)mesasge.data;
              await UpdateMavMissionItem(data.seq, data);
              break;
            }
            case MAVLink.MAVINK_MSG_ID.MISSION_ITEM
            {
              var missionItem = (MAVLink.mavlink_mission_item_t)message.data;
              await UpldateMavMissionItem(missionItem.seq, new MAVLink.mavlink_mission_item_int_t
              {
                command = missionItem.command,
                param1 = missionItem.param1,
                param2 = missionItem.param2,
                param3 = missionItem.param3,
                param4 = missionItem.param4,
                x = (int)missionItem.x,
                y = (int)missionItem.y,
                z = missionItem.z,
                seq = missionItem.seq
              });
              break;
            }
          }
        }
      }
      
#### SendMavMissionSeq
      public async Task SendMavMissionSeq(ushort seq)
      {
        // _mavMissionSession은 2. 에서 Seq -1 로 생성 했었는데 그 값이 null 인지 확인하고 null 이 아니면 가져옴
        if (_mavMissionSession != null)
        {
          var session = _mavMissionSession.GetValueOrDefault();

          // 10. parameter 로 입력받은 seq 에 해당하는 미션 아이템 메시지를 생성하여 전송
          try
          {
            var missionItemMsg = _mavlinkParser.GenerateMAVLinkPacket20(MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, session.MissionItems[seq]);
            await _mavLinkDroneState.SendMavlinkMsg(new MAVLink.MAVLinkMessage(missionItemsMsg));
            Console.WriteLine($"RequestUpload_seq 보냄 {seq}");
          }
          catch (Exception e)
          {
            Console.WriteLIne("SendMavMissionSeq: " + e);
          }

          // 11. 메시지지를 전송하고 _mavMissionSession, _mavLinkDroneState 의 ProgressEvent 상태 업데이트
          _mavMissionSession = new MavlinkMissionStatus
          {
            MissionItems = _mavMissionSession?.MissionItems!,
            Seq = seq,
            StartedAt = (DateTime)_mavMissionSession?.StartedAt!,
            LastMsgReceivedAt = DateTime.Now,
            MissionType = (MAVLink.MAV_MISSION_TYPE)_mavMissionSession?.MissionType!
          };
          _mavLinkDroneState.HandleProgressEvent(new ProgressEvent
          {
            Type = "UploadMissionItems",
            Current = seq + 1,
            Total = _mavMissionSession?.MissionItems.Length ?? 0
          });
        }
      }


#### _handleMissionAck
      private void _handleMissionAck(MAVLink.mavlink_mission_ack_t data)
      {
        // FinishedAt 이 null 인지 체크 (_mavMissionSession 생성시 설정하지 않아서 null 인 상태이다. mission_ack 를 받은 경우 그 값을 설정하기 때문에 이미 완료되지 않은 경우로 이해할 수 있다.)
        if(_mavMissionSession is { FinishedAt: null })
        {
          // 13. _mavMissionSession, _mavLinkDroneState 의 ProgressEvent 상태 업데이트
          _mavMissionSession = new MavlinkMissionStatus
          {
            MissionItems = _mavMissionSession?.MissionItems!,
            MissionItemReceivingStatus = [],
            Seq = _mavMissionSession?.Seq ?? 0,
            StartedAt = (DateTime)_mavMissionSession?.StartedAt!,
            FinishedAt = DateTime.Now,
            LastMsgReceivedAt = DateTime.Now,
            MissionType = (MAVLink.MAV_MISSION_TYPE)_mavMissionSession?.MissionType!
          };
          // HandleProgressEvent( ) 메소드가 ProgressEvent 의 이전 필드 값과 다른 경우 덮어씌우고 OnNewProgressEvent?.Invoke( ); 를 호출하기 때문에 UploadMissionItems 를 쓰고 바로 UploadMission_Sucess 로 갱신하는 듯 
          _mavLinkDroneState.HandleProgressEvent(new ProgressEvent
          {
            Type = "UploadMissionItems",
            Current = _mavMissionSession?.MissionItems.Length ?? 0,
            Total = _mavMissionSession?.MIssionItems.Length ?? 0
          });
          _mavLinkDroneState.HandleProgressEvent(new ProgressEnvet
          {
            Type = "UploadMission_Success",
            Current = _mavMissionSession?.MissionItems.Length ?? 0,
            Total = _mavMissionSession?.MissionItems.Length ?? 0
          });
          
          // 14. 미션 설정 업데이트 (잘모름)
          if (_mavMissionSession?.MissionItems != null)
          {
            SetMavMission(_mavMissionSession?.MissionItems!);
          }
        }

        // 16. _missionDownloadAckTask를 완료처리하고 mission_ack 에 담긴 데이터 타입에 따라 _mavLinkDroneState 의 ProgressEvent 상태 등을 업데이트
        switch ((MAVLink.MAV_MISSION_RESULT)data.type)
        {
          // 미션 에러를 반환한 경우
          case MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ERROR:
          {
            _missionDownloadAckTask?.TrySetResult(true);
            if (_msgType != "")
            {
              _mavLinkDroneState.HandleProgressEvent(new ProgressEvent
              {
                Type = $"{_msgType}_Success",
                Current = _mavMIssionSession?.MissionItems.Length ?? 0,
                Total = _mavMissionSession?.MissionItems.Length ?? 0
              });
              _msgType = "";
              _isMission = false;
              break;
            }
          }
          // 미션 수락을 반환한 경우
          case MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED:
          {
            this._missionDownloadAckTask?.TrySetResult(true);
    
            if (_mesTpye != "")
            {
              this._mavLinkDroneState.HandleProgressEvent(new ProgressEvent()
              {
                Type = $"{_mesTpye}_Success",
                Current = this._mavMissionSession?.MissionItems.Length ?? 0,
                Total = this._mavMissionSession?.MissionItems.Length ?? 0
              });
            }
    
            _mesTpye = "";
            _lsMission = false;
            break;
          }
          // 미션 취소를 반환한 경우
          case MAVLink.MAV_MISSION_RESULT.MAV_MISSION_OPERATION_CANCELLED:
          {
            _missionDownloadAckTask?.TrySetResult(true);
            _mavLinkDroneState.HandleProgressEvent(new ProgressEvent
            {
              Type = $"{_msgType}"
            });
            _msgType = "";
            _isMission = false;
          }
        }
      }

#### SetMavMission
      // 15. MavMissionMicroservice 객체의 미션 아이템즈와 총 거리 그리고 _mavLinkDroneState 객체의 미션 아이템즈를 업데이트
      public void SetMavMission(MAVLink.mavlink_mission_item_int_t[] missionItems)
      {
        this.MavMission = missionItems;
        this.TotalDistance = MAVLinkDroneState.MavMissionTotalDistance(missionItems);
        this._mavLinkDroneState.HandleMavMissionChanged(missionItems);
      }


#### InitMavMissionDownloadSession
      public async Task InitMavMissionDownloadSession(ushort count, MAVLink.MAV_MISSION_TYPE missionType)
      {
        _mavMissionDownloadSession = new MavlinkMissionStatus
        {
          MissionItems = new MAVLink.mavlink_mission_items_int_t[count],
          MissionItemReceivingStatus = new bool[count],
          Seq = -1,
          StartedAt = DateTime.Now,
          LastMsgReceivedAt = DateTime.Now,
          MissionType = missionType
        }
    
        if (count != 0)
        {
          await _requestMissionItemAt(0, missionType);
          _mavLinkDroneState.HandleProgressEvent(
          {
            Type = "DownloadMissionItems",
            Current = 0,
            Total = count
          });
        }
      }

#### UdatemavMissionItem
      public async Task UpdateMavMissionItem(ushort seq, MAVLink.mavlink_mission_item_int_t missionItem)
      {
        if (_mavMissionDownloadSession != null 
            && _mavMissionDownloadSession.GetValueOrDefault().FinishedAt == null 
            && _mavMissionDownloadSession.GetValueOrDefault().MissionTiems.Length > seq)
        {
          _mavMissionDownloadSession. GetValueOrDefault().MissionItems[seq] = missionItem;
          _responseMissionSeq = seq;
          var status = _mavMissionDownloadSession;
      
          if (status?.MissionItemReceivingStatus != null) status.Value.MissionItemReceivingStatus[seq] = true;
          _mavMissionDownloadSession = new MavlinkMissionStatus
          {
            MissionItems = _mavMissionDownloadSession?.MissionItems!,
            MissionItemReceivingStatus = _mavMissionDownloadSession?.MissionItemReceivingStatus!,
            Seq = seq,
            StartedAt = (_mavMissionDownloadSession?.StartedAt).GetValueOrDefault(),
            LastMsgReceivedAt = (_mavMissionDownloadSession?.LastMsgReceivedAt)GetValueOrDefault(),
            FinishedAt = null,
            MissionType = (_mavMissionDownloadSession?.MissinoType).GetValueOrDefault()
          };
      
          var minFalseIndex = _mavMissionDonwloadSession?.MissionItemReceivingStatus.ToList().FindIndex(i => !i);
          await ResponseToMissionItemRequestFromDrone((ushort) (minFalseIndex ?? 0));
        }
      }
      ...
      
    }


<br>

### TaskCompletionSource
- C# 에서 TaskCompletionSource 는 비동기 작업을 수동으로 제어하고 완료 상태를 설정할 수 있게 해주는 클래스이다.
- TaskCompletionSource 를 사용하면 코드에서 비동기 작업을 명시적으로 완료하거나 실패할 수 있게 만들 수 있다.
- 주요 용도
  - 비동기 작업을 보다 세밀하게 제어할 때 사용한다.
  - 비동기 방식으로 동작하는 API 를 동기 방식의 API로 감싸거나, 반대로 동기 방식의 API 를 비동기 방식으로 감쌀 때 유용하다.
  - 콜백 기반의 비동기 코드(ex. 이벤트 핸들러)를 Task 기반 패턴으로 변환할 때 자주 사용한다.
 
#### 예제 코드 
    using System;
    using System.Threading.Tasks;
    
    public class Program
    {
        public static Task<int> DoSomethingAsync)_
        {
            var tcs = new TaskCompletionSource<int>();
    
            // 비동기 작업을 수행하는 코드 예시
            Task.Run(() => 
            {
                try
                {
                    // 작업 수행
                    int result = 42;        // 예제 결과값
                    tcs.SetResult(result);  // 작업 성공 시 결과 설정
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);   // 작업 실패 시 예외 설정
                }
            });
    
            return tcs.Task;  // Task 반환
        }
    
    
        public static async Task Main(string[] args)
        {
            try
            {
                int result = await DoSomethingAsync();
                Console.WriteLine($"작업 완료: 결과 = {result}");
            }
            catch (Exception ex)
            {
                Console.Writeline($"작업 실패: 예외 = {ex.Message}");
            }
        }
    }

#### 주요 메서드
- SetResult( ): 작압을 성공적으로 완료하고 결과를 설정한다.
- SetException( ): 작업이 실패했음을 나타내고, 예외를 설정한다.
- SetCanceled( ): 작업이 취소되었음을 나타낸다.

#### 주의사항
- TaskCompletionSource 를 사용하여 한 번 작업이 완료되면, 그 후에는 다시 작업 완료, 실패 또는 취소 상태로 설정할 수 없다.
  이미 설정된 상태를 다시 설정하려고 하면 InvalidOperationException 이 발생한다.
- 비동기 작업의 흐름을 명확히 관리하지 않으면, TaskCompetionSource 를 사용한 작업이 영원히 완료되지 않은 상태에 빠질 수 있다.
  따라서 적절한 타임 아웃이나 예외 처리가 필요하다.

#### 정리
- TaskCompletionSource<T> 는 비동기 작업의 완료 상태를 수동으로 제어할 수 있게 해주며,
  복잡한 동기 시나리오에서 유용하게 사용할 수 있다.


### Task
- C# 에서 Task 는 비동기 프로그래밍에서 중요한 역할을 하는 클래스이다.
- Task 는 비동기 작업을 나타내며, 작업의 완료 상태를 추적하고 결과를 처리할 수 있게 해준다.
- Task 는 일반적으로 비동기 메서드에서 반환되며, 작업이 완료될 때까지 기다리거나 결과를 처리하는데 사용된다.

#### 주요 특징
- 비동기 작업: Task 는 작업이 시작된 후 완료될 때까지의 비동기 작업을 나타낸다.
  작업이 비동기로 수행되기 때문에, 호출 스레드는 작업이 완료될 때까지 차단되지 않는다.
- 결과 처리: Task 는 작업의 결과를 반환할 수 있다. Task<TResult> 는 작업이 성공적으로 완료되었을 때 결과를 반환하고,
  Task 는 결과가 없는 작업을 나타낸다.
- 예외 처리: Task 는 작업 도중 발생한 예외를 캡처하고, 작업이 완료된 후 예외를 처리할 수 있다.
- 작업 상태 추적: Task 는 작업의 상태를 추적할 수 있다. 예를 들어, 작업이 아직 실행 중인지, 완료되었는지,
  취소되었는지 또는 예외가 발생했는지를 확인할 수 있다.

#### 생성 및 실행
- 가장 기본적인 Task 는 Task.Run 메서드를 사용하여 생성하고 실행할 수 있다.
  
        using System;
        using System.Threading.Tasks;
        
        public class Program
        {
            public static async Task Main(string[] args)
            {
                Task task = Task.Run(() => 
                {
                    // 비동기 작업 수행
                    Conosle.WriteLine("작업 실행 중...);
                });
        
                await task; // 작업이 완료될 때까지 대기
                Console.WriteLine("작업 완료");
            }
        }


#### Task<TResult> 로 결과 반환
- 비동기 작업이 완료된 후 결과를 반환해야 할 때는 Task<TResult> 를 사용한다.

        using System;
        usint System.Threading.Tasks;

        public class Program
        {
            public static async Task<int> ComputeAsync()
            {
                return await Task.Run(() =>
                {
                    // 비동기 작업 수행
                    int result = 42;
                    return result;
                });
            }
        
            public static async Task Main(string[] args)
            {
                int result = await ComputeAsync();
                Console.WriteLine($"결과: {result}");
            }
        }


#### 예외 처리
- Task 에서 발생한 예외는 await 키워드를 통해 잡ㅇ르 수 있다.

        using System;
        usint System.Threading.Tasks;

        public class Program
        {
            public static async Task<int> FaultyTaskAsync()
            {
                return await Task.Run(() =>
                {
                    throw new InvalidOperationException("에러 발생!");
                });
            }
        
            public static async Task Main(string[] args)
            {
                try
                {
                    int result = await FaultyTaskAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"예외 발생: {ex.Message}");
                }
            }
        }

#### 여러 Task 를 병렬로 실행
- 여러 개의 작업을 병렬로 실행하고, 모든 작업이 완료될 때까지 기다릴 수 있다.

        using System;
        usint System.Threading.Tasks;

        public class Program
        {
            public static async Task Main(string[] args)
            {
                Task<int> task1 = Task.Run(() => 10);
                Task<int> task2 = Task.Run(() => 20);
                Task<int> task3 = Task.Run(() => 30);

                int[] results = await Task.WhenAll(task1, task2, task3);
                Console.WriteLine($"합계: {results[0] + results[1] + results[2]}");
            }
        }

#### Task 와 async/await
- Task 는 C#의 async/await 키워드와 함께 주로 사용된다. async 메서드는 Task 또는 Task<TResult> 를 반환한다.
- await 키워드를 사용하면 비동기 작업이 완료될 때까지 기다릴 수 있다. 이패턴을 사용하면 비동기 작업을 동기코드처럼
  작서알 수 있어 코드가 훨씬 간결해진다.

#### 정리
- Task 는 비동기 작업을 나타내는 클래스이다.
- Task 는 비동기 작업의 완료 상태를 추적하고 결과를 처리할 수 있게 해준다.
- Task<TResult> 는 비동기 작업이 완료된 후 결과를 반환할 수 있는 형태의 Task 이다.
  TResult 는 작업의 반환 타입을 의미한다.

<br>

### BaseDroneState.cs / ProgressEvent
    public struct ProgressEvent
    {
      public int Total;
      public int Current;
      public string Type;
    }
- 우리 코드에서 ProgressEvent 객체는 Progress Reporting 을 위해 사용한다.
- 이는 긴 작업이나 비동기 작업을 수행하는 동안 현재 작업의 진행 상태를 사용자에게 제공하는데 유용하다.
