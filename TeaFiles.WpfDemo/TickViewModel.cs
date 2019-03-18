using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaFiles.WpfDemo
{
  class TickViewModel
    {
        public DateTime Time { get; set; }

        public double Price { get; set; }

        public int Volume { get; set; }

    }


    static class Mapper
    {
        public static TickViewModel Map(Tick tick)
        {
            return new TickViewModel { Time = tick.Time, Price = tick.Price, Volume = tick.Volume };

        }

    }

}
