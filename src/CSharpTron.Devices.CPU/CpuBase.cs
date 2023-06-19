using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTron.Devices.CPU
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public abstract class CpuBase
    {
        private Queue<Action> actions = new Queue<Action>();

        protected void AddAction(Action action)
        {
            actions.Enqueue(action);
        }

        public virtual void Tick()
        {
            if (HaltFlag)
            {
                return;
            }

            TickCounter++;

            GetActionName();

            if (actions.Count == 0)
            {
                OnFetch();
            }
            else
            {
                actions.Dequeue().Invoke();
            }
        }

        public virtual void Halt()
        {
            HaltFlag = true;
        }

        public virtual void Reset()
        {
            HaltFlag = false;
            OnReset();
        }

        protected void Exception()
        {
            ExceptionFlag = true;
        }

        protected abstract void OnReset();

        protected abstract void OnFetch();

        public bool HaltFlag { get; private set; }

        public bool ExceptionFlag { get; private set; }

        public bool CarryFlag { get; protected set; }

        public long TickCounter { get; private set; }

        protected string GetDebuggerDisplay()
        {
            return $"Queue={actions.Count}, CurrentAction={GetActionName()}";
        }

        private string GetActionName()
        {
            if (actions.Count == 0)
            {
                return "---";
            }

            var a = actions.Peek();


            return a.GetMethodInfo().Name;
        }
    }
}
