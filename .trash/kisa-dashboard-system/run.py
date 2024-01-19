from app import app
from app.controllers import api_blueprint

app.register_blueprint(api_blueprint)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5050, debug=True)

