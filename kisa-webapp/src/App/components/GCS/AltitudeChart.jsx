import React, {useContext} from "react";
import {DroneContext} from "./SignalRContainer";
import {Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";

export const AltitudeChart = () => {
    const {droneMessage} = useContext(DroneContext);
    const data = droneMessage ? droneMessage['droneMessage']['DroneMission']['DroneTrails']['q'] : null;

    if (!data) return null

    const drone_alt = data.map((object, index) => ({index, value: object.global_frame_alt}));
    const terrain_alt = data.map((object, index) => ({index, value: object.terrain_alt}));
    const minYValue = Math.min(...terrain_alt.map(point => point.value));

    return (
        <div id="altitudechart" className="flex mt-3">
            <ResponsiveContainer width="98%" height={200}>
                <LineChart>
                    <XAxis dataKey="index" type="number" tick={false} allowDuplicatedCategory={false}/>
                    <YAxis domain={[minYValue - 1, 'auto']}/>
                    {/*<Legend />*/}
                    <Tooltip />
                    <Line type="monotone" dataKey="value" data={drone_alt} stroke="#64CFF6" dot={false} name={'드론 고도'} />
                    <Line type="monotone" dataKey="value" data={terrain_alt} stroke="#8FE388" dot={false} name={'지형 고도'} />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
};