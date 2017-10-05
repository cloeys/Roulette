using System;
using System.Collections.Generic;
using System.Linq;

namespace Roulette
{
    public class Game
    {
        public Table Table = new Table();
        public Turn CurrentTurn;
        public List<Player> Players = new List<Player>();
        public List<Turn> TurnHistory = new List<Turn>();

        public Game()
        {
            
        }

        public Game(int minimumBet, int totalLimit)
        {
            Table.MinimumBet = minimumBet;
            Table.TotalLimit = totalLimit;
        }

        public Tile GetWinningTile()
        {
            return CurrentTurn.WinningTile;
        }

        public Tile GetWinningTile(Turn turn)
        {
            return turn.WinningTile;
        }

        public double GetCurrentBetAmount(Player player)
        {
            return CurrentTurn.Bets.Where(b => b.Player == player).ToList().Sum(b => b.Amount);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void StartTurn()
        {
            CurrentTurn = new Turn(this);
            TurnHistory.Add(CurrentTurn);
        }

        public Tile SpinRoulette()
        {
            return Table.SpinRoulette();
        }

        public void PlayTurn()
        {
            CurrentTurn.PlayTurn();
        }

        public void AddCreditsPlayer(double amount, Bet bet)
        {
            bet.Player.AddCredits(amount);
        }

        public string GetResultsPlayer(Player player)
        {
            var bets = CurrentTurn.Bets.Where(b => b.Player == player).ToList();
            if (!bets.Any(bet => bet.WinAmount > 0))
            {
                return $"{player.Name} didn't win anything this turn!";
            }

            string text = $"{player.Name}: \n";

            return bets.Where(bet => bet.WinAmount > 0).Aggregate(text, (current, bet) => current + $"\t{bet} won {bet.WinAmount} \n");
        }

        public bool PlayerPlaceBet(Player player, Bet bet)
        {
            return player.PlaceBet(bet);
        }

        public void AssignStrategy(Player player, Strategy strategy)
        {
            player.Strategy = strategy;
        }

        public string RepeatBet(Player player)
        {
            const string placing = "Rebetting:\n";
            var betStrings = "";
            var bets = new List<Bet>();
            try
            {
                var lastBetsForPlayer = TurnHistory[TurnHistory.Count - 2].Bets.Where(b => b.Player == player);
                foreach (var bet in lastBetsForPlayer)
                {
                    bets.Add(bet);
                    betStrings += bet + "\n";
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return "No history to repeat bets";
            }

            if (!bets.Any()) return "You didn't make any bets previous turn!";

            var sum = bets.Sum(bet => bet.Amount);
            if (sum > player.TotalCredits) return $"Insufficient funds to repeat bet(s):\n{betStrings}";

            foreach (var bet in bets)
            {
                PlayerPlaceBet(player, (Bet)bet.Clone());
            }
            return placing + betStrings;

        }

        public void AssignPlayerStrategy(Player player, Strategy strategy)
        {
            player.Strategy = strategy;
        }

        public Tile GetTileByValue(string value)
        {
            return Table.Tiles.FirstOrDefault(t => t.Value == value);
        }
    }
}
