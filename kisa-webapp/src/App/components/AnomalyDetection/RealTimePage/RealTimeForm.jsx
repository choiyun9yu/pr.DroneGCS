import React from 'react';

export const RealTimeForm = (props) => {
    return (
        <div id='realtime-form' className="flex ml-3 mb-5">
            <div className="flex flex-col mr-5 w-full ">
                <form>
                    <select className="h-[35px] w-[135px] px-2 rounded-lg border border-gray-300  text-gray-400 bg-transparent"
                        name={'DroneId'} value={props.selectedDrone} onChange={props.handleSelectDrone}>
                        {props.droneList.map((item,index) => (
                                <option key={index} value={item}>{item}</option>
                        ))}
                    </select>
                </form>
            </div>
        </div>
    );
};
