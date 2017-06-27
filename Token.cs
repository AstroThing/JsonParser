using System.Collections.Generic;
using System.Linq;

namespace JsonParser.Lexical {
    public class Token {
        
        public enum Type {
            EOF,
            CurlyBraceOpen,
            CurlyBraceClose,
            SquareBraceOpen,
            SquareBraceClose,
            Colon,
            Comma,
            StringLiteral,
            FractionLiteral,
            IntLiteral,
            TrueLiteral,
            FalseLiteral,
            NullLiteral
        }

        public List<Symbol> Lexeme;

        public Type TokenType;

        public long Line => Lexeme.FirstOrDefault().Line;

        public long Column => Lexeme.FirstOrDefault().Column;

        public Token(List<Symbol> lexeme, Type type) {
            Lexeme = lexeme;
            TokenType = type;
        }

        public override string ToString() {
            return string.Join("", Lexeme);
        }
    }
}