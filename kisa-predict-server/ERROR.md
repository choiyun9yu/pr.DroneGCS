# ERROR

## 1. SocketIO 사용 
- .Net : < PackageReference Include="SocketIoClientDotNet" Version="1.0.2-beta1" />
- Flask : flask-SocketIO 5.3.6
######
    The client is using an unsupported version of the Socket.IO or Engine.IO protocols (further occurrences of this error will be logged with level INFO)
    127.0.0.1 - - [04/Dec/2023 10:25:14] "GET /socket.io/?EIO=3&transport=polling&t=638372823140135725-0&b64=1 HTTP/1.1" 400 -

Socket.IO or Engine.IO의 버전이 호환되지 않는 것으로 파악   
SocketIoClientDotNet 1.0.2-beta1 은 SocketIO 1.x프로토콜을 사용하고 flask_socketIO 5.3.6은 2.X프로토콜을 사용    
flask_socketIO를 SocketIO 1.X 프로토콜을 사용하는 1.1로 다운그레이드 -> Werkzeug와 호환이 안됨 -> Werkzeug 다운그레이드 -> Flask와 호환이 안됨 -> Flask 다운그레이드 -> Markup, jinja2와 호환이 안됨 

.NET의 NuGetPackage에 있는 SocketIO 라이브러리는 너무 옛날 버전이라서 최신 Flask SocketIO 들과는 호환이 어려워 보임


## 2. WebSocket과 SocketIO는 서로 다른 프로토콜을 사용해서 호환되지 않는다.


## 3. WebSockets 사용

    % pip insall websocket

이 방식의 경우 WebSocket 서버는 Flask 서버와 별도의 서버이다.

>  Error: Error parsing handshake response: SyntaxError: Unexpected token 'S', "Server rec"... is not valid JSON

SignalR과 Websockets를 동시에 사용할 때 SignalR의 핸드쉐이크가 제대로 작동하지 않는 오류가 발생했다.


