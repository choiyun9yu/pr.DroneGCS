import React from 'react';
import { ColorThema } from '../ProejctThema';

export const PredictionServiceHeader = (props) => {
    const handleRealTime = () => {
        props.setPredMode('realTime');
    };

    const handleLogData = () => {
        props.setPredMode('logData');
    };

    const handlePredResult = () => {
        props.setPredMode('predResult');
    };

    return (
        <>
            <div
                id="prediction-header"
                className={`m-5 p-3 rounded-lg font-normal  text-[#6359E9]  ${ColorThema.Secondary4}`}
            >
                {props.predMode === 'realTime' ? (
                    <button
                        className={`ml-1 mr-4 p-1 px-2 rounded-md border  border-[#6359E9] text-white ${ColorThema.Primary1}`}
                    >
                        실시간 장애진단
                    </button>
                ) : (
                    <button
                        className={`mr-4  ml-1 p-1 px-2 rounded-md border  border-[#6359E9] hover:text-white   hover:${ColorThema.Primary1}`}
                        onClick={handleRealTime}
                    >
                        실시간 장애진단
                    </button>
                )}
                {props.predMode === 'logData' ? (
                    <button
                        className={`ml-1 mr-4 p-1 px-2 rounded-md border  border-[#6359E9] text-white ${ColorThema.Primary1}`}
                    >
                        로그데이터 조회
                    </button>
                ) : (
                    <button
                        className={`mr-4  ml-1 p-1 px-2 rounded-md border  border-[#6359E9] hover:text-white   hover:${ColorThema.Primary1}`}
                        onClick={handleLogData}
                    >
                        로그데이터 조회
                    </button>
                )}
                {props.predMode === 'predResult' ? (
                    <button
                        className={`ml-1 mr-4 p-1 px-2 rounded-md border  border-[#6359E9] text-white ${ColorThema.Primary1}`}
                    >
                        장애진단 예측결과 조회
                    </button>
                ) : (
                    <button
                        className={`mr-4  ml-1 p-1 px-2 rounded-md border  border-[#6359E9] hover:text-white   hover:${ColorThema.Primary1}`}
                        onClick={handlePredResult}
                    >
                        장애진단 예측결과 조회
                    </button>
                )}
            </div>
        </>
    );
};
