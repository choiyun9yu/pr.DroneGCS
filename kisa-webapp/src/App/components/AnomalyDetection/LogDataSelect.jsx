import React, { useState } from 'react';
import { LogTable } from './LogTable';
import { LogForm } from './LogForm';
import { ColorThema } from '../ProejctThema';

export const LogDataSelect = () => {
    const [tableData, setTableData] = useState([]);

    const dataTransfer = (data) => {
        setTableData(data);
    };

    return (
        <>
            <div className={`flex flex-col w-full h-full overflow-clip mx-5 mb-5 p-5 rounded-lg font-normal text-[#6359E9] ${ColorThema.Secondary4}`}>
                <LogForm dataTransfer={dataTransfer} />
                <div className="flex h-full mt-8 overflow-hidden">
                    <LogTable tableData={tableData} />
                </div>
            </div>
        </>
    );
};
