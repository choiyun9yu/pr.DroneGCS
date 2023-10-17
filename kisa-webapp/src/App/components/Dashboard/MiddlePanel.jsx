import React from "react";
import {ColorThema} from "../ProejctThema";
import {Cell, Legend, Pie, PieChart, ResponsiveContainer} from "recharts";

export const MiddlePanel = () => {
    return (
        <div className={`flex-col w-[20%] h-full mr-5 `}>

            <div className={`w-full pb-5 h-[55%]`}>
                <div className={`flex w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                    <div className="w-full mt-1 ml-4 items-center">
                        <div className={`pt-3 pl-2`}>• 비행 일지</div>
                    </div>
                </div>
            </div>

            <div className={`flex w-full h-[45%] rounded-2xl ${ColorThema.Secondary4}`}>
                <div className="w-full mt-1 ml-4 items-center">
                    <div className={`pt-3 pl-2`}>• 비행 비율</div>
                    <div className={`w-full h-full`}>
                        <FlightPieChart/>
                    </div>
                </div>
            </div>
        </div>
    );
};

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
            <ResponsiveContainer width="95%" height="80%">
                <PieChart>
                    <Legend verticalAlign="bottom" align="top" />
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
        </>
    );
}