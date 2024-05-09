import React, {useContext} from 'react';
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from 'recharts';
import {ColorThema} from "../../ProjectThema";
import {DroneContext} from "../../GCS/SignalRContainer";

export const RealTimeGraph = (props) => {
    const {droneMessage} = useContext(DroneContext);
    const data = droneMessage ? droneMessage['DroneMission']['DroneTrails']['q'] : [];

    if (!data) {
        return (
            <div className="flex mb-4 h-[250px]">
                <div className={`flex flex-col w-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 mb-5 font-bold text-medium">• 드론 고도 </span>
                    <div id="realtime-graph" className="flex h-[200px]">
                        <span>현재 비행 중이 아닙니다.</span>
                    </div>
                </div>
            </div>
        )
    }
    const drone_alt = data.map((object, index) => ({index, value: object.global_frame_alt}));
    const terrain_alt = data.map((object, index) => ({index, value: object.terrain_alt}));
    const minYValue = Math.min(...terrain_alt.map(point => point.value)); // terrain_alt의 최소값 계산

    return (
        <div className="flex mb-4 h-[250px]">
            <div className={`flex flex-col w-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
                <span className="text-white rounded-md ml-3 mb-5 font-bold text-medium">• 드론 고도 </span>
                <div id="realtime-graph" className="flex">
                    <ResponsiveContainer width="98%" height={200}>
                        <LineChart>
                            <XAxis dataKey="index" type="number" tick={false} allowDuplicatedCategory={false}/>
                            <YAxis domain={[minYValue - 1, 'auto']}/>
                            {/*<Legend />*/}
                            <Tooltip />
                            <Line type="monotone" dataKey="value" data={drone_alt} stroke="#64CFF6" dot={false} name={'드론 고도'} />
                            {/*<Line type="monotone" dataKey="value" data={terrain_alt} stroke="#8FE388" dot={false} name={'지형 고도'} />*/}
                        </LineChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </div>
    );
};
