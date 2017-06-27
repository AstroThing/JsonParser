using System;
using JsonParser.Lexical;

namespace JsonParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Input input = new Input("./sample.json");
            Lexer lexer = new Lexer(input);

            Token token = lexer.Read();

            while(token.TokenType != Token.Type.EOF) {
                Console.WriteLine(token);
                token = lexer.Read();
            }
        }
    }
}
