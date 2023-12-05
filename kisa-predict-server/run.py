import asyncio
import logging

from signalrcore.hub_connection_builder import HubConnectionBuilder

async def main():
    hub_connection = HubConnectionBuilder() \
        .with_url("ws://localhost:5000/droneHub/") \
        .configure_logging(logging.DEBUG) \
        .with_automatic_reconnect({
        "type": "raw",
        "keep_alive_interval": 10,
        "reconnect_interval": 5,
        "max_attempts": 10
    }).build()

    hub_connection.on_open(lambda: print("connection opened and handshake received ready to send messages"))
    hub_connection.on_close(lambda: print("connection closed"))
    hub_connection.on_reconnect(lambda: print("reconnection in progress"))

    hub_connection.on("ReceiveMavMessage", print)

    hub_connection.start()
    await asyncio.sleep(20) # hub_connection이 기다릴 수 있는 객체를 반환해야하는데 bool을 반환해서 오류가나니까 강제로 sleep을 걸어버린 것 (이것도 해법은 아님)

    # start 메서드가 반환하는 Future 객체를 await로 기다림
    # start_result = await hub_connection.start()
    # print(f"start result: {start_result}")
    # await start_result

if __name__ == "__main__":
    asyncio.run(main())
