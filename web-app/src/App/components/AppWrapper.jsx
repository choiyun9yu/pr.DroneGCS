import { Outlet } from 'react-router-dom';

import { Menu } from './Others/Menu';
import { GradientBackground } from './ProjectThema';
import {WarningModal} from "./ProejctModal";
import {DroneContext, SignalRProvider} from "./GCS/SignalRContainer";
import React, {useContext} from "react";

export const AppWrapper = () => {
    const {isWarningModal, warningList} = useContext(DroneContext)

    return (
        <GradientBackground>
                <div id="app-wrapper" className="flex flex-row h-screen w-screen overflow-hidden">
                    <Menu />
                    <div className="h-screen w-full overflow-hidden">
                        <Outlet />
                    </div>
                    {isWarningModal && warningList.map((d, i) => (
                        <WarningModal index={i} drone={d} />
                    ))}
                </div>
        </GradientBackground>
    );
};
