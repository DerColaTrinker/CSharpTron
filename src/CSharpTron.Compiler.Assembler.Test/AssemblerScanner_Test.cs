using System.Net.Http.Headers;
using System.Text;

namespace CSharpTron.Compiler.Assembler.Frontend
{
    [TestClass]
    public class AssemblerScanner_Test
    {
        private string TestCode_1 = "Wort1";

        [TestMethod]
        public void CodeTest_1()
        {
            var scanner = new TextScanner(CreateStream(TestCode_1));
            var charList = new List<char>();

            while (!scanner.EndOfStream)
            {
                scanner.Read();

                charList.Add(scanner.CC);                
            }

            Assert.IsTrue(TestCode_1.ToCharArray().SequenceEqual(charList.ToArray()));
        }

        private Stream CreateStream(string code)
        {
            var bytes = new List<byte>();

            // Unicode BOM
            bytes.Add(0xff);
            bytes.Add(0xfe);
            bytes.AddRange(Encoding.Unicode.GetBytes(code));

            return new MemoryStream(bytes.ToArray());
        }
    }
}