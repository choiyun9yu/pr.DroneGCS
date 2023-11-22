from flask import jsonify, Blueprint, request

from .controller import Drone

api_blueprint = Blueprint('api', __name__)

@api_blueprint.route('/', methods=['GET'])
def index():
    if request.method == 'GET':
        drones = Drone.get_drones()
        return jsonify({"ai_drones": drones})

@api_blueprint.route('/api/realtime', methods=['GET', 'POST'])
def realtime_data():
    if request.method == 'POST':
        form_data = request.form
        parsed_data = dict(form_data)
        DroneId = parsed_data['DroneId']
        data = Drone.get_realtime(DroneId)
        alt = Drone.get_alt(DroneId)
        data['Alt'] = alt

        return jsonify({"realtimePage": data})
    else:
        drones = Drone.all_devices_id()
        return jsonify({"drones": drones})

@api_blueprint.route('/api/logdata', methods=['GET', 'POST'])
def drone_logdata():
    if request.method == 'POST':
        form_data = request.form
        parsed_data = dict(form_data)
        DroneId, FlightId, periodFrom, periodTo = (parsed_data['DroneId'], parsed_data['FlightId'],
                                                   parsed_data['periodFrom'], parsed_data['periodTo'])
        data = Drone.get_logdata(DroneId, FlightId, periodFrom, periodTo)
        return jsonify({"logPage": data})
    else:
        drones = Drone.all_devices_id()
        flights = Drone.all_flights_id()
        return jsonify({"drones": drones, "flights": flights})

@api_blueprint.route('/api/predict', methods=['GET', 'POST'])
def predict_result():
    if request.method == 'POST':
        form_data = request.form
        parsed_data = dict(form_data)
        DroneId, FlightId, periodFrom, periodTo, SelectData = (parsed_data['DroneId'], parsed_data['FlightId'],
                                                               parsed_data['periodFrom'], parsed_data['periodTo'],
                                                               parsed_data['SelectData'])
        data = Drone.get_predict(DroneId, FlightId, periodFrom, periodTo, SelectData)
        return jsonify({"predictPage": data})
    else:
        drones = Drone.all_devices_id()
        flights = Drone.all_flights_id()
        return jsonify({"drones": drones, "flights": flights})













