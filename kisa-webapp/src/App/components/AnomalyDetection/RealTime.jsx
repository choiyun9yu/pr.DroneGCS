import React, {useState} from 'react';

import {DataMap} from "../DataMap";
import {RealTimeForm} from './RealTimeForm';
import {RealTimeBoardLeft} from "./RealTimeBoardLeft";
import {RealTimeBoardRight} from "./RealtimeBoardRight";
import {RealTimeTable} from "./RealTimeTable";
import {RealTimeGraph} from "./RealTimeGraph";

export const RealTime = () => {
    const [realTimeData, setRealTimeData] = useState({})


    const dataTransfer = (data) => {
        setRealTimeData(data)
    }

    const graphData = realTimeData.Alt // 드론 고도와 관련해서 시간 범위를 정하는(x축의 범위를 정하는 기준이 있어야 할듯
    const WarningData = filterWarningData(realTimeData.PredictData, realTimeData.SensorData)
    const RangeMax = filterRangeMax(realTimeData.SensorData)
    const RangeMin = filterRangeMin(realTimeData.SensorData)

    return (
            <div id="real-time" className={`flex flex-col w-full h-full mx-5 mb-5 rounded-lg font-normal text-[#8C89B4] `}>
                <RealTimeForm dataTransfer={dataTransfer} />
                <div id="realtime-board" className="flex w-full h-full ">
                    <div className={`flex flex-row w-full h-full `}>
                        <div className="flex flex-col w-[59%]">
                            <div id="real-time-board-left" className="flex flex-col mr-4 h-full">
                                <RealTimeTable WarningData={WarningData}/>
                                <RealTimeBoardLeft PredictData={realTimeData.PredictData} WarningData={WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
                            </div>
                        </div>
                        <div className="flex flex-col w-[41%]">
                            <RealTimeGraph graphData={graphData}/>
                            <RealTimeBoardRight PredictData={realTimeData.PredictData} WarningData={WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
                        </div>
                    </div>
                </div>
            </div>
    );
};

const filterWarningData = (PredictData, SensorData) => {
    if (PredictData === undefined) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const warningData = {};
    DataMap.dependent_var.forEach((key) => {
        const predictKey = `${key}_PREDICT`;
        const diff = Math.abs(PredictData[predictKey] - SensorData[key]);
        warningData[key] = diff <= THRESHOLD[key];
    });

    return warningData;
};

const filterRangeMax = (SensorData) => {
    if (SensorData === undefined) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const rangeMax = {};
    DataMap.dependent_var.forEach((key) => {
        rangeMax[key] = SensorData[key] + THRESHOLD[key]
    })
    return rangeMax
}

const filterRangeMin = (SensorData) => {
    if (SensorData === undefined) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const rangeMin = {};
    DataMap.dependent_var.forEach((key) => {
        rangeMin[key] = SensorData[key] - THRESHOLD[key]
    })
    return rangeMin
}
