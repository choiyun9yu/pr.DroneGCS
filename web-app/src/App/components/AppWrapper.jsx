import { Outlet } from 'react-router-dom';

import { Menu } from './Others/Menu';
import { GradientBackground } from './ProejctThema';

export const AppWrapper = () => {
    return (
        <GradientBackground>
            <div id="app-wrapper" className="flex flex-row h-screen w-screen overflow-hidden">
                <Menu />
                <div className="h-screen w-full overflow-hidden">
                    <Outlet />
                </div>
            </div>
        </GradientBackground>
    );
};
