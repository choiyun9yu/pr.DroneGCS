from app import app
from app.routes import api_blueprint

app.register_blueprint(api_blueprint)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5050, debug=True)

# from app import app, socketio
#
# if __name__ == '__main__':
#     socketio.run(app, port=5050, debug=True, allow_unsafe_werkzeug=True)
