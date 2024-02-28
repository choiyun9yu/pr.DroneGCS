import React, { useState } from 'react';

import { PredictionServiceHeader } from './PredictionServiceHeader';
import { LogDataSelect } from '../LogPage/LogDataSelect';
import { PredictionResultSelect } from './PredictionResultSelect';
import { RealTime } from '../RealTimePage/RealTime';

export const Prediction = () => {
    const [predMode, setPredMode] = useState('realTime');
    return (
        <div id="prdiction" className="flex flex-col w-full h-full">
            <PredictionServiceHeader predMode={predMode} setPredMode={setPredMode} />
            <div className="flex w-full h-full mb-5 overflow-hidden">
                {predMode === 'realTime' ? <RealTime /> : null}
                {predMode === 'logData' ? <LogDataSelect /> : null}
                {predMode === 'predResult' ? <PredictionResultSelect /> : null}
            </div>
        </div>
    );
};
