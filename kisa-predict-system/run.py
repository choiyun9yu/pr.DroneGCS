from module.signalr_connection_builder import HubConnection
import asyncio

async def main():
    await HubConnection()

if __name__ == "__main__":
    asyncio.run(main())