using System.Collections.Generic;
using System.Linq;

namespace Roulette
{
    public class Turn
    {
        private readonly Game _game;
        public List<Bet> Bets;
        public Tile WinningTile;
        public bool IsOver;

        public Turn(Game game)
        {
            _game = game;
            Bets = new List<Bet>();
        }

        public void AddBet(Bet bet)
        {
            if (_game.Players.Contains(bet.Player))
                Bets.Add(bet);
        }

        public void PlayTurn()
        {
            if (ArePlayersReady().Any()) return;
            WinningTile = _game.Table.SpinRoulette();
            CalculatePlayerPayout();

            IsOver = true;
        }

        private IEnumerable<Player> ArePlayersReady()
        {
            //return (from player in _game.Players let total = bets.Where(b => b.Player == player).ToList().Sum(bet => bet.Amount) where total < _game.Table.MinimumBet select player).ToList();

            List<Player> notReady = new List<Player>();

            foreach (var player in _game.Players)
            {
                double total = Bets.Where(b => b.Player == player).ToList().Sum(bet => bet.Amount);

                if (total < _game.Table.MinimumBet)
                {
                    notReady.Add(player);
                }
            }

            return notReady;
        }

        private void CalculatePlayerPayout()
        {
            foreach (var bet in Bets)
            {
                var winnings = bet.CalculatePayout(WinningTile);
                _game.AddCreditsPlayer(winnings, bet);
                bet.WinAmount = 0;
                bet.HasWon = false;
            }
        }
    }
}
