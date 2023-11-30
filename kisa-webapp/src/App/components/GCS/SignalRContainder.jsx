import React, {useState, useEffect, useRef} from 'react'
import * as signalR from '@microsoft/signalr';

export const DroneContext = React.createContext({})
export const ChatContext = React.createContext({})

export const SignalRProvider = ({ children }) => {
 // const [ connection, setConnection ] = useState(null);
 const [ droneStates, setDroneStates ] = useState({});
 const connection = useRef();

 useEffect(() => {
     // 연결 객체 생성
     const connectionObj = new signalR.HubConnectionBuilder()
         .withUrl("http://localhost:5000/droneHub") // 서버의 Hub URL 설정
         .configureLogging(signalR.LogLevel.Information)
         .build();
     connection.current = connectionObj

     connectionObj
         .start()
         .then(async () => {
             console.log('SignalR 연결 성공');
         })
         .catch(error => {
             console.error('SignalR 연결 실패', error);
         });

     // 다양한 이벤트 핸들러 등록
     connectionObj.on("ReceiveMavMessage", function (mavMessage) {    // signalR 연결 객체('connection')가 서버로부터 ReceiveEvent를 수신할 때 실행할 함수를 정의,
         // console.log("Received event from server:", mavMessage);         // ReceivEvent는 이벤트의 이름을 의미, 서버에서 이 이름으로 이벤트르 보내면 클라이언트에서 이를 수신할 수 있다.
        setDroneStates(old => ({...old, mavMessage}))                                                                   // function (message)는 이벤트 핸들러 함수로, 서버에서 전송된 메시지를 받아서 원하는 동작을 수행한다.
     });


     return () => {
         connectionObj.stop();
     };
 }, []);

    // const SendMessageToClient = (message) => {
    //     connection
    //         .invoke("SendMessageToClient", message)
    //         .catch(error => console.error(error.toString()));
    // };

    return (
        <DroneContext.Provider value={{ droneStates, connection }}>
            {children}
        </DroneContext.Provider>
    );
};


///////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////// 채팅 /////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////
export const ChatRProvider = ({ children }) => {
    const [ connStt, setConnStt ] = useState('init');
    const [ connection, setConnection ] = useState(null);

    useEffect(() => {
        // 1. HubConnectionBuilder를 사용하여 SignalR Hub 연결을 위한 객체 생성
        const connection = new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:5000/chatHub') // SignalR Hub의 URL 설정
            .build(); // HubConnection 객체 생성
        // connection.invoke('SayHello');

        // 2. SignalR Hub 연결 시작
        connection
            .start()
            .then(async () => {
                setConnStt('connected');
                console.log('SignalR 연결 성공');   // Hub 연결 시작
                // const droneIdList = await connection.invoke('GetDroneIds'); // 서버측 메소드 GetDroneIds 실행하고 반환값 받음
                // console.log(droneIdList);
            })
            .catch(error => {
                setConnStt('error');
                console.error('SignalR 연결 실패: ', error);    //
            });

        setConnection(connection);

        return () => {
            connection.stop();
        };
    }, []);

    // 3. SignalR을 사용하여 메시지를 서버로 보내는 역할
    const sendMessage = (userInput, messageInput) => {
        connection
            .invoke("SendMessage", userInput, messageInput)   // SignalR 커넥션을 통해 서버측 "SendMessage" 메서드를 호출하고 사용자 입력과 메시지 입력을 전달한다.
            .catch(error => console.error(error.toString()));
    };

    // 4. SignalRContext.Provider로 컨텍스트 값을 하위 컴포넌트에 전달
    return (
        <ChatContext.Provider value={{ sendMessage, connection }}>
            {children}
        </ChatContext.Provider>
    );
}