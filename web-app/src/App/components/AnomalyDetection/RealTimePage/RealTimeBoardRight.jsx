import {ColorThema} from "../../ProjectThema";
import React from "react";

export const RealTimeBoardRight = (props) => {

    function transformPropsObject(obj) {
        const transformedObject = {};

        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                const value = obj[key];
                if (typeof value === 'number') {
                    transformedObject[key] = value.toFixed(1);
                } else {
                    // 숫자가 아닌 경우 그대로 할당
                    transformedObject[key] = value;
                }
            }
        }

        return transformedObject;
    }

    const PredictData = transformPropsObject(props.PredictData || {});
    const RangeMax = transformPropsObject(props.RangeMax || {});
    const RangeMin = transformPropsObject(props.RangeMin || {});

    return(
        <div className="flex flex-col h-full w-full">
            <div className="flex  flex-col w-full h-full">
                <div className={`flex flex-col w-full h-full p-3 mb-4 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 font-bold text-medium">• 가속도 센서 </span>
                    <div className="flex flex-row justify-around h-full p-5">
                        <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">X축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.XaccRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{PredictData.XaccRAWIMUPREDICT}</span>
                            <span className="mx-auto">{RangeMin.xacc_RAW_IMU} ~ {RangeMax.xacc_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Y축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.YaccRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{PredictData.YaccRAWIMUPREDICT}</span>
                            <span className="mx-auto">{RangeMin.yacc_RAW_IMU} ~ {RangeMax.yacc_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Z축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.ZaccRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{PredictData.ZaccRAWIMUPREDICT}</span>
                            <span className="mx-auto">{RangeMin.zacc_RAW_IMU} ~ {RangeMax.zacc_RAW_IMU}</span>
                        </div>
                    </div>
                </div>

                <RealTimeBoardRightVib PredictData={PredictData} WarningData={props.WarningData} RangeMax={RangeMax} RangeMin={RangeMin}/>
            </div>
        </div>
    )
}

export const RealTimeBoardRightVib = (props) => {

    return (
        <div className={`flex flex-col w-full h-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
            <span className="text-white rounded-md ml-3 font-bold text-medium">• 진동 센서 </span>
            <div className="flex flex-row justify-around h-full p-5">
                <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">X축</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${props.WarningData.VibrationXVIBRATIONWARNING ? `text-[#FFFF00]` : 'text-[#8FE388]'}`}>{props.PredictData.VibrationXVIBRATIONPREDICT}</span>
                    <span className="mx-auto">{props.RangeMin.vibration_x_VIBRATION} ~ {props.RangeMax.vibration_x_VIBRATION}</span>
                </div>
                <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Y축</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${props.WarningData.VibrationYVIBRATIONWARNING ? `text-[#FFFF00]` : 'text-[#8FE388]'}`}>{props.PredictData.VibrationYVIBRATIONPREDICT}</span>
                    <span className="mx-auto">{props.RangeMin.vibration_y_VIBRATION} ~ {props.RangeMax.vibration_y_VIBRATION}</span>
                </div>
                <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Z축</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${props.WarningData.VibrationZVIBRATIONWARNING ? `text-[#FFFF00]` : 'text-[#8FE388]'}`}>{props.PredictData.VibrationZVIBRATIONPREDICT}</span>
                    <span className="mx-auto">{props.RangeMin.vibration_z_VIBRATION} ~ {props.RangeMax.vibration_z_VIBRATION}</span>
                </div>
            </div>
        </div>
    )
}