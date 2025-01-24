from ParseRequestHandler import ParseRequestHandler
from pathlib import Path

def test_parse_request_handler_wrong_data_type():
    handler = ParseRequestHandler()
    req = 10
    result, error = handler.handle_request(req)
    assert result == False
    assert error == "Error: Wrong data format"

def test_parse_request_handler_no_data():
    handler = ParseRequestHandler()
    result, error = handler.handle_request(None)
    assert result == False
    assert error == "Error: Wrong data format"

def test_parse_request_handler_no_input():
    handler = ParseRequestHandler()
    req = {"parser_type": "0"}
    result, error = handler.handle_request(req)
    assert result == False
    assert error == "Error: No input string"

def test_parse_request_handler_no_parser_type():
    handler = ParseRequestHandler()
    req = {"input_string": "test"}
    result, error = handler.handle_request(req)
    assert result == False
    assert error == "Error: Wrong parser type. Must be SysMLv2 or KerMLv2"
def test_parse_request_handler_wrong_parser_type():
    handler = ParseRequestHandler()
    req = {"input_string": "test", "parser_type": "aaa"}
    result, error = handler.handle_request(req)
    assert result == False
    assert error == "Error: Wrong parser type. Must be SysMLv2 or KerMLv2"

def test_parse_request_empty_kerml():
    handler = ParseRequestHandler()
    test_file_path = Path(__file__).parent / "data" / "test_room.sysml"

    req = {"input_string": "", "parser_type": "1"}
    result, output = handler.handle_request(req)
    assert result == True
    assert output == "Parsing successful!"

def test_parse_request_empty_sysml():
    handler = ParseRequestHandler()
    test_file_path = Path(__file__).parent / "data" / "test_room.sysml"

    req = {"input_string": "", "parser_type": "0"}
    result, output = handler.handle_request(req)
    assert result == True
    assert output == "Parsing successful!"

def test_parse_request_correct_sysml():
    handler = ParseRequestHandler()
    test_file_path = Path(__file__).parent / "data" / "test_room.sysml"
    
    with open(test_file_path, "r") as fd:
        input_str = fd.read()

    req = {"input_string": input_str, "parser_type": "1"}
    result, output = handler.handle_request(req)
    assert result == True
    assert output == "Parsing successful!"

def test_parse_request_correct_kerml():
    handler = ParseRequestHandler()
    test_file_path = Path(__file__).parent / "data" / "test_vehicle.kerml"
    
    with open(test_file_path, "r") as fd:
        input_str = fd.read()

    req = {"input_string": input_str, "parser_type": "0"}
    result, output = handler.handle_request(req)
    assert result == True
    assert output == "Parsing successful!"

def test_parse_sysml_error():
    handler = ParseRequestHandler()
    test_file_path = Path(__file__).parent / "data" / "test_room_error.sysml"
    
    with open(test_file_path, "r") as fd:
        input_str = fd.read()

    req = {"input_string": input_str, "parser_type": "1"}
    result, output = handler.handle_request(req)
    assert result == True
    assert output == "Syntax error at line 7, column 8: mismatched input 'package' expecting {'default', '{', ';', '=', ':='}"

def test_parse_kerml_2_errors():
    handler = ParseRequestHandler()
    test_file_path = Path(__file__).parent / "data" / "test_vehicle_error.kerml"
    
    with open(test_file_path, "r") as fd:
        input_str = fd.read()

    req = {"input_string": input_str, "parser_type": "0"}
    result, output = handler.handle_request(req)
    error_message = (
        "Syntax error at line 12, column 1: mismatched input 'class' "
        "expecting {'default', '{', ';', '=', ':='}\n"
        "Syntax error at line 13, column 1: mismatched input 'class' "
        "expecting {'{', ';'}"
    )

    assert result == True
    assert output == error_message
