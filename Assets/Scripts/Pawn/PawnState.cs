using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspektML
{
    public enum Teams
    {
        TeamA, TeamB
    }

    public struct PawnState
    {
        public Tile CurrentTile;
        public bool IsAlive;
        public Teams Team;
    }
}
