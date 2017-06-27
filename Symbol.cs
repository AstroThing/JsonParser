namespace JsonParser.Lexical {
    public class Symbol {
        public char Char;

        public long Line;

        public long Column;

        public Symbol(char @char, long line, long column) {
            Char = @char;
            Line = line;
            Column = column;
        }

        public bool IsHexDigit() {
            return (Char >= '0' && Char <= '9') ||
            (Char >= 'a' && Char <= 'f') ||
            (Char >= 'A' && Char <= 'F');
        }
        
        public override string ToString() {
            return Char.ToString();
        }
    }
}