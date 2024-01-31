import React, {useContext, useState, useEffect} from "react";
import { GoogleMap, useJsApiLoader,  OverlayView, Polyline, Marker} from '@react-google-maps/api';

import {DroneContext} from "./SignalRContainer";
import { ColorThema } from '../ProejctThema';
import {FlightContents} from "./FlightMode";
import {AltitudeChart} from "./AltitudeChart";
import './GCSstyles.css';
import {MissionContents} from "./MissionMode";
import {VideoContent, VideoContents} from "./VideoMode";

export const MiddleMap = (props) => {
    const { droneMessage, handleDroneStartMarking} = useContext(DroneContext);
    const droneState = droneMessage ? droneMessage['droneMessage'] : null;
    const startPoint = droneMessage ? droneState.DroneMission.StartPoint: null;
    const [isController, setIsController] = useState(true);
    const [isMarker, setIsMarker] = useState(false);
    const [isRtl, setIsRtl] = useState(false);
    const [monitorTable, setMonitorTable] = useState(true);

    const [markerId, setMarkerId] = useState(1);
    const [pathLine, setPathLine] = useState([]);

    const dronePath = droneMessage ? droneState.DroneMission.DroneTrails.q : [];

    const [localPoint, setLocalPoint] = useState({lat:0,lng:0});

    const redMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png',
    };

    const yellowMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png',
    };

    const orangeMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/orange-dot.png',
    };

    const purpleMarkerIcon = {
        url: 'http://maps.google.com/mapfiles/ms/icons/purple-dot.png',
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

    const handleMarkerReset = () => {
        setMarkerId(1);
        props.setTargetPoints([]);
        setPathLine([]);
        setIsMarker(false);
        props.setFlightSchedule([]);
    }

    const handleIsRtl = () => {
        setIsRtl(!isRtl);
        setPathLine([{
            lat: droneMessage && droneState.DroneMission.StartPoint.lat,
            lng: droneMessage && droneState.DroneMission.StartPoint.lng
        }, startPoint])
    }

    const handleMapClick = e => {
        if (isMarker){
            props.setTargetPoints([...props.targetPoints, {id:props.markerId, position:{lat: e.latLng.lat(),lng: e.latLng.lng()}}])
            setMarkerId(props.markerId+1)
            setPathLine([startPoint, ...props.targetPoints.map(marker => marker.position)])
            handleDroneStartMarking(droneState.DroneStt.Lat, droneState.DroneStt.Lon)
        }

        if (props.isLocalMarker){
            props.setLocalLat(e.latLng.lat())
            props.setLocalLon(e.latLng.lng())
            setLocalPoint({
                lat: e.latLng.lat(),
                lng: e.latLng.lng()
            })
        }
        props.setIsLocalMarker(false)
    };

    useEffect(() => {
        setPathLine([startPoint, ...props.targetPoints.map(marker => marker.position)]);
    }, [props.targetPoints, startPoint])

    const handleStartPointCenter = () => {
        props.setCenter({
            lat: droneMessage && droneState.DroneMission.StartPoint.lat,
            lng: droneMessage && droneState.DroneMission.StartPoint.lng
        });
    }

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

    // Google Maps JavaScript API가 로드되지 않았을 때 아무것도 렌더링하지 않음
    if (!isLoaded) {
        return null;
    }


    return (
        props.swapMap
            ? <div id='google-map' className={`w-full h-full rounded-2xl bg-${ColorThema.Secondary4}`}></div>
            : <div id='google-map' className={`w-full h-full rounded-2xl bg-${ColorThema.Secondary4}`}>
                <GoogleMap
                    mapContainerClassName={`flex w-full h-full rounded-xl`}
                    center={props.center}
                    zoom={18}
                    onClick={handleMapClick}>

                    {/* 드론 PNG */}
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

                    { isRtl
                        ? <Marker position={{
                            lat: droneMessage && droneState.DroneStt.Lat,
                            lng: droneMessage && droneState.DroneStt.Lon
                        }} icon={yellowMarkerIcon}/>
                        : null
                    }

                    {props.targetPoints.map((marker) => (
                        <Marker key={marker.id} position={marker.position} icon={redMarkerIcon} />
                    ))}

                    <Polyline path={pathLine} options={{ strokeColor: '#FF3333', strokeWeight: 1}}/>

                    <Polyline path={dronePath} options={{ strokeColor: '#BCBEC0', strokeWeight: 2 }} />

                    <Marker position={localPoint} icon={purpleMarkerIcon}/>

                    <Marker position={startPoint} icon={blueMarkerIcon}/>

                    {props.isModalOpen && (
                        <AddNewLinkModal handleIsModal={props.handleIsModal}/>
                    )}

                    {props.gcsMode === 'flight'
                        ? <FlightContents
                            isLeftPanel={props.isLeftPanel}
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
                            // lastPathReset={lastPathReset}
                            monitorTable={monitorTable}
                            setMonitorTable={setMonitorTable}
                            handleMarkerReset={handleMarkerReset}
                            targetPoints= {props.targetPoints}/>
                        : null}

                    {props.gcsMode === 'mission'
                        ? <MissionContents
                           middleTable={props.middleTable}
                           handleCurrentCenter={handleCurrentCenter}
                           handleStartPointCenter={handleStartPointCenter}
                           handleTargetPointCenter={handleTargetPointCenter}
                           // lastPathReset={lastPathReset}
                           handleIsRtl={handleIsRtl}
                           monitorTable={monitorTable}
                           setMonitorTable={setMonitorTable}
                           handleIsWayPointBtn={props.handleIsWayPointBtn}
                           isMissionBtn={props.isMissionBtn}
                           handleIsMissionBtn={props.handleIsMissionBtn}/>
                        : null}

                    {props.gcsMode === 'video'
                        ? <VideoContents
                            middleTable={props.middleTable}
                            handleIsRtl={handleIsRtl}
                            monitorTable={monitorTable}
                            setMonitorTable={setMonitorTable}/>
                        :null}

                </GoogleMap>
            </div>
    );
};

