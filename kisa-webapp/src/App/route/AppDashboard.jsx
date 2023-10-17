import {LeftPanel} from "../components/Dashboard/LeftPanel";
import {MiddlePanel} from "../components/Dashboard/MiddlePanel";
import {RightPanel} from "../components/Dashboard/RightPanel";

export const AppDashboard = () => {
    return (
        <div id={'dash-board'} className={`flex flex-row w-full h-full p-5 text-white text-lg font-semibold`}>
            <LeftPanel/>
            <MiddlePanel/>
            <RightPanel/>
        </div>
    );

};
