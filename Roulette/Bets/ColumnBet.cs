using System;
using Roulette.Enums;

namespace Roulette.Bets
{
    public class ColumnBet : Bet
    {
        private readonly Column _column;

        public ColumnBet(Player player, Column column) : base(3, player)
        {
            _column = column;

            for (int i = (int)column; i < 37; i += 3)
            {
                Tiles.Add(player.Game.Table.Tiles[i]);
            }
        }

        public ColumnBet(Player player, double amount, Column column) : base(3, player, amount)
        {
            _column = column;
            

            for (int i = (int)column; i < 37; i+=3)
            {
                Tiles.Add(player.Game.Table.Tiles[i]);
            }
        }

        public override string ToString()
        {
            return $"Column bet on {_column.ToString().ToLower()} column for $ {Amount}";
        }
    }
}
