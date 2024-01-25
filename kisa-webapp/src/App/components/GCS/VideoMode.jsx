import { ColorThema } from '../ProejctThema';
import React, {useContext, useState} from "react";
import {Table} from "./MiddleMap";
import {AltitudeChart} from "./AltitudeChart";
import {DroneContext} from "./SignalRContainer";

export const VideoContents = (props) => {
    // const {handleDroneFlightMode, handleDroneFlightCommand} = useContext(DroneContext);
    // const [controlBtn, setControlBtn] = useState(false);
    //
    // const handleControlBtn = () => {
    //     setControlBtn(!controlBtn)
    // }
    //
    // const handleTakeoffBtn = async () => {
    //     handleDroneFlightMode(4)
    //     await waitOneSecond()
    //     handleDroneFlightCommand(0)
    //     handleDroneFlightCommand(2)
    // }
    //
    // const handleLandBtn = async () => {
    //     handleDroneFlightMode(4)
    //
    //     await waitOneSecond()
    //
    //     handleDroneFlightCommand(3)
    // }
    //
    // const handelReturnBtn = () => {
    //     props.handleIsRtl();
    //     handleDroneFlightMode(6)
    // }
    //
    // function waitOneSecond() {
    //     return new Promise(resolve => setTimeout(resolve, 1000));
    // }

    return (
        <>
            <Table className={'z-30'} middleTable={props.middleTable} monitorTable={props.monitorTable} setMonitorTable={props.setMonitorTable}/>

            <div className={`absolute left-2 bottom-6 w-[90%] h-[200px] rounded-xl bg-black opacity-70`}>
                <AltitudeChart />
            </div>

            {/*<div className={`absolute flex flex-col justify-around items-center left-[10px] top-[200px] text-xs rounded bg-white opacity-75`}>*/}
            {/*    <button onClick={handleControlBtn}>*/}
            {/*        controller*/}
            {/*    </button>*/}

            {/*    {controlBtn*/}
            {/*        ? (*/}
            {/*            <div className={`flex p-2 w-[170px] flex-col text-xs rounded-md`} style={{*/}
            {/*                position: 'absolute',*/}
            {/*                top: '0px',*/}
            {/*                left: '60px',*/}
            {/*                opacity: '70%',*/}
            {/*                background: '#ffff',*/}
            {/*            }}>*/}
            {/*            <div className={`flex flex-row justify-center`}>*/}
            {/*                    <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"*/}
            {/*                             strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">*/}
            {/*                            <path strokeLinecap="round" strokeLinejoin="round"*/}
            {/*                                  d="M15 10.5a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"/>*/}
            {/*                            <path strokeLinecap="round" strokeLinejoin="round"*/}
            {/*                                  d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1 1 15 0Z"/>*/}
            {/*                        </svg>*/}
            {/*                        Starting*/}
            {/*                    </button>*/}
            {/*                    <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path fillRule="evenodd"*/}
            {/*                                  d="m11.54 22.351.07.04.028.016a.76.76 0 0 0 .723 0l.028-.015.071-.041a16.975 16.975 0 0 0 1.144-.742 19.58 19.58 0 0 0 2.683-2.282c1.944-1.99 3.963-4.98 3.963-8.827a8.25 8.25 0 0 0-16.5 0c0 3.846 2.02 6.837 3.963 8.827a19.58 19.58 0 0 0 2.682 2.282 16.975 16.975 0 0 0 1.145.742ZM12 13.5a3 3 0 1 0 0-6 3 3 0 0 0 0 6Z"*/}
            {/*                                  clipRule="evenodd"/>*/}
            {/*                        </svg>*/}
            {/*                        Targeting*/}
            {/*                    </button>*/}
            {/*                </div>*/}

            {/*                <br/>*/}

            {/*                <div className={`flex flex-row justify-center`}>*/}
            {/*                    <button onClick={handelReturnBtn}*/}
            {/*                        className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path*/}
            {/*                                d="M9.195 18.44c1.25.714 2.805-.189 2.805-1.629v-2.34l6.945 3.968c1.25.715 2.805-.188 2.805-1.628V8.69c0-1.44-1.555-2.343-2.805-1.628L12 11.029v-2.34c0-1.44-1.555-2.343-2.805-1.628l-7.108 4.061c-1.26.72-1.26 2.536 0 3.256l7.108 4.061Z"/>*/}
            {/*                        </svg>*/}
            {/*                        Return*/}
            {/*                    </button>*/}

            {/*                    <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path fillRule="evenodd"*/}
            {/*                                  d="M4.5 7.5a3 3 0 0 1 3-3h9a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9Z"*/}
            {/*                                  clipRule="evenodd"/>*/}
            {/*                        </svg>*/}
            {/*                        Break*/}
            {/*                    </button>*/}

            {/*                    <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path fillRule="evenodd"*/}
            {/*                                  d="M4.5 5.653c0-1.427 1.529-2.33 2.779-1.643l11.54 6.347c1.295.712 1.295 2.573 0 3.286L7.28 19.99c-1.25.687-2.779-.217-2.779-1.643V5.653Z"*/}
            {/*                                  clipRule="evenodd"/>*/}
            {/*                        </svg>*/}
            {/*                        Go To*/}
            {/*                    </button>*/}
            {/*                </div>*/}

            {/*                <br/>*/}

            {/*                <div className={`flex flex-row justify-center`}>*/}
            {/*                    <button className={`flex flex-col mx-auto items-center w-[50px] hover:text-[#AEABD8]`}*/}
            {/*                            onClick={() => handleTakeoffBtn()}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path*/}
            {/*                                d="M11.47 1.72a.75.75 0 0 1 1.06 0l3 3a.75.75 0 0 1-1.06 1.06l-1.72-1.72V7.5h-1.5V4.06L9.53 5.78a.75.75 0 0 1-1.06-1.06l3-3ZM11.25 7.5V15a.75.75 0 0 0 1.5 0V7.5h3.75a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9a3 3 0 0 1 3-3h3.75Z"/>*/}
            {/*                        </svg>*/}
            {/*                        Take Off*/}
            {/*                    </button>*/}

            {/*                    <button className={`flex flex-col mx-auto items-center w-[50px] hover:text-[#AEABD8]`}*/}
            {/*                            onClick={() => handleLandBtn()}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path*/}
            {/*                                d="M12 1.5a.75.75 0 0 1 .75.75V7.5h-1.5V2.25A.75.75 0 0 1 12 1.5ZM11.25 7.5v5.69l-1.72-1.72a.75.75 0 0 0-1.06 1.06l3 3a.75.75 0 0 0 1.06 0l3-3a.75.75 0 1 0-1.06-1.06l-1.72 1.72V7.5h3.75a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9a3 3 0 0 1 3-3h3.75Z"/>*/}
            {/*                        </svg>*/}
            {/*                        Land*/}
            {/*                    </button>*/}

            {/*                    <button className={`flex flex-col mx-auto items-center w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                             className="w-6 h-6">*/}
            {/*                            <path fillRule="evenodd"*/}
            {/*                                  d="M3.792 2.938A49.069 49.069 0 0 1 12 2.25c2.797 0 5.54.236 8.209.688a1.857 1.857 0 0 1 1.541 1.836v1.044a3 3 0 0 1-.879 2.121l-6.182 6.182a1.5 1.5 0 0 0-.439 1.061v2.927a3 3 0 0 1-1.658 2.684l-1.757.878A.75.75 0 0 1 9.75 21v-5.818a1.5 1.5 0 0 0-.44-1.06L3.13 7.938a3 3 0 0 1-.879-2.121V4.774c0-.897.64-1.683 1.542-1.836Z"*/}
            {/*                                  clipRule="evenodd"/>*/}
            {/*                        </svg>*/}
            {/*                        Alt*/}
            {/*                    </button>*/}
            {/*                </div>*/}
            {/*            </div>*/}
            {/*        )*/}
            {/*        : null}*/}
            {/*</div>*/}
        </>
    );
}

export const VideoMode = () => {
    return (
        <div id="right-sidebar" className="flex flex-col w-[720px]">
            <VideoModeTop />
            <VideoModeBottom />
        </div>
    );
};

const VideoModeTop = () => {
    return (
        <div className={`flex items-start w-full h-1/2 mb-3 rounded-2xl ${ColorThema.Secondary4}`}>
            <div className="flex flex-col w-full h-full m-2">
                <span className="text-white rounded-md m-3 font-bold text-medium">• 드론 영상1</span>
                <div className={`flex justify-center items-center w-full h-full px-2 pt-3 pb-7`}>
                    <div className={`flex w-full h-full bg-black`}></div>
                </div>
            </div>
        </div>
    );
};

const VideoModeBottom = () => {
    return (
        <div className={`flex items-start w-full h-1/2 rounded-2xl ${ColorThema.Secondary4}`}>
            <div className="flex flex-col w-full h-full m-2">
                <span className="text-white rounded-md m-3 font-bold text-medium">• 드론 영상2</span>
                <div className={`flex justify-center items-center w-full h-full px-2 pt-3 pb-7`}>
                    <div className={`flex w-full h-full bg-black`}></div>
                </div>
            </div>
        </div>
    );
};
