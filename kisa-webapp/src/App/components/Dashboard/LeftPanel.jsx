import React, {useState, useEffect, useRef} from "react";
import {ColorThema} from "../ProejctThema";
import {Bar, BarChart, Brush, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";

export const LeftPanel = (props) => {
    return (
        <div className={`flex-col w-[40%] h-full mr-5`}>

            <div className={`flex flex-row w-full h-[24%]`}>
                <div className={`w-[50%] mr-2.5 rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className={`w-full ml-4 items-center `}>
                        <div className={`pt-3 pl-2`}>• 드론 현황</div>
                    </div>
                    <div className={`flex flex-col justify-between h-[80%] w-full mx-auto text-sm text-[#AEABD8]`}>
                        <div className={`flex mt-5 ml-10 flex-row items-end`}>
                            총 비행횟수<span className={`flex text-white text-2xl px-2`}>{props.flightCount}</span>소티
                        </div>
                        <div className={`flex ml-10 flex-row items-end`}>
                            총 비행시간<span className={`flex text-white text-2xl px-2`}>{props.flightTime}</span>
                        </div>
                        <div className={`flex mb-7 ml-10 flex-row items-end`}>
                            총 비행거리<span className={`flex text-white text-2xl px-2`}>{(props.flightDistance/1000).toFixed(3)}</span>km
                        </div>
                    </div>
                </div>
                <div className={`flex flex-col justify-between w-[50%] ml-2.5 rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className={`flex w-full ml-4`}>
                        <div className={`pt-3 pl-2`}>• 장애 진단</div>
                    </div>
                    <div className={`flex justify-center items-end text-[#AEABD8]`}>
                        <span className={`text-white text-4xl px-2`}>{props.anomalyCount}</span> 건
                    </div>
                    <div className={`flex justify-center items-end mb-7 text-[#AEABD8]`}>
                        총 로그 수<span className={`flex text-white text-2xl px-2`}>{props.logCount}</span>건
                    </div>
                </div>
            </div>

            <div className={`w-full h-[38%] pt-5`}>
                <div className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className="w-full h-full ml-4 items-center">
                        <div className={`pt-3 pl-2`}>
                            <div className={`flex flex-row`}>
                                <span>• 비행 시간</span>
                            </div>
                        </div>
                        <div className={`h-full pt-5`}>
                            <FlightTime dailyFlightTime={props.dailyFlightTime}/>
                        </div>
                    </div>
                </div>
            </div>

            <div className={`w-full h-[38%] pt-5`}>
                <div className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className="w-full h-full ml-4">
                        <div className={`flex flex-row items-center pt-3 pl-2`}>
                            <span>• 장애진단 현황</span>
                        </div>
                        <div className={`h-full pt-5`}>
                            <PredictionChart dailyAnomalyCount={props.dailyAnomalyCount}/>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    );
};

const FlightTime = (props) => {
    // const brushRef = useRef();
    return (
        <>
            <ResponsiveContainer width="90%" height="80%">
                <BarChart data={props.dailyFlightTime}>
                    <XAxis dataKey="flightDay" tick={{fontSize: 15}}/>
                    <YAxis tick={{fontSize: 15}}/>
                    <Tooltip />
                    <Bar dataKey="flightTime" fill="#48d1cc" />
                    {/*<Brush dataKey="flightDay" ref={brushRef}/>*/}
                </BarChart>
            </ResponsiveContainer>
        </>
    );
}

const PredictionChart = (props) => {
    return (
        <>
            <ResponsiveContainer width="90%" height="80%">
                <BarChart data={props.dailyAnomalyCount}>
                    <XAxis dataKey="flightDay" tick={{fontSize: 15}} />
                    <YAxis tick={{fontSize: 15}}/>
                    <Tooltip />
                    <Bar dataKey="anomalyCount" fill="#FF6347" />
                </BarChart>
            </ResponsiveContainer>
        </>);
}


