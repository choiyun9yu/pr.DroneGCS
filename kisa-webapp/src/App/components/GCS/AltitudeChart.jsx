import React, {useContext} from "react";
import {DroneContext} from "./SignalRContainer";
import {Line, LineChart, AreaChart, Area, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";

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
                    <Line type="basisOpen" dataKey="value" data={drone_alt} stroke="#64CFF6" dot={false} name={'드론 고도'} />
                    <Line type="basis" dataKey="value" data={terrain_alt} stroke="#8FE388" dot={false} name={'지형 고도'} />
                    {/*<Area type="basisClosed" dataKey="value" stroke="#8FE388" fill="#8FE388" name={'지형 고도'} />*/}
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
};