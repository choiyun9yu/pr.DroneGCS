import {useOutletContext} from 'react-router-dom';

import {ColorThema} from '../ProejctThema';
import {LeftSideBtn} from '../ProejctBtn';
import React, {useContext, useEffect, useState} from "react";
import {DroneContext} from "./SignalRContainder";

export const LeftSidebar = (props) => {
    const [gcsMode, setGcsMode] = useOutletContext();
    const {droneMessage} = useContext(DroneContext);

    return (
        <div
            className={`flex flex-col items-start w-full h-full rounded-2xl font-bold text-medium text-gray-200 ${ColorThema.Secondary4}`}>
            <LeftSideBtn gcsMode={gcsMode} setGcsMode={setGcsMode}/>
            <div className={`m-2 items-center `}>
                <button
                    className={`flex flex-row w-full items-center ml-2 px-2 py-1 rounded-md ${ColorThema.Secondary3}`}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5}
                         stroke="currentColor" className="mr-1 w-5 h-5">
                        <path strokeLinecap="round" strokeLinejoin="round"
                              d="M13.19 8.688a4.5 4.5 0 011.242 7.244l-4.5 4.5a4.5 4.5 0 01-6.364-6.364l1.757-1.757m13.35-.622l1.757-1.757a4.5 4.5 0 00-6.364-6.364l-4.5 4.5a4.5 4.5 0 001.242 7.244"/>
                    </svg>
                    Add New Link
                </button>
            </div>

            <div className="w-full m-2 items-center">
                <span className="ml-3">• 등록 드론 </span>
                {droneMessage && droneMessage.droneMessage.DroneId !== '' && (
                    <button className="flex items-center ml-3 my-1 px-3 w-[80%] h-[40px] rounded bg-[#6359E9]"
                    >
                        {/* onClick하면 현재 해당 드론 센터로 가는 기능*/}

                        {/* 드론이 등록되었을 때 보여질 내용 */}
                        <span>{droneMessage.droneMessage.DroneId}</span>

                        <span className={`ml-2`}>
                            {droneMessage.droneMessage.IsOnline
                                ? <div className="w-3 h-3 bg-green-500 rounded-full"></div>
                                : <div className="w-3 h-3 bg-red-500 rounded-full"></div>
                            }
                        </span>
                        <div className="flex ml-auto">
                            <span className="text-gray-300 text-sm mr-0.5">
                                {droneMessage.droneMessage.DroneStt.BatteryStt}%
                            </span>
                                {
                                    droneMessage.droneMessage.DroneStt.BatteryStt >= 80
                                        ? (
                                                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"
                                                     fill="currentColor" className="w-6 h-5">
                                                    <path fillRule="evenodd"
                                                          d="M3.75 6.75a3 3 0 0 0-3 3v6a3 3 0 0 0 3 3h15a3 3 0 0 0 3-3v-.037c.856-.174 1.5-.93 1.5-1.838v-2.25c0-.907-.644-1.664-1.5-1.837V9.75a3 3 0 0 0-3-3h-15Zm15 1.5a1.5 1.5 0 0 1 1.5 1.5v6a1.5 1.5 0 0 1-1.5 1.5h-15a1.5 1.5 0 0 1-1.5-1.5v-6a1.5 1.5 0 0 1 1.5-1.5h15ZM4.5 9.75a.75.75 0 0 0-.75.75V15c0 .414.336.75.75.75H18a.75.75 0 0 0 .75-.75v-4.5a.75.75 0 0 0-.75-.75H4.5Z"
                                                          clipRule="evenodd"/>
                                                </svg>
                                           )
                                        : (droneMessage.droneMessage.DroneStt.BatteryStt < 80 && droneMessage.droneMessage.DroneStt.BatteryStt > 20
                                                ? (
                                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"
                                                         fill="currentColor" className="w-6 h-5">
                                                        <path
                                                            d="M4.5 9.75a.75.75 0 0 0-.75.75V15c0 .414.336.75.75.75h6.75A.75.75 0 0 0 12 15v-4.5a.75.75 0 0 0-.75-.75H4.5Z"/>
                                                        <path fillRule="evenodd"
                                                              d="M3.75 6.75a3 3 0 0 0-3 3v6a3 3 0 0 0 3 3h15a3 3 0 0 0 3-3v-.037c.856-.174 1.5-.93 1.5-1.838v-2.25c0-.907-.644-1.664-1.5-1.837V9.75a3 3 0 0 0-3-3h-15Zm15 1.5a1.5 1.5 0 0 1 1.5 1.5v6a1.5 1.5 0 0 1-1.5 1.5h-15a1.5 1.5 0 0 1-1.5-1.5v-6a1.5 1.5 0 0 1 1.5-1.5h15Z"
                                                              clipRule="evenodd"/>
                                                    </svg>

                                                )
                                                : (droneMessage.droneMessage.DroneStt.BatteryStt <= 20
                                                        ? (
                                                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"
                                                                 fill="currentColor" className="w-6 h-5">
                                                                <path fillRule="evenodd"
                                                                      d="M.75 9.75a3 3 0 0 1 3-3h15a3 3 0 0 1 3 3v.038c.856.173 1.5.93 1.5 1.837v2.25c0 .907-.644 1.664-1.5 1.838v.037a3 3 0 0 1-3 3h-15a3 3 0 0 1-3-3v-6Zm19.5 0a1.5 1.5 0 0 0-1.5-1.5h-15a1.5 1.5 0 0 0-1.5 1.5v6a1.5 1.5 0 0 0 1.5 1.5h15a1.5 1.5 0 0 0 1.5-1.5v-6Z"
                                                                      clipRule="evenodd"/>
                                                            </svg>

                                                        )
                                                        : null
                                                )
                                        )
                                }
                        </div>
                    </button>
                )}
            </div>

            <div className="w-full m-2 items-center">
                <span className="ml-3">• 비행 경로 </span>
            </div>

        </div>
    );
};
