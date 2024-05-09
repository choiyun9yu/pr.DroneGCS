import { Outlet } from 'react-router-dom';

import { Menu } from './Others/Menu';
import { GradientBackground } from './ProjectThema';
import {WarningModal} from "./ProejctModal";
import {DroneContext, SignalRProvider} from "./GCS/SignalRContainer";
import React, {useContext, useEffect, useState} from "react";

export const AppWrapper = () => {
    const {droneStates, warningSkipList} = useContext(DroneContext)
    const [warningList, setWarningList] = useState([])

    useEffect(() => {
        const newWarningList = []
        for (const key in droneStates) {
            const drone = droneStates[key]
            if (drone.WarningData?.WarningCount >= 10) {
                newWarningList.push(drone.DroneId)
                console.log(drone.WarningData?.WarningCount)
            }
        }
        setWarningList(newWarningList)
    }, [droneStates])

    const WarningResult = warningList.filter(item => !warningSkipList.includes(item))

    return (
        <GradientBackground>
                <div id="app-wrapper" className="flex flex-row h-screen w-screen overflow-hidden">
                    <Menu />
                    <div className="h-screen w-full overflow-hidden">
                        <Outlet />
                    </div>
                    {WarningResult.map((d, i) => (
                        <WarningModal index={i} key={i} drone={d} />
                    ))}
                </div>
        </GradientBackground>
    );
};
