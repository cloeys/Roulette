using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roulette.Exceptions;

namespace Roulette
{
    public abstract class Strategy
    {
        public Bet Bet { get; set; }
        public int AmountOfTurns { get; set; }
        public int CurrentTurn { get; set; }

        protected Strategy(Bet bet, int amountOfTurns)
        {
            Bet = bet;
            AmountOfTurns = amountOfTurns;
            CurrentTurn = 0;
        }

        public void ApplyStrategy()
        {
            if (CurrentTurn < AmountOfTurns)
            {
                ExecuteStrategy();
                CurrentTurn++;
            }
            else
            {
                throw new RouletteException("Strategy has expired");
            }
        }

        protected abstract void ExecuteStrategy();
    }
}
