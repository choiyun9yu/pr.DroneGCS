import React, { useState } from 'react';
import { PredictionForm } from './PredictionForm';
import { PredictionMiniTable } from './PredictionMiniTable';
import { PredictionGraph } from './PredictionGraph';

export const PredictionResultSelect = () => {
    const [tableData, setTableData] = useState([]);
    const [graphData, setGraphData] = useState([]);

    const tableTransfer = (data) => {
        setTableData(data);
    };

    const graphTransfer = (data) => {
        setGraphData(data);
    };
    return (
            <div className="flex flex-col  w-full h-full overflow-hidden bg-[#1D1D41] ml-5 mr-5 mb-5 p-5 rounded-lg text-[#6359E9] font-normal">
                <PredictionForm tableTransfer={tableTransfer} graphTransfer={graphTransfer} />
                <PredictionGraph graphData={graphData} />
                <div className="flex h-full mt-2 overflow-hidden">
                    <PredictionMiniTable tableData={tableData} />
                </div>
            </div>
    );
};
