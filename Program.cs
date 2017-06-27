using System;
using JsonParser.Lexical;
using JsonParser.Syntactical;

namespace JsonParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length == 0) {
                Console.WriteLine("No arguments given.");
                System.Environment.Exit(0);
            }

            Input input = new Input(args[0]);
            Lexer lexer = new Lexer(input);
            Parser parser = new Parser(lexer);

            try {
                parser.Parse();
            } catch(LexicalException exception) {
                Console.Error.WriteLine($"Lexical error: {exception.Message}");
            } catch(SyntaxException exception) {
                Console.Error.WriteLine($"Syntax error: {exception.Message}");
            }
        }
    }
}
