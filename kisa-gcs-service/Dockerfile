#FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
#
#WORKDIR .
#
#COPY . .
#
#ENTRYPOINT ["dotnet", "run"]

# 'as build'를 붙이면 빌드단계에서 필요한 것만 설치하고 나중에 최종 이미지를 가져와야한다.
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

LABEL authors="choiyungyu"

# 명령 실행시킬 디렉토리 설정
WORKDIR .

# 현재 Docker 빌드 컨텍스트 내의 모든 파일과 디렉토리를 현재 작업 디렉토리로 복
COPY kisa-gcs-service.csproj ./
#RUN #dotnet restore -r linux-musl-x64

# 소스코드 복사
COPY . .

# .NET 프로젝트를 빌드하고 배포
RUN dotnet publish -c Release -o out

# 런타임 이미지 가져오기
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR .

# 빌드된 실행 파일 복사
COPY --from=build ./out ./
# 컨테이너에 노출시킬 포트 번호 설정
EXPOSE 5000

# 개발중에는 dotnet run을 사용하지만 배포할 땐 어셈블리 파일로 실행
ENTRYPOINT ["dotnet", "kisa-gcs-service.dll"]


