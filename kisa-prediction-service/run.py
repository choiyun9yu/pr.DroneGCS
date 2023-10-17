from app import app
from app.routes import api_blueprint

app.register_blueprint(api_blueprint)

if __name__ == '__main__':
    app.run(port=5050, debug=True)