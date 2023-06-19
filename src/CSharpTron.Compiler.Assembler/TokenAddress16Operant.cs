using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTron.Compiler.Assembler
{
    public sealed class TokenAddress16Operant : TokenOperantBase
    {       
        public ushort Address { get; set; }
    }
}
