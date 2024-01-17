import React, {useContext, useState, useRef, useEffect} from "react";
import { GoogleMap, useJsApiLoader,  OverlayView, Polyline, Marker} from '@react-google-maps/api';

import { ColorThema } from '../ProejctThema';
import './GCSstyles.css';
import {FlightContents} from "./FlightMode";
import {OtherContents} from "./MissionMode";
import {DroneContext} from "./SignalRContainder";

export const MiddleMap = (props) => {
    const { droneMessage, handleDroneMarkerMission } = useContext(DroneContext);
    const droneState = droneMessage ? droneMessage['droneMessage'] : null;
    console.log(droneState)
    const droneLanded = droneMessage ? droneState.DroneStt.Landed : true;
    const dronePath = droneMessage ? droneState.DroneTrack.DroneTrails.q : [];
    const StartingPoint = droneMessage ? droneState.DroneMission.StartingPoint: null;
    const TargetPoint = droneMessage ? droneState.DroneMission.TargetPoint: null;

    const [isController, setIsController] = useState(true);
    const [isMarker, setIsMarker] = useState(false);
    const [isRtl, setIsRtl] = useState(false);

    const [monitorTable, setMonitorTable] = useState(true);

    const [makerPosition, setMarkerPosition] = useState({lat:null,lng:null});
    const [currentPosition, setCurrentPosition] = useState({lat:null,lng:null})

    const redMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png',
    };

    const yellowMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png',
    };

    const blueMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png',
    };

    const handleIsController= () => {
        setIsController(!isController)
    }

    const handleIsMarker = () => {
        setIsMarker(!isMarker)
    }

    const lastPathReset = () => {
        setCurrentPosition(null);
        setMarkerPosition(null);
    }

    const handleIsRtl = () => {
        setIsRtl(!isRtl);
        setCurrentPosition({
            lat: droneMessage && droneState.DroneStt.Lat,
            lng: droneMessage && droneState.DroneStt.Lon
        })
        setMarkerPosition(droneMessage ? droneState.DroneMission.StartingPoint: null);
    }

    const handleMapClick = event => {
        if (isMarker){
            setMarkerPosition({
                lat: event.latLng.lat(),
                lng: event.latLng.lng()
            });
            handleDroneMarkerMission(event.latLng.lat(), event.latLng.lng())
            setIsMarker(false)
        }
        setCurrentPosition({
            lat: droneMessage && droneState.DroneStt.Lat,
            lng: droneMessage && droneState.DroneStt.Lon
        })
    };

    // 드론 시작점 센터 기능과
    const handleStartPointCenter = () => {
        props.setCenter({
            lat: droneMessage && droneState.DroneMission.StartingPoint.lat,
            lng: droneMessage && droneState.DroneMission.StartingPoint.lng
        });
    }

    // 드론 도착점 센터 기능
    const handleTargetPointCenter = () => {
        props.setCenter({
            lat: droneMessage && droneState.DroneMission.TargetPoint.lat,
            lng: droneMessage && droneState.DroneMission.TargetPoint.lng
        });
    }

    const handleCurrentCenter= () => {
        props.setCenter({
            lat: droneMessage && droneState.DroneStt.Lat,
            lng: droneMessage && droneState.DroneStt.Lon
        });
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
            ? <div id='google-map' className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}></div>
            : <div id='google-map' className={`w-full h-full rounded-2xl ${ColorThema.Secondary4}`}>
                <GoogleMap
                    mapContainerClassName={`flex w-full h-full rounded-xl`}
                    center={props.center}
                    zoom={18}
                    onClick={handleMapClick}> {/* 마우스 클릭 이벤트 핸들러 추가 */}

                    {/* 드론 마커 */}
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
                                width: '50px',
                                height: '68px',
                                backgroundImage: `url(${process.env.PUBLIC_URL}/Drone.png)`,
                                backgroundSize: 'contain',
                                transform: `translate(-50%, -50%) rotate(${droneMessage && droneState.DroneStt.Head}deg)`,
                            }}
                        />
                    </OverlayView>


                    {/* 드론이 이동할 경로 */}
                    { !droneLanded ? <Polyline path={[currentPosition, makerPosition]} options={{ strokeColor: '#000000', strokeWeight: 1 }} /> : null}

                    {/* 드론이 이동한 경로 */}
                    <Polyline path={dronePath} options={{ strokeColor: '#BCBEC0', strokeWeight: 2 }} />

                    {/* 출발 지점 마커 */}
                    <Marker position={StartingPoint} icon={blueMarkerIcon}/>

                    {/* 목표 지점 마커 */}
                    <Marker position={TargetPoint} icon={redMarkerIcon} />

                    {props.gcsMode === 'flight' ?
                        <FlightContents isLeftPanel={props.isLeftPanel}
                                                                  handleIsLeftPanel={props.handleIsLeftPanel}
                                                                  isRightPanel={props.isRightPanel}
                                                                  handleIsRightPanel={props.handleIsRightPanel}
                                                                  isController={isController}
                                                                  handleIsController={handleIsController}
                                                                  handleSwapMap={props.handleSwapMap}
                                                                  middleTable={props.middleTable}
                                                                  isMarker={isMarker}
                                                                  handleIsMarker={handleIsMarker}
                                                                  handleIsRtl={handleIsRtl}
                                                                  lastPathReset={lastPathReset}
                                                                  monitorTable={monitorTable}
                                                                  setMonitorTable={setMonitorTable}
                        /> : null}
                    {props.gcsMode !== 'flight' ?
                        <OtherContents
                            middleTable={props.middleTable}
                            handleCurrentCenter={handleCurrentCenter}
                            handleStartPointCenter={handleStartPointCenter}
                            handleTargetPointCenter={handleTargetPointCenter}
                            lastPathReset={lastPathReset}
                            handleIsRtl={handleIsRtl}
                            monitorTable={monitorTable}
                            setMonitorTable={setMonitorTable}
                        /> : null}
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

