namespace Roulette
{
    public class Player
    {
        public string Name { get; set; }
        public double TotalCredits { get; set; }
        public Strategy Strategy { get; set; }

        public Game Game { get; }

        public Player(Game game, double startAmount, string name)
        {
            Game = game;
            TotalCredits = startAmount;
            Name = name;
        }

        public bool PlaceBet(Bet bet)
        {
            if (!(TotalCredits - bet.Amount >= 0)) return false;
            Game.CurrentTurn.AddBet(bet);
            TotalCredits -= bet.Amount;
            return true;
        }

        public void AddCredits(double amount)
        {
            TotalCredits += amount;
        }
    }
}
