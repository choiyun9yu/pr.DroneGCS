import React, {useState, useEffect} from 'react'
import * as signalR from '@microsoft/signalr';

export const ChatContext = React.createContext({})

export const SignalRProvider = ({ children }) => {
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