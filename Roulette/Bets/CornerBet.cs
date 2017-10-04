using System;
using System.Collections.Generic;
using Roulette.Exceptions;

namespace Roulette.Bets
{
    public class CornerBet : Bet
    {
        private readonly int _first;
        private readonly int _second;
        private readonly int _third;
        private readonly int _fourth;

        public CornerBet(Player player, Tile firstTile, Tile secondTile, Tile thirdTile, Tile fourthTile) : base(9, player)
        {
            _first = Int32.Parse(firstTile.Value);
            _second = Int32.Parse(secondTile.Value);
            _third = Int32.Parse(thirdTile.Value);
            _fourth = Int32.Parse(fourthTile.Value);

            List<int> values = new List<int> { _first, _second, _third, _fourth };
            values.Sort();

            if (IsSquare(values[0], values[1], values[2], values[3]))
            {
                Tiles.Add(firstTile);
                Tiles.Add(secondTile);
                Tiles.Add(thirdTile);
                Tiles.Add(fourthTile);
            }
            else
            {
                throw new RouletteException($"No corner found for tiles {values[0]}, {values[1]}, {values[2]}, {values[3]}");
            }
        }

        public CornerBet(Player player, double amount, Tile firstTile, Tile secondTile, Tile thirdTile, Tile fourthTile) : base(9, player, amount)
        {
            _first = Int32.Parse(firstTile.Value);
            _second = Int32.Parse(secondTile.Value);
            _third = Int32.Parse(thirdTile.Value);
            _fourth = Int32.Parse(fourthTile.Value);

            List<int> values = new List<int>{_first, _second, _third, _fourth};
            values.Sort();

            if (IsSquare(values[0], values[1], values[2], values[3]))
            {
                Tiles.Add(firstTile);
                Tiles.Add(secondTile);
                Tiles.Add(thirdTile);
                Tiles.Add(fourthTile);
            }
            else
            {
                throw new RouletteException($"No corner found for tiles {values[0]}, {values[1]}, {values[2]}, {values[3]}");
            }
        }

        private static bool IsSquare(int first, int second, int third, int fourth)
        {
            return IsNeighbourRow(first, second) && IsNeighbourRow(third, fourth) && IsNeighbourColumn(first, third) && IsNeighbourColumn(second, fourth);
        }

        private static bool IsNeighbourRow(int first, int second)
        {
            // Controle op twee getallen die horizontaal naast elkaar liggen
            return second - first == 1;
        }

        private static bool IsNeighbourColumn(int first, int second)
        {
            //Controle ofdat twee getallen verticaal naast elkaar liggen
            return second - first == 3;
        }

        public override string ToString()
        {
            return $"Corner bet on {_first}, {_second}, {_third} and {_fourth} for $ {Amount}";
        }
    }
}
