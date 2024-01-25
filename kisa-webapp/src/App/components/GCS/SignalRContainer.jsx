import React, {useEffect, useState, useRef} from 'react'
import * as signalR from '@microsoft/signalr';

export const DroneContext = React.createContext({})

export const SignalRProvider = ({ children }) => {
    const [droneMessage, setDroneMessage] = useState(null)
    const connection = useRef();

    // SignalR 연결 설정
    useEffect(() => {
        const connectionObj = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5000/droneHub")         // 서버의 Hub URL 설정
            .configureLogging(signalR.LogLevel.Information)    // 로깅 수준 설정
            .build();                                          // 연결 객체 생성
        // useRef로 생성한 connection.current에 연결 객체 할당
        connection.current = connectionObj

        // 연결 시작
        connectionObj
            .start()
            .then(async () => {
                console.log('SignalR 연결 성공');
                /*
                * .invoke: 클라이언트에서 서버로 특정한 Hub 메서드를 호출하는 데 사용
                *          클라이언트에서 서버로 데이터를 전송하거나 서버에서 특정 작업을 수행하도록 요청할 때 사용
                *          예를 들어, 채팅 애플리케이션에서 새로운 메시지를 보낼 때 사용
                *          따라서 클라이언트에서 서버로 메서드 호출을 위해 사용
                */
                // await connectionObj.invoke('GetDroneList')
            })
            .catch(error => {
                console.error('SignalR 연결 실패', error);
            });

        return () => {
            connectionObj.stop();
        };
    }, []);

    // 드론 메시지 수신 및 처리
    useEffect(() => {
        // useRef로 생성한 connection을 통해 위에서 생성한 연결 객체 넘겨 받음
        const connectionObj = connection.current
        if (!connectionObj) return
        /*
        * .on: 서버에서 클라이언트로부터 메시지를 받을 때 사용
        *      특정 이벤트에 대한 핸들러를 등록하여, 서버에서 해당 이벤트가 발생하면 클라이언트에서 특정 작업을 수행 가능
        *      예를 들어, 채팅 애플리케이션에서 새로운 메시지가 도착했을 때 클라이언트에서 특정 동작을 수행하도록 등록
        *      따라서 서버에서 클라이언트로부터 오는 메시지나 이벤트를 처리하기 위해 사용
        */
        connectionObj.on('droneMessage', (msg) => {
            const droneMessage = JSON.parse(msg);
            setDroneMessage(old => ({...old, droneMessage}));
        })

        // useEffect 훅에서 반환되는 클린업 함수, 컴퓨넌트가 언마운트 되거나 업데이트되기 전에 실행되며, 주로 리소스의 정리나 이벤트 리스너의 해제와 같은 작업을 수행
        return () => {
        ['droneMessage']                                            // SignalR 연결 객체에서 해제할 이벤트 핸들러들을 나타낸다.
                .forEach(handler => {connectionObj.off(handler)})   // 배열에 포함된 각 핸들러에 대해 connectionObj.off(handler)를 호출하여 해당 이벤트 핸들러를 제거
        };
    }, []);

    const handleDroneFlightMode = flightMode => {
        connection.current.invoke('HandleDroneFlightMode', flightMode)
    }
    const handleDroneFlightCommand = flightCommand => {
        connection.current.invoke('HandleDroneFlightCommand', flightCommand)
    }
    const handleDroneStartingMarking = (lat, lng) => {
        connection.current.invoke('HandleDroneStartingMarking', lat, lng)
    }
    const handleDroneTargetMarking = (lat, lng) => {
        connection.current.invoke('HandleDroneTargetMarking', lat, lng)
    }
    const handleMissionAlt = (missionAlt) => {
        connection.current.invoke('HandleMissionAlt', missionAlt)
    }
    const handleDroneMovetoTarget = () => {
        connection.current.invoke('HandleDroneMoveToTarget')
    }
    const handleDroneMovetoBase = () => {
        connection.current.invoke('HandleDroneMoveToBase')
    }
    const handleDroneJoystick = arrow => {
        connection.current.invoke('HandleDroneJoystick', arrow)
    }
    const handleControlJoystick = arrow => {
        connection.current.invoke('HandleControlJoystick', arrow)
    }
    const handleCameraCommand = command => {
        connection.current.invoke('HandleCameraCommand', command)
    }
    const handleCameraJoystick = arrow => {
        connection.current.invoke('HandleCameraJoystick', arrow)
    }

    return (
        <DroneContext.Provider value={{
            droneMessage,
            handleDroneFlightMode,
            handleDroneFlightCommand,
            handleDroneJoystick,
            handleControlJoystick,
            handleCameraJoystick,
            handleCameraCommand,
            handleDroneTargetMarking,
            handleDroneMovetoTarget,
            handleDroneMovetoBase,
            handleDroneStartingMarking,
            handleMissionAlt
        }}>
            {children}
        </DroneContext.Provider>
    );
};