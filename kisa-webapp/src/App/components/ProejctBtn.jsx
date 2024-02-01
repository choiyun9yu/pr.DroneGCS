import React from 'react';

import { ColorThema } from './ProejctThema';
import { LeftSideIcon } from './ProejctIcon';
// import { Link } from 'react-router-dom';

export const MenuBtn = ({ toggle }) => {
    return (
        <button onClick={toggle}
            className={`flex items-center w-[40px] h-[40px] rounded-md mt-5 text-white ${ColorThema.Secondary3} hover:${ColorThema.Primary1}`}>
            <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="2.5"
                stroke="currentColor"
                className="w-6 h-6 mx-auto"
            >
                <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5" />
            </svg>
        </button>
    );
};

export const LeftSideBtn = (props) => {
    const handleFlight = () => {
        props.setGcsMode('flight');
    };

    const handleMission = () => {
        props.setGcsMode('mission');
    };

    const handleVideo = () => {
        props.setGcsMode('video');
    };

    return (
        <>
            <div id="left-sidebar-btn-box" className="flex justify-between m-4 ">
                {props.gcsMode === 'flight' ? (
                    <button className={`m-1 p-1 rounded-md  text-gray-300 ${ColorThema.Primary1}`}>
                        {LeftSideIcon.flight}
                    </button>
                ) : (
                    <button
                        className={`m-1 p-1 rounded-md  text-white  hover:text-gray-300  ${ColorThema.Secondary3} hover:${ColorThema.Primary1} `}
                        onClick={handleFlight}
                    >
                        {LeftSideIcon.flight}
                    </button>
                )}

                {props.gcsMode === 'mission' ? (
                    <button className={`m-1 p-1 rounded-md  text-gray-300 ${ColorThema.Primary1}`}>
                        {LeftSideIcon.mission}
                    </button>
                ) : (
                    <button
                        className={`m-1 p-1 rounded-md  text-white  hover:text-gray-300  ${ColorThema.Secondary3} hover:${ColorThema.Primary1} `}
                        onClick={handleMission}
                    >
                        {LeftSideIcon.mission}
                    </button>
                )}

                {props.gcsMode === 'video' ? (
                    <button className={`m-1 p-1 rounded-md  text-gray-300 ${ColorThema.Primary1}`}>
                        {LeftSideIcon.video}
                    </button>
                ) : (
                    <button
                        className={`m-1 p-1 rounded-md  text-white  hover:text-gray-300  ${ColorThema.Secondary3} hover:${ColorThema.Primary1} `}
                        onClick={handleVideo}
                    >
                        {LeftSideIcon.video}
                    </button>
                )}
            </div>
        </>
    );
};
