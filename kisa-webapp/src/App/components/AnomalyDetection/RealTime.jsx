import React, {useContext, useState} from 'react';

import {DataMap} from "../DataMap";
import {RealTimeForm} from './RealTimeForm';
import {RealTimeBoardLeft} from "./RealTimeBoardLeft";
import {RealTimeBoardRight} from "./RealTimeBoardRight";
import {RealTimeTable} from "./RealTimeTable";
import {RealTimeGraph} from "./RealTimeGraph";
import {DroneContext} from "../GCS/SignalRContainer";

export const RealTime = () => {
    const [realTimeData, setRealTimeData] = useState({})
    // const {droneMessage} = useContext(DroneContext);
    // const DroneState = droneMessage ? droneMessage['droneMessage'] : null;

    const dataTransfer = (data) => {
        setRealTimeData(data)
    }


    const WarningData = filterWarningData(realTimeData.predictData, realTimeData.sensorData)
    const RangeMax = filterRangeMax(realTimeData.predictData)
    const RangeMin = filterRangeMin(realTimeData.predictData)
    // const graphData = realTimeData.Alt

    return (
            <div id="real-time" className={`flex flex-col w-full h-full mx-5 mb-5 rounded-lg font-normal text-[#8C89B4] `}>
                <RealTimeForm dataTransfer={dataTransfer} />
                <div id="realtime-board" className="flex w-full h-full ">
                    <div className={`flex flex-row w-full h-full `}>
                        <div className="flex flex-col w-[59%]">
                            <div id="real-time-board-left" className="flex flex-col mr-4 h-full">
                                <RealTimeTable WarningData={WarningData}/>
                                <RealTimeBoardLeft SensorData={realTimeData.sensorData} WarningData={WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
                            </div>
                        </div>
                        <div className="flex flex-col w-[41%]">
                            {/*<RealTimeGraph graphData={graphData}/>*/}
                            <RealTimeGraph />
                            <RealTimeBoardRight SensorData={realTimeData.sensorData} WarningData={WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
                        </div>
                    </div>
                </div>
            </div>
    );
};

const filterWarningData = (PredictData, SensorData) => {
    if (PredictData === undefined) return;

    const THRESHOLD = DataMap.originalThreshold;
    const warningData = {};

    DataMap.dependent_var.forEach((key) => {
        const predictKey = `${key}_PREDICT`;
        const diff = Math.abs(PredictData[predictKey] - SensorData[key]);
        warningData[key] = diff <= THRESHOLD[key];
    });

    return warningData;
};

const filterRangeMax = (PredictData) => {
    if (PredictData === undefined) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const rangeMax = {};
    DataMap.dependent_var.forEach((key) => {
        const predictKey = `${key}_PREDICT`;
        rangeMax[key] = PredictData[predictKey] + THRESHOLD[key]
    })
    return rangeMax
}

const filterRangeMin = (PredictData) => {
    if (PredictData === undefined) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const rangeMin = {};
    DataMap.dependent_var.forEach((key) => {
        const predictKey = `${key}_PREDICT`;
        rangeMin[key] = PredictData[predictKey] - THRESHOLD[key]
    })
    return rangeMin
}