const AddNewLinkModal = (props) => {
    // const [connectionType, setConnectionType] = useState();
    // const [connectionAddress, setConnectionAddress] = useState();

    // const handleConnectionType = (e) => {
    //     e.preventDefault();
    //     const { name, value } = e.target;
    //     setConnectionType(value);
    // }

    const addLinkModalSelectList = [
        "Communication Link",
        "TCP",
        "UDP",
        "SERIAL"
    ]

    const handleNewConnectionBtn = (e) => {
        e.preventDefault();
        const formData = new FormData(e.target);
    }


    return (
        <div className={`modal absolute top-[30%] left-[35%] w-[450px] h-[250px] rounded-2xl bg-${ColorThema.Primary1}`}>
            <div className={`flex flex-row pl-4 py-2 items-start justify-start text-2xl font-bold`}>
                <div className={`flex items-start justify-start`}>New Link Connection</div>

                <button className={"close flex ml-auto mr-3"} onClick={props.handleIsModal}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5}
                         stroke="#4B4B99" className="w-5 h-5">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M6 18 18 6M6 6l12 12"/>
                    </svg>
                </button>

            </div>
            <div className={"flex px-4 items-center "}>
                <div className={"modla-content"}>
                    <form className={`flex-col`}>
                        <div className={`my-3`}>
                            <select
                                className={`ml-2 px-1 w-[400px] h-[35px] rounded border border-gray-700`}
                                name={`connectionType`}
                                // onChange={handleConnectionType}
                            >
                                {addLinkModalSelectList.map((item, index) => (
                                    <option value={item} key={index}>{item}</option>
                                ))}
                            </select>
                        </div>

                        <div className={``}>
                            <span> Address </span>
                            <input
                                className={`ml-2 p-2 w-[400px] h-[35px] rounded border border-gray-700`}
                                type={'text'}
                                name={'connectionAddress'}
                            >

                            </input>
                        </div>

                        <div className={`flex mr-2 mt-5`}>
                            <button
                                type={'submit'} onClick={handleNewConnectionBtn}
                                className={`ml-auto py-1 px-4 rounded-md text-lg text-white bg-${ColorThema.Secondary4}`}
                            >
                                Add
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export const MiniMap = (props) => {
    return (
        <>
            <GoogleMap mapContainerClassName={`flex w-full h-full`} center={props.center} zoom={15}>
                <button onClick={props.handleSwapMap}
                        className={`absolute bottom-[10px] left-[10px] flex justify-center items-center w-[40px] h-[40px] bg-white hover:bg-gray-200`}>
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="w-6 h-6">
                        <path fillRule="evenodd" d="M15.97 2.47a.75.75 0 011.06 0l4.5 4.5a.75.75 0 010 1.06l-4.5 4.5a.75.75 0 11-1.06-1.06l3.22-3.22H7.5a.75.75 0 010-1.5h11.69l-3.22-3.22a.75.75 0 010-1.06zm-7.94 9a.75.75 0 010 1.06l-3.22 3.22H16.5a.75.75 0 010 1.5H4.81l3.22 3.22a.75.75 0 11-1.06 1.06l-4.5-4.5a.75.75 0 010-1.06l4.5-4.5a.75.75 0 011.06 0z" clipRule="evenodd" />
                    </svg>
                </button>
            </GoogleMap>
        </>
    );
}

export const Table = (props) => {
    const {droneMessage} = useContext(DroneContext);
    const monitorTable = props.monitorTable;

    let droneState;

    let averageSpeed;

    let totalDistance;
    let remainDistance;
    let elapsedDistance;

    let startTime;
    let completeTime;
    let takeTime;
    let currentTime;

    let formattedStartTime;
    let formattedTakeTime;
    let formattedCompleteTime;

    if (droneMessage !== null) {
        droneState = droneMessage['droneMessage'];

        totalDistance = (droneState.DroneMission.TotalDistance);
        elapsedDistance = (droneState.DroneMission.CurrentDistance);
        remainDistance = totalDistance - elapsedDistance;
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

        averageSpeed = (takeTime>0) && (elapsedDistance>0) ? elapsedDistance / (takeTime / 1000) : 0;

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
                            <td className={`px-2`}> {droneMessage && (totalDistance / 1000).toFixed(3)} km</td>
                            {/*<td className={`px-2`}> {} km</td>*/}
                        </tr>
                        <tr>
                            <th className={`px-2`}>잔여 이동거리</th>
                            <td className={`px-2`}> {droneMessage && (remainDistance / 1000).toFixed(3)} km</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>현재 비행거리</th>
                            <td className={`px-2`}> {droneMessage && (elapsedDistance / 1000).toFixed(3)} km</td>
                        </tr>

                        <tr>
                            <th className={`px-2`}>현재 이동속력</th>
                            <td className={`px-2`}>{droneMessage && (droneState.DroneStt.Speed).toFixed(3)} m/s</td>
                        </tr>
                        <tr>
                            <th className={`px-2`}>평균 이동속도</th>
                            <td className={`px-2`}>{((averageSpeed > 0) ? averageSpeed : 0).toFixed(3)} m/s</td>
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