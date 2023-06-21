using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTron.Compiler.Assembler.Frontend
{
    public class TextScanner
    {
        private StreamReader reader;

        public char NewLineCharacter { get; set; } = '\n';
        public char[] IgnoreCharacters { get; set; } = new char[] { '\t', '\v', '\r' };

        public TextScanner(Stream inputStream)
        {
            reader = new StreamReader(inputStream, true);

            PositionLine = 1;
        }

        public virtual void Read()
        {
            while (true)
            {
                LC = CC;

                var nextChar = (char)reader.Read();

                if (reader.Peek() == -1)
                {
                    NC = '\0';
                    break;
                }
                else
                {
                    NC = (char)reader.Peek();
                }

                if(nextChar == NewLineCharacter)
                {
                    continue;
                }

                CC = nextChar;

                PositionColumn++;



                break;
            };
        }

        public bool EndOfStream { get => NC == '\0' && reader.EndOfStream; }

        public int PositionLine { get; private set; }

        public int PositionColumn { get; private set; }

        public char LC { get; private set; }

        public char CC { get; private set; }

        public char NC { get; private set; }
    }
}
