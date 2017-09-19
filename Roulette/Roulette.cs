using System;

namespace Roulette
{
    public class Roulette
    {
        private readonly Table _table;

        public Roulette(Table table)
        {
            _table = table;
        }

        public int SpinRoulette()
        {
            return new Random().Next(_table.Tiles.Count);
        }
    }
}
