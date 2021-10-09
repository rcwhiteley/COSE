using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel
{
    public struct InterlockedBoolean
    {
        const int True = 1, False = 0;

        private int state;

        public bool State => BooleanFromInt(state);

        public InterlockedBoolean(bool initialState)
        {
            state = initialState ? True : False;
        }

        public bool CompareExchange(bool value, bool comparand)
        {
            int _value = IntFromBoolean(value);
            int _comparand = IntFromBoolean(comparand);
            int result = Interlocked.CompareExchange(ref state, _value, _comparand);
            return BooleanFromInt(result);
        }

        public bool Exchange(bool newValue)
        {
            return BooleanFromInt(Interlocked.Exchange(ref state, IntFromBoolean(newValue)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int IntFromBoolean(bool value)
        {
            return value ? True : False;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool BooleanFromInt(int value)
        {
            return value == True ? true : false;
        }
    }
}
