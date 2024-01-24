import React, {useContext, useState, useRef, useEffect} from "react";
import { GoogleMap, useJsApiLoader,  OverlayView, Polyline, Marker} from '@react-google-maps/api';
import {Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";

import { ColorThema } from '../ProejctThema';
import './GCSstyles.css';
import {FlightContents} from "./FlightMode";
import {DroneContext} from "./SignalRContainer";


export const MiddleMap = (props) => {
    const { droneMessage, handleDroneTargetMarking } = useContext(DroneContext);
    const droneState = droneMessage ? droneMessage['droneMessage'] : null;

    const dronePath = droneMessage ? droneState.DroneMission.DroneTrails.q : [];
    const StartingPoint = droneMessage ? droneState.DroneMission.StartingPoint: null;

    const TargetPoint = droneMessage ? droneState.DroneMission.TargetPoint: null;

    const [isController, setIsController] = useState(true);
    const [isMarker, setIsMarker] = useState(false);
    const [isRtl, setIsRtl] = useState(false);
    const [monitorTable, setMonitorTable] = useState(true);

    const [makerPosition, setMarkerPosition] = useState({lat:0,lng:0});
    const [currentPosition, setCurrentPosition] = useState({lat:0,lng:0})
    const [localPoint, setLocalPoint] = useState({lat:0,lng:0});

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
            handleDroneTargetMarking(event.latLng.lat(), event.latLng.lng())
            setIsMarker(false)
        }
        setCurrentPosition({
            lat: droneMessage && droneState.DroneStt.Lat,
            lng: droneMessage && droneState.DroneStt.Lon
        })

        if (props.isLocalMarker){
            props.setLocalLat(event.latLng.lat())
            props.setLocalLon(event.latLng.lng())
            setLocalPoint({
                lat: event.latLng.lat(),
                lng: event.latLng.lng()
            })
        }
        props.setIsLocalMarker(false)
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
                    <Polyline path={[currentPosition, makerPosition]} options={{ strokeColor: '#000000', strokeWeight: 1 }} />

                    {/* 드론이 이동한 경로 */}
                    <Polyline path={dronePath} options={{ strokeColor: '#BCBEC0', strokeWeight: 2 }} />

                    {/* 출발 지점 마커 */}
                    <Marker position={StartingPoint} icon={blueMarkerIcon}/>

                    {/* 목표 지점 마커 */}
                    <Marker position={TargetPoint} icon={redMarkerIcon} />

                    {/* 로컬 지점 마커 */}
                    <Marker position={localPoint} icon={yellowMarkerIcon}/>

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
                        <MissionContents
                            middleTable={props.middleTable}
                            handleCurrentCenter={handleCurrentCenter}
                            handleStartPointCenter={handleStartPointCenter}
                            handleTargetPointCenter={handleTargetPointCenter}
                            lastPathReset={lastPathReset}
                            handleIsRtl={handleIsRtl}
                            monitorTable={monitorTable}
                            setMonitorTable={setMonitorTable}
                            handleIsWayPointBtn={props.handleIsWayPointBtn}
                            isMissionBtn={props.isMissionBtn}
                            handleIsMissionBtn={props.handleIsMissionBtn}
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
    const haversine = require('haversine');
    const { droneMessage } = useContext(DroneContext);
    const monitorTable = props.monitorTable;

    let droneState;

    // let averageSpped;
    //
    // let totalDistance;
    // let remainDistance;
    // let elapsedDistance;

    let startTime;
    let completeTime;
    let takeTime;
    let currentTime;

    let formattedStartTime;
    let formattedTakeTime;
    let formattedCompleteTime;

    if (droneMessage !== null) {
        droneState = droneMessage['droneMessage'];

        // totalDistance = (droneState.DroneMission.TotalDistance);
        // remainDistance = (droneState.DroneMission.RemainDistance);
        // elapsedDistance = (totalDistance - remainDistance).toFixed(3);

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

        // averageSpped = takeTime > 0 ? elapsedDistance / (takeTime / 1000) : 0;

        // console.log(averageSpped)

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
                            {/*<td className={`px-2`}> {droneMessage && (totalDistance / 1000).toFixed(3)} km</td>*/}
                            <td className={`px-2`}> {} km</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>잔여 이동거리</th>
                            {/*<td className={`px-2`}> {droneMessage && (remainDistance / 1000).toFixed(3)} km</td>*/}
                            <td className={`px-2`}> {} km</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>현재 비행거리</th>
                            {/*<td className={`px-2`}> {droneMessage && (elapsedDistance / 1000).toFixed(3)} km</td>*/}
                            <td className={`px-2`}> {} km</td>
                        </tr>

                        <tr>
                            <th className={`px-2`}>현재 이동속력</th>
                            <td className={`px-2`}>{droneMessage && (droneState.DroneStt.Speed <= 0.020 ? 0 : droneState.DroneStt.Speed).toFixed(3)} m/s</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>평균 이동속도</th>
                            {/*<td className={`px-2`}>{((averageSpped > 0) ? averageSpped : 0).toFixed(3)} m/s</td>*/}
                            <td className={`px-2`}>{} m/s</td>
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

const MissionContents = (props) => {
    const {handleDroneFlightMode, handleDroneFlightCommand} = useContext(DroneContext);
    const [isCenterBtn, setIsCenterBtn] = useState(false);

    const handleCurrentCenter = () => {
        props.handleCurrentCenter();
        setIsCenterBtn(!isCenterBtn);
    }

    const handleStartPointCenter = () => {
        props.handleStartPointCenter();
        setIsCenterBtn(!isCenterBtn);
    }

    const handleTargetPointCenter = () => {
        props.handleTargetPointCenter();
        setIsCenterBtn(!isCenterBtn);
    }

    const handleIsCenterBtn = () => {
        setIsCenterBtn(!isCenterBtn);
    }

    const handleTakeoffBtn = async () => {
        handleDroneFlightMode(4)
        await waitOneSecond()
        handleDroneFlightCommand(0)
        handleDroneFlightCommand(2)
    }

    const handleLandBtn = async () => {
        handleDroneFlightMode(4)

        await waitOneSecond()

        handleDroneFlightCommand(3)
    }

    const handelReturnBtn = () => {
        props.handleIsRtl();
        handleDroneFlightMode(6)
    }

    function waitOneSecond() {
        return new Promise(resolve => setTimeout(resolve, 1000));
    }

    return (
        <>
            <div
                className={`absolute flex flex-col justify-around items-center left-[10px] top-[200px] w-[60px] h-[25%] text-xs rounded bg-white opacity-75`}>
                <div className={`flex text-lg mb-4 font-bold`}>
                    Plan
                </div>

                <button className={`flex flex-col items-center`} onClick={props.handleIsMissionBtn}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path fillRule="evenodd"
                              d="M5.625 1.5c-1.036 0-1.875.84-1.875 1.875v17.25c0 1.035.84 1.875 1.875 1.875h12.75c1.035 0 1.875-.84 1.875-1.875V12.75A3.75 3.75 0 0 0 16.5 9h-1.875a1.875 1.875 0 0 1-1.875-1.875V5.25A3.75 3.75 0 0 0 9 1.5H5.625ZM7.5 15a.75.75 0 0 1 .75-.75h7.5a.75.75 0 0 1 0 1.5h-7.5A.75.75 0 0 1 7.5 15Zm.75 2.25a.75.75 0 0 0 0 1.5H12a.75.75 0 0 0 0-1.5H8.25Z"
                              clipRule="evenodd"/>
                        <path
                            d="M12.971 1.816A5.23 5.23 0 0 1 14.25 5.25v1.875c0 .207.168.375.375.375H16.5a5.23 5.23 0 0 1 3.434 1.279 9.768 9.768 0 0 0-6.963-6.963Z"/>
                    </svg>
                    Mission
                </button>

                <button className={`flex flex-col items-center`} onClick={props.handleIsWayPointBtn}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path fillRule="evenodd"
                              d="M2.25 12c0-5.385 4.365-9.75 9.75-9.75s9.75 4.365 9.75 9.75-4.365 9.75-9.75 9.75S2.25 17.385 2.25 12Zm13.36-1.814a.75.75 0 1 0-1.22-.872l-3.236 4.53L9.53 12.22a.75.75 0 0 0-1.06 1.06l2.25 2.25a.75.75 0 0 0 1.14-.094l3.75-5.25Z"
                              clipRule="evenodd"/>
                    </svg>
                    Way Point
                </button>


                {/*<button className={`flex flex-col items-center`} onClick={handelReturnBtn}>*/}
                {/*    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">*/}
                {/*        <path fillRule="evenodd"*/}
                {/*              d="M15 3.75A5.25 5.25 0 009.75 9v10.19l4.72-4.72a.75.75 0 111.06 1.06l-6 6a.75.75 0 01-1.06 0l-6-6a.75.75 0 111.06-1.06l4.72 4.72V9a6.75 6.75 0 0113.5 0v3a.75.75 0 01-1.5 0V9c0-2.9-2.35-5.25-5.25-5.25z"*/}
                {/*              clipRule="evenodd"/>*/}
                {/*    </svg>*/}
                {/*    Return*/}
                {/*</button>*/}

                <button className={`flex flex-col items-center`} onClick={handleIsCenterBtn}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path
                            d="M6 3a3 3 0 00-3 3v1.5a.75.75 0 001.5 0V6A1.5 1.5 0 016 4.5h1.5a.75.75 0 000-1.5H6zM16.5 3a.75.75 0 000 1.5H18A1.5 1.5 0 0119.5 6v1.5a.75.75 0 001.5 0V6a3 3 0 00-3-3h-1.5zM12 8.25a3.75 3.75 0 100 7.5 3.75 3.75 0 000-7.5zM4.5 16.5a.75.75 0 00-1.5 0V18a3 3 0 003 3h1.5a.75.75 0 000-1.5H6A1.5 1.5 0 014.5 18v-1.5zM21 16.5a.75.75 0 00-1.5 0V18a1.5 1.5 0 01-1.5 1.5h-1.5a.75.75 0 000 1.5H18a3 3 0 003-3v-1.5z"/>
                    </svg>
                    Center
                </button>
            </div>

            {/*{props.isMissionBtn*/}
            {/*    ?(*/}
            {/*        <div className={`flex p-2 w-[170px170px] flex-col text-xs rounded-md`} style={{*/}
            {/*            position: 'absolute',*/}
            {/*            top: '210px',*/}
            {/*            left: '70px',*/}
            {/*            opacity: '70%',*/}
            {/*            background: '#ffff',*/}
            {/*        }}>*/}
            {/*            <div className={`flex flex-row justify-center`}>*/}
            {/*                <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"*/}
            {/*                         strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">*/}
            {/*                        <path strokeLinecap="round" strokeLinejoin="round"*/}
            {/*                              d="M15 10.5a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"/>*/}
            {/*                        <path strokeLinecap="round" strokeLinejoin="round"*/}
            {/*                              d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1 1 15 0Z"/>*/}
            {/*                    </svg>*/}
            {/*                    Starting*/}
            {/*                </button>*/}
            {/*                <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path fillRule="evenodd"*/}
            {/*                              d="m11.54 22.351.07.04.028.016a.76.76 0 0 0 .723 0l.028-.015.071-.041a16.975 16.975 0 0 0 1.144-.742 19.58 19.58 0 0 0 2.683-2.282c1.944-1.99 3.963-4.98 3.963-8.827a8.25 8.25 0 0 0-16.5 0c0 3.846 2.02 6.837 3.963 8.827a19.58 19.58 0 0 0 2.682 2.282 16.975 16.975 0 0 0 1.145.742ZM12 13.5a3 3 0 1 0 0-6 3 3 0 0 0 0 6Z"*/}
            {/*                              clipRule="evenodd"/>*/}
            {/*                    </svg>*/}
            {/*                    Targeting*/}
            {/*                </button>*/}
            {/*            </div>*/}

            {/*            <br/>*/}

            {/*            <div className={`flex flex-row justify-center`}>*/}
            {/*                <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path*/}
            {/*                            d="M9.195 18.44c1.25.714 2.805-.189 2.805-1.629v-2.34l6.945 3.968c1.25.715 2.805-.188 2.805-1.628V8.69c0-1.44-1.555-2.343-2.805-1.628L12 11.029v-2.34c0-1.44-1.555-2.343-2.805-1.628l-7.108 4.061c-1.26.72-1.26 2.536 0 3.256l7.108 4.061Z"/>*/}
            {/*                    </svg>*/}
            {/*                    Return*/}
            {/*                </button>*/}

            {/*                <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path fillRule="evenodd"*/}
            {/*                              d="M4.5 7.5a3 3 0 0 1 3-3h9a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9Z"*/}
            {/*                              clipRule="evenodd"/>*/}
            {/*                    </svg>*/}
            {/*                    Break*/}
            {/*                </button>*/}

            {/*                <button className={`flex flex-col items-center mx-auto w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path fillRule="evenodd"*/}
            {/*                              d="M4.5 5.653c0-1.427 1.529-2.33 2.779-1.643l11.54 6.347c1.295.712 1.295 2.573 0 3.286L7.28 19.99c-1.25.687-2.779-.217-2.779-1.643V5.653Z"*/}
            {/*                              clipRule="evenodd"/>*/}
            {/*                    </svg>*/}
            {/*                    Go To*/}
            {/*                </button>*/}
            {/*            </div>*/}

            {/*            <br/>*/}

            {/*            <div className={`flex flex-row justify-center`}>*/}
            {/*                <button className={`flex flex-col mx-auto items-center w-[50px] hover:text-[#AEABD8]`}*/}
            {/*                        onClick={() => handleTakeoffBtn()}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path*/}
            {/*                            d="M11.47 1.72a.75.75 0 0 1 1.06 0l3 3a.75.75 0 0 1-1.06 1.06l-1.72-1.72V7.5h-1.5V4.06L9.53 5.78a.75.75 0 0 1-1.06-1.06l3-3ZM11.25 7.5V15a.75.75 0 0 0 1.5 0V7.5h3.75a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9a3 3 0 0 1 3-3h3.75Z"/>*/}
            {/*                    </svg>*/}
            {/*                    Take Off*/}
            {/*                </button>*/}

            {/*                <button className={`flex flex-col mx-auto items-center w-[50px] hover:text-[#AEABD8]`}*/}
            {/*                        onClick={() => handleLandBtn()}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path*/}
            {/*                            d="M12 1.5a.75.75 0 0 1 .75.75V7.5h-1.5V2.25A.75.75 0 0 1 12 1.5ZM11.25 7.5v5.69l-1.72-1.72a.75.75 0 0 0-1.06 1.06l3 3a.75.75 0 0 0 1.06 0l3-3a.75.75 0 1 0-1.06-1.06l-1.72 1.72V7.5h3.75a3 3 0 0 1 3 3v9a3 3 0 0 1-3 3h-9a3 3 0 0 1-3-3v-9a3 3 0 0 1 3-3h3.75Z"/>*/}
            {/*                    </svg>*/}
            {/*                    Land*/}
            {/*                </button>*/}

            {/*                <button className={`flex flex-col mx-auto items-center w-[50px] hover:text-[#AEABD8]`}>*/}
            {/*                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor"*/}
            {/*                         className="w-6 h-6">*/}
            {/*                        <path fillRule="evenodd"*/}
            {/*                              d="M3.792 2.938A49.069 49.069 0 0 1 12 2.25c2.797 0 5.54.236 8.209.688a1.857 1.857 0 0 1 1.541 1.836v1.044a3 3 0 0 1-.879 2.121l-6.182 6.182a1.5 1.5 0 0 0-.439 1.061v2.927a3 3 0 0 1-1.658 2.684l-1.757.878A.75.75 0 0 1 9.75 21v-5.818a1.5 1.5 0 0 0-.44-1.06L3.13 7.938a3 3 0 0 1-.879-2.121V4.774c0-.897.64-1.683 1.542-1.836Z"*/}
            {/*                              clipRule="evenodd"/>*/}
            {/*                    </svg>*/}
            {/*                    Alt*/}
            {/*                </button>*/}
            {/*            </div>*/}


            {/*        </div>*/}
            {/*    )*/}
            {/*    : null}*/}

            {isCenterBtn
                ? (
                    <div className={`flex p-2 flex-col items-start rounded-md`} style={{
                        position: 'absolute',
                        top: '380px',
                        left: '70px',
                        opacity: '70%',
                        background: '#ffff',
                    }}>
                        <button className={`px-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]`}
                                onClick={handleCurrentCenter}>
                            Drone
                        </button>
                        <button className={`px-1 my-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]`}
                                onClick={handleStartPointCenter}>
                            Start Point</button>
                        <button className={`px-1 w-full rounded border border-gray-700 hover:bg-[#AEABD8]`} onClick={handleTargetPointCenter}>
                            Target Point</button>
                    </div>
                )
                : null}

            <Table className={'z-30'} middleTable={props.middleTable} monitorTable={props.monitorTable} setMonitorTable={props.setMonitorTable}/>

            {/* AltitudeChart */}
            <div className={`absolute left-2 bottom-6 w-[90%] h-[200px] rounded-xl bg-black opacity-70`}>
                <AltChart />
            </div>
        </>
    );
}

const AltChart = () => {
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