from flask import Flask, request
from flask_socketio import SocketIO


app = Flask(__name__)
socketio = SocketIO(app)

@app.route('/')
def index():
    return 'Hello World!'

@socketio.on('connect')
def handle_connect():
    print('Connected to Flask WebSocket')

@socketio.on('disconnect')
def handle_disconnect():
    print('Disconnected from Flask WebSocket')

def send_data_to_signalr(data):
    # 'send_data_to_signalr' 이벤트를 통해 데이터를 .NET SignalR 서버로 전송
    socketio.emit('send_data_to_signalr', data)

# 사용자가 데이터를 전송할 수 있는 엔드포인트
@app.route('/send_data', methods=['POST'])
def send_data():
    data = request.form.get('data')
    send_data_to_signalr(data)
    return 'Data sent to SignalR'

if __name__ == '__main__':
    socketio.run(app, port=5050, debug=True, allow_unsafe_werkzeug=True)