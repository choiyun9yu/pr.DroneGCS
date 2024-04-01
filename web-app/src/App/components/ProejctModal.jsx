import {useContext} from "react";
import {DroneContext} from "./GCS/SignalRContainer";
import {Link} from "react-router-dom";

export const WarningModal = (props) => {
    const {setWarningSkipList, setSelectedDrone, handleSelectedDrone, handleDroneFlightMode} = useContext(DroneContext)

    const handleManualControl = (droneId) => {
        handleSelectedDrone(droneId)
        setSelectedDrone(droneId)
        handleDroneFlightMode(17)
        setWarningSkipList(old => ([
                ...old, droneId
            ])
        )
    }

    const handleWarningIgnore = (droneId) => {
        setWarningSkipList(old => ([
                ...old, droneId
            ])
        )
    }

    return (
        <div className={'modal absolute top-[30%] left-[40%] w-[400px] h-[300px] z-50 rounded-lg text-black bg-[#CDCFE9]'}>
            <div className={'flex flex-col w-[400px] h-[220px] mx-auto justify-center items-center rounded-t-lg font-bold text-2xl text-white bg-[#6359e9]'}>

                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="white" className="w-16 h-16">
                    <path fillRule="evenodd"
                          d="M9.401 3.003c1.155-2 4.043-2 5.197 0l7.355 12.748c1.154 2-.29 4.5-2.599 4.5H4.645c-2.309 0-3.752-2.5-2.598-4.5L9.4 3.003ZM12 8.25a.75.75 0 0 1 .75.75v3.75a.75.75 0 0 1-1.5 0V9a.75.75 0 0 1 .75-.75Zm0 8.25a.75.75 0 1 0 0-1.5.75.75 0 0 0 0 1.5Z"
                          clipRule="evenodd"/>
                </svg>

                <div className={'flex flex-col w-[400px] h-[80px] mx-auto items-center justify-center'}>
                    <span className={'ml-1 font-bold'}>WARNING!</span>
                    <span className={'mt-2 text-sm font-medium'}> {props.drone} 번 드론에서</span>
                    <span className={'mt-1 text-sm font-medium'}> 장애가 탐지되었습니다. </span>
                </div>
            </div>

            <div className={'flex text-md h-[80px] font-medium items-center justify-center'}>
                <Link className={'mr-5 py-2 px-4 rounded-md text-white bg-[#1D1D41] hover:bg-amber-300'} to={'/gcs'} onClick={() => {handleManualControl(props.drone.toString())}}>수동조종</Link>
                <button className={'ml-5 py-2 px-4 rounded-md text-white bg-[#1D1D41] hover:bg-red-600'} onClick={() => {handleWarningIgnore(props.drone.toString())}}>무시하기</button>
            </div>
        </div>
    )
}