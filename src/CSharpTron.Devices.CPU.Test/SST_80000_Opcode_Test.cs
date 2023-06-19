using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTron.Devices.CPU.Test
{
    [TestClass]
    public class SST_80000_Opcode_Test
    {
        private void DefaultMemoryHandling(SST_8000 cpu, ref byte[] ram)
        {
            if (cpu.AddressBus >= ram.Length)
            {
                cpu.Halt();
                return;
            }

            if (cpu.DataBusMode == ModeBus.Read)
            {
                cpu.DataBus = ram[cpu.AddressBus];
            }
            else
            {
                ram[cpu.AddressBus] = cpu.DataBus;
            }
        }

        [TestMethod]
        public void Test_NOP()
        {
            var ram = new byte[] { 0 };
            var cpu = new SST_8000();
            cpu.DataBusAccess += delegate () { DefaultMemoryHandling(cpu, ref ram); };

            while (!cpu.HaltFlag)
            {
                cpu.Tick();
            }

            Assert.AreEqual(0, cpu.DataBus);
            Assert.AreEqual(1, cpu.AddressBus);
            Assert.AreEqual(1, cpu.ProgramCounter);
        }

        [TestMethod]
        public void Test_LDA()
        {
            var ram = new byte[]
            {
                20,         // LDA Val
                100,        // Value
            };

            var cpu = new SST_8000();
            cpu.DataBusAccess += delegate () { DefaultMemoryHandling(cpu, ref ram); };

            while (!cpu.HaltFlag)
            {
                cpu.Tick();
            }

            Assert.AreEqual(100, cpu.RegisterA);
            Assert.AreEqual(2, cpu.ProgramCounter);
        }

        [TestMethod]
        public void Test_LDX()
        {
            var ram = new byte[]
            {
                21,         // LDX Val
                100,        // Value
            };

            var cpu = new SST_8000();
            cpu.DataBusAccess += delegate () { DefaultMemoryHandling(cpu, ref ram); };

            while (!cpu.HaltFlag)
            {
                cpu.Tick();
            }

            Assert.AreEqual(100, cpu.RegisterX);
            Assert.AreEqual(2, cpu.ProgramCounter);
        }

        [TestMethod]
        public void Test_LDY()
        {
            var ram = new byte[]
            {
                22,         // LDY Val
                100,        // Value
            };

            var cpu = new SST_8000();
            cpu.DataBusAccess += delegate () { DefaultMemoryHandling(cpu, ref ram); };

            while (!cpu.HaltFlag)
            {
                cpu.Tick();
            }

            Assert.AreEqual(100, cpu.RegisterY);
            Assert.AreEqual(2, cpu.ProgramCounter);
        }
    }
}
