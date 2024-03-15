import {ColorThema} from "../../ProejctThema";
import React from "react";

export const RealTimeBoardLeft = (props) => {

    function transformPropsObject(obj) {
        const transformedObject = {};

        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                const value = obj[key];
                if (typeof value === 'number') {
                    transformedObject[key] = value.toFixed(1)
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
        <div className="flex flex-row w-full h-full">
            <RealTimeBoardSide PredictData={props.PredictData} WarningData={props.WarningData} RangeMax={props.RangeMax} RangeMin={props.RangeMin}/>
            <div className="flex  flex-col w-full h-full ">
                <div className={`flex flex-col w-full h-full  p-3 mb-4 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 font-bold text-medium">• 자이로 센서 </span>
                    <div className="flex flex-row justify-around h-full p-5">
                        <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">X축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.XgyroRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{(PredictData.XgyroRAWIMUPREDICT)}</span>
                            <span className="mx-auto">{(RangeMin.xgyro_RAW_IMU)} ~ {(RangeMax.xgyro_RAW_IMU)}</span>
                        </div>
                        <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Y축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.YgyroRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{(PredictData.YgyroRAWIMUPREDICT)}</span>
                            <span className="mx-auto">{(RangeMin.ygyro_RAW_IMU)} ~ {(RangeMax.ygyro_RAW_IMU)}</span>
                        </div>
                        <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Z축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.ZgyroRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{(PredictData.ZgyroRAWIMUPREDICT)}</span>
                            <span className="mx-auto">{(RangeMin.zgyro_RAW_IMU)} ~ {(RangeMax.zgyro_RAW_IMU)}</span>
                        </div>
                    </div>
                </div>

                <div className={`flex flex-col w-full h-full p-3 rounded-2xl ${ColorThema.Secondary4}`}>
                    <span className="text-white rounded-md ml-3 font-bold text-medium">• 지자기 센서 </span>
                    <div className="flex flex-row justify-around h-full p-5">
                        <div className={`flex flex-col justify-around mr-4 w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">X축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.XmagRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{(PredictData.XmagRAWIMUPREDICT)}</span>
                            <span className="mx-auto">{(RangeMin.xmag_RAW_IMU)} ~ {(RangeMax.xmag_RAW_IMU)}</span>
                        </div>
                        <div className={`flex flex-col justify-around  mr-4 w-full h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Y축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.YmagRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{(PredictData.YmagRAWIMUPREDICT)}</span>
                            <span className="mx-auto">{(RangeMin.ymag_RAW_IMU)} ~ {(RangeMax.ymag_RAW_IMU)}</span>
                        </div>
                        <div className={`flex flex-col justify-around w-full  h-full rounded-md ${ColorThema.Secondary2}`}>
                            <span className="ml-5">Z축</span>
                            <span className={`mx-auto font-extrabold text-2xl ${props.WarningData.ZmagRAWIMUWARNING?`text-[#FFFF00]`:'text-[#8FE388]'}`}>{(PredictData.ZmagRAWIMUPREDICT)}</span>
                            <span className="mx-auto">{(RangeMin.zmag_RAW_IMU)} ~ {(RangeMax.zmag_RAW_IMU)}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

const RealTimeBoardSide = (props) => {
    function transformPropsObject(obj) {
        const transformedObject = {};

        for (const key in obj) {
            if (obj.hasOwnProperty(key)) {
                const value = obj[key];
                if (typeof value === 'number') {
                    transformedObject[key] = value.toFixed(4)
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

    return (
        <div className={`flex flex-col w-[38%] h-full p-3 mr-4 rounded-2xl ${ColorThema.Secondary4}`}>
            <span className="text-white rounded-md ml-3 font-bold text-medium">• 기체 자세 </span>
            <div className="flex flex-col justify-center h-full ">
                <div className={`flex flex-col justify-around m-5  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Roll</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${props.WarningData.RollATTITUDEWARNING ? `text-[#FFFF00]` : 'text-[#8FE388]'}`}>{(PredictData.RollATTITUDEPREDICT)}</span>
                    <span className="mx-auto">{RangeMin.roll_ATTITUDE} ~ {RangeMax.roll_ATTITUDE}</span>
                </div>

                <div className={`flex flex-col justify-around m-5  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Pitch</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${props.WarningData.PitchATTITUDEWARNING ? `text-[#FFFF00]` : 'text-[#8FE388]'}`}>{(PredictData.PitchATTITUDEPREDICT)}</span>
                    <span className="mx-auto">{(RangeMin.pitch_ATTITUDE)} ~ {(RangeMax.pitch_ATTITUDE)}</span>
                </div>

                <div className={`flex flex-col justify-around m-5  h-full rounded-md ${ColorThema.Secondary2}`}>
                    <span className="ml-5">Yaw</span>
                    <span
                        className={`mx-auto font-extrabold text-2xl ${props.WarningData.YawATTITUDEWARNING ? `text-[#FFFF00]` : 'text-[#8FE388]'}`}>{(PredictData.YawATTITUDEPREDICT)}</span>
                    <span className="mx-auto">{(RangeMin.yaw_ATTITUDE)} ~ {(RangeMax.yaw_ATTITUDE)}</span>
                </div>
            </div>
        </div>
    )
}