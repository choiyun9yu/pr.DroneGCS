import {ColorThema} from "../../ProejctThema";
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

    const SensorData = transformPropsObject(props.SensorData || {});
    const RangeMax = transformPropsObject(props.RangeMax || {});
    const RangeMin = transformPropsObject(props.RangeMin || {});
    const WarningData = transformPropsObject(props.WarningData || {});

    return(
        <div className="flex flex-col h-full w-full">
            <div className="flex  flex-col w-full h-full">
                <div className={`flex flex-col w-full h-full p-3 mb-4 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 font-bold text-medium">• 가속도 센서 </span>
                    <div className="flex flex-row justify-around h-full p-5">
                        <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">X축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.xacc_RAW_IMU?`text-[#8FE388]`:'text-[#FFFF00]'}`}>{SensorData.xacc_RAW_IMU}</span>
                            <span className="mx-auto">{RangeMin.xacc_RAW_IMU} ~ {RangeMax.xacc_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Y축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.yacc_RAW_IMU?`text-[#8FE388]`:'text-[#FFFF00]'}`}>{SensorData.yacc_RAW_IMU}</span>
                            <span className="mx-auto">{RangeMin.yacc_RAW_IMU} ~ {RangeMax.yacc_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Z축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.acc_RAW_IMU?`text-[#8FE388]`:'text-[#FFFF00]'}`}>{SensorData.zacc_RAW_IMU}</span>
                            <span className="mx-auto">{RangeMin.zacc_RAW_IMU} ~ {RangeMax.zacc_RAW_IMU}</span>
                        </div>
                    </div>
                </div>

                <RealTimeBoardRightVib SensorData={props.SensorData} WarningData={props.WarningData} RangeMax={props.RangeMax} RangeMin={props.RangeMin}/>
            </div>
        </div>
    )
}

export const RealTimeBoardRightVib = (props) => {

    function transformPropsObject(obj) {
        const transformedObject = {};

        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                const value = obj[key];
                if (typeof value === 'number') {
                    transformedObject[key] = value.toFixed(4);
                } else {
                    // 숫자가 아닌 경우 그대로 할당
                    transformedObject[key] = value;
                }
            }
        }

        return transformedObject;
    }

    const SensorData = transformPropsObject(props.SensorData || {});
    const RangeMax = transformPropsObject(props.RangeMax || {});
    const RangeMin = transformPropsObject(props.RangeMin || {});
    const WarningData = transformPropsObject(props.WarningData || {});

    return (
        <div className={`flex flex-col w-full h-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
            <span className="text-white rounded-md ml-3 font-bold text-medium">• 진동 센서 </span>
            <div className="flex flex-row justify-around h-full p-5">
                <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">X축</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${WarningData.vibration_x_VIBRATION ? `text-[#8FE388]` : 'text-[#FFFF00]'}`}>{SensorData.vibration_x_VIBRATION}</span>
                    <span className="mx-auto">{RangeMin.vibration_x_VIBRATION} ~ {RangeMax.vibration_x_VIBRATION}</span>
                </div>
                <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Y축</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${WarningData.vibration_y_VIBRATION ? `text-[#8FE388]` : 'text-[#FFFF00]'}`}>{SensorData.vibration_y_VIBRATION}</span>
                    <span className="mx-auto">{RangeMin.vibration_y_VIBRATION} ~ {RangeMax.vibration_y_VIBRATION}</span>
                </div>
                <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Z축</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${WarningData.vibration_z_VIBRATION ? `text-[#8FE388]` : 'text-[#FFFF00]'}`}>{SensorData.vibration_z_VIBRATION}</span>
                    <span className="mx-auto">{RangeMin.vibration_z_VIBRATION} ~ {RangeMax.vibration_z_VIBRATION}</span>
                </div>
            </div>
        </div>
    )
}