using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public struct DirectionContents
    {
        public Contents N{ get; set; }

        public Contents S { get; set; }

        public Contents E { get; set; }

        public Contents W { get; set; }

        public Contents Current { get; set; }
    }
}
