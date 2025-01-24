using LoggingParsers;
using System;

namespace MyAspNetApp.Services
{
    /// <summary>
    /// Class for validating the input JSON data and running the logging parsers.
    /// </summary>
    public class ParseRequestHandler
    {
        public (bool, string) HandleRequest(dynamic data)
        {
            // Validate input data
            if (data == null || data.input_string == null || data.parser_type == null)
            {
                return (false, "Missing required data.");
            }

            string inputString = data.input_string;

            if (string.IsNullOrWhiteSpace(inputString))
            {
                return (false, "Empty input string.");
            }

            if (!int.TryParse((string)data.parser_type, out int parserType))
            {
                return (false, "Invalid parser type format.");
            }

            LoggingParser parser;

            // Process based on parser type
            if (parserType == 0)
            {
                parser = new KerMLv2LoggingParser(inputString);
            }
            else if (parserType == 1)
            {
                parser = new SysMLv2LoggingParser(inputString);
            }
            else
            {
                return (false, "Invalid parser type. Must be 0 (KerMLv2) or 1 (SysMLv2).");
            }

            // Parse the input string and get errors
            try
            {
                parser.Parse();
                string errors = parser.GetErrors();
                return (true, errors);
            }
            catch (Exception ex)
            {
                return (false, $"Error during parsing: {ex.Message}");
            }
        }
    }
}
