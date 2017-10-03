using System;
using System.Collections.Generic;
using System.Linq;

namespace Roulette.Bets
{
    public class ColorBet : Bet
    {
        private readonly string _color;

        public ColorBet(Player player, string color) : this(player, 0, color)
        {
        }

        public ColorBet(Player player, double amount, string color) : base(2, player, amount)
        {
            _color = color;
            ((List<Tile>)Tiles).AddRange(player.Game.Table.Tiles.Where(x => x.Color != null && x.Color.Equals(color, StringComparison.InvariantCultureIgnoreCase)).ToList());
        }

        public override string ToString()
        {
            return $"Color bet on {_color} for $ {Amount}";
        }
    }
}
