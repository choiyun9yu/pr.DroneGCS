# Front-ent Source Code

## 1. Start React App
    
    % cd web-app

    % yarn

    % yarn start 

## 2. Project Directory Structure

    web-app/
    ├── public/        
    │   ├── Drone.png
    │   └── index.html
    ├── src/                   
    │   ├── App/
    │   │   ├── components/
    │   │   │   ├── AnomalyDetection/
    │   │   │   │   ├── LogPage/...
    │   │   │   │   ├── PredictionPage/...
    │   │   │   │   └── RealTimePage/...
    │   │   │   ├── Dashboard/
    │   │   │   │   ├── LeftPanel.jsx
    │   │   │   │   ├── MiddlePanel.jsx
    │   │   │   │   └── RightPanel.jsx
    │   │   │   ├── GCS/
    │   │   │   │   ├── components/...
    │   │   │   │   ├── FlightMode/...
    │   │   │   │   ├── MissionMode/...
    │   │   │   │   ├── VideoMode/...
    │   │   │   │   ├── DroneSystem.jsx
    │   │   │   │   ├── LeftSidebar.jsx
    │   │   │   │   ├── MiddleMap.jsx
    │   │   │   │   └── SignalRContainer.jsx
    │   │   │   ├── Others/
    │   │   │   │   ├── ...
    │   │   │   │   └── Menu.jsx
    │   │   │   ├── AppWrapper.jsx
    │   │   │   ├── DataMap.jsx
    │   │   │   ├── ProjectBtn.jsx
    │   │   │   ├── ProjectIcon.jsx
    │   │   │   └── ProjectThema.jsx
    │   │   ├── routes/
    │   │   │   ├── AppAnomalyDetection.jsx
    │   │   │   ├── AppDashboard.jsx
    │   │   │   ├── AppDeIdentification.jsx
    │   │   │   ├── AppGCS.jsx
    │   │   │   └── Home.jsx
    │   │   ├── styles/...
    │   │   └── Main.jsx
    │   └── index.jsx
    ├── .env(.gitignore)
    ├── package.json
    ├── package-lock.json
    └── yarn.lock

## 3. Create Project

    % mkdir kisa-webapp
    % cd kisa-webapp
    % npx create-reat-app .

    % yarn start

### 3-1. Tailwind CSS

    % yarn add tailwindcss
    % npx tailwindcss init

    # tailwind.config.js
    module.exports = {
        purge:[ './src/**/*.{js,jsx,ts,tsx}' ],
        content: [
        "./src/**/*.{js,jsx,ts,tsx}",
        ],
        theme: {
        extend: {},
        },
        plugins: [],
    }

### 3-2. framer-motion

    % yarn add framer-motion

    # 컴포넌트.js
    import { motion } from "framer-motion";

### 3-3. styled-

    % yarn add styled-components

#### package.json에 추가

    "@babel/core": "7.22.5",
    "@babel/eslint-parser": "7.22.5",
    "@babel/plugin-proposal-private-property-in-object": "7.21.11",
    "@babel/preset-env": "7.22.5",
######
    % yarn install

### 3-4. others install dependencies

    % yarn add @microsoft/signalr
    % yarn add grpc-web