export const Table = (props) => {
    const { droneMessage } = useContext(DroneContext);
    const monitorTable = props.monitorTable;

    let droneState;

    let startTime;
    let completeTime;
    let takeTime;
    let currentTime;

    let formattedStartTime;
    let formattedTakeTime;
    let formattedCompleteTime;

    if (droneMessage !== null) {
        droneState = droneMessage['droneMessage'];

        startTime = droneState.DroneMission.StartTime
            ? new Date(droneState.DroneMission.StartTime).getTime()
            : null;
        completeTime = droneState.DroneMission.CompleteTime
            ? new Date(droneState.DroneMission.CompleteTime).getTime()
            : null;

        if ((startTime !== null && completeTime === null) || completeTime < startTime) {
            currentTime = Date.now(); // 현재 시간을 밀리초로 얻음
            takeTime = currentTime - startTime;
            formattedTakeTime = formatTime(takeTime);
        } else {
            takeTime = completeTime - startTime;
            formattedTakeTime = formatTime(takeTime);
        }

        formattedStartTime = startTime
            ? new Date(startTime).toLocaleTimeString('en-US', {hour12: false})
            : '00:00:00';
        formattedCompleteTime = completeTime
            ? new Date(completeTime).toLocaleTimeString('en-US', {hour12: false})
            : '00:00:00';

        function formatTime(timeInMilliseconds) {
            const date = new Date(timeInMilliseconds);
            const hours = date.getUTCHours().toString().padStart(2, '0');
            const minutes = date.getUTCMinutes().toString().padStart(2, '0');
            const seconds = date.getUTCSeconds().toString().padStart(2, '0');

            return `${hours}:${minutes}:${seconds}`;
        }

    return(
        monitorTable
                ? (
                <div className={`absolute top-[50px] right-[60px] rounded-xl bg-black opacity-70`}>
                    <table className={`mx-3 my-2 text-lg text-[#00DCF8]`}>
                        <tbody>
                        <tr>
                            <th className={`px-2`}>전체 이동거리</th>
                            <td className={`px-2`}> {droneMessage && ((droneState.DroneTrack.TotalDistance)/1000).toFixed(3)} km</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>현재 비행거리</th>
                            <td className={`px-2`}> {droneMessage && ((droneState.DroneTrack.ElapsedDistance)/1000).toFixed(3)} km</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>잔여 이동거리</th>
                            <td className={`px-2`}> {droneMessage && ((droneState.DroneTrack.RemainDistance)/1000).toFixed(3)} km</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>현재 이동속도</th>
                            {/*<td className={`px-2`}> {droneMessage && (droneState.DroneStt.Speed).toFixed(3)} m/s</td>*/}
                            <td className={`px-2`}>{droneMessage && (droneState.DroneStt.Speed <= 0.020 ? 0 : droneState.DroneStt.Speed).toFixed(3)} m/s</td>
                        </tr>
                        <tr>
                            {/* 현재 비행 거리(킬로 미터) 나누기 비행 소요 시간(마이크로 초) -> 단위 맞추기*/}
                            <th className={`px-2`}>평균 이동속도</th>
                            <td className={`px-2`}>{(0).toFixed(3)} m/s</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>이륙 시작시간</th>
                            <td className={`px-2`}>{formattedStartTime}</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>비행 소요시간</th>
                            <td className={`px-2`}>{formattedTakeTime}</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>비행 완료시간</th>
                            <td className={`px-2`}>{formattedCompleteTime}</td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            )
            : null
    );
    }
}