import React, {useContext, useState} from "react";
import { GoogleMap, useJsApiLoader, Polyline, OverlayView } from '@react-google-maps/api';

import { ColorThema } from '../ProejctThema';
import './GCSstyles.css';
import {FlightContents} from "./FlightMode";
import {OtherContents} from "./MissionMode";
import {DroneContext} from "./SignalRContainder";

export const MiddleMap = (props) => {
    const [isController, setIsController] = useState(true)
    const { droneMessage } = useContext(DroneContext);
    // const [dronePosition, setDronePosition] = useState({ lat: -35.3632623, lng: 149.1652378 });
    // const [dronePath, setDronePath] = useState([{ lat: -35.3632623, lng: 149.1652378 }]);

    const droneState = droneMessage ? droneMessage['droneMessage'] : null;

    const handleIsController= () => {
        setIsController(!isController)
    }

    const { isLoaded } = useJsApiLoader({
        id: 'google-map-script',
        googleMapsApiKey: process.env.REACT_APP_GOOGLE_MAPS_API_KEY
    })

    if (!isLoaded) {
        return null; // Google Maps JavaScript API가 로드되지 않았을 때 아무것도 렌더링하지 않음
    }

    return (
        props.swapMap
            ? <div id='google-map' className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
            </div>
            : <div id='google-map' className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                <GoogleMap mapContainerClassName={`flex w-full h-full rounded-xl`}
                           center={props.center} zoom={18}>

                    {/* 드론 마커 */}
                    {/*usecontext로 드론 위치 가져와서  position 자리에 넣어보기 */}
                    <OverlayView
                        position={{ lat: droneMessage && droneState.DroneStt.Lat, lng: droneMessage && droneState.DroneStt.Lon }}
                        mapPaneName={OverlayView.OVERLAY_MOUSE_TARGET}
                        getPixelPositionOffset={(width, height) => ({
                            x: -(width / 2),
                            y: -(height / 2),
                        })}
                    >
                        <div
                            style={{
                                width: '60px',
                                height: '76px',
                                backgroundImage: `url(${process.env.PUBLIC_URL}/Drone.png)`,
                                backgroundSize: 'contain',
                                transform: `translate(-50%, -50%) rotate(${droneMessage && droneState.DroneStt.Head}deg)`,
                            }}
                        />
                    </OverlayView>

                    {/*/!* 드론 경로 *!/*/}
                    {/*<Polyline path={dronePath} options={{ strokeColor: '#000000', strokeWeight: 2 }} />*/}

                    {props.gcsMode === 'flight' ? <FlightContents isLeftPanel={props.isLeftPanel}
                                                                  handleIsLeftPanel={props.handleIsLeftPanel}
                                                                  isRightPanel={props.isRightPanel}
                                                                  handleIsRightPanel={props.handleIsRightPanel}
                                                                  isController={isController}
                                                                  handleIsController={handleIsController}
                                                                  handleSwapMap={props.handleSwapMap}
                                                                  middleTable={props.middleTable}
                    /> : null}
                    {props.gcsMode === 'mission' ? <OtherContents middleTable={props.middleTable}/> : null}
                    {props.gcsMode === 'video' ? <OtherContents middleTable={props.middleTable}/> : null}
                </GoogleMap>
            </div>
    );
};

export const MiniMap = (props) => {
    return (
        <>
            <GoogleMap mapContainerClassName={`flex w-full h-full`} center={props.center} zoom={15}>
                <button onClick={props.handleSwapMap} className={`absolute bottom-[10px] left-[10px] flex justify-center items-center w-[40px] h-[40px] bg-white hover:bg-gray-200`}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path fillRule="evenodd" d="M15.97 2.47a.75.75 0 011.06 0l4.5 4.5a.75.75 0 010 1.06l-4.5 4.5a.75.75 0 11-1.06-1.06l3.22-3.22H7.5a.75.75 0 010-1.5h11.69l-3.22-3.22a.75.75 0 010-1.06zm-7.94 9a.75.75 0 010 1.06l-3.22 3.22H16.5a.75.75 0 010 1.5H4.81l3.22 3.22a.75.75 0 11-1.06 1.06l-4.5-4.5a.75.75 0 010-1.06l4.5-4.5a.75.75 0 011.06 0z" clipRule="evenodd" />
                    </svg>
                </button>
            </GoogleMap>
        </>
    );
}

export const Table = () => {
    const { droneMessage } = useContext(DroneContext);
    let droneState;
    let startTime;
    let completeTime;
    let takeTime;

    if (droneMessage !== null)
    {
        droneState = droneMessage['droneMessage'];

        startTime = droneMessage && new Date(droneState.DroneMission.StartTime).toLocaleTimeString('en-US', {hour12: false})
        completeTime = droneMessage && new Date(droneState.DroneMission.CompleteTime).toLocaleTimeString('en-US', {hour12: false})

        if ((completeTime-startTime) > 0) {
            takeTime = completeTime-startTime
        }
        else {
            takeTime = '00:00:00'
        }
    }

    return(
        <div className={`absolute top-[50px] right-[60px] rounded-xl bg-black opacity-70`}>
            <table className={`mx-3 my-2 text-lg text-[#00DCF8]`}>
                <tbody>
                <tr>
                    <th className={`px-2`}>전체 이동거리</th>
                    <td className={`px-2`}> {droneMessage && droneState.DroneTrack.TotalDistance} km</td>
                </tr>
                <tr>
                    <th className={`px-2`}>현재 비행거리</th>
                    <td className={`px-2`}> {droneMessage && droneState.DroneTrack.ElapsedDistance} km</td>
                </tr>
                <tr>
                    <th className={`px-2`}>잔여 이동거리</th>
                    <td className={`px-2`}> {droneMessage && droneState.DroneTrack.RemainDistance} km</td>
                </tr>
                <tr>
                    <th className={`px-2`}>현재 이동속도</th>
                    <td className={`px-2`}> {droneMessage && droneState.DroneStt.Speed} m/s</td>
                </tr>
                <tr>
                    <th className={`px-2`}>평균 이동속도</th>
                    <td className={`px-2`}> m/s</td>
                </tr>
                <tr>
                    <th className={`px-2`}>이륙 시작시간</th>
                    <td className={`px-2`}>{startTime}</td>
                </tr>
                <tr>
                    <th className={`px-2`}>비행 소요시간</th>
                    <td className={`px-2`}>{takeTime}</td>
                </tr>
                <tr>
                    <th className={`px-2`}>비행 완료시간</th>
                    <td className={`px-2`}>{completeTime}</td>
                </tr>
                </tbody>
            </table>
        </div>
    );
    // }
}