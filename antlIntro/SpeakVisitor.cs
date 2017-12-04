using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace antlIntro
{
    public class SpeakLine
    {
        public string Person { get; set; }
        public string Text { get; set; }
    }

    public class SpeakVisitor : SpeakBaseVisitor<object>
    {
        public List<SpeakLine> Lines = new List<SpeakLine>();

        public override object VisitLine(SpeakParser.LineContext context)
        {
            SpeakParser.NameContext name = context.name();
            SpeakParser.WordContext word = context.word();

            SpeakLine line = new SpeakLine() { Person = name.GetText(), Text = word.GetText() };
            Lines.Add(line);

            return line;
        }
    }
}
