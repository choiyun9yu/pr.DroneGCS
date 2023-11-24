import React, { useState, useEffect } from 'react';

export const RealTimeForm = (props) => {
    const [time, setTime] = useState(Date.now())
    const [drones, setDrones] = useState([]);
    const [formData, setFormData] = useState({
        DroneId: '12345678CD',
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

            const response = await fetch('http://localhost:5050/api/realtime', {
                method: 'POST',
                body: Body,
            });

            if (response.ok) {
                const data = await response.json();
                // console.log('요청 성공');
                props.dataTransfer(data['realtimePage']);
            } else {
                console.error('요청 실패');
            }
        } catch (error) {
            console.error('요청 중 오류 발생', error);
        }
    };

    useEffect(() => {
        const intervalId = setInterval(() => {
            // 1초마다 현재 시간 업데이트
            setTime(Date.now());
        }, 1000)

        // 컴포넌트가 언마운트되면 clearInterval을 사용하여 인터벌 정리
        return () => clearInterval(intervalId);
    }, []);

    useEffect(() => {
        const fetchGet = async () => {
            try {
                const response = await fetch('http://localhost:5050/api/realtime', {
                    method: 'GET',
                });

                if (response.ok) {
                    const data = await response.json();
                    // console.log(`요청 성공 ${time}`);
                    // console.log(data);
                    setDrones(data['drones']);
                } else {
                    console.error('요청 실패');
                }
            } catch (error) {
                console.error('요청 중 오류 발생', error);
            }
        };

        fetchGet();
    }, []);

    useEffect(() => {
        const fetchPost = async () => {
            // POST 요청
            try {
                const Body = new FormData();
                Body.append('DroneId', formData["DroneId"]);

                const response = await fetch('http://localhost:5050/api/realtime', {
                    method: 'POST',
                    body: Body,
                });

                if (response.ok) {
                    const data = await response.json();
                    // console.log('요청 성공');
                    props.dataTransfer(data['realtimePage']);
                } else {
                    console.error('요청 실패');
                }
            } catch (error) {
                console.error('요청 중 오류 발생', error);
            }
        }

        fetchPost()
    }, [time, formData, props])

    return (
        <div id='realtime-form' className="flex ml-3 mb-5">
            <div className="flex flex-col mr-5 w-full ">
                <form>
                    <label>
                        <select className="h-[35px] w-[135px] px-2 rounded-lg border border-gray-300  text-gray-400 bg-transparent"
                            name={'DroneId'} value={formData.DroneId} onChange={handleSelectChange}>
                            {/*<option> 드론 선택 </option>*/}
                            {drones.map((item, index) => (
                                <option value={item} key={index}>{item}</option>
                            ))}
                        </select>
                    </label>
                </form>
            </div>
        </div>
    );
};
