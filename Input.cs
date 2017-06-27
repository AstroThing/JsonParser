using System.IO;

namespace JsonParser.Lexical {
    public class Input {
        private readonly StreamReader _stream;

        private long _line = 1;

        private long _column;

        public Input(string path) {
            _stream = new StreamReader(File.OpenRead(path));
        }

        public Symbol Read() {
            var @byte = _stream.Read();

            if (@byte == -1) return new Symbol('\0', _line, _column);

            var character = (char)@byte;

            return character.Equals('\n') ? new Symbol('\n', _line++, _column = 1) : new Symbol(character, _line, _column++);
        }

        public Symbol Peek() {
            var @byte = _stream.Peek();

            if (@byte == -1) return new Symbol('\0', _line, _column);

            var character = (char)@byte;

            return character.Equals('\n') ? new Symbol('\n', _line + 1, 1) : new Symbol(character, _line, _column + 1);
        }
    }
}