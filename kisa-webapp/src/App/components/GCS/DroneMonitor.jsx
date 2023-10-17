import { useOutletContext } from 'react-router-dom';

import { LeftSidebar } from './LeftSidebar';
import { MiddleMap } from './MiddleMap';
import { FlightMode } from './FlightMode';
import { MissionMode } from './MissionMode';
import { VideoMode } from './VideoMode';
import { useState } from "react";
import {DataMap} from "../DataMap";

export const DroneMonitor = () => {
    const [gcsMode, setGcsMode] = useOutletContext();
    const [swapMap, setSwapMap] = useState(false);
    const [isLeftPanel, setIsLeftPanel ] = useState(true);
    const [isRightPanel, setIsRightPanel] = useState(true)
    const [monitorItems, setMonitorItems] = useState(DataMap.monitorMap);

    const dataTransfer = (data) => {
        setMonitorItems(data)
    }

    const handleSwapMap = () => {
        setSwapMap(!swapMap)
    }

    const handleIsLeftPanel = () => {
        setIsLeftPanel(!isLeftPanel)
    }

    const handleIsRightPanel = () => {
        setIsRightPanel(!isRightPanel)
    }


    return (
        <div className="flex flex-row w-full h-full p-3 overflow-hidden">
                { isLeftPanel
                    ? <div id="left-sidebar" className="w-[180px]">
                        <LeftSidebar gcsMode={gcsMode} setGcsMode={setGcsMode} dataTransfer={dataTransfer} />
                      </div>
                    : null}
            <div id="middle-map" className="flex-grow m-3">
                <MiddleMap gcsMode={gcsMode}
                           swapMap={swapMap}
                           handleSwapMap={handleSwapMap}
                           isLeftPanel={isLeftPanel}
                           handleIsLeftPanel={handleIsLeftPanel}
                           isRightPanel={isRightPanel}
                           handleIsRightPanel={handleIsRightPanel}
                           middleTable={monitorItems.middleTable}
                />
            </div>
                {gcsMode === 'flight' && isRightPanel ? (
                    <FlightMode
                        handleSwapMap={handleSwapMap}
                        handleIsRightPanel={handleIsRightPanel}
                        swapMap={swapMap}
                        rightTable={monitorItems.rightTable}
                    />) : null}
                {gcsMode === 'mission' ? <MissionMode /> : null}
                {gcsMode === 'video' ? <VideoMode /> : null}
        </div>
    );
};
