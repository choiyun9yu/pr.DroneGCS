import { useOutletContext } from 'react-router-dom';
import { LeftSidebar } from './LeftSidebar';
import { MiddleMap } from './MiddleMap';
import { FlightMode } from './FlightMode';
import { MissionMode } from './MissionMode';
import { VideoMode } from './VideoMode';
import {useContext, useState} from "react";
import {DroneContext} from "./SignalRContainer";

export const DroneSystem = () => {
    const {droneMessage} = useContext(DroneContext)
    const droneState = droneMessage ? droneMessage['droneMessage'] : null;

    const [gcsMode, setGcsMode] = useOutletContext();
    const [swapMap, setSwapMap] = useState(false);
    const [isLeftPanel, setIsLeftPanel ] = useState(true);
    const [isRightPanel, setIsRightPanel] = useState(true);
    const [isWayPointBtn, setIsWayPointBtn] = useState(false);
    const [isLocalMarker, setIsLocalMarker] = useState(false);
    const [isMissionBtn, setIsMissionBtn] = useState(false);

    const [targetPoints, setTargetPoints] = useState([]); // {id:1, position:{lat:0,lng:0}}
    const [flightSchedule, setFlightSchedule] = useState([]);

    const [localLat, setLocalLat] = useState();
    const [localLon, setLocalLon] = useState();

    const [ isModalOpen, setIsModalOpen ] = useState(false);

    const handleIsModal = () => {
        setIsModalOpen(!isModalOpen)
    }

    const handleIsLocalMarker = () => {
        setIsLocalMarker(!isLocalMarker)
    }

    const handleIsMissionBtn = () => {
        setIsMissionBtn(!isMissionBtn)
    }

    const handleCurrentPoint = () => {
        setLocalLat(droneState.DroneStt.Lat)
        setLocalLon(droneState.DroneStt.Lon)
    }

    const [center, setCenter] = useState({
        lat: -35.3632623,
        lng: 149.1652378
    });

    const handleSwapMap = () => {
        setSwapMap(!swapMap)
    }

    const handleIsLeftPanel = () => {
        setIsLeftPanel(!isLeftPanel)
    }

    const handleIsRightPanel = () => {
        setIsRightPanel(!isRightPanel)
    }

    const handleIsWayPointBtn = () => {
        setIsWayPointBtn(!isWayPointBtn)
    }

    return (
        <div className="flex flex-row w-full h-full p-3 overflow-hidden">
                { isLeftPanel
                    ? <div id="left-sidebar" className="w-[180px]">
                        <LeftSidebar gcsMode={gcsMode}
                                     setGcsMode={setGcsMode}
                                     setCenter={setCenter}
                                     flightSchedule={flightSchedule}
                                     handleIsModal={handleIsModal}
                        />
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
                           handleIsWayPointBtn={handleIsWayPointBtn}
                           center={center}
                           setCenter={setCenter}
                           isLocalMarker={isLocalMarker}
                           setIsLocalMarker={setIsLocalMarker}
                           setLocalLat={setLocalLat}
                           setLocalLon={setLocalLon}
                           isMissionBtn={isMissionBtn}
                           handleIsMissionBtn={handleIsMissionBtn}
                           targetPoints={targetPoints}
                           setTargetPoints={setTargetPoints}
                           setFlightSchedule={setFlightSchedule}
                           isModalOpen={isModalOpen}
                           handleIsModal={handleIsModal}
                />
            </div>
                {gcsMode === 'flight' && isRightPanel ? (
                    <FlightMode
                        handleSwapMap={handleSwapMap}
                        handleIsRightPanel={handleIsRightPanel}
                        swapMap={swapMap}
                        center={center}
                    />) : null}
                {gcsMode === 'mission' ?
                    <MissionMode
                        isWayPoint={isWayPointBtn}
                        isLocalMarker={isLocalMarker}
                        handleIsLocalMarker={handleIsLocalMarker}
                        localLat={localLat}
                        localLon={localLon}
                        handleCurrentPoint={handleCurrentPoint}
                        isMissionBtn={isMissionBtn}
                        handleIsMissionBtn={handleIsMissionBtn}
                        setTargetPoints={setTargetPoints}
                        setFlightSchedule={setFlightSchedule}
                    /> : null}
                {gcsMode === 'video' ? <VideoMode /> : null}
        </div>
    );
};
