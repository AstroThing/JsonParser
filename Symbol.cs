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
        
        public override string ToString() {
            return Char.ToString();
        }
    }
}