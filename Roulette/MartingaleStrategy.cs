using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    class MartingaleStrategy : Strategy
    {

        public double OriginalAmount { get; set; } 

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
    }
}
