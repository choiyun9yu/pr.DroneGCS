import React, {useState} from "react";
import {Cell, Pie, PieChart, ResponsiveContainer} from "recharts";
import Calendar from "react-calendar";
import moment from "moment";
// import {useQuery} from "react-query";

import {ColorThema} from "../ProejctThema";
import 'react-calendar/dist/Calendar.css';
import './Calendar.css';


export const MiddlePanel = () => {
    return (
        <div className={`flex-col w-[20%] h-full mr-5 `}>

            <div className={`w-full pb-5 h-[55%]`}>
                <div className={`flex w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className="w-full mt-1 ml-4 items-center">
                        <div className={`pt-3 pl-2`}>• 비행 일지</div>
                        <div className={`w-full h-full `}>
                            <FlightCalendar/>
                        </div>
                    </div>

                </div>
            </div>

            <div className={`flex w-full h-[45%] rounded-2xl ${ColorThema.Secondary4}`}>
                <div className="w-full mt-1 ml-4 items-center">
                    <div className={`pt-3 pl-2`}>• 비행 비율</div>
                    <div className={`w-full h-[80%]`}>
                        <FlightPieChart/>
                    </div>
                </div>

            </div>
        </div>
    );
};

const FlightCalendar = () => {
    const [value, onChange] = useState(new Date());

    // const [mark, setMark] = useState([]);
    // const { data } = useQuery(
    //     ["logData", Month],
    //     async () => {
    //         const result;
    //         return result.data
    //     },
    //     {
    //         onSuccess: (data) => {
    //             setMark(data);  // ["2022-02-02", "2022-02-02", "2022-02-10"] 형태로 가져옴
    //         }
    //     }
    // )
    const mark = ["2023-1-02", "2023-10-02", "2023-10-10"];

    return(
        <div id={`calendar-container`} className={`font-normal text-sm`}>
            <Calendar
                onChange={onChange}
                formatDay={(locale, date) => moment(date).format("DD")}
                value={value}
                showNeighboringMonth={false}
                tileContent={({ date, view }) => {
                if (mark.find((x) => x === moment(date).format("YYYY-MM-DD"))) {
                    return (
                        <>
                            <div className="flex justify-center items-center absoluteDiv">
                                <div className="dot"></div>
                            </div>
                        </>
                    );
                }
            }}
            />
            <div className={`pr-3`}><ChartLegend/></div>
        </div>
    );
}

const FlightPieChart = () => {
    const data = [
        { "name": "Drone01", "value": 638 },
        { "name": "Drone02", "value": 248 },
        { "name": "Drone03", "value": 248 },
    ];

    const COLORS= ['#6359E9', '#64CFF6', '#8FE388']
    const RADIAN = Math.PI / 180;

    const renderCustomizedLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, percent, index }) => {
        const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
        const x = cx + radius * Math.cos(-midAngle * RADIAN);
        const y = cy + radius * Math.sin(-midAngle * RADIAN);

        return (
            <text x={x} y={y} fill="white" textAnchor={x > cx ? 'start' : 'end'} dominantBaseline="central">
                {`${(percent * 100).toFixed(0)}%`}
            </text>
        );
    };

    return (
        <>
            <ResponsiveContainer width="95%" height="87%">
                <PieChart>
                    <Pie
                        data={data}
                        cx="50%"
                        cy="50%"
                        labelLine={false}
                        label={renderCustomizedLabel}
                        innerRadius={80} outerRadius={120}
                        fill="#8884d8"
                        dataKey="value">
                        {data.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                    </Pie>
                </PieChart>
            </ResponsiveContainer>
            <ChartLegend/>
        </>
    );
}

export const ChartLegend = () => {
    return(
        <div className={`flex w-[95%] h-[300px] my-5`}>
            <div className={`flex flex-row w-full h-[10%] justify-center items-center font-normal text-sm border border-[#27264E] rounded-md ${ColorThema.Secondary3}`}>
                <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full bg-[#6359e9]`}></div><span>Drone01</span></div>
                <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full bg-[#64cff6]`}></div><span>Drone02</span></div>
                <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full bg-[#8fe388]`}></div><span>Drone03</span></div>
            </div>
        </div>
    );
}