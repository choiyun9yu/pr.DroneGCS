import { ColorThema } from '../ProejctThema';

export const VideoMode = () => {
    return (
        <div id="right-sidebar" className="flex flex-col w-[720px]">
            <VideoModeTop />
            <VideoModeBottom />
        </div>
    );
};

const VideoModeTop = () => {
    return (
        <div className={`flex items-start w-full h-1/2 mb-3 rounded-2xl ${ColorThema.Secondary4}`}>
            <div className="flex flex-col w-full h-full m-2">
                <span className="text-white rounded-md m-3 font-bold text-medium">• 드론 영상1</span>
                <div className={`flex justify-center items-center w-full h-full px-2 pt-3 pb-7`}>
                    <div className={`flex w-full h-full bg-black`}></div>
                </div>
            </div>
        </div>
    );
};

const VideoModeBottom = () => {
    return (
        <div className={`flex items-start w-full h-1/2 rounded-2xl ${ColorThema.Secondary4}`}>
            <div className="flex flex-col w-full h-full m-2">
                <span className="text-white rounded-md m-3 font-bold text-medium">• 드론 영상2</span>
                <div className={`flex justify-center items-center w-full h-full px-2 pt-3 pb-7`}>
                    <div className={`flex w-full h-full bg-black`}></div>
                </div>
            </div>
        </div>
    );
};
