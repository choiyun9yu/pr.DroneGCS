
# Mission Start 


<br>

# Mission Download


<br>

# Mission Upload
![image](https://github.com/user-attachments/assets/ea4655da-40f1-4c3f-a870-40f278b62b24)

## 1. Mission upload process in our source code 
- React 가 mission item 들을 MongoDB 로부터 가져와 GCS 로 보낸다.
  (React 가 SignalR 로 GCS 의 DroneMonitorManager.cs 의 SendMavlinkMission 메소드 호출)
- GCS 에서 SendMavlinkMission( ) -> _sendMavlinkMission( ) -> MAVLinkDroneState.cs StartMAVMission
  -> MavMissionMicroservice.cs StartUploadMAVMission 호출

#### MavMissionMicroservice.cs / StartUploadMAVMission
    public class MavMissionMicroservice
    {
      private MavlinkMissionStatus? _mavMissionSession;
      private TaskCompletionSource<bool>? _missionDownloadAckTask;
      ...
  
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
  
      /**
       * Send a `MISSION_COUNT` to the drone, create `MavMissionSession` and wait for `MISSION_REQUEST_INT` msg from drone
       */
      public async Task StartUploadMAVMission(MAVLink.mavlink_mission_item_int_t[] mavMission, MAVLink.MAV_MISSION_TYPE missionType = MAVLink.MAV_MISSION_TYPE.MISSION)
      {
        // 1. create mission count msg 
        var missionCountMsg = this._mavlinkParser.GenerateMAVLinkPacket20(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT,
          new MAVLink.mavlink_mission_count_t()
          {
            count = (ushort)mavMission.Length,
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,
            target_system = byte.Parse(this._droneId)
          });

        // 2. create session about mav mission
        this._mavMissionSession = new MavlinkMissionStatus()
        {
          MissionItems = mavMission,
          Seq = -1,
          StartedAt = DateTime.Now,
          LastMsgReceivedAt = DateTime.Now,
          MissionType = missionType,
        };
        
        // 3. after setting _mavMissionSession and then request MISSION_COUNT Message
        if (this._missionDownloadAckTask is { Task.IsCompleted: false })
        {
          this._missionDownloadAckTask.SetCanceled();
        }
        
        await WaitforResponseAsync(new MAVLink.MAVLinkMessage(missionCountMsg)); //재전송 체크 
        this._mavLinkDroneState.HandleProgressEvent(
          new ProgressEvent()
          {
            Type = "UploadMissionItems",
            Current = 0,
            Total = MavMission?.Length ?? 0,
          });
      }
      ...
      
    }


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
