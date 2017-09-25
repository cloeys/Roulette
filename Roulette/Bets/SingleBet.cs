namespace Roulette.Bets
{
    public class SingleBet : Bet
    {
        private readonly Tile _tile;

        public SingleBet(Player player, Tile tile) : base(36, player)
        {
            _tile = tile;
            Tiles.Add(_tile);
        }

        public SingleBet(Player player, double amount, Tile tile) : base(36, player, amount)
        {
            _tile = tile;
            Tiles.Add(_tile);    
        }

        public override string ToString()
        {
            return $"Single bet on tile {_tile.Value}";
        }

    }
}
