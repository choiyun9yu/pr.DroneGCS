import React from 'react';
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from 'recharts';
import {ColorThema} from "../ProejctThema";

export const RealTimeGraph = (props) => {
    // console.log(props.graphData)

    return (
        <div className="flex mb-4 h-[250px]">
            <div className={`flex flex-col w-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
                <span className="text-white rounded-md ml-3 mb-5 font-bold text-medium">• 드론 고도 </span>
                <div id="realtime-graph" className="flex">
                    <ResponsiveContainer width="98%" height={200}>
                        <LineChart data={props.graphData}>
                            <XAxis
                                dataKey="PredictTime" // dataKey에 X축의 이름을 넣는다.
                                interval={60}
                            />
                            <YAxis
                                dataKey="Alt"
                                tickCount={3}
                            />
                            <Tooltip />
                            <Line type="linear" dataKey="Alt" dot={false} stroke="#65CFF6" />
                        </LineChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </div>
    );
};
