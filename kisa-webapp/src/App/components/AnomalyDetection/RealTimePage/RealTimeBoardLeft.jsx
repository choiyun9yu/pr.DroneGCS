import {ColorThema} from "../../ProejctThema";
import React from "react";

export const RealTimeBoardLeft = (props) => {

    function transformPropsObject(obj) {
        const transformedObject = {};

        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                const value = obj[key];
                if (typeof value === 'number') {
                    transformedObject[key] = value.toExponential(2);
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
    const WarningData = transformPropsObject(props.WarningData || {});

    return(
        <div className="flex flex-row w-full h-full">
            <div className={`flex flex-col w-[38%] h-full p-3 mr-4 rounded-2xl ${ColorThema.Secondary4}`}>
                <span className="text-white rounded-md ml-3 font-bold text-medium">• 기체 자세 </span>
                <div className="flex flex-col justify-center h-full ">
                    <div className={`flex flex-col justify-around m-5  h-full rounded-md ${ColorThema.Secondary2}`}>
                        <span className="ml-5">Roll</span>
                        <span className={`mx-auto font-extrabold text-2xl ${WarningData.roll_ATTITUDE?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.roll_ATTITUDE_PREDICT}</span>
                        <span className="mx-auto">{RangeMin.roll_ATTITUDE} ~ {RangeMax.roll_ATTITUDE}</span>
                    </div>

                    <div className={`flex flex-col justify-around m-5  h-full rounded-md ${ColorThema.Secondary2}`}>
                        <span className="ml-5">Pitch</span>
                        <span className={`mx-auto font-extrabold text-2xl ${WarningData.pitch_ATTITUDE?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.pitch_ATTITUDE_PREDICT}</span>
                        <span className="mx-auto">{RangeMin.pitch_ATTITUDE} ~ {RangeMax.pitch_ATTITUDE}</span>
                    </div>

                    <div className={`flex flex-col justify-around m-5  h-full rounded-md ${ColorThema.Secondary2}`}>
                        <span className="ml-5">Yaw</span>
                        <span className={`mx-auto font-extrabold text-2xl ${WarningData.yaw_ATTITUDE?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.yaw_ATTITUDE_PREDICT}</span>
                        <span className="mx-auto">{RangeMin.yaw_ATTITUDE} ~ {RangeMax.yaw_ATTITUDE}</span>
                    </div>
                </div>
            </div>
            <div className="flex  flex-col w-full h-full ">
                <div className={`flex flex-col w-full h-full  p-3 mb-4 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 font-bold text-medium">• 자이로 센서 </span>
                    <div className="flex flex-row justify-around h-full p-5">
                        <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">X축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.xgyro_RAW_IMU?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.xgyro_RAW_IMU_PREDICT}</span>
                            <span className="mx-auto">{RangeMin.xgyro_RAW_IMU} ~ {RangeMax.xgyro_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Y축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.ygyro_RAW_IMU?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.ygyro_RAW_IMU_PREDICT}</span>
                            <span className="mx-auto">{RangeMin.ygyro_RAW_IMU} ~ {RangeMax.ygyro_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Z축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.zgyro_RAW_IMU?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.zgyro_RAW_IMU_PREDICT}</span>
                            <span className="mx-auto">{RangeMin.zgyro_RAW_IMU} ~ {RangeMax.zgyro_RAW_IMU}</span>
                        </div>
                    </div>
                </div>

                <div className={`flex flex-col w-full h-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 font-bold text-medium">• 지자기 센서 </span>
                    <div className="flex flex-row justify-around h-full p-5">
                        <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">X축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.xmag_RAW_IMU?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.xmag_RAW_IMU_PREDICT}</span>
                            <span className="mx-auto">{RangeMin.xmag_RAW_IMU} ~ {RangeMax.xmag_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Y축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.ymag_RAW_IMU?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.ymag_RAW_IMU_PREDICT}</span>
                            <span className="mx-auto">{RangeMin.ymag_RAW_IMU} ~ {RangeMax.ymag_RAW_IMU}</span>
                        </div>
                        <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Z축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${WarningData.zmag_RAW_IMU?'text-[#8FE388]':'text-[#FFFF00]'}`}>{PredictData.zmag_RAW_IMU_PREDICT}</span>
                            <span className="mx-auto">{RangeMin.zmag_RAW_IMU} ~ {RangeMax.zmag_RAW_IMU}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}