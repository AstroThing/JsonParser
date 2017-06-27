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
        }
    }
}
