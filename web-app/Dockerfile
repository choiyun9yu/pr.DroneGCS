FROM node:18-alpine
LABEL authors="choiyungyu"

# 명령 실행시킬 디렉토리 설정
WORKDIR /web-app

# 의존성 파일 복사
COPY package.json package-lock.json ./
COPY yarn.lock .

# 의존성 설치
RUN yarn install

# 소스코드 복사
COPY . ./

# 컨테이너 시작시 최초로 실행될 명령
ENTRYPOINT ["yarn", "start"]