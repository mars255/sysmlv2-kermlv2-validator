"""

Class to handle parse requests

"""
from antlr4 import *
from LoggingParsers.KerMLv2LoggingParser import KerMLv2LoggingParser
from LoggingParsers.SysMLv2LoggingParser import SysMLv2LoggingParser


class ParseRequestHandler():
    def __init__(self):
        pass

    def handle_request(self, data: dict):
        """Validates input data and runs the parser"""
        if data is None or not isinstance(data, dict):
            return False, "Error: Wrong data format"
        input_string = data.get('input_string')
        if input_string is None:
            return False, "Error: No input string"

        parser_type = data.get('parser_type')
        if parser_type == "0":
            parser = KerMLv2LoggingParser(input_string)
        elif parser_type == "1":
            parser = SysMLv2LoggingParser(input_string)
        else:
            return False, "Error: Wrong parser type. Must be SysMLv2 or KerMLv2"
        parser.parse()
        return True, parser.get_errors()
