import { LineChart, Legend, XAxis, YAxis, Tooltip, Line, ResponsiveContainer } from 'recharts'
import { ColorThema } from '../ProejctThema';
import {Table} from "./MiddleMap";
import React, {useContext, useState, useEffect} from "react";
import {DroneContext} from "./SignalRContainder";

export const MissionMode = (props) => {
    const [pointsList, setPointsList] = useState([]);
    const [transitCount, setTransitCount] = useState(0);
    const [directInput, setDirectInput] = useState(true);
    const [flightAlt, setFlightAlt] = useState(10);
    const [altScale, setAltScale] = useState(1);

    const handleAltUp = () => {
        setFlightAlt(flightAlt+(1*altScale))
    }

    const handleAltDown = () => {
        if (flightAlt > 1) {
            setFlightAlt(flightAlt-(1*altScale))
        }
    }

    const handleAltScale = () => {
        if (altScale === 1) {
            setAltScale(10);
        }
        else if (altScale === 10) {
            setAltScale(1);
        }
    }

    const handleTransitUp = () => {
        if (transitCount<9){
            setTransitCount(transitCount+1);
        }
    }

    const handleTransitDown = () => {
        if (transitCount > 0) {
            setTransitCount(transitCount-1)
        }
    }

    const handleCurrentPoint = () => {
        props.handleCurrentPoint();
        if (directInput) {
            setDirectInput(false)
        }
        if (props.isLocalMarker){
            props.handleIsLocalMarker();
        }
    }

    const handleIsLocalMarker = () => {
        props.handleIsLocalMarker();
        if (directInput) {
            setDirectInput(false)
        }
    }

    const handleDirectInput = () => {
        setDirectInput(!directInput);
        if (props.isLocalMarker){
            props.handleIsLocalMarker();
        }
    }

    const handleCreateMission = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.target);

        try {
            const response = await fetch('http://localhost:5000/api/createmission', {
                method: 'POST',
                body: formData,
            });
            if (response.ok) {
                // console.log('요청 성공');
            } else {
                console.error('요청 실패');
            }
        } catch (error) {
            console.error('요청 중 오류 발생', error);
        } finally {
            const data = {};
            formData.forEach((value, key) => {
                data[key] = value;
            });
            // console.log('폼 데이터:', data)
        }
    }

    const handleDeletePoint = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.target);

        try {
            const response = await fetch('http://localhost:5000/api/deletelocalpoint', {
                method: 'DELETE',
                body: formData,
            });
            if (response.ok) {
                // console.log('요청 성공');
            } else {
                console.error('요청 실패');
            }
        } catch (error) {
                console.error('요청 중 오류 발생', error);
        } finally {
            const data = {};
            formData.forEach((value, key) => {
                data[key] = value;
            });
            // console.log('폼 데이터:', data)
        }
    };

    const handleEnrollPoint = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.target);

        try {
            const response = await fetch('http://localhost:5000/api/addwaypoint', {
                method: 'POST',
                body: formData,
            });
            if (response.ok) {
                // console.log('요청 성공');
            } else {
                console.error('요청 실패');
            }
        } catch (error) {
            console.error('요청 중 오류 발생', error);
        } finally {
            const data = {};
            formData.forEach((value, key) => {
                data[key] = value;
            });
            // console.log('폼 데이터:', data)
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('http://localhost:5000/api/localpoints', {
                    method: 'GET',
                });
                if (response.ok) {
                    // console.log('요청 성공');
                    const data = await response.json();
                    // console.log(data);
                    setPointsList(data['localPointList']);
                } else {
                    console.error('요청 실패');
                }
            } catch (error) {
                console.error('요청 중 오류 발생', error);
            }
        };
        fetchData()
    }, [handleEnrollPoint, handleDeletePoint])

    return (
        <div id="right-sidebar" className="flex flex-col w-[300px]">
            <div className={`flex flex-col w-full h-full overflow-auto rounded-2xl ${ColorThema.Secondary4}`}>
                <div className="flex m-2 items-center">
                    <span className="text-white rounded-md m-3 font-bold text-medium">• 드론 미션</span>
                </div>
                {props.isMissionBtn
                    ?(
                        <div className={`m-2 text-white`}>
                            <form id={'missionload'}>
                                <div className={`font-bold`}>
                                    미션 불러 오기
                                </div>
                                <div className={`m-2`}>
                                    <div className={`flex items-center`}>
                                        <span>미션 선택 : </span>
                                        <select
                                            className={`flex m-1 w-[170px] h-[23px] text-black px-2`}
                                            name={'MissionName'}>
                                            {/*{pointsList.map((item, index) => (*/}
                                            {/*    <option*/}
                                            {/*        value={item} key={index}>{item}</option>*/}
                                            {/*))}*/}
                                        </select>
                                    </div>

                                    <div className={`flex flex-col mx-3 text-gray-400`}>
                                        <span>출발 지점 : {}</span>
                                        <span>경유 지점 : {}</span>
                                        <span>목표 지점 : {}</span>
                                        <span>비행 고도 : {}</span>
                                        <span>예상 비행 거리 : {} km</span>
                                        <span>예상 소요 시간  : {} 분</span>
                                    </div>
                                </div>

                                <div className={`flex justify-end mx-5 mt-2`}>
                                    <button
                                        // onSubmit={}
                                        className={`flex px-2 rounded-xl border hover:bg-[#6359E9]`}>
                                        시작
                                    </button>
                                </div>
                            </form>

                            <form id={'missionenroll'} className={`mt-5`}
                                  onSubmit={handleCreateMission}>
                                <div className={`font-bold`}>
                                    미션 생성 하기
                                </div>

                                <div className={`flex flex-col m-2`}>
                                    <div className={`flex items-center`}>
                                        <span className={`mr-2`}>출발 지점</span>
                                        :
                                        <select
                                            className={`flex m-1 w-[170px] h-[23px] text-black px-2`}
                                            name={'StartPoint'}>
                                            {pointsList.map((item, index) => (
                                                <option
                                                    value={item} key={index}>{item}</option>
                                            ))}
                                        </select>
                                    </div>

                                    <TransitInput transitCount={transitCount} pointsList={pointsList}/>

                                    <div className={`flex items-center`}>
                                        <span className={`mr-2`}>목표 지점 </span>
                                        :
                                        <select
                                            className={`flex m-1 w-[170px] h-[23px] text-black px-2`}
                                            name={'TargetPoint'}>
                                            {pointsList.map((item, index) => (
                                                <option
                                                    value={item} key={index}>{item}</option>
                                            ))}
                                        </select>
                                    </div>

                                    <div className={`flex items-center`}>
                                        <span className={`mr-2`}>비행 고도</span>
                                        :
                                        <input
                                            className={`m-1 w-[45px] text-black px-2`}
                                            name={'FlightAlt'}
                                            type={'text'}
                                            value={flightAlt}
                                            placeholder={'비행 고도를 입력하세요'}>
                                        </input>

                                        <div className={`flex`}>
                                            <button type="button" onClick={handleAltUp} className={`flex justify-center ml-2 w-[30px] h-full px-2 rounded-xl border hover:bg-[#6359E9]`}>
                                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                                                     strokeWidth={1.5} stroke="currentColor" className="w-4 h-6">
                                                    <path strokeLinecap="round" strokeLinejoin="round"
                                                          d="m4.5 15.75 7.5-7.5 7.5 7.5"/>
                                                </svg>
                                            </button>
                                            <button type="button" onClick={handleAltDown} className={`flex justify-center ml-1 w-[30px] h-full px-2 rounded-xl border hover:bg-[#6359E9]`}>
                                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                                                     strokeWidth={1.5} stroke="currentColor" className="w-4 h-6">
                                                    <path strokeLinecap="round" strokeLinejoin="round"
                                                          d="m19.5 8.25-7.5 7.5-7.5-7.5"/>
                                                </svg>
                                            </button>

                                            {(altScale === 1)
                                                ? (
                                                    <button type="button" onClick={handleAltScale} className={`flex justify-center ml-1 w-[40px] h-full px-2 rounded-xl border hover:bg-[#6359E9]`}>
                                                        x10
                                                    </button>
                                                )
                                                : (
                                                    <button type="button" onClick={handleAltScale} className={`flex justify-center ml-1 w-[40px] h-full px-2 rounded-xl border bg-[#6359E9]`}>
                                                        x10
                                                    </button>
                                                )
                                            }
                                        </div>

                                    </div>

                                    <div className={`flex justify-end mx-3 mt-2`}>
                                        <div className={`flex flex-row mr-3 pl-2 rounded-xl border`}>
                                            경유지
                                            <button type="button" onClick={handleTransitUp} className={`flex px-1 hover:text-[#6359E9]`}>
                                                추가
                                            </button>
                                            |
                                            <button type="button" onClick={handleTransitDown} className={`flex mr-2 pl-1 hover:text-[#6359E9]`}>
                                                삭제
                                            </button>
                                        </div>

                                        <button
                                            className={`flex px-2 rounded-xl border hover:bg-[#6359E9]`}>
                                            생성
                                        </button>
                                    </div>
                                </div>

                            </form>
                        </div>
                    )
                    : null
                }

                {props.isWayPoint
                    ? (
                        <div className={`m-2 text-white mt-5`}>
                            <form id={`waypointadd`}
                                  onSubmit={handleEnrollPoint}>
                                <div className={`font-bold`}>
                                    지점 추가 하기
                                </div>

                                <div className={`m-2`}>
                                    <div>
                                        <span>지점 이름 : </span>
                                        <input
                                            className={`m-1 w-[170px] text-black px-2`}
                                            name={'LocalName'}
                                            type={'text'}
                                            placeholder={`지점 이름을 임력하세요`}>
                                        </input>
                                    </div>
                                    {directInput
                                    ?(
                                        <>
                                            <div>
                                                <span>지점 위도 : </span>
                                                <input
                                                    className={`m-1 w-[170px] text-black px-2`}
                                                    name={'LocalLat'}
                                                    type={'text'}
                                                    placeholder={'위도를 입력하세요'}>
                                                </input>
                                            </div>

                                            <div>
                                                <span>지점 경도 : </span>
                                                <input
                                                    className={`m-1 w-[170px] text-black px-2`}
                                                    name={'LocalLon'}
                                                    type={'text'}
                                                    placeholder={'경도를 입력하세요'}>
                                                </input>
                                            </div>
                                        </>
                                    )
                                    : (<>
                                            <div>
                                                <span>지점 위도 : </span>
                                                <input
                                                    className={`m-1 w-[170px] text-black px-2`}
                                                    name={'LocalLat'}
                                                    type={'text'}
                                                    value={`${props.localLat ?? ""}`}
                                                    placeholder={'위도를 입력하세요'}>
                                                </input>
                                            </div>

                                            <div>
                                                <span>지점 경도 : </span>
                                                <input
                                                    className={`m-1 w-[170px] text-black px-2`}
                                                    name={'LocalLon'}
                                                    type={'text'}
                                                    value={`${props.localLon ?? ""}`}
                                                    placeholder={'경도를 입력하세요'}>
                                                </input>
                                            </div>
                                        </>
                                    )
                                }
                            </div>

                            <div className={`flex flex-col px-2 mr-2`}>
                                <div className={`flex flex-row justify-end`}>
                                    <button
                                        onClick={handleCurrentPoint}
                                        className={`flex px-1 rounded-xl border hover:bg-[#6359E9]`}>
                                        현재 위치
                                    </button>

                                    {props.isLocalMarker
                                        ? (<button
                                            onClick={handleIsLocalMarker}
                                            className={`flex mx-2 px-1 rounded-xl border bg-[#6359E9]`}>
                                            마커 위치
                                        </button>)
                                        : (<button
                                            onClick={handleIsLocalMarker}
                                            className={`flex mx-2  px-1 rounded-xl border hover:bg-[#6359E9]`}>
                                            마커 위치
                                        </button>)
                                        }

                                        {directInput
                                            ? (<button
                                                onClick={handleDirectInput}
                                                className={`flex px-1 rounded-xl border hover:bg-[#6359E9]`}>
                                                직접 입력
                                            </button>)
                                            : (<button
                                                onClick={handleDirectInput}
                                                className={`flex px-1 rounded-xl border hover:bg-[#6359E9]`}>
                                                직접 입력
                                            </button>)
                                        }
                                    </div>

                                    <div className={`flex justify-end mt-2`}>
                                        <button className={`flex px-2 rounded-xl border hover:bg-[#6359E9]`}>
                                        추가
                                        </button>
                                    </div>
                                </div>
                            </form>

                            <form id={`waypointdelete`} className={`mt-5`}
                                  onSubmit={handleDeletePoint}>
                                <div className={`font-bold`}>
                                    지점 삭제 하기
                                </div>
                                <div className={`flex flex-row justify-end px-2 m-2`}>
                                    <div>
                                        <select
                                            className={`flex m-1 w-[170px] h-[23px] text-black px-2`}
                                            name={'LocalName'}>
                                            {pointsList.map((item, index) => (
                                                <option
                                                    value={item} key={index}>{item}</option>
                                            ))}
                                        </select>
                                    </div>
                                    <div className={`flex items-end ml-3`}>
                                        <button
                                            // onClick={}
                                            className={`flex px-2 mb-0.5 rounded-xl border hover:bg-[#6359E9]`}>
                                            삭제
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    )
                    : null}

            </div>
        </div>
    );
};

const TransitInput = (props) => {
    const transitElements = [];

    for (let i=0; i < props.transitCount; i++) {
        transitElements.push(
            <div className={`flex flex-row items-center`} key={i}>
                <span>경유지 ({i+1}) : </span>
                <select
                    className={`flex m-1 w-[170px] h-[23px] text-black px-2`}
                    name={`TransitPoint${i+1}`}>
                    {props.pointsList.map((item, index) => (
                        <option value={item} key={index}>{item}</option>
                    ))}
                </select>
            </div>
        )
    }

    return (
        <div>
            {transitElements}
        </div>
    )
}


export const OtherContents = (props) => {
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
                        top: '330px',
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
                <AltitudeChart />
            </div>
        </>
    );
}

export const AltitudeChart = () => {
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