using System.Linq;
using Roulette.Exceptions;

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
            if (!HasEnoughCredits(bet.Amount)) throw new RouletteException("You don't have enough credits to place this bet");
            if (!IsOverMinimumBet(bet.Amount)) throw new RouletteException($"The bet is not over the minimum bet ({Game.Table.MinimumBet})");
            if (!BetDoesntExceedLimit(bet.Amount)) throw new RouletteException($"Placing this bet would exceed the table's bet limit ({Game.Table.TotalLimit})");
            Game.CurrentTurn.AddBet(bet);
            TotalCredits -= bet.Amount;
            return true;
        }

        public void AddCredits(double amount)
        {
            TotalCredits += amount;
        }

        private bool HasEnoughCredits(double amount)
        {
            return TotalCredits - amount >= 0;
        }

        private bool IsOverMinimumBet(double amount)
        {
            return (amount > Game.Table.MinimumBet);
        }

        private bool BetDoesntExceedLimit(double amount)
        {
            double total = Game.CurrentTurn.Bets.Where(b => b.Player == this).ToList().Sum(bet => bet.Amount);
            return amount + total <= Game.Table.TotalLimit;
        }

        public void CancelStrategy()
        {
            Strategy = null;
        }
    }
}
