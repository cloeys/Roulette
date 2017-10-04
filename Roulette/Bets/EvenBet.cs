using Roulette.Enums;

namespace Roulette.Bets
{
    public class EvenBet : Bet
    {
        private readonly Even _even;

        public EvenBet(Player player, Even even) : base(2, player)
        {
            _even = even;

            if (_even == Even.Even)
            {
                for (int i = 2; i < player.Game.Table.Tiles.Count; i += 2)
                    Tiles.Add(player.Game.Table.Tiles[i]);
            }
            else
            {
                for (int i = 1; i < player.Game.Table.Tiles.Count; i += 2)
                    Tiles.Add(player.Game.Table.Tiles[i]);
            }
        }

        public override string ToString()
        {
            return $"Even bet on {_even.ToString().ToLower()} for $ {Amount}";
        }
    }
}
