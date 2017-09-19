﻿using System.Collections.Generic;

namespace Roulette
{
    public abstract class Bet
    {
        public double Amount { get; set; }
        public Player Player;
        public IList<Tile> Tiles = new List<Tile>();
        public double WinAmount { get; private set; }

        protected readonly double PayoutRate;

        protected Bet(double payoutRate)
        {
            PayoutRate = payoutRate;
        }

        protected Bet(double payoutRate, double amount)
        {
            PayoutRate = payoutRate;
            Amount = amount;
        }

        protected Bet(double payoutRate, Player player)
        {
            PayoutRate = payoutRate;
            Player = player;
        }

        protected Bet(double payoutRate, Player player, double amount)
        {
            PayoutRate = payoutRate;
            Amount = amount;
            Player = player;
        }

        public double CalculatePayout(Tile tile)
        {
            if (!IsWinner(tile)) return 0;
            WinAmount = Amount * PayoutRate;
            return WinAmount;
        }

        private bool IsWinner(Tile tile)
        {
            return Tiles.Contains(tile);
        }
    }
}
