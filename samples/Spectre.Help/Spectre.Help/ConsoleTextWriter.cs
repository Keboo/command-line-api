using System.CommandLine.IO;
using System.IO;
using System.Text;

namespace Spectre.Help
{
    public class ConsoleTextWriter : TextWriter
    {
        public ConsoleTextWriter(IStandardStreamWriter writer)
        {
            Writer = writer;
        }

        public override void Write(char value)
        {
            Writer.Write(value.ToString());
        }

        public IStandardStreamWriter Writer { get; }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
