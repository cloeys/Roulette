using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class MartingaleStrategy : Strategy
    {

        public double OriginalAmount { get; set; } 

        public MartingaleStrategy() { }

        public MartingaleStrategy(Bet bet, int amountOfTurns) : base(bet, amountOfTurns)
        {
            OriginalAmount = bet.Amount;
        }

        protected override void ExecuteStrategy()
        {
            if (!Bet.HasWon)
            {
                Bet.Amount *= 2;
            }
            else
            {
                Bet.Amount = OriginalAmount;
            }
        }

        public override string ToString()
        {
            return $"Martingale Strategy: {Bet}, for {Bet.Amount} credits, turn {CurrentTurn}/{AmountOfTurns}";
        }
    }
}
