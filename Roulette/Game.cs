﻿using System;
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

            string text = $"{player.Name}: ";

            return bets.Where(bet => bet.WinAmount > 0).Aggregate(text, (current, bet) => current + $"{bet} won {bet.WinAmount} \n");
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
            var placing = "Replacing bet(s):\n";
            var betStrings = "";
            var bets = new List<Bet>();
            try
            {
                var lastBetsForPlayer = TurnHistory[TurnHistory.Count - 2].Bets.Where(b => b.Player == player);
                foreach (var bet in lastBetsForPlayer)
                {
                    bets.Add(bet);
                    betStrings += bet + "\n";
                    //PlayerPlaceBet(player, (Bet)bet.Clone());
                    //placing += bet.ToString() + "\n";
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                return "No history to repeat bets";
            }

            var sum = bets.Sum(bet => bet.Amount);
            if (sum > player.TotalCredits) return $"Insufficient funds to repeat bet(s):\n{betStrings}";

            foreach (var bet in bets)
            {
                PlayerPlaceBet(player, (Bet)bet.Clone());
            }
            return placing + betStrings;

        }
    }
}
