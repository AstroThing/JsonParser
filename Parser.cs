using System.Collections.Generic;
using System.Linq;
using JsonParser.Lexical;

namespace JsonParser.Syntactical {
    public class Parser {
        private readonly Lexer _lexer;

        private Token _token;

        private static List<Token.Type> ValuePrefix;

        static Parser() {
            ValuePrefix = new List<Token.Type>{Token.Type.StringLiteral, Token.Type.IntLiteral, Token.Type.FractionLiteral, Token.Type.CurlyBraceOpen, Token.Type.SquareBraceOpen, Token.Type.TrueLiteral, Token.Type.FalseLiteral, Token.Type.NullLiteral};
        }

        public Parser(Lexer lexer) {
            _lexer = lexer;
            _token = _lexer.Read();
        }

        public void Parse() {
            JSON();

            if(_token.TokenType != Token.Type.EOF)
                throw new SyntaxException("Expected EOF", _token);
        }

        private void JSON() {
            if(_token.TokenType == Token.Type.CurlyBraceOpen)
                Object();
        }

        private void Object() {
            if(_token.TokenType != Token.Type.CurlyBraceOpen)
                throw new SyntaxException("Expected '{'", _token);
            
            _token = _lexer.Read();
            ObjectP();
        }

        private void ObjectP() {
            if(_token.TokenType == Token.Type.CurlyBraceClose) {
                _token = _lexer.Read();
            } else if(_token.TokenType == Token.Type.StringLiteral){
                Members();

                if(_token.TokenType != Token.Type.CurlyBraceClose)
                    throw new SyntaxException("Expected '}'", _token);

                _token = _lexer.Read();
            } else {
                throw new SyntaxException("Expected '{' or string", _token);
            }
        }

        private void Members() {
            Pair();
            MembersP();
        }

        private void MembersP() {
            if(_token.TokenType == Token.Type.Comma) {
                _token = _lexer.Read();
                Members();
            }
        }

        private void Pair() {
            if(_token.TokenType != Token.Type.StringLiteral)
                throw new SyntaxException("Expected string", _token);
            
            _token = _lexer.Read();

            if(_token.TokenType != Token.Type.Colon)
                throw new SyntaxException("Expected ':'", _token);

            _token = _lexer.Read();

            Value();
        }

        private void Array() {
            if(_token.TokenType != Token.Type.SquareBraceOpen)
                throw new SyntaxException("Expected '['", _token);
            
            _token = _lexer.Read();

            ArrayP();
        }

        private void ArrayP() {
            if(_token.TokenType == Token.Type.SquareBraceClose) {
                _token = _lexer.Read();
            } else if (ValuePrefix.Any(t => t == _token.TokenType)) {
                Elements();

                if(_token.TokenType != Token.Type.SquareBraceClose)
                    throw new SyntaxException("Expected ']'", _token);

                _token = _lexer.Read();
            } else {
                throw new SyntaxException("Expected value or ']'", _token);
            }
        }

        private void Elements() {
            Value();
            ElementsP();
        }

        private void ElementsP() {
            if(_token.TokenType == Token.Type.Comma) {
                _token = _lexer.Read();
                Elements();
            }
        }

        private void Value() {
            if (_token.TokenType == Token.Type.CurlyBraceOpen) {
                Object();
            } else if (_token.TokenType == Token.Type.SquareBraceOpen) {
                Array();
            } else if (_token.TokenType == Token.Type.StringLiteral || _token.TokenType == Token.Type.IntLiteral || _token.TokenType == Token.Type.FractionLiteral || _token.TokenType == Token.Type.TrueLiteral || _token.TokenType == Token.Type.FalseLiteral || _token.TokenType == Token.Type.NullLiteral) {
                _token = _lexer.Read();
            } else {
                throw new SyntaxException("Expected 'object', 'array', 'string', 'number', 'true', 'false', or 'null'", _token);
            }
        }
    }
}