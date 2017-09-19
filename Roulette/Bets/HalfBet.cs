using Roulette.Enums;

namespace Roulette.Bets
{
    public class HalfBet : Bet
    {
        private readonly Half _half;

        public HalfBet(Player player, Half half) : base(2, player)
        {
            _half = half;

            if (_half == Half.FirstHalf)
            {
                for (int i = 1; i <= 18; i++)
                    Tiles.Add(player.Game.Table.Tiles[i]);
            }
            else
            {
                for (int i = 19; i < 36; i++)
                    Tiles.Add(player.Game.Table.Tiles[i]);
            }
        }

        public HalfBet(Player player, double amount, Half half) : base(2, player, amount)
        {
            _half = half;

            if (_half == Half.FirstHalf)
            {
                for (int i = 1; i <= 18; i++)
                    Tiles.Add(player.Game.Table.Tiles[i]);
            }
            else
            {
                for (int i = 19; i < 36; i ++)
                    Tiles.Add(player.Game.Table.Tiles[i]);
            }
        }

        public override string ToString()
        {
            return $"Half bet on {_half.ToString().ToLower()} half";
        }
    }
}
