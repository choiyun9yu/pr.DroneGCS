import React, { useEffect, useState } from 'react';
import { DataMap } from '../DataMap';
import { ColorThema } from '../ProejctThema';

export const PredictionForm = (props) => {
    const [drones, setDrones] = useState([]);
    const [flights, setFlights] = useState([]);

    const dependent_var = DataMap.dependent_var;

    const handleSubmit = async (event) => {
        event.preventDefault(); // 폼 제출 기본 동작을 막음
        const formData = new FormData(event.target); // 폼 데이터 수집
        try {
            const response = await fetch('http://localhost:5050/api/predict', {
                method: 'POST',
                body: formData, // 폼 데이터 전송
            });
            if (response.ok) {
                // console.log('요청 성공');
                const data = await response.json();
                // console.log(data)
                // 여기서 포를 한 번 떠서 올려주자 이제 배열이니까 맵 적용하면된다
                props.graphTransfer(
                    data['predictPage'].map((obj) => {
                        return {
                            PredictTime: obj['PredictTime'],
                            PredictData: obj['PredictData'],
                            SelectData: obj['SelectData'],
                        };
                    })
                );

                props.tableTransfer(
                    data['predictPage'].map((obj) => {
                        return {
                            DroneId: obj['DroneId'],
                            PredictTime: obj['PredictTime'],
                            PredictData: obj['PredictData'],
                            SelectData: obj['SelectData'],
                            SensorData: obj['SensorData'],
                        };
                    })
                );
            } else {
                console.error('요청 실패');
            }
        } catch (error) {
            console.error('요청 중 오류 발생', error);
        } finally {
            // 데이터 확인 (테스트용)
            const data = {};
            formData.forEach((value, key) => {
                data[key] = value;
            });
            // console.log('폼 데이터:', data)
        }
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('http://localhost:5050/api/predict', {
                    method: 'GET',
                });
                if (response.ok) {
                    // console.log('요청 성공');
                    const data = await response.json();
                    // console.log(data);
                    setDrones(data['drones']);
                    setFlights(data['flights']);
                } else {
                    console.error('요청 실패');
                }
            } catch (error) {
                console.error('요청 중 오류 발생', error);
            }
        };

        fetchData();
    }, []);

    return (
        <div id="predictiondropdown" className={`flex flex-col  w-full p-5 rounded-lg ${ColorThema.Secondary2}`}>
            <span className="mb-5 rounded-md font-bold text-medium text-white">• 부품 및 조회기간 선택</span>
            <form
                method="POST"
                action="http://localhost:5050/api/predict"
                onSubmit={handleSubmit}
                className="flex flex-row text-[#AEABD8]"
            >
                <div className="flex flex-col mr-5">
                    <span className="mb-5">✓ 드론 선택</span>
                    <select className="h-[30px] w-[175px] px-2 rounded text-gray-500 " name={'DroneId'}>
                        {drones.map((item, index) => (
                            <option value={item} key={index}>{item}</option>
                        ))}
                    </select>
                </div>
                <div className="flex flex-col mr-5">
                    <span className="mb-5">✓ 기간 선택</span>
                    <div className="flex flex-row items-center">
                        <input
                            className=" h-[30px] w-[175px] px-2 rounded text-gray-500"
                            name={'periodFrom'}
                            type={'date'}
                        ></input>
                        <span className="mr-3 ml-3"> ㅡ </span>
                        <input
                            className="h-[30px] w-[175px] px-2 rounded text-gray-500 "
                            name={'periodTo'}
                            type={'date'}
                        ></input>
                    </div>
                </div>
                <div className="flex flex-col mr-5">
                    <span className="mb-5">✓ 비행 로그 선택</span>
                    <select className="h-[30px] w-[175px] px-2 rounded text-gray-500 " name={'FlightId'}>
                        {flights.map((item, index) => (
                            <option value={item} key={index}>{item}</option>
                        ))}
                    </select>
                </div>
                <div className="flex flex-col mr-5">
                    <span className="mb-5">✓ 부품 선택</span>
                    <div>
                        <select className="h-[30px] w-[210px] px-2 rounded text-gray-500 " name={'SelectData'}>
                            {dependent_var.map((item, index) => {
                                return <option value={item} key={index}>{item}</option>;
                            })}
                        </select>
                    </div>
                </div>
                <div className="flex flex-col justify-end">
                    <div className="flex ">
                        <button
                            type="submit"
                            className={`flex items-center h-[30px] py-1 px-3  rounded border hover:bg-[#6359E9] border-[#6359E9] text-white ${ColorThema.Secondary5}`}
                        >
                            조회
                        </button>
                    </div>
                </div>
            </form>
        </div>
    );
};
