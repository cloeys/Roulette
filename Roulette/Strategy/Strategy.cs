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

        protected Strategy() { }

        protected Strategy(Bet bet, int amountOfTurns)
        {
            Bet = bet;
            AmountOfTurns = amountOfTurns;
            CurrentTurn = 0;
        }

        public Bet ApplyStrategy()
        {
            if (CurrentTurn < AmountOfTurns)
            {
                if (Bet.Player.TotalCredits < Bet.Amount)
                {
                    Bet.Player.Strategy = null;
                    throw new RouletteException("Not enough credits to further apply strategy");
                }
                ExecuteStrategy();
                CurrentTurn++;
                return Bet;
            }
            else
            {
                Bet.Player.Strategy = null;
                throw new RouletteException("Strategy has expired");
            }
        }

        protected abstract void ExecuteStrategy();
        
    }
}
