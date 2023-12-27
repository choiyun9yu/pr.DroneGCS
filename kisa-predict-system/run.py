import asyncio
from signalrcore.hub_connection_builder import HubConnectionBuilder

async def main():
    hub_connection = HubConnectionBuilder() \
        .with_url("ws://localhost:5000/droneHub/") \
        .configure_logging(logging_level=30) \
        .with_automatic_reconnect({
            "type": "raw",
            "keep_alive_interval": 10,
            "reconnect_interval": 5,
            "max_attempts": 10
        }).build()

    close_event = asyncio.Event()

    hub_connection.on_open(lambda: print("connection opened and handshake received ready to send messages"))
    hub_connection.on_close(lambda: close_event.set())  # 연결이 닫히면 이벤트 설정
    hub_connection.on_reconnect(lambda: print("reconnection in progress"))
    hub_connection.on("PredictionMessage", lambda data: print(f"{data}"))

    hub_connection.start()
    try:
        await close_event.wait()  # 연결이 닫힐 때까지 대기
    finally:
        await hub_connection.stop()  # 종료 전에 연결을 중지해야 합니다

if __name__ == "__main__":
    asyncio.run(main())