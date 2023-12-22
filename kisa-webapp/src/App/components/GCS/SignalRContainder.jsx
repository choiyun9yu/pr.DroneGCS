import React, {useEffect, useState, useRef} from 'react'
import * as signalR from '@microsoft/signalr';

export const DroneContext = React.createContext({})

export const SignalRProvider = ({ children }) => {
    const [droneStates, setDroneStates] = useState(null)
    const connection = useRef();

     useEffect(() => {
         // 연결 객체 생성
         const connectionObj = new signalR.HubConnectionBuilder()
             .withUrl("http://localhost:5000/droneHub") // 서버의 Hub URL 설정   // droneHub
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

         return () => {
             connectionObj.stop();
         };
     }, []);

     useEffect(() => {
         const connectionObj = connection.current
         if (!connectionObj) return

         // GCS로 부터 드론 상태 수신
         connectionObj.on('DroneStateUpdate', (decoded) => {
             setDroneStates(old => ({...old, decoded}));
             // console.log(decoded['payloadlength'])
         })

         return () => {
             ['SelectedDroneStateUpdate'].forEach(handler => {
                 connectionObj.off(handler)
             })
         };
     }, []);

    return (
        <DroneContext.Provider value={{
            droneStates
        }}>
            {children}
        </DroneContext.Provider>
    );
};


