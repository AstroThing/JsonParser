using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonParser.Lexical {
    public class Lexer {
        private readonly Input _input;

        private static readonly Dictionary<string, Token.Type> _words;

        private static readonly char[] _escapes;

        static Lexer() {
            _escapes = new char[]{'"', '\\', '/', 'b', 'f', 'n', 'r', 't', 'u'};

            _words = new Dictionary<string, Token.Type>{
                {"true", Token.Type.TrueLiteral},
                {"false", Token.Type.FalseLiteral},
                {"null", Token.Type.NullLiteral}
            };
        }

        public Lexer(Input input) {
            _input = input;
        }

        private Symbol Next() {
            Symbol symbol;

            do {
                symbol = _input.Read();
            } while(char.IsWhiteSpace(symbol.Char));

            return symbol;
        }

        private Symbol Peek() {
            return _input.Peek();
        }

        public Token Read() {
            Symbol symbol = Next();
            List<Symbol> lexeme = new List<Symbol>{symbol};

            if(symbol.Char == '\0')
                return new Token(lexeme, Token.Type.EOF);

            if(symbol.Char == '{')
                return new Token(lexeme, Token.Type.CurlyBraceOpen);

            if(symbol.Char == '}')
                return new Token(lexeme, Token.Type.CurlyBraceClose);

            if(symbol.Char == '[')
                return new Token(lexeme, Token.Type.SquareBraceOpen);

            if(symbol.Char == ']')
                return new Token(lexeme, Token.Type.SquareBraceClose);

            if(symbol.Char == ':')
                return new Token(lexeme, Token.Type.Colon);

            if(symbol.Char == ',')
                return new Token(lexeme, Token.Type.Comma);

            if(symbol.Char == '"')
                return StringLiteral(lexeme);

            if(char.IsDigit(symbol.Char) || symbol.Char == '-')
                return NumberLiteral(lexeme);

            if(char.IsLetter(symbol.Char))
                return WordLiteral(lexeme);

            throw new LexicalException($"Unexpected token '{symbol}'", symbol);
        }

        private Token StringLiteral(List<Symbol> lexeme) {
            Symbol symbol;

             while(true) {
                symbol = _input.Read();

                if(symbol.Char == '\0' || symbol.Char == '\n' || symbol.Char == '\r')
                    throw new LexicalException("Unexected end of string", symbol);

                lexeme.Add(symbol);

                if(symbol.Char == '"') break;

                if(symbol.Char != '\\') continue;

                symbol = Next();

                if(_escapes.All(e => symbol.Char != e))
                    throw new LexicalException("Invalid escape character in string", symbol);

                if(symbol.Char == 'u') {
                    for(var i = 0; i < 4; i++) {
                        symbol = Next();
                        if(!symbol.IsHexDigit())
                            throw new LexicalException("Invalid unicode sequence in string", symbol);
                        lexeme.Add(symbol);
                    }
                }

                lexeme.Add(symbol);
            }

            return new Token(lexeme, Token.Type.StringLiteral);
        }

        private Token NumberLiteral(List<Symbol> lexeme) {
            ReadNumbers(lexeme);

            Symbol symbol = Peek();

            if(symbol.Char != '.') return new Token(lexeme, Token.Type.IntLiteral);

            lexeme.Add(Next());

            symbol = Peek();

            if(!char.IsDigit(symbol.Char)) throw new LexicalException("Unexpected end of number", symbol);

            ReadNumbers(lexeme);

            return new Token(lexeme, Token.Type.FractionLiteral);
        }

        private void ReadNumbers(List<Symbol> lexeme) {
            while(true) {
                Symbol symbol = Peek();

                if(!char.IsDigit(symbol.Char)) break;

                lexeme.Add(symbol);
                Next();
            }
        }

        private Token WordLiteral(List<Symbol> lexeme) {
            while(true) {
                Symbol symbol = Peek();

                if(!char.IsLetter(symbol.Char)) break;

                lexeme.Add(symbol);
                Next();
            }

            string key = string.Join("", lexeme);
            if(!_words.ContainsKey(key))
                throw new LexicalException("Expected 'String', 'Number', 'Null', 'True' or 'False'", lexeme.First());

            return new Token(lexeme, _words[key]);
        }
    }
}