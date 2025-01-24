using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using SysMLv2Validator;
using KerMLv2Validator;
using Antlr4.Runtime.Tree;

namespace LoggingParsers
{
    /// <summary>
    /// Base class for custom parsers that log errors.
    /// </summary>
    public abstract class LoggingParser
    {
        protected readonly string InputString;
        protected readonly ErrorLogger ErrorLogger;

        protected LoggingParser(string inputString)
        {
            InputString = inputString;
            ErrorLogger = new ErrorLogger();
        }

        /// <summary>
        /// Main parsing method that sets up the specific lexer and parser for each subclass.
        /// </summary>
        public string Parse()
        {
            var inputStream = new AntlrInputStream(InputString);
            var lexer = GetLexerInstance(inputStream);

            var tokenStream = new CommonTokenStream(lexer);
            var parser = GetParserInstance(tokenStream);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(ErrorLogger);

            // Cast to the specific parser type to call the rule
            if (parser is SysMLv2Parser sysMLParser)
            {
                var tree = sysMLParser.entryRuleRootNamespace();
                return tree.ToStringTree(sysMLParser);
            }
            else if (parser is KerMLv2Parser kerMLParser)
            {
                var tree = kerMLParser.entryRuleRootNamespace();
                return tree.ToStringTree(kerMLParser);
            }
            else
            {
                throw new InvalidOperationException("The parser instance is not of type SysMLv2Parser.");
            }
        }


        /// <summary>
        /// Retrieves the errors encountered during parsing.
        /// </summary>
        public string GetErrors()
        {
            var errorsString = ErrorLogger.GetErrorsString();
            return string.IsNullOrEmpty(errorsString) ? "Parsing successful!" : errorsString;
        }

        /// <summary>
        /// Abstract method to obtain specific lexer instance.
        /// </summary>
        protected abstract Lexer GetLexerInstance(ICharStream inputStream);

        /// <summary>
        /// Abstract method to obtain specific parser instance.
        /// </summary>
        protected abstract Parser GetParserInstance(ITokenStream tokenStream);
    }

    /// <summary>
    /// Parser for validating SysMLv2 specifically.
    /// </summary>
    public class SysMLv2LoggingParser : LoggingParser
    {
        public SysMLv2LoggingParser(string inputString) : base(inputString) { }

        protected override Lexer GetLexerInstance(ICharStream inputStream)
        {
            return new SysMLv2Lexer(inputStream);
        }

        protected override Parser GetParserInstance(ITokenStream tokenStream)
        {
            return new SysMLv2Parser(tokenStream);
        }
    }

    /// <summary>
    /// Parser for validating KerMLv2 specifically.
    /// </summary>
    public class KerMLv2LoggingParser : LoggingParser
    {
        public KerMLv2LoggingParser(string inputString) : base(inputString) { }

        protected override Lexer GetLexerInstance(ICharStream inputStream)
        {
            return new KerMLv2Lexer(inputStream);
        }

        protected override Parser GetParserInstance(ITokenStream tokenStream)
        {
            return new KerMLv2Parser(tokenStream);
        }

    }


    /// <summary>
    /// Custom error logger for capturing parsing errors.
    /// </summary>
    public class ErrorLogger : BaseErrorListener
    {
        private readonly List<string> _errors = new List<string>();

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            var errorMessage = $"Syntax error at line {line}, column {charPositionInLine}: {msg}";
            _errors.Add(errorMessage);
        }

        public string GetErrorsString()
        {
            return string.Join("\n", _errors);
        }
    }
}
