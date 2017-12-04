using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace antlIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var input = "";
                var text = new StringBuilder();
                Console.WriteLine("Input the chat.");

                // to type the EOF character and end the input: use CTRL+D, then press <enter>
                while ((input = Console.ReadLine()) != "\u0004")
                {
                    text.AppendLine(input);
                }

                var inputStream = new AntlrInputStream(text.ToString());
                var speakLexer = new SpeakLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(speakLexer);
                var speakParser = new SpeakParser(commonTokenStream);

                var chatContext = speakParser.chat();
                var visitor = new SpeakVisitor();
                visitor.Visit(chatContext);

                foreach (var line in visitor.Lines)
                {
                    Console.WriteLine("{0} has said \"{1}\"", line.Person, line.Text);
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}
