import { useOutletContext } from 'react-router-dom';

import { ColorThema } from '../ProejctThema';
import { LeftSideBtn } from '../ProejctBtn';
import React, {useEffect, useState} from "react";

export const LeftSidebar = (props) => {
    const [gcsMode, setGcsMode] = useOutletContext();

    return (
        <div className={`flex flex-col items-start w-full h-full rounded-2xl font-bold text-medium text-gray-200 ${ColorThema.Secondary4}`}>
            <LeftSideBtn gcsMode={gcsMode} setGcsMode={setGcsMode} />
            <div className={`m-2 items-center `}>
                <button className={`flex flex-row w-full items-center ml-2 px-2 py-1 rounded-md ${ColorThema.Secondary3}`}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="mr-1 w-5 h-5">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M13.19 8.688a4.5 4.5 0 011.242 7.244l-4.5 4.5a4.5 4.5 0 01-6.364-6.364l1.757-1.757m13.35-.622l1.757-1.757a4.5 4.5 0 00-6.364-6.364l-4.5 4.5a4.5 4.5 0 001.242 7.244" />
                    </svg>
                    Add New Link
                </button>
            </div>

            <div className="w-full m-2 items-center">
                <span className="ml-3">• 등록 드론 </span>
            </div>

            <div className="w-full m-2 items-center">
                <span className="ml-3">• 비행 경로 </span>
            </div>

        </div>
    );
};
