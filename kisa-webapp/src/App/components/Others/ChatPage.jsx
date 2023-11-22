import React, { useEffect, useState } from 'react';
import './ChatPage.css';
import Connector from './signalr-connection';

function ChatPage() {
    const { newMessage, events } = Connector();
    const [message, setMessage] = useState("initial value");

    useEffect(() => {
        events((_, message) => setMessage(message));
    }, []);

    return (
        <div className="chat">
            <span>message from signalR: <span>{message}</span> </span>
            <br />
            <button onClick={() => newMessage((new Date()).toISOString())}>send date </button>
        </div>
    );
}
export default ChatPage();