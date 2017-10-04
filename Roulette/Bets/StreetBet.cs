namespace Roulette.Bets
{
    public class StreetBet : Bet
    {
        private readonly int _row;

        public StreetBet(Player player, int row) : this(player, 0, row)
        {
        }

        public StreetBet(Player player, double amount, int row) : base(12, player, amount)
        {
            _row = row;

            if (_row >= 0 && _row <= 11)
            {
                for (int i = _row*3+1; i <= _row*3+3; i++)
                {
                    Tiles.Add(player.Game.Table.Tiles[i]);
                }
            }
        }

        public override string ToString()
        {
            return $"Street bet on row {_row+1} for $ {Amount}";
        }
    }
}
