from flask import Flask
from flask_socketio import SocketIO
from flask_cors import CORS

app = Flask(__name__)
socketio = SocketIO(app) # 경로를 /socket.io로 설정
CORS(app)

@app.route('/')
def index():
    return 'hello world!'

@socketio.on('connect')
def handle_connect():
    print('Client Connected!')

@socketio.on('message')
def handle_message(message):
    print('Received message:', message)

if __name__ == '__main__':
    socketio.run(app, port=5050, debug=True, allow_unsafe_werkzeug=True)