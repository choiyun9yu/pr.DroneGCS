import React, {useContext, useState} from 'react';

import {DataMap} from "../../DataMap";
import {RealTimeForm} from './RealTimeForm';
import {RealTimeBoardLeft} from "./RealTimeBoardLeft";
import {RealTimeBoardRight} from "./RealTimeBoardRight";
import {RealTimeTable} from "./RealTimeTable";
import {RealTimeGraph} from "./RealTimeGraph";
import {DroneContext} from "../../GCS/SignalRContainer";
import {ColorThema} from "../../ProjectThema";
import {setSelectionRange} from "@testing-library/user-event/dist/utils";

export const RealTime = () => {
    const { droneMessage, droneList, handleSelectedDrone, selectedDrone, setSelectedDrone } = useContext(DroneContext);
    const SensorData = droneMessage ? droneMessage.SensorData : DataMap.dependent_var;
    const PredictData = droneMessage ? droneMessage.PredictData : DataMap.PredictData;
    const WarningData = droneMessage ? droneMessage.WarningData : DataMap.WarningData;

    const RangeMax = filterRangeMax(SensorData)
    const RangeMin = filterRangeMin(SensorData)

    const handleSelectDrone = async (e) => {
        const { name, value } = e.target;
        setSelectedDrone(value)
        handleSelectedDrone(value);
        console.log(selectedDrone)
    }

    return (
            <div id="real-time" className={`flex flex-col w-full h-full mx-5 mb-5 rounded-lg font-normal text-[#8C89B4]`}>
                <RealTimeForm droneList={droneList} selectedDrone={selectedDrone} handleSelectDrone={handleSelectDrone}/>
                <div id="realtime-board" className="flex w-full h-full ">
                    <div className={`flex flex-row w-full h-full `}>
                        <div className="flex flex-col w-[59%]">
                            <div id="real-time-board-left" className="flex flex-col mr-4 h-full">
                                <RealTimeTable WarningData={WarningData}/>
                                <RealTimeBoardLeft PredictData={PredictData} WarningData={WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
                            </div>
                        </div>
                        <div className="flex flex-col w-[41%]">
                            <RealTimeGraph />
                            <RealTimeBoardRight PredictData={PredictData} WarningData={WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
                        </div>
                    </div>
                </div>
            </div>
    );
};

const filterRangeMax = (SensorData) => {
    if (SensorData === null) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const rangeMax = {};
    DataMap.dependent_var.forEach((key) => {
        rangeMax[key] = SensorData[key] + THRESHOLD[key]
    })
    return rangeMax
}

const filterRangeMin = (SensorData) => {
    if (SensorData === null) {return;}
    const THRESHOLD = DataMap.originalThreshold;
    const rangeMin = {};
    DataMap.dependent_var.forEach((key) => {
        rangeMin[key] = SensorData[key] - THRESHOLD[key]
    })
    return rangeMin
}
