import asyncio
import websockets
from motor.motor_asyncio import AsyncIOMotorClient

async def connect_to_mongodb():
    mongo_client = AsyncIOMotorClient('mongodb://localhost:27017')
    db = mongo_client['test_service']
    return db

# WebSocket 서버 설정
async def handle_websocket(websocket, path):
    db = await connect_to_mongodb()

    async for message in websocket:
        print(f"Received message: {message}")

        collection = db['test']
        data_to_insert = {'message': message}
        await collection.insert_one(data_to_insert)

if __name__ == '__main__':
    start_server = websockets.serve(handle_websocket, "localhost", 8765)    # 웹 소켓 서버를 localhost의 8765 포트에서 시작
    asyncio.get_event_loop().run_until_complete(start_server)               # 비동기 서버로 설정하고 시작
    asyncio.get_event_loop().run_forever()                                  # 비동기 이벤트 루프를 계속해서 실행하여 서버가 계속 실행되도록 함