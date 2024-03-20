import React from 'react';

import { DataMap } from '../../DataMap';
import '../../../styles/TableStyles.css';
import {ColorThema} from "../../ProjectThema";

export const PredictionMiniTable = (props) => {
    const minitableObj = {
        header: DataMap.mini_header,
        data: props.tableData,
    };

    function formatDate(date){
        const options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
            timeZone: 'UTC'
        };

        const newDate = new Date(date)
        return newDate.toLocaleString('en-US', options)
    }

    return (
        <div className={`flex flex-col h-full overflow-hidden border-[#6359E9] border rounded-md justify-start items-center text-center ${ColorThema.Secondary4}`}>
            <table id="predict-table" className="flex flex-col w-full h-full overflow-auto">
                <thead className="flex flex-col justify-around w-full">
                    <tr className="flex flex-row  text-white font-bold">
                        {minitableObj.header.map((item, index) => {
                            return (
                                <th className={`flex flex-col justify-center h-[50px] border-[#6359E9] border-r border-b ${ColorThema.Secondary2}`} key={index}>
                                    <span className="flex flex-row justify-center w-[156px]">{item}</span>
                                </th>
                            );
                        })}
                    </tr>
                </thead>
                <tbody className="flex flex-col justify-around w-full">
                {minitableObj.data.map((item, index) => (
                    <tr className={`flex flex-row border-[#6359E9] text-[#AEABD8] text-s`} key={index}>
                        <TableCell key={`PredictData-${index}`} value={item.PredictData} />
                        <TableCell key={`SelectData-${index}`} value={item.SelectData} />
                        <TableCell key={`DroneId-${index}`} value={item.DroneId} />
                        <TableCell key={`PredictTime-${index}`} value={formatDate(item.PredictTime)} />
                        <TableCell key={`roll_ATTITUDE-${index}`} value={item.SensorData.rollATTITUDE} />
                        <TableCell key={`pitch_ATTITUDE-${index}`} value={item.SensorData.pitchATTITUDE} />
                        <TableCell key={`yaw_ATTITUDE-${index}`} value={item.SensorData.yawATTITUDE} />
                        <TableCell key={`xacc_RAW_IMU-${index}`} value={item.SensorData.xaccRAWIMU} />
                        <TableCell key={`yacc_RAW_IMU-${index}`} value={item.SensorData.yaccRAWIMU} />
                        <TableCell key={`zacc_RAW_IMU-${index}`} value={item.SensorData.zaccRAWIMU} />
                        <TableCell key={`xgyro_RAW_IMU-${index}`} value={item.SensorData.xgyroRAWIMU} />
                        <TableCell key={`ygyro_RAW_IMU-${index}`} value={item.SensorData.ygyroRAWIMU} />
                        <TableCell key={`zgyro_RAW_IMU-${index}`} value={item.SensorData.zgyroRAWIMU} />
                        <TableCell key={`xmag_RAW_IMU-${index}`} value={item.SensorData.xmagRAWIMU} />
                        <TableCell key={`ymag_RAW_IMU-${index}`} value={item.SensorData.ymagRAWIMU} />
                        <TableCell key={`zmag_RAW_IMU-${index}`} value={item.SensorData.zmagRAWIMU} />
                        <TableCell key={`vibration_x_VIBRATION-${index}`} value={item.SensorData.vibrationXVIBRATION} />
                        <TableCell key={`vibration_y_VIBRATION-${index}`} value={item.SensorData.vibrationYVIBRATION} />
                        <TableCell key={`vibration_z_VIBRATION-${index}`} value={item.SensorData.vibrationZVIBRATION} />
                        <TableCell key={`accel_cal_x_SENSOR_OFFSETS-${index}`} value={item.SensorData.accelCalXSENSOROFFSETS} />
                        <TableCell key={`accel_cal_y_SENSOR_OFFSETS-${index}`} value={item.SensorData.accelCalYSENSOROFFSETS} />
                        <TableCell key={`accel_cal_z_SENSOR_OFFSETS-${index}`} value={item.SensorData.accelCalZSENSOROFFSETS} />
                        <TableCell key={`mag_ofs_x_SENSOR_OFFSETS-${index}`} value={item.SensorData.magOfsXSENSOROFFSETS} />
                        <TableCell key={`mag_ofs_y_SENSOR_OFFSETS-${index}`} value={item.SensorData.magOfsYSENSOROFFSETS} />
                        <TableCell key={`vx_GLOBAL_POSITION_INT-${index}`} value={item.SensorData.vxGLOBALPOSITIONINT} />
                        <TableCell key={`vy_GLOBAL_POSITION_INT-${index}`} value={item.SensorData.vyGLOBALPOSITIONINT} />
                        <TableCell key={`x_LOCAL_POSITION_NED-${index}`} value={item.SensorData.xLOCALPOSITIONNED} />
                        <TableCell key={`vx_LOCAL_POSITION_NED-${index}`} value={item.SensorData.vxLOCALPOSITIONNED} />
                        <TableCell key={`vy_LOCAL_POSITION_NED-${index}`} value={item.SensorData.vyLOCALPOSITIONNED} />
                        <TableCell key={`nav_pitch_NAV_CONTROLLER_OUTPUT-${index}`} value={item.SensorData.navPitchNAVCONTROLLEROUTPUT} />
                        <TableCell key={`nav_bearing_NAV_CONTROLLER_OUTPUT-${index}`} value={item.SensorData.navBearingNAVCONTROLLEROUTPUT} />
                        <TableCell key={`servo3_raw_SERVO_OUTPUT_RAW-${index}`} value={item.SensorData.servo3RawSERVOOUTPUTRAW} />
                        <TableCell key={`servo8_raw_SERVO_OUTPUT_RAW-${index}`} value={item.SensorData.servo8RawSERVOOUTPUTRAW} />
                        <TableCell key={`groundspeed_VFR_HUD-${index}`} value={item.SensorData.groundspeedVFRHUD} />
                        <TableCell key={`airspeed_VFR_HUD-${index}`} value={item.SensorData.airspeedVFRHUD} />
                        <TableCell key={`press_abs_SCALED_PRESSURE-${index}`} value={item.SensorData.pressAbsSCALEDPRESSURE} />
                        <TableCell key={`Vservo_POWER_STATUS-${index}`} value={item.SensorData.vservoPOWERSTATUS} />
                        <TableCell key={`voltages1_BATTERY_STATUS-${index}`} value={item.SensorData.voltages1BATTERYSTATUS} />
                        <TableCell key={`chancount_RC_CHANNELS-${index}`} value={item.SensorData.chancountRCCHANNELS} />
                        <TableCell key={`chan12_raw_RC_CHANNELS-${index}`} value={item.SensorData.chan12RawRCCHANNELS} />
                        <TableCell key={`chan13_raw_RC_CHANNELS-${index}`} value={item.SensorData.chan13RawRCCHANNELS} />
                        <TableCell key={`chan14_raw_RC_CHANNELS-${index}`} value={item.SensorData.chan14RawRCCHANNELS} />
                        <TableCell key={`chan15_raw_RC_CHANNELS-${index}`} value={item.SensorData.chan15RawRCCHANNELS} />
                        <TableCell key={`chan16_raw_RC_CHANNELS-${index}`} value={item.SensorData.chan16RawRCCHANNELS} />
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
};

const TableCell = ({ value }) => (
    <td className={`flex flex-col justify-center h-[50px] border-b border-[#6359E9] border-r font-normal`}>
        <span className="flex flex-row justify-center w-[156px] text-xs">{value}</span>
    </td>
);