using Roulette.Enums;

namespace Roulette.Bets
{
    public class DozenBet : Bet
    {
        private readonly Column _column;

        public DozenBet(Player player, Column column) : this(player, 0, column)
        {
        }

        public DozenBet(Player player, double amount, Column column) : base(3, player, amount)
        {
            _column = column;

            for (int i = ((int)_column-1)*12+1; i <= (int)_column * 12; i++)
            {
                Tiles.Add(player.Game.Table.Tiles[i]);
            }
        }

        public override string ToString()
        {
            return $"Dozen bet on {_column.ToString().ToLower()} dozen for $ {Amount}";
        }
    }
}
