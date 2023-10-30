import React, { useEffect, useState } from 'react';
import { ColorThema } from '../../ProejctThema';

export const LogForm = (props) => {
    const [drones, setDrones] = useState([]);
    const [flights, setFlights] = useState([]);

    const handleSubmit2 = async (event) => {
        event.preventDefault(); // 폼 제출 기본 동작을 막음
        const formData = new FormData(event.target); // 폼 데이터 수집
        try {
            const response = await fetch('http://127.0.0.1:5050/api/logdata', {
                method: 'POST',
                body: formData, // 폼 데이터 전송
            });
            if (response.ok) {
                // console.log('요청 성공');
                const data = await response.json();
                // console.log(data);
                props.dataTransfer(data['logPage']);
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
                const response = await fetch('http://localhost:5050/api/logdata', {
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
        <div className={`flex flex-col w-full p-5 rounded-lg ${ColorThema.Secondary2}`}>
            <span className="rounded-md mb-5 font-bold text-medium text-white">• 부품 및 조회기간 선택</span>
            <form
                method="POST"
                action="http://127.0.0.1:5050/api/logdata"
                onSubmit={handleSubmit2}
                className="flex flex-row text-[#AEABD8] "
            >
                <div className="flex flex-col mr-5">
                    <span className="mb-5">✓ 드론 선택</span>
                    <select className="h-[30px] w-[175px] px-2  rounded  text-gray-500" name={'DroneId'}>
                        {drones.map((item, index) => (
                            <option value={item} key={index}>{item}</option>
                        ))}
                    </select>
                </div>
                <div className="flex flex-col mr-5">
                    <span className="mb-5">✓ 기간 선택</span>
                    <div className="flex flex-row items-center">
                        <input
                            className="h-[30px] w-[175px] px-2 rounded text-gray-500"
                            placeholder="yyyy-mm-dd"
                            name={'periodFrom'}
                            type={'date'}
                        ></input>
                        <span className="mr-3 ml-3"> ㅡ </span>
                        <input
                            className="h-[30px] w-[175px] px-2 rounded text-gray-500"
                            placeholder="yyyy-mm-dd"
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
                <div className="flex flex-col justify-end">
                    <button
                        type="submit"
                        className={`flex items-center h-[30px] py-1 px-3  rounded border hover:bg-[#6359E9] border-[#6359E9]  text-white ${ColorThema.Secondary5}`}
                    >
                        조회
                    </button>
                </div>
            </form>
        </div>
    );
};
