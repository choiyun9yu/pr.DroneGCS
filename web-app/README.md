# kisa-webapp

## 1. Create Project

    % mkdir kisa-webapp
    % cd kisa-webapp
    % npx create-reat-app .

    % yarn start

### 1-1. Tailwind CSS

    % yarn add tailwindcss
    % npx tailwindcss init

    # tailwind.config.js
    module.exports = {
        // 템플릿 파일의 경로 설정 👀
        purge:[ './src/**/*.{js,jsx,ts,tsx}' ],
        content: [
        "./src/**/*.{js,jsx,ts,tsx}",
        ],
        theme: {
        extend: {},
        },
        plugins: [],
    }

### 1-2. framer-motion

    % yarn add framer-motion

    # 컴포넌트.js
    import { motion } from "framer-motion";

### 1-3. styled-

    % yarn add styled-components

#### package.json에 추가

        "@babel/core": "7.22.5",
        "@babel/eslint-parser": "7.22.5",
        "@babel/plugin-proposal-private-property-in-object": "7.21.11",
        "@babel/preset-env": "7.22.5",

######

    % yarn install

### 1-4. others install dependencies

    % yarn add @microsoft/signalr
    % yarn add grpc-web