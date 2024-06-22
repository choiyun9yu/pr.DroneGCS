import {LeftPanel} from "../components/Dashboard/LeftPanel";
import {MiddlePanel} from "../components/Dashboard/MiddlePanel";
import {RightPanel} from "../components/Dashboard/RightPanel";
import {useEffect, useState} from "react";
import {WarningModal} from "../components/ProejctModal";

export const AppDashboard = () => {

    const [value, setValue] = useState(new Date());

    const [flightCount, setFlightCount] = useState();
    const [flightTime, setFlightTime] = useState();
    const [flightDistance, setFlightDistance] = useState();

    const [anomalyCount, setAnomalyCount] = useState();
    const [logCount, setLogCount] = useState();

    const [dailyFlightTime, setDailyFlightTime] = useState();
    const [dailyAnomalyCount, setDailyAnomalyCount] = useState();

    const [flightDay, setFlightDay] = useState();

    useEffect(() => {
        const fetchPost = async () => {
            try {
                const Body = new FormData();
                Body.append('year', value.getFullYear());
                Body.append('month', value.getMonth()+1);

                const response = await fetch('http://125.183.175.200:5000/api/dashboard', {
                    method: 'POST',
                    body: Body,
                });

                if (response.ok) {
                    const data = await response.json();
                    setFlightCount(data.flightCount);
                    setFlightTime(data.flightTime);
                    setFlightDistance(data.fligthDistance);
                    setAnomalyCount(data.anomlayCount);
                    setLogCount(data.logCount);
                    setDailyFlightTime(data.dailyFlightTime);
                    setDailyAnomalyCount(data.dailyAnomalyCount);
                    setFlightDay(data.flightDay)
                } else {
                    console.error('요청 실패');
                }
            } catch (error) {
                console.error('요청 중 오류 발생', error);
            }
        }

        fetchPost();
    }, []);

    return (
        <div id={'dash-board'} className={`flex flex-row w-full h-full p-5 text-white text-lg font-semibold`}>
            <LeftPanel
                flightCount={flightCount}
                flightTime={flightTime}
                flightDistance={flightDistance}
                anomalyCount={anomalyCount}
                logCount={logCount}
                dailyFlightTime={dailyFlightTime}
                dailyAnomalyCount={dailyAnomalyCount}
            />
            <MiddlePanel value={value} flightDay={flightDay}/>
            <RightPanel/>
            {/*<WarningModal/>*/}
        </div>
    );

};
