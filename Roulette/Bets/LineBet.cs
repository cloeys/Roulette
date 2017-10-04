namespace Roulette.Bets
{
    public class LineBet : Bet
    {
        private readonly int _row;

        public LineBet(Player player, int row) : base(6, player)
        {
            _row = row-1;

            if (_row >= 0 && _row <= 10)
            {
                for (int i = _row * 3 + 1; i <= _row * 3 + 6; i++)
                {
                    Tiles.Add(player.Game.Table.Tiles[i]);
                }
            }
        }

        public LineBet(Player player, double amount, int row) : base(6, player, amount)
        {
            _row = row-1;

            if (_row >= 0 && _row <= 10)
            {
                for (int i = _row * 3 + 1; i <= _row * 3 + 6; i++)
                {
                    Tiles.Add(player.Game.Table.Tiles[i]);
                }
            }
        }

        public override string ToString()
        {
            return $"Row bet on row {_row+1} of two rows for $ {Amount}";
        }
    }
}
