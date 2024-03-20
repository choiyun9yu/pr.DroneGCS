import React, {useState, useEffect} from "react";
import {Cell, Pie, PieChart, ResponsiveContainer} from "recharts";
import Calendar from "react-calendar";
import moment from "moment";

import {ColorThema} from "../ProjectThema";
import 'react-calendar/dist/Calendar.css';
import '../../styles/Calendar.css';

export const MiddlePanel = (props) => {
    // console.log(props.flightDay)
    return (
        <div className={`flex-col w-[20%] h-full mr-5 `}>

            <div className={`w-full pb-5 h-[55%]`}>
                <div className={`flex w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className="w-full mt-1 ml-4 items-center">
                        <div className={`pt-3 pl-2`}>• 비행 일지</div>
                        <div className={`w-full h-full `}>
                            <FlightCalendar flightDay={props.flightDay} value={props.value}/>
                        </div>
                    </div>

                </div>
            </div>

            <div className={`flex w-full h-[45%] rounded-2xl ${ColorThema.Secondary4}`}>
                <div className="w-full mt-1 ml-4 items-center">
                    <div className={`pt-3 pl-2`}>• 비행 비율</div>
                    <div className={`w-full h-[80%]`}>
                        <FlightPieChart flightDay={props.flightDay}/>
                    </div>
                </div>
            </div>

        </div>
    );
};

const FlightCalendar = (props) => {

    const markDrone1 = props.flightDay && props.flightDay["1"] ?
        [...new Set(props.flightDay["1"].map(date => moment(date).format("YYYY-MM-DD")))] : [];
    const markDrone2 = props.flightDay && props.flightDay["2"] ?
        [...new Set(props.flightDay["2"].map(date => moment(date).format("YYYY-MM-DD")))] : [];
    const markDrone3 = props.flightDay && props.flightDay["3"] ?
        [...new Set(props.flightDay["3"].map(date => moment(date).format("YYYY-MM-DD")))] : [];

    return(
        <div id={`calendar-container`} className={`font-normal text-sm`}>
            <Calendar
                formatDay={(locale, date) => moment(date).format("DD")}
                value={props.value}
                showNeighboringMonth={false}
                tileContent={({ date }) => {
                    const markerPositions = [];
                    markDrone1.forEach((dateStr) => {
                        if (dateStr === moment(date).format("YYYY-MM-DD")) {
                            markerPositions.push({ color: "#6359E9" });
                        }
                    });
                    markDrone2.forEach((dateStr) => {
                        if (dateStr === moment(date).format("YYYY-MM-DD")) {
                            markerPositions.push({ color: "#8fe388" });
                        }
                    });
                    markDrone3.forEach((dateStr) => {
                        if (dateStr === moment(date).format("YYYY-MM-DD")) {
                            markerPositions.push({ color: "#64cff6" });
                        }
                    });

                    return (
                        <div className="flex justify-center items-center absoluteDiv tile">
                            {markerPositions.map((marker, index) => (
                                <div
                                    key={index}
                                    className="w-[6px] h-[6px] m-0.5 rounded-full"
                                    style={{ backgroundColor: marker.color }}
                                ></div>
                            ))}
                        </div>
                    );
                }}
            />
            <div className={`pr-3`}><ChartLegend/></div>
        </div>
    );
}

const FlightPieChart = (props) => {

    const data = [
        { "name": "Drone01", "value":
                props.flightDay && props.flightDay["1"]
                    ? props.flightDay["1"].length : 0
        },
        { "name": "Drone02", "value":
                props.flightDay && props.flightDay["2"]
                    ? props.flightDay["2"].length : 0
        },
        { "name": "Drone03", "value":
                props.flightDay && props.flightDay["3"]
                    ? props.flightDay["3"].length : 0
        },
    ];

    const COLORS= ['#6359E9', '#64CFF6', '#8FE388']
    const RADIAN = Math.PI / 180;

    const renderCustomizedLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, percent, index }) => {
        const radius = innerRadius + (outerRadius - innerRadius) * 0.28;
        const x = cx + radius * Math.cos(-midAngle * RADIAN);
        const y = cy + radius * Math.sin(-midAngle * RADIAN);

        return (
            <text className={`flex text-sm`} x={x} y={y} fill="white" textAnchor={x > cx ? 'start' : 'end'} dominantBaseline="central">
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
                        innerRadius={70} outerRadius={120}
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
                <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full ${ColorThema.Semantic1}`}></div><span>Drone01</span></div>
                <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full ${ColorThema.Semantic4}`}></div><span>Drone02</span></div>
                <div className={`flex h-full p-2 items-center`}><div className={`flex w-3 h-3 mr-1 rounded-full ${ColorThema.Semantic3}`}></div><span>Drone03</span></div>
            </div>
        </div>
    );
}