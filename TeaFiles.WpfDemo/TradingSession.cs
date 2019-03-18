using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaFiles.WpfDemo
{
    using TeaTime;

    class TradingSession
    {
        public Time Begin;

        public Time End;

        public int TickCount;

        public TradingSession(Time time)
        {
            this.Begin = time.Date;
            this.End = this.Begin.AddDays(1);
        }

        public override string ToString()
        {
            return this.Begin + " " + this.TickCount;
        }
    }
}
