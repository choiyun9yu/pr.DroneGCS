import React, { useState, useContext } from 'react';
import {ChatContext} from "../GCS/SignalRContainder";

const ChatPage = () => {
    const [user, setUser] = useState('');
    const [message, setMessage] = useState('');
    const [messages, setMessages] = useState([]);
    const { sendMessage, connStt } = useContext(ChatContext);

    const handleUserChange = (e) => {
        setUser(e.target.value);
    };

    const handleMessageChange = (e) => {
        setMessage(e.target.value);
    };

    const handleSendMessage = () => {
        if (user && message) {
            sendMessage(user, message)
            const newMessage = `${user}: ${message}`;
            setMessages([...messages, newMessage]);
            // 메시지를 전송한 후 입력 필드 초기화
            setUser('');
            setMessage('');
        }
    };

    return (
        <div className="container">
            <div className="flex flex-column p-1">
                <div className="text-white">Name</div>
                <div className="ml-7">
                    <input type="text" value={user} onChange={handleUserChange} />
                </div>
            </div>

            <div className="flex flex-column p-1">
                <div className="text-white">Message</div>
                <div className="ml-2">
                    <input type="text" className="w-100" value={message} onChange={handleMessageChange} />
                </div>
            </div>

            <div className="flex-column p-1">
                <div className="text-end text-white">
                    <input type="button" id="sendButton" value="Send Message" onClick={handleSendMessage} />
                </div>
            </div>

            <hr />

            <div className="flex-row p-1 text-white">
                <div className="col-6">
                    <ul>
                        {messages.map((msg, index) => (
                            <li key={index}>{msg}</li>
                        ))}
                    </ul>
                </div>
            </div>

        </div>
    );
};

export default ChatPage;