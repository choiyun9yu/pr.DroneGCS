from flask import Flask
from flask_cors import CORS
from flask_pymongo import PyMongo
from flask_socketio import SocketIO

# socketio = SocketIO()

def create_app():
    app = Flask(__name__)
    CORS(app)
    # socketio.init_app(app)

    return app

app = create_app()

__all__ = ['app']
# __all__ = ['app', 'socketio']
