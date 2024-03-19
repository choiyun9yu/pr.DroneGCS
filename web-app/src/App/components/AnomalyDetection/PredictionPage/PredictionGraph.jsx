import React from 'react';
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer, Legend } from 'recharts';
import { ColorThema } from '../../ProejctThema';

export const PredictionGraph = (props) => {
    const dayReverse = props.graphData.reverse();

    return (
        <div id="predictionresultselect" className="flex w-full mt-5 mb-5">
            <div id="predictiondropdown" className={`flex flex-col w-full p-5 rounded-lg ${ColorThema.Secondary2}`}>
                <span className="mb-5 rounded-md font-bold text-medium text-white">• 로그 예측 모델 결과 조회</span>
                <div id="loggraph" className="flex">
                    <ResponsiveContainer width="98%" height={150}>
                        <LineChart data={dayReverse}>
                            <XAxis
                                dataKey="PredictTime" // dataKey에 X축의 이름을 넣는다.
                            />
                            <YAxis />
                            <Legend />
                            <Tooltip />
                            <Line type="monotone" dataKey="PredictData" stroke="#64CFF6" dot={false} name={'예측값'} />
                            <Line type="monotone" dataKey="SelectData" stroke="#8FE388" dot={false} name={'실제값'} />
                        </LineChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </div>
    );
};
