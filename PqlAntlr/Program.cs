using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace PqlAntlr
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                do
                {
                    var input = "";
                    var text = new StringBuilder();
                    Console.WriteLine("Input the PQL.");

                    // to type the EOF character and end the input: use CTRL+D, then press <enter>
                    while ((input = Console.ReadLine()) != "\u0004")
                        text.AppendLine(input);

                    var inputStream = new AntlrInputStream(text.ToString());
                    var lexer = new PQLLexer(inputStream);
                    var commonTokenStream = new CommonTokenStream(lexer);
                    var parser = new PQLParser(commonTokenStream);

                    var pqlExpr = parser.pqlStmtList();
                    var visitor = new PqlExprVisitor();
                    visitor.Visit(pqlExpr);

                    foreach (var line in visitor.Lines)
                        Console.WriteLine(line.Content);
                    // Console.ReadKey();
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }

    public class PqlBaseExpr
    {
        public PqlBaseExpr(string text)
        {
            Content = text;
        }

        public string Content { get; set; }
    }

    internal class PqlExprVisitor : PQLBaseVisitor<object>
    {
        public List<PqlBaseExpr> Lines = new List<PqlBaseExpr>();

        public override object VisitPqlStmt(PQLParser.PqlStmtContext context)
        {
            Lines.Add(new PqlBaseExpr($"pql stmt: {context.GetText()} is of type: {context.GetType()}"));                
            return base.VisitPqlStmt(context);
        }
    }
}