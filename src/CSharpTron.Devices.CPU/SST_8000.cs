using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpTron.Devices.CPU
{
    //[DebuggerDisplay("AB={AddressBus}, DB={DataBus}, PC={ProgramCounter}, A={RegisterY}, X={RegisterX}, Y={RegisterY}")]
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class SST_8000 : CpuBase
    {
        private Stack<byte> stack = new Stack<byte>(20);

        private void DecodeOpCode()
        {
            switch (DataBus)
            {
                case 0:     // NOP
                    break;

                case 10:     // TAX
                    AddAction(() => Transfer_RegisterA_RegisterX());
                    break;
                case 11:     // TAY
                    AddAction(() => Transfer_RegisterA_RegisterY());
                    break;
                case 12:     // TXA
                    AddAction(() => Transfer_RegisterX_RegisterA());
                    break;
                case 13:     // TXY
                    AddAction(() => Transfer_RegisterX_RegisterY());
                    break;
                case 14:     // TYA
                    AddAction(() => Transfer_RegisterY_RegisterA());
                    break;
                case 15:     // TYX
                    AddAction(() => Transfer_RegisterY_RegisterX());
                    break;

                case 20:    // LDA Val
                    AddAction( Copy_ProgramCounter_AddressBus);
                    AddAction(Read_8);
                    AddAction(Increment_ProgramCounter);
                    AddAction(Copy_DataBus_RegisterA);
                    break;

                case 21:    // LDX Val
                    AddAction(() => Copy_ProgramCounter_AddressBus());
                    AddAction(() => Read_8());
                    AddAction(() => Increment_ProgramCounter());
                    AddAction(() => Copy_DataBus_RegisterX());
                    break;

                case 22:    // LDY Val
                    AddAction(() => Copy_ProgramCounter_AddressBus());
                    AddAction(() => Read_8());
                    AddAction(() => Increment_ProgramCounter());
                    AddAction(() => Copy_DataBus_RegisterY());
                    break;

                default:
                    Halt();
                    Exception();
                    break;
            }
        }

        private void Add_RegisterX_A()
        {
            var a = (int)RegisterA;
            var x = (int)RegisterX;

            var result = a + x;

            if (result > 255)
            {
                RegisterY = (byte)(result - 255);
                RegisterX = 255;
                CarryFlag = true;
            }
            else
            {
                RegisterX = (byte)result;
                RegisterY = 0;
                CarryFlag = false;
            }
        }

        private void Sub_RegisterX_A()
        {
            var a = (int)RegisterX;
            var b = (int)RegisterY;

            var result = a - b;

            if (result < 0)
            {
                RegisterY = (byte)(result * -1);
                RegisterX = 0;
                CarryFlag = true;
            }
            else
            {
                RegisterX = (byte)result;
                RegisterY = 0;
                CarryFlag = false;
            }
        }

        private void Transfer_RegisterA_RegisterX()
        {
            RegisterX = RegisterA;
        }

        private void Transfer_RegisterA_RegisterY()
        {
            RegisterY = RegisterA;
        }

        private void Transfer_RegisterX_RegisterA()
        {
            RegisterA = RegisterX;
        }

        private void Transfer_RegisterX_RegisterY()
        {
            RegisterY = RegisterX;
        }

        private void Transfer_RegisterY_RegisterA()
        {
            RegisterA = RegisterY;
        }

        private void Transfer_RegisterY_RegisterX()
        {
            RegisterX = RegisterY;
        }

        private void Copy_ProgramCounter_AddressBus()
        {
            AddressBus = ProgramCounter;
        }

        private void Copy_AdressBus_ProgramCounter()
        {
            ProgramCounter = AddressBus;
        }

        private void Copy_DataBus_RegisterA()
        {
            RegisterA = DataBus;
        }

        private void Copy_DataBus_RegisterX()
        {
            RegisterX = DataBus;
        }

        private void Copy_DataBus_RegisterY()
        {
            RegisterY = DataBus;
        }

        private void Read_Address_AddressBus()
        {
            Copy_ProgramCounter_AddressBus();
            Increment_ProgramCounter();
            Read_8();
            var high = DataBus;
            Copy_ProgramCounter_AddressBus();
            Increment_ProgramCounter();
            Read_8();
            var low = DataBus;

            AddressBus = (ushort)(low + (high << 8));
        }

        private void Increment_ProgramCounter()
        {
            ProgramCounter++;
        }

        private void Read_8()
        {
            DataBusMode = ModeBus.Read;
            DataBusAccess?.Invoke();
        }

        private void Write_8()
        {
            DataBusMode = ModeBus.Write;
            DataBusAccess?.Invoke();
        }

        protected override void OnFetch()
        {
            Copy_ProgramCounter_AddressBus();
            Read_8();

            if (HaltFlag)
            {
                return;
            }

            Increment_ProgramCounter();

            DecodeOpCode();
        }

        protected override void OnReset()
        {
            ProgramCounter = 0;
            DataBus = 0;
            AddressBus = 0;
            RegisterX = 0;
        }

        public event Action? DataBusAccess;

        public ModeBus DataBusMode { get; private set; }

        public byte DataBus { get; set; }

        public ushort AddressBus { get; private set; }

        public byte RegisterA { get; private set; }

        public byte RegisterX { get; private set; }

        public byte RegisterY { get; private set; }

        public byte StackCounter { get => (byte)stack.Count(); }

        public ushort ProgramCounter { get; private set; }

        private string GetDebuggerDisplay()
        {
            return $"AB={AddressBus}, DB={DataBus}, PC={ProgramCounter}, A={RegisterA}, X={RegisterX}, Y={RegisterY} ({base.GetDebuggerDisplay()})";
        }
    }
}
