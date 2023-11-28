import { Routes, Route, BrowserRouter } from 'react-router-dom';

import { Home } from './route/Home';
import { LoginPage } from './components/Authentication/LoginPage';
import { RegisterPage } from './components/Authentication/RegisterPage';
import { AppWrapper } from './components/AppWrapper';
import { AppDashboard } from './route/AppDashboard';
import { AppGCS } from './route/AppGCS';
import { DroneMonitor } from './components/GCS/DroneMonitor';
import { AppAnomalyDetection } from './route/AppAnomalyDetection';
import { Prediction } from './components/AnomalyDetection/Prediction';
import { AppDeIdentification } from './route/AppDeIdentification';
import { VideoProcessing } from './components/DeIdentification/VideoProcessing';
import {Chat} from "./components/Others/Chat";
import {ProjectDocuments} from "./components/Others/ProjectDocuments";
import {ChatRProvider, SignalRProvider} from "./components/GCS/SignalRContainder";

export const Main = () => {
    return (
        <BrowserRouter>
            {/* <항상 사용할 컴포넌트> */}
            <Routes>
                {/* 홈페이지 */}
                <Route path="/" element={<Home />} />
                <Route path="/auth/login" element={<LoginPage />} />
                <Route path="/auth/register" element={<RegisterPage />} />
                <Route element={<AppWrapper />}>
                    <Route path="dashboard" element={<AppDashboard />} />
                    <Route element={ <SignalRProvider>
                                         <AppGCS />
                                     </SignalRProvider> }>
                        <Route path="gcs" element={<DroneMonitor />} />
                    </Route>
                    <Route element={<AppAnomalyDetection />}>
                        <Route path="ml" element={<Prediction />} />
                    </Route>
                    <Route element={<AppDeIdentification />}>
                        <Route path="img" element={<VideoProcessing />} />
                    </Route>
                    <Route path={"chat"} element={
                        <ChatRProvider>
                            <Chat />
                        </ChatRProvider>
                        } />
                    <Route path={"docs"} element={<ProjectDocuments />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
};
