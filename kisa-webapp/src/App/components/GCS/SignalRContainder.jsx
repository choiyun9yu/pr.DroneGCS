import React, {useEffect} from 'react'
import { HubConnectionBuilder } from '@microsoft/signalr';

export const SignalRContext = React.createContext({})

export const SignalRProvider = ({ children }) => {
    // HubConnectionBuilder를 사용하여 클라이언트 생성
    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/droneHub') // 연결 URL 설정
            .build(); // 연결 빌드

        // 연결 시작
        connection.start()
            .then(() => {
                console.log('SignalR 연결 성공');
            })
            .catch(error => {
                console.error('SignalR 연결 실패: ', error);
            });
    }, []);

    return <>{children}</>
}
