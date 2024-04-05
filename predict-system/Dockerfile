FROM python:3.10
LABEL authors="choiyungyu"

# 이미지 내에서 명령어를 실행할 경로 설정
WORKDIR /app

# Poetry 설치
RUN pip install poetry

# 애플리케이션 소스 코드 추가
COPY pyproject.toml poetry.lock /app/

# Poetry에서 의존성 설치
RUN poetry install

# 애플리케이션 소스 코드 추가
COPY . /app

# 애플리케이션 실행
CMD ["poetry", "run", "python", "run.py"]

########################################################################################################################

# 베이스 이미지로 alpine 리눅스 사용
#FROM python:3.10-alpine
#LABEL authors="choiyungyu"
#
## 필요한 패키지 설치
#RUN apk update && \
#    apk add --no-cache \
#        build-base \
#        libffi-dev \
#        openssl-dev \
#        libc-dev \
#        linux-headers \
#        g++
#
## 이미지 내에서 명령어를 실행할 경로 설정
#WORKDIR /app
#
## requirements.txt 복사
#COPY requirements.txt /app/
#
## 필요한 Python 패키지 설치
#RUN pip install --no-cache-dir -r requirements.txt
#
## 애플리케이션 소스 코드 추가
#COPY . /app
#
## 애플리케이션 실행
#CMD ["python", "run.py"]