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
      options.MaximumReceiveMessageSize = 1024 * 1024 * 32;
      options.MaximumParallelInvocationsPerClient = 10;
      options.StatefulReconnectBufferSize = 1024 * 1024 * 32;
    }).AddNewtonsoftJsonProtocol(options =>
    {
      options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
      options.PayloadSerializerSettings.Converters
        .Add(new StringEnumConverter());
    });
