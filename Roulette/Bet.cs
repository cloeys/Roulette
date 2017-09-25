using System;
using System.Collections.Generic;

namespace Roulette
{
    public abstract class Bet : ICloneable
    {
        public double Amount { get; set; }
        public Player Player;
        public IList<Tile> Tiles = new List<Tile>();
        public double WinAmount { get; private set; }
        public bool HasWon { get; set; }

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
            if (!IsWinner(tile)) {return 0;}
            WinAmount = Amount * PayoutRate;
            HasWon = true;
            return WinAmount;
        }

        private bool IsWinner(Tile tile)
        {
            return Tiles.Contains(tile);
        }

        //public abstract object Clone();
        public object Clone()
        {
            var clone = this.MemberwiseClone();
            return clone;
        }
    }
}
