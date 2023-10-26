import React, {useState, useEffect} from 'react'
import { HubConnectionBuilder } from '@microsoft/signalr';

export const SignalRContext = React.createContext({})

export const SignalRProvider = ({ children }) => {
    const [ connStt, setConnStt ] = useState('init');

    useEffect(() => {
        // 1. HubConnectionBuilder를 사용하여 SignalR Hub 연결을 위한 객체 생성
        const connection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/droneHub') // SignalR Hub의 URL 설정
            .build(); // HubConnection 객체 생성
        // connection.invoke('SayHello');

        // 2. SignalR Hub 연결 시작
        connection.start()
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
    }, []);

    return <>{children}</>
}