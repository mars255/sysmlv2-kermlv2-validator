"""
Main flask module to run the app
"""
from flask import Flask, request, jsonify, render_template
from ParseRequestHandler import ParseRequestHandler
app = Flask(__name__)

@app.route('/')
def index():
    """Loads HTML"""
    return render_template('index.html')

@app.route('/parse', methods=['POST'])
def handle_parse_kml_or_sysml():
    '''
    Generalized method for parsing the string received for KerMLv2 and SysMLv2

    Expected JSON format:
    {
        "input_string": "string to validate",
        "parser_type": "0" for KerMLv2 or "1" for SysMLv2
    }
    '''
    data = request.json

    request_handler = ParseRequestHandler()
    result, message = request_handler.handle_request(data)
    
    return_code = 201 if result else 400 # Error code or success

    return jsonify({'success': result, 'result': message}), return_code

if __name__ == '__main__':
    app.run(debug=True)

