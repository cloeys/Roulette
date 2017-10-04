using System;
using Roulette.Exceptions;

namespace Roulette.Bets
{
    public class SplitBet : Bet
    {
        private readonly int _first;
        private readonly int _second;

        public SplitBet(Player player, Tile firstTile, Tile secondTile) : this(player, 0, firstTile, secondTile)
        {
        }

        public SplitBet(Player player, double amount, Tile firstTile, Tile secondTile) : base(18, player, amount)
        {
            _first = Int32.Parse(firstTile.Value);
            _second = Int32.Parse(secondTile.Value);

            if (CheckNeighbour(_first, _second))
            {
                Tiles.Add(firstTile);
                Tiles.Add(secondTile);
            }
            else
            {
                throw new RouletteException("No split between the given tiles");
            }
        }

        private static bool CheckNeighbour(int first, int second)
        {
            return CheckMod(first, second) && CheckAb(first, second);
        }

        private static bool CheckMod(int first, int second)
        {
            // Controle op zelfde kolom, kolom +1 of kolom -1
            if (((first + 2) % 3 == (second + 2) % 3) || ((first + 2) % 3 == ((second + 2) % 3) + 1) || ((first + 2) % 3 == ((second + 2) % 3) - 1))
                return true;

            return false;
        }

        private static bool CheckAb(int first, int second)
        {
            //Controle op maximum 1 cijfer of 3 cijfers verschil
            if (Math.Abs(first - second) == 3 || Math.Abs(first - second) == 1)
                return true;

            return false;
        }

        public override string ToString()
        {
            return $"Split bet on tiles {_first} and {_second}";
        }
    }
}
