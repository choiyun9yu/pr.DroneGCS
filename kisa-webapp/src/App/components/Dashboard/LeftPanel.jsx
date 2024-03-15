import React from "react";
import {ColorThema} from "../ProejctThema";
import {Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";

export const LeftPanel = () => {


    return (
        <div className={`flex-col w-[40%] h-full mr-5`}>

            <div className={`flex flex-row w-full h-[24%]`}>
                <div className={`w-[50%] mr-2.5 rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className={`w-full ml-4 items-center `}>
                        <div className={`pt-3 pl-2`}>• 드론 현황</div>
                    </div>
                    <div className={`flex flex-col justify-between h-[80%] w-full mx-auto text-sm text-[#AEABD8]`}>
                        <div className={`flex mt-5 ml-10 flex-row items-end`}>
                            총 비행횟수<span className={`flex text-white text-2xl px-2`}>37</span>소티
                        </div>
                        <div className={`flex ml-10 flex-row items-end`}>
                            총 비행시간<span className={`flex text-white text-2xl px-2`}>20:37:67</span>
                        </div>
                        <div className={`flex mb-7 ml-10 flex-row items-end`}>
                            총 비행거리<span className={`flex text-white text-2xl px-2`}>75.5334</span>km
                        </div>
                    </div>
                </div>
                <div className={`flex flex-col justify-between w-[50%] ml-2.5 rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className={`flex w-full ml-4`}>
                        <div className={`pt-3 pl-2`}>• 장애 진단</div>
                    </div>
                    <div className={`flex justify-center items-end text-[#AEABD8]`}>
                        <span className={`text-white text-4xl px-2`}>376</span> 건
                    </div>
                    <div className={`flex justify-center items-end mb-7 text-[#AEABD8]`}>
                        총 로그 수<span className={`flex text-white text-2xl px-2`}>189,653</span>건
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
                            <FlightTime/>
                        </div>
                    </div>
                </div>
            </div>

            <div className={`w-full h-[38%] pt-5`}>
                <div className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className="w-full h-full ml-4">
                        <div className={`flex flex-row items-center pt-3 pl-2`}>
                            <span>• 장애진단 현황</span>
                            {/*<div className={`flex flex-row`}>*/}
                            {/*    <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full bg-[#6359e9]`}></div><span>Drone01</span></div>*/}
                            {/*    <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full bg-[#64cff6]`}></div><span>Drone02</span></div>*/}
                            {/*    <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full bg-[#8fe388]`}></div><span>Drone03</span></div>*/}
                            {/*</div>*/}
                        </div>
                        <div className={`h-full pt-5`}>
                            <PredictionChart/>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    );
};

const FlightTime = (props) => {
    const data = [
        {
            "name": "1",
            "FlightTime": 20,
        },
        {
            "name": "2",
            "FlightTime": 23,
        },
        {
            "name": "3",
            "FlightTime": 20,
        },
        {
            "name": "4",
            "FlightTime": 17,
        },
        {
            "name": "5",
            "FlightTime": 18,
        },
        {
            "name": "6",
            "FlightTime": 23,
        },
        {
            "name": "7",
            "FlightTime": 14,
        },
        {
            "name": "8",
            "FlightTime": 10,
        },
        {
            "name": "9",
            "FlightTime": 21,
        },
        {
            "name": "10",
            "FlightTime": 10,
        },
        {
            "name": "11",
            "FlightTime": 20,
        },
        {
            "name": "12",
            "FlightTime": 11,
        },
        {
            "name": "13",
            "FlightTime": 12,
        },
        {
            "name": "14",
            "FlightTime": 20,
        },
        {
            "name": "15",
            "FlightTime": 19,
        },
        {
            "name": "16",
            "FlightTime": 23,
        },
        {
            "name": "17",
            "FlightTime": 24,
        },
        {
            "name": "18",
            "FlightTime": 14,
        },
        {
            "name": "19",
            "FlightTime": 13,
        },
        {
            "name": "20",
            "FlightTime": 20,
        },
        {
            "name": "21",
            "FlightTime": 14,
        },
        {
            "name": "22",
            "FlightTime": 13,
        },
        {
            "name": "23",
            "FlightTime": 20,
        },
        {
            "name": "24",
            "FlightTime": 17,
        },
        {
            "name": "25",
            "FlightTime": 18,
        },
        {
            "name": "26",
            "FlightTime": 23,
        },
        {
            "name": "27",
            "FlightTime": 23,
        },
        {
            "name": "28",
            "FlightTime": 14,
        },
        {
            "name": "29",
            "FlightTime": 23,
        },
        {
            "name": "30",
            "FlightTime": 20,
        },
    ]
    return (
        <>
            <ResponsiveContainer width="90%" height="80%">
                <BarChart data={data}>
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey="FlightTime" fill="#00CCCC" />
                </BarChart>
            </ResponsiveContainer>
        </>);
}

const PredictionChart = (props) => {
    const data = [
        {
            "name": "1",
            "AnomalyCount": 40,
        },
        {
            "name": "2",
            "AnomalyCount": 30,
        },
        {
            "name": "3",
            "AnomalyCount": 20,
        },
        {
            "name": "4",
            "AnomalyCount": 27,
        },
        {
            "name": "5",
            "AnomalyCount": 18,
        },
        {
            "name": "6",
            "AnomalyCount": 23,
        },
        {
            "name": "7",
            "AnomalyCount": 34,
        },
        {
            "name": "8",
            "AnomalyCount": 40,
        },
        {
            "name": "9",
            "AnomalyCount": 13
        },
        {
            "name": "10",
            "AnomalyCount": 90
        },
        {
            "name": "11",
            "AnomalyCount": 40,
        },
        {
            "name": "12",
            "AnomalyCount": 30,
        },
        {
            "name": "13",
            "AnomalyCount": 20,
        },
        {
            "name": "14",
            "AnomalyCount": 20,
        },
        {
            "name": "15",
            "AnomalyCount": 98
        },
        {
            "name": "16",
            "AnomalyCount": 23,
        },
        {
            "name": "17",
            "AnomalyCount": 34,
        },
        {
            "name": "18",
            "AnomalyCount": 34,
        },
        {
            "name": "19",
            "AnomalyCount": 30,
        },
        {
            "name": "20",
            "AnomalyCount": 90
        },
        {
            "name": "21",
            "AnomalyCount": 40,
        },
        {
            "name": "22",
            "AnomalyCount": 18
        },
        {
            "name": "23",
            "AnomalyCount": 98
        },
        {
            "name": "24",
            "AnomalyCount": 27,
        },
        {
            "name": "25",
            "AnomalyCount": 18,
        },
        {
            "name": "26",
            "AnomalyCount": 23,
        },
        {
            "name": "27",
            "AnomalyCount": 40
        },
        {
            "name": "28",
            "AnomalyCount": 30,
        },
        {
            "name": "29",
            "AnomalyCount": 18
        },
        {
            "name": "30",
            "AnomalyCount": 90
        },
    ]

    return (
        <>
            <ResponsiveContainer width="90%" height="80%">
                <BarChart data={data}>
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey="AnomalyCount" fill="#FF6666" />
                </BarChart>
            </ResponsiveContainer>
        </>);
}


