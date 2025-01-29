using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konves.ChordPro
{
    public class Whitespace : Block
    {
        public int Length { get; set; }

        public Whitespace(int length)
        {
            Length = length;
        }

        public override string ToString()
        {
            return new string(' ', Length);
        }
    }
}
