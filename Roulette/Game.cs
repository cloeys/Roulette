using System.Collections.Generic;
using System.Linq;

namespace Roulette
{
    public class Game
    {
        public Table Table = new Table();
        public Turn CurrentTurn;
        public List<Player> Players= new List<Player>();
        List<Turn> turnHistory = new List<Turn>();

        public void AddPlayer(Player player)
        {
            Players.Add(player);

        }

        public void StartTurn()
        {
            CurrentTurn = new Turn(this);
            turnHistory.Add(CurrentTurn);
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

            string text = $"{player.Name}: ";

            return bets.Where(bet => bet.WinAmount > 0).Aggregate(text, (current, bet) => current + $"{bet} won {bet.WinAmount} \n");
        }

        public bool PlayerPlaceBet(Player player, Bet bet)
        {
            return player.PlaceBet(bet);
        }
    }
}
