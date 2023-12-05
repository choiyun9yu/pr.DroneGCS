FROM python:3.10-alpine
LABEL authors="choiyungyu"

# 이미지 내에서 명령어를 실행할 경로 설정
WORKDIR /kisa-predict-server

# 경로의 디렉토리 내용물을 컨테이너로 복사
COPY requirements.txt .

# 이미지 생성 과정에서 실행될 명령
RUN apk add --no-cache py3-setuptools
RUN pip install --upgrade pip
RUN pip install -r requirements.txt

# 소스코드 복사
COPY . ./

# 컨테이너 실행시 최초 실행될 명령
ENTRYPOINT ["python", "run.py"]