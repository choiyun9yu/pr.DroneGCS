# SignalR buffer size

![image](https://github.com/user-attachments/assets/9c5f50ef-1b9b-49dd-9065-3b8e26ca48cc)

#### before
    services.AddSignalR(options => { options.EnableDetailedErrors = true; }).AddNewtonsoftJsonProtocol(options =>
    {
      options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
      options.PayloadSerializerSettings.Converters
        .Add(new StringEnumConverter());
    });

#### after
     services.AddSignalR(options => { 
      options.EnableDetailedErrors = true;
      options.MaximumReceiveMessageSize = 1024 * 1024 * 32;    // 클라이언트가 한번에 받을 수 있는 메시지 크기
      options.MaximumParallelInvocationsPerClient = 10;        // 클라이언트가 서버에 동시에 보낼 수 있는 메서드 호출의 수 제한, 서버의 안정성과 성능 관리
      options.StatefulReconnectBufferSize = 1024 * 1024 * 32;  // 연결이 끊겼다가 재연결될 때, 클라이언트가 놓친 메시지를 버퍼에 저장할 수 있는 크기
    }).AddNewtonsoftJsonProtocol(options =>
    {
      options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
      options.PayloadSerializerSettings.Converters
        .Add(new StringEnumConverter());
    });
