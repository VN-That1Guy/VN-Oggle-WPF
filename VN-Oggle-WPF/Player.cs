using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFOggle
{
    public class Player
    {
        private string name = "Anonymous Player";

        private int points = 0;
        public string Name { get => name; set => name = value; }

        public int Points { get => points; set => points = value; }
    }
}
