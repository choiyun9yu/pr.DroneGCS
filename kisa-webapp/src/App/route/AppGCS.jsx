import { Outlet } from 'react-router-dom';
import { useState } from 'react';

export const AppGCS = () => {
    const [gcsMode, setGcsMode] = useState('flight');

    return <Outlet context={[gcsMode, setGcsMode]} />;
};
