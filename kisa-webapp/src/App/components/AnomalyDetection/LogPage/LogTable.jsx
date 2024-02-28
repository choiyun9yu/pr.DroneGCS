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
                        <TableCell key={`roll_ATTITUDE-${index}`} value={item.sensorData.roll_ATTITUDE} />
                        <TableCell key={`pitch_ATTITUDE-${index}`} value={item.sensorData.pitch_ATTITUDE} />
                        <TableCell key={`yaw_ATTITUDE-${index}`} value={item.sensorData.yaw_ATTITUDE} />
                        <TableCell key={`xacc_RAW_IMU-${index}`} value={item.sensorData.xacc_RAW_IMU} />
                        <TableCell key={`yacc_RAW_IMU-${index}`} value={item.sensorData.yacc_RAW_IMU} />
                        <TableCell key={`zacc_RAW_IMU-${index}`} value={item.sensorData.zacc_RAW_IMU} />
                        <TableCell key={`xgyro_RAW_IMU-${index}`} value={item.sensorData.xgyro_RAW_IMU} />
                        <TableCell key={`ygyro_RAW_IMU-${index}`} value={item.sensorData.ygyro_RAW_IMU} />
                        <TableCell key={`zgyro_RAW_IMU-${index}`} value={item.sensorData.zgyro_RAW_IMU} />
                        <TableCell key={`xmag_RAW_IMU-${index}`} value={item.sensorData.xmag_RAW_IMU} />
                        <TableCell key={`ymag_RAW_IMU-${index}`} value={item.sensorData.ymag_RAW_IMU} />
                        <TableCell key={`zmag_RAW_IMU-${index}`} value={item.sensorData.zmag_RAW_IMU} />
                        <TableCell key={`vibration_x_VIBRATION-${index}`} value={item.sensorData.vibration_x_VIBRATION} />
                        <TableCell key={`vibration_y_VIBRATION-${index}`} value={item.sensorData.vibration_y_VIBRATION} />
                        <TableCell key={`vibration_z_VIBRATION-${index}`} value={item.sensorData.vibration_z_VIBRATION} />
                        <TableCell key={`accel_cal_x_SENSOR_OFFSETS-${index}`} value={item.sensorData.accel_cal_x_SENSOR_OFFSETS} />
                        <TableCell key={`accel_cal_y_SENSOR_OFFSETS-${index}`} value={item.sensorData.accel_cal_y_SENSOR_OFFSETS} />
                        <TableCell key={`accel_cal_z_SENSOR_OFFSETS-${index}`} value={item.sensorData.accel_cal_z_SENSOR_OFFSETS} />
                        <TableCell key={`mag_ofs_x_SENSOR_OFFSETS-${index}`} value={item.sensorData.mag_ofs_x_SENSOR_OFFSETS} />
                        <TableCell key={`mag_ofs_y_SENSOR_OFFSETS-${index}`} value={item.sensorData.mag_ofs_y_SENSOR_OFFSETS} />
                        <TableCell key={`vx_GLOBAL_POSITION_INT-${index}`} value={item.sensorData.vx_GLOBAL_POSITION_INT} />
                        <TableCell key={`vy_GLOBAL_POSITION_INT-${index}`} value={item.sensorData.vy_GLOBAL_POSITION_INT} />
                        <TableCell key={`x_LOCAL_POSITION_NED-${index}`} value={item.sensorData.x_LOCAL_POSITION_NED} />
                        <TableCell key={`vx_LOCAL_POSITION_NED-${index}`} value={item.sensorData.vx_LOCAL_POSITION_NED} />
                        <TableCell key={`vy_LOCAL_POSITION_NED-${index}`} value={item.sensorData.vy_LOVAL_POSITION_NED} />
                        <TableCell key={`nav_pitch_NAV_CONTROLLER_OUTPUT-${index}`} value={item.sensorData.nav_pitch_NAV_CONTROLLER_OUTPUT} />
                        <TableCell key={`nav_bearing_NAV_CONTROLLER_OUTPUT-${index}`} value={item.sensorData.nav_bearing_NAV_CONTROLLER_OUTPUT} />
                        <TableCell key={`servo3_raw_SERVO_OUTPUT_RAW-${index}`} value={item.sensorData.servo3_raw_SERVO_OUTPUT_RAW} />
                        <TableCell key={`servo8_raw_SERVO_OUTPUT_RAW-${index}`} value={item.sensorData.servo8_raw_SERVO_OUTPUT_RAW} />
                        <TableCell key={`groundspeed_VFR_HUD-${index}`} value={item.sensorData.groundspeed_VFR_HUD} />
                        <TableCell key={`airspeed_VFR_HUD-${index}`} value={item.sensorData.airspeed_VFR_HUD} />
                        <TableCell key={`press_abs_SCALED_PRESSURE-${index}`} value={item.sensorData.press_abs_SCALED_PRESSURE} />
                        <TableCell key={`Vservo_POWER_STATUS-${index}`} value={item.sensorData.Vservo_POWER_STATUS} />
                        <TableCell key={`voltages1_BATTERY_STATUS-${index}`} value={item.sensorData.voltages1_BATTERY_STATUS} />
                        <TableCell key={`chancount_RC_CHANNELS-${index}`} value={item.sensorData.chancount_RC_CHANNELS} />
                        <TableCell key={`chan12_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan12_raw_RC_CHANNELS} />
                        <TableCell key={`chan13_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan13_raw_RC_CHANNELS} />
                        <TableCell key={`chan14_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan14_raw_RC_CHANNELS} />
                        <TableCell key={`chan15_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan15_raw_RC_CHANNELS} />
                        <TableCell key={`chan16_raw_RC_CHANNELS-${index}`} value={item.sensorData.chan16_raw_RC_CHANNELS} />
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