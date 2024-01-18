import { ColorThema } from '../ProejctThema';
import {Table} from "./MiddleMap";
import React, {useContext, useState} from "react";
import {DroneContext} from "./SignalRContainder";

export const MissionMode = () => {
    return (
        <div id="right-sidebar" className="flex  flex-col w-[300px]  ">
            <div className={`flex flex-col w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                <div className="flex m-2 items-center">
                    <span className="text-white rounded-md m-3 font-bold text-medium">• 드론 미션</span>
                </div>
            </div>
        </div>
    );
};

export const OtherContents = (props) => {
    const { handleDroneFlightMode, handleDroneFlightCommand } = useContext(DroneContext);
    const [ isMissionBtn, setIsMissionBtn ] = useState(true);
    const [ isWayPointBtn, setIsWayPointBtn ] = useState(false);
    const [ isCenterBtn, setIsCenterBtn ] = useState(false);

    const handleCurrentCenter = () => {
        props.handleCurrentCenter();
        setIsCenterBtn(!isCenterBtn);
    }

    const handleStartPointCenter = () => {
        props.handleStartPointCenter();
        setIsCenterBtn(!isCenterBtn);
    }

    const handleTargetPointCenter = () => {
        props.handleTargetPointCenter();
        setIsCenterBtn(!isCenterBtn);
    }

    const handleIsMissionBtn = () => {

    }

    const handleIsWayPointBtn = () => {

    }

    const handleIsCenterBtn = () => {
        setIsCenterBtn(!isCenterBtn);
    }


    const handleTakeoffBtn = async () => {
        handleDroneFlightMode(4)
        await waitOneSecond()
        handleDroneFlightCommand(0)
        handleDroneFlightCommand(2)
    }

    const handleLandBtn = async () => {
        handleDroneFlightMode(4)

        await waitOneSecond()

        handleDroneFlightCommand(3)
    }

    const handelReturnBtn = () => {
        props.handleIsRtl();
        handleDroneFlightMode(6)
    }

    function waitOneSecond() {
        return new Promise(resolve => setTimeout(resolve, 1000));
    }

    return (
        <>
            <div
                className={`absolute flex flex-col justify-around items-center left-[10px] top-[150px] w-[60px] h-[25%] text-xs rounded bg-white opacity-75`}>
                <div className={`flex text-lg font-bold`}>
                    Plan
                </div>

                <button className={`flex flex-col items-center`}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path
                            d="M5.625 1.5c-1.036 0-1.875.84-1.875 1.875v17.25c0 1.035.84 1.875 1.875 1.875h12.75c1.035 0 1.875-.84 1.875-1.875V12.75A3.75 3.75 0 0016.5 9h-1.875a1.875 1.875 0 01-1.875-1.875V5.25A3.75 3.75 0 009 1.5H5.625z"/>
                        <path
                            d="M12.971 1.816A5.23 5.23 0 0114.25 5.25v1.875c0 .207.168.375.375.375H16.5a5.23 5.23 0 013.434 1.279 9.768 9.768 0 00-6.963-6.963z"/>
                    </svg>
                    Mission
                </button>

                <button className={`flex flex-col items-center`}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path fillRule="evenodd"
                              d="M12 2.25c-5.385 0-9.75 4.365-9.75 9.75s4.365 9.75 9.75 9.75 9.75-4.365 9.75-9.75S17.385 2.25 12 2.25zM12.75 9a.75.75 0 00-1.5 0v2.25H9a.75.75 0 000 1.5h2.25V15a.75.75 0 001.5 0v-2.25H15a.75.75 0 000-1.5h-2.25V9z"
                              clipRule="evenodd"/>
                    </svg>
                    Way Point
                </button>

                {/*<button className={`flex flex-col items-center`} onClick={handelReturnBtn}>*/}
                {/*    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">*/}
                {/*        <path fillRule="evenodd"*/}
                {/*              d="M15 3.75A5.25 5.25 0 009.75 9v10.19l4.72-4.72a.75.75 0 111.06 1.06l-6 6a.75.75 0 01-1.06 0l-6-6a.75.75 0 111.06-1.06l4.72 4.72V9a6.75 6.75 0 0113.5 0v3a.75.75 0 01-1.5 0V9c0-2.9-2.35-5.25-5.25-5.25z"*/}
                {/*              clipRule="evenodd"/>*/}
                {/*    </svg>*/}
                {/*    Return*/}
                {/*</button>*/}

                <button className={`flex flex-col items-center`} onClick={handleIsCenterBtn}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path
                            d="M6 3a3 3 0 00-3 3v1.5a.75.75 0 001.5 0V6A1.5 1.5 0 016 4.5h1.5a.75.75 0 000-1.5H6zM16.5 3a.75.75 0 000 1.5H18A1.5 1.5 0 0119.5 6v1.5a.75.75 0 001.5 0V6a3 3 0 00-3-3h-1.5zM12 8.25a3.75 3.75 0 100 7.5 3.75 3.75 0 000-7.5zM4.5 16.5a.75.75 0 00-1.5 0V18a3 3 0 003 3h1.5a.75.75 0 000-1.5H6A1.5 1.5 0 014.5 18v-1.5zM21 16.5a.75.75 0 00-1.5 0V18a1.5 1.5 0 01-1.5 1.5h-1.5a.75.75 0 000 1.5H18a3 3 0 003-3v-1.5z"/>
                    </svg>
                    Center
                </button>
            </div>

            {isMissionBtn
                ?(
                    <div className={`flex p-2 flex-col text-xs rounded-md`} style={{
                        position: 'absolute',
                        top: '200px',
                        left: '70px',
                        opacity: '70%',
                        background: '#ffff',
                    }}>
                        <div className={`flex flex-row`}>
                            <button className={`flex  m-1`}>
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"
                                     className="w-6 h-6">
                                    <path
                                        d="M9.195 18.44c1.25.714 2.805-.189 2.805-1.629v-2.34l6.945 3.968c1.25.715 2.805-.188 2.805-1.628V8.69c0-1.44-1.555-2.343-2.805-1.628L12 11.029v-2.34c0-1.44-1.555-2.343-2.805-1.628l-7.108 4.061c-1.26.72-1.26 2.536 0 3.256l7.108 4.061Z"/>
                                </svg>
                                Return
                            </button>

                            <button className={`m-1`}>
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"
                                     className="w-6 h-6">
                                    <path fillRule="evenodd"
                                          d="M4.5 7.5a3 3 0 0 1 3-3h9a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9Z"
                                          clipRule="evenodd"/>
                                </svg>
                                Break
                            </button>

                            <button className={`m-1`}>
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"
                                     className="w-6 h-6">
                                    <path fillRule="evenodd"
                                          d="M4.5 5.653c0-1.427 1.529-2.33 2.779-1.643l11.54 6.347c1.295.712 1.295 2.573 0 3.286L7.28 19.99c-1.25.687-2.779-.217-2.779-1.643V5.653Z"
                                          clipRule="evenodd"/>
                                </svg>
                                Go To
                            </button>
                        </div>

                        <br/>

                        <div className={`flex flex-row`}>
                            <button className={`flex flex-col m-1 items-center`} onClick={() => handleTakeoffBtn()}>
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"
                                     className="w-6 h-6">
                                    <path
                                        d="M11.47 1.72a.75.75 0 0 1 1.06 0l3 3a.75.75 0 0 1-1.06 1.06l-1.72-1.72V7.5h-1.5V4.06L9.53 5.78a.75.75 0 0 1-1.06-1.06l3-3ZM11.25 7.5V15a.75.75 0 0 0 1.5 0V7.5h3.75a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9a3 3 0 0 1 3-3h3.75Z"/>
                                </svg>
                                Take Off
                            </button>

                            <button className={`flex flex-col m-1 items-center`} onClick={() => handleLandBtn()}>
                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"
                                     className="w-6 h-6">
                                    <path
                                        d="M12 1.5a.75.75 0 0 1 .75.75V7.5h-1.5V2.25A.75.75 0 0 1 12 1.5ZM11.25 7.5v5.69l-1.72-1.72a.75.75 0 0 0-1.06 1.06l3 3a.75.75 0 0 0 1.06 0l3-3a.75.75 0 1 0-1.06-1.06l-1.72 1.72V7.5h3.75a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9a3 3 0 0 1 3-3h3.75Z"/>
                                </svg>
                                Land
                            </button>
                        </div>


                    </div>
                )
                : null}

            {isCenterBtn
                ? (
                    <div className={`flex p-2 flex-col items-start rounded-md`} style={{
                        position: 'absolute',
                        top: '450px',
                        left: '70px',
                        opacity: '70%',
                        background: '#ffff',
                    }}>
                        <button className={`px-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]`} onClick={handleCurrentCenter}>
                            Drone
                        </button>
                        <button className={`px-1 my-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]`}
                                onClick={handleStartPointCenter}>
                        Start Point</button>
                        <button className={`px-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]`} onClick={handleTargetPointCenter}>
                            Target Point</button>
                    </div>
                )
                : null}

            <Table className={'z-30'} middleTable={props.middleTable} monitorTable={props.monitorTable} setMonitorTable={props.setMonitorTable}/>

            {/* Altitude */}
            <div className={`absolute left-2 bottom-6 w-[90%] h-[200px] rounded-xl bg-black opacity-70`}></div>
        </>
    );
}
