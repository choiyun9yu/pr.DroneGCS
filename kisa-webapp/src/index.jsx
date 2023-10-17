import React from 'react';
import ReactDOM from 'react-dom/client';

import './App/styles/index.css';
import { Main } from './App/Main';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <Main />
    </React.StrictMode>
);
