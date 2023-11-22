from flask import Flask
from flask_cors import CORS
from flask_socketio import SocketIO
from .routes import api_blueprint, handle_connect, handle_disconnect, handle_my_event  # 수정된 부분

def create_app():
    app = Flask(__name__)
    CORS(app)
    socketio = SocketIO(app)

    socketio.on_event('connect', handle_connect)
    socketio.on_event('disconnect', handle_disconnect)
    socketio.on_event('my_event', handle_my_event)

    app.register_blueprint(api_blueprint)

    return app, socketio

app, socketio = create_app()

__all__ = ['app', 'socketio']