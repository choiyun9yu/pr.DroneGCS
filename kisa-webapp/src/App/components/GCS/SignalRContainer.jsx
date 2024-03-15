import { createContext, useState, useEffect, useRef } from 'react'
import * as signalR from '@microsoft/signalr'

export const DroneContext = createContext({})
// python sim_vehicle.py -v ArduCopter -I 0 -n 3 --auto-sysid --out=udp:127.0.0.1:14556
// python sim_vehicle.py -L ETRI -v ArduCopter --out=udp:127.0.0.1:14556
export const SignalRProvider = ({ children }) => {
    const [droneList, setDroneList] = useState([])
    const [selectedDrone, setSelectedDrone] = useState()
    const [droneMessage, setDroneMessage] = useState(null)
    const connection = useRef()

    useEffect(() => {
        const connectionObj = new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:5000/droneHub')
            .configureLogging(signalR.LogLevel.Information)
            .build()
        connection.current = connectionObj

        connectionObj
            .start()
            .then(async () => {
                console.log('SignalR 연결 성공')
                await connectionObj.send('SendClientType')
                await connectionObj.invoke('GetDroneList')
            })
            .catch(error => {
                console.error('SignalR 연결 실패', error)
            })

        return () => {
            connectionObj.stop()
        }
    }, [])

    useEffect(() => {
        const connectionObj = connection.current
        if (!connectionObj) return

        connectionObj.on('droneList', (json) => {
            const list = JSON.parse(json)
            setDroneList(list)
            console.log(list)
        })

        connectionObj.on('selectedDrone', sd => {
            setSelectedDrone(JSON.parse(sd))
        })

        connectionObj.on('droneState', msg => {
            const droneMessage = JSON.parse(msg)
            setDroneMessage(droneMessage)
            // console.log(droneMessage)
        })

        return () => {['droneState']
            .forEach(handler => {connectionObj.off(handler)})
        }
    }, [])

    const handleSelectedDrone = selectedDroneId => {
        connection.current.invoke('SelectDrone', selectedDroneId)
    }
    const handleDroneFlightMode = flightMode => {
        connection.current.invoke('HandleDroneFlightMode', flightMode)
    }
    const handleDroneFlightCommand = flightCommand => {
        connection.current.invoke('HandleDroneFlightCommand', flightCommand)
    }
    const handleDroneStartMarking = (lat, lng) => {
        connection.current.invoke('HandleDroneStartMarking', lat, lng)
    }
    const handleDroneTargetMarking = (lat, lng) => {
        connection.current.invoke('HandleDroneTargetMarking', lat, lng)
    }
    const handleDroneTransitMarking = (transitList) => {
        connection.current.invoke('HandleDroneTransitMarking', transitList)
    }
    const handleMissionAlt = (missionAlt) => {
        connection.current.invoke('HandleMissionAlt', missionAlt)
    }
    const handleMoveBtn = (lat, lng) => {
        connection.current.invoke('HandleMoveBtn', lat, lng)
    }
    const handleDroneMovetoTarget = () => {
        connection.current.invoke('HandleDroneMoveToTarget')
    }
    const handleDroneMovetoBase = () => {
        connection.current.invoke('HandleDroneMoveToBase')
    }
    const handleDroneMovetoMission = (startPoint, targetPoint, transitPoint, alt, totalDistance) => {
        connection.current.invoke('HandleDroneMoveToMission', startPoint, targetPoint, transitPoint, alt, totalDistance)
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
            droneList,
            selectedDrone,
            setSelectedDrone,
            droneMessage,
            handleDroneFlightMode,
            handleDroneFlightCommand,
            handleDroneJoystick,
            handleControlJoystick,
            handleCameraJoystick,
            handleCameraCommand,
            handleDroneStartMarking,
            handleDroneTransitMarking,
            handleDroneTargetMarking,
            handleDroneMovetoTarget,
            handleDroneMovetoBase,
            handleMissionAlt,
            handleDroneMovetoMission,
            handleSelectedDrone,
            handleMoveBtn
        }}>
            {children}
        </DroneContext.Provider>
    )
}