using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchTime
{
    public class MenuNotFoundException : Exception
    {
        public MenuNotFoundException():base() { }
    }
}
