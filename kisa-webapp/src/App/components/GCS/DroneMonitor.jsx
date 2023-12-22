import { useOutletContext } from 'react-router-dom';
import { LeftSidebar } from './LeftSidebar';
import { MiddleMap } from './MiddleMap';
import { FlightMode } from './FlightMode';
import { MissionMode } from './MissionMode';
import { VideoMode } from './VideoMode';
import {useContext, useState} from "react";
import {DataMap} from "../DataMap";
import {DroneContext, SignalRProvider} from "../GCS/SignalRContainder";

export const DroneMonitor = () => {
    const [gcsMode, setGcsMode] = useOutletContext();
    const [swapMap, setSwapMap] = useState(false);
    const [isLeftPanel, setIsLeftPanel ] = useState(true);
    const [isRightPanel, setIsRightPanel] = useState(true);

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
                        <LeftSidebar gcsMode={gcsMode} setGcsMode={setGcsMode} />
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
                />
            </div>
                {gcsMode === 'flight' && isRightPanel ? (
                    <FlightMode
                        handleSwapMap={handleSwapMap}
                        handleIsRightPanel={handleIsRightPanel}
                        swapMap={swapMap}
                    />) : null}
                {gcsMode === 'mission' ? <MissionMode /> : null}
                {gcsMode === 'video' ? <VideoMode /> : null}
        </div>
    );
};
