using System;
using JsonParser.Lexical;

namespace JsonParser {
    public class LexicalException : Exception {
        public LexicalException(string msg, Symbol symbol) : base($"{msg}:({symbol.Line},{symbol.Column})") {}
    }
}