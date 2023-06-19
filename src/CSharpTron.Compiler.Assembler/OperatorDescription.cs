using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTron.Compiler.Assembler
{
    public sealed class OperatorDescription
    {
        public OperatorDescription(string memonic, byte opcode) : this(memonic, opcode, null, null)
        { }

        public OperatorDescription(string memonic, byte opcode, OperantDescription? operant1) : this(memonic, opcode, operant1, null)
        { }

        public OperatorDescription(string memonic, byte opcode, OperantDescription? operant1, OperantDescription? operant2)
        {
            Memonic = memonic;
            Opcode = opcode;
            Operant1 = operant1;
            Operant2 = operant2;
        }

        public string Memonic { get; private set; }
        public byte Opcode { get; private set; }
        public OperantDescription? Operant1 { get; private set; }
        public OperantDescription? Operant2 { get; private set; }
    }
}
