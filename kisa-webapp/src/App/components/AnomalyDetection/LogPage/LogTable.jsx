import React from 'react';

import '../TableStyles.css';
import { DataMap } from '../../DataMap';
import {ColorThema} from "../../ProejctThema";

export const LogTable = (props) => {
    const tableObj = {
        header: DataMap.table_header,
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
        <div className={`flex flex-col h-full overflow-hidden mb-1 border-[#6359E9] border rounded-md justify-start items-center text-center ${ColorThema.Secondary4}`}>
            <table id="predict-table" className="flex flex-col h-full w-full overflow-auto">
              <thead className="flex flex-col justify-around w-full">
                  <tr className="flex flex-row  text-white font-bold">
                     {tableObj.header.map((item,index) => {
                            return (
                                <th className={`flex flex-col justify-center h-[50px] border-[#6359E9] border-r border-b ${ColorThema.Secondary2}`} key={index}>
                                    <span className="flex flex-row justify-center w-[156px]">{item}</span>
                                </th>
                            );
                        })}
                    </tr>
                </thead>
                <tbody className="flex flex-col justify-around w-full">
                {tableObj.data.map((item, index) => (
                    <tr className={`flex flex-row border-[#6359E9] text-[#AEABD8] text-s`} key={index}>
                        <TableCell key={`DroneId-${index}`} value={item.droneId} />
                        <TableCell key={`PredictTime-${index}`} value={formatDate(item.predictTime)} />
                        <TableCell key={`roll_ATTITUDE-${index}`} value={item.sensorData.rollATTITUDE} />
                        <TableCell key={`pitch_ATTITUDE-${index}`} value={item.sensorData.pitchATTITUDE} />
                        <TableCell key={`yaw_ATTITUDE-${index}`} value={item.sensorData.yawATTITUDE} />
                        <TableCell key={`xacc_RAW_IMU-${index}`} value={item.sensorData.xaccRAWIMU} />
                        <TableCell key={`yacc_RAW_IMU-${index}`} value={item.sensorData.yaccRAWIMU} />
                        <TableCell key={`zacc_RAW_IMU-${index}`} value={item.sensorData.zaccRAWIMU} />
                        <TableCell key={`xgyro_RAW_IMU-${index}`} value={item.sensorData.xgyroRAWIMU} />
                        <TableCell key={`ygyro_RAW_IMU-${index}`} value={item.sensorData.ygyroRAWIMU} />
                        <TableCell key={`zgyro_RAW_IMU-${index}`} value={item.sensorData.zgyroRAWIMU} />
                        <TableCell key={`xmag_RAW_IMU-${index}`} value={item.sensorData.xmagRAWIMU} />
                        <TableCell key={`ymag_RAW_IMU-${index}`} value={item.sensorData.ymagRAWIMU} />
                        <TableCell key={`zmag_RAW_IMU-${index}`} value={item.sensorData.zmagRAWIMU} />
                        <TableCell key={`vibration_x_VIBRATION-${index}`} value={item.sensorData.vibrationXVIBRATION} />
                        <TableCell key={`vibration_y_VIBRATION-${index}`} value={item.sensorData.vibrationYVIBRATION} />
                        <TableCell key={`vibration_z_VIBRATION-${index}`} value={item.sensorData.vibrationZVIBRATION} />
                        <TableCell key={`accel_cal_x_SENSOR_OFFSETS-${index}`} value={item.sensorData.accelCalXSENSOROFFSETS} />
                        <TableCell key={`accel_cal_y_SENSOR_OFFSETS-${index}`} value={item.sensorData.accelCalYSENSOROFFSETS} />
                        <TableCell key={`accel_cal_z_SENSOR_OFFSETS-${index}`} value={item.sensorData.accelCalZSENSOROFFSETS} />
                        <TableCell key={`mag_ofs_x_SENSOR_OFFSETS-${index}`} value={item.sensorData.magOfsXSENSOROFFSETS} />
                        <TableCell key={`mag_ofs_y_SENSOR_OFFSETS-${index}`} value={item.sensorData.magOfsYSENSOROFFSETS} />
                        <TableCell key={`vx_GLOBAL_POSITION_INT-${index}`} value={item.sensorData.vxGLOBALPOSITIONINT} />
                        <TableCell key={`vy_GLOBAL_POSITION_INT-${index}`} value={item.sensorData.vyGLOBALPOSITIONINT} />
                        <TableCell key={`x_LOCAL_POSITION_NED-${index}`} value={item.sensorData.xLOCALPOSITIONNED} />
                        <TableCell key={`vx_LOCAL_POSITION_NED-${index}`} value={item.sensorData.vxLOCALPOSITIONNED} />
                        <TableCell key={`vy_LOCAL_POSITION_NED-${index}`} value={item.sensorData.vyLOCALPOSITIONNED} />
                        <TableCell key={`nav_pitch_NAV_CONTROLLER_OUTPUT-${index}`} value={item.sensorData.navPitchNAVCONTROLLEROUTPUT} />
                        <TableCell key={`nav_bearing_NAV_CONTROLLER_OUTPUT-${index}`} value={item.sensorData.navBearingNAVCONTROLLEROUTPUT} />
                        <TableCell key={`servo3_raw_SERVO_OUTPUT_RAW-${index}`} value={item.sensorData.servo3RawSERVOOUTPUTRAW} />
                        <TableCell key={`servo8_raw_SERVO_OUTPUT_RAW-${index}`} value={item.sensorData.servo8RawSERVOOUTPUTRAW} />
                        <TableCell key={`groundspeed_VFR_HUD-${index}`} value={item.sensorData.groundspeedVFRHUD} />
                        <TableCell key={`airspeed_VFR_HUD-${index}`} value={item.sensorData.airspeedVFRHUD} />
                        <TableCell key={`press_abs_SCALED_PRESSURE-${index}`} value={item.sensorData.pressAbsSCALEDPRESSURE} />
                        <TableCell key={`Vservo_POWER_STATUS-${index}`} value={item.sensorData.VservoPOWERSTATUS} />
                        <TableCell key={`voltages1_BATTERY_STATUS-${index}`} value={item.sensorData.voltages1BATTERYSTATUS} />
                        <TableCell key={`chancount_RC_CHANNELS-${index}`} value={item.sensorData.chancountRCCHANNELS} />
                        <TableCell key={`chan12_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan12RawRCCHANNELS} />
                        <TableCell key={`chan13_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan13RawRCCHANNELS} />
                        <TableCell key={`chan14_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan14RawRCCHANNELS} />
                        <TableCell key={`chan15_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan15RawRCCHANNELS} />
                        <TableCell key={`chan16_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan16RawRCCHANNELS} />
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