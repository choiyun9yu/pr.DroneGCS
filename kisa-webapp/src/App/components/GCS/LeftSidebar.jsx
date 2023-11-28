import { useOutletContext } from 'react-router-dom';

import { ColorThema } from '../ProejctThema';
import { LeftSideBtn } from '../ProejctBtn';
import React, {useEffect, useState} from "react";

export const LeftSidebar = (props) => {
    const [gcsMode, setGcsMode] = useOutletContext();
    const [droneList, setDroneList] = useState([]);
    const [formData, setFormData] = useState({
        DroneId: ' ',
    });


    const handleSelectChange = async (e) => {
        const { name, value } = e.target;

        // 선택된 값이 변경될 때마다 로컬 스토리지에 저장
        // useState의 setter가 비동기로 상태 업데이트를 수행하는 경우 업데이트 이후의 값을 사용하려면 await로 기다려줘야 한다.
        await setFormData({
            [name]: value,
        });

        // POST 요청
        try {
            const Body = new FormData();
            Body.append([name], value);

            const response = await fetch('http://localhost:5000/api/drones', {
                method: 'POST',
                body: Body,
            });

            if (response.ok) {
                const data = await response.json();
                // console.log('요청 성공');

                // 데이터 포뜨기
                const transferData = {
                    rightTable : {
                        DroneId: data.droneId,
                        WayPointNum: data.droneRawState.WP_NO,
                        PowerV: data.droneRawState.POWER_V,
                        TempC: data.droneRawState.TEMPERATURE_C,
                        GpsStt: data.droneRawState.GPS_STATE,
                        HDOP: data.droneRawState.HDOP,
                        Lat: data.droneRawState.DR_LAT,
                        Lon: data.droneRawState.DR_LON,
                        Alt: data.droneRawState.DR_ALT,
                        Control: '???',
                    },
                    middleTable : {
                        TotalDistance: data.totalDistance,
                        ElapsedDistance: data.elapsedDistance,
                        RemainDistance: data.remainDistance,
                        DroneSpeed: data.droneRawState.DR_SPEED,
                        StartTime: data.startTime,
                        CompleteTime: data.completeTime,
                        TakenTime: '???',
                        DroneAvgSpeed: '???',
                    }
                }
                props.dataTransfer(transferData);

            } else {
                console.error('요청 실패');
            }
        } catch (error) {
            console.error('요청 중 오류 발생', error);
        }
    };


    // useEffect(() => {
    //     const fetchGet = async () => {
    //         try {
    //             const response = await fetch('http://localhost:5000/api/drones', {
    //                 method: 'GET',
    //             });
    //             if (response.ok) {
    //                 // console.log('요청 성공');
    //                 const data = await response.json();
    //                 setDroneList(data);
    //             } else {
    //                 console.error('요청 실패');
    //             }
    //         } catch (error) {
    //             console.error('요청 중 오류 발생', error);
    //         }
    //     };
    //
    //     fetchGet();
    // }, []);


    return (
        <div className={`flex flex-col items-start w-full h-full rounded-2xl font-bold text-medium text-gray-200 ${ColorThema.Secondary4}`}>
            <LeftSideBtn gcsMode={gcsMode} setGcsMode={setGcsMode} />
            <div className={`m-2 items-center `}>
                <button className={`flex flex-row w-full items-center ml-2 px-2 py-1 rounded-md ${ColorThema.Secondary3}`}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="mr-1 w-5 h-5">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M13.19 8.688a4.5 4.5 0 011.242 7.244l-4.5 4.5a4.5 4.5 0 01-6.364-6.364l1.757-1.757m13.35-.622l1.757-1.757a4.5 4.5 0 00-6.364-6.364l-4.5 4.5a4.5 4.5 0 001.242 7.244" />
                    </svg>
                    Add New Link
                </button>
            </div>

            <div className="w-full m-2 items-center">
                <span className="ml-3">• 등록 드론 </span>
                <form>
                    <label>
                        <select className="h-[30px] w-[130px] ml-5 mt-1 px-2 rounded-lg border border-gray-300  text-gray-400 bg-transparent font-normal"
                                name={'DroneId'} value={formData.DroneId} onChange={handleSelectChange}>
                            <option> 드론 선택 </option>
                            {droneList.map((item, index) => (
                                <option value={item} key={index}>{item}</option>
                            ))}
                        </select>
                    </label>
                </form>
            </div>

            <div className="w-full m-2 items-center">
                <span className="ml-3">• Way Point </span>
            </div>
        </div>
    );
};
