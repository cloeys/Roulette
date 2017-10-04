using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class MartingaleStrategy : Strategy
    {

        protected override void ExecuteStrategy()
        {
            if (!Bet.HasWon)
            {
                Bet.Amount *= 2;
            }
            else
            {
                Bet.Amount = OriginalAmount;
                Bet.WinAmount = 0;
                Bet.HasWon = false;
            }
        }

        public override string ToString()
        {
            return $"Martingale Strategy: {Bet}, turn {CurrentTurn}/{AmountOfTurns}";
        }
    }
}
