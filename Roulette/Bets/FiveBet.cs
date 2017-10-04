using System.Linq;

namespace Roulette.Bets
{
    public class FiveBet : Bet
    {
        public FiveBet(Player player) : this(player, 0)
        {
        }

        public FiveBet(Player player, double amount) : base(7, player, amount)
        {
            for (int i = 0; i <= 3; i++)
                Tiles.Add(player.Game.Table.Tiles[i]);

            Tiles.Add(player.Game.Table.Tiles.Last());
        }

        public override string ToString()
        {
            return $"Five bet for $ {Amount}";
        }
    }
}
