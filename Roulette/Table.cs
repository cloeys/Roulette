using System.Collections.Generic;

namespace Roulette
{
    public class Table
    {
        public double TotalLimit { get; set; }
        public double MinimumBet { get; set; }
        private readonly Roulette _roulette;
        public IList<Tile> Tiles { get; set; }

        public Table()
        {
            CreateTilesList();
            _roulette = new Roulette(this);
        }

        private void CreateTilesList()
        {
            Tiles = new List<Tile> {new Tile {Value = "0"}};

            for (int numberOfBin = 1; numberOfBin < 37; numberOfBin++)
            {
                var mod = 0;
                // 1 -> 10 + 19 -> 28 => uneven is Red
                if ((numberOfBin >= 1 && numberOfBin <= 10) || (numberOfBin >= 19 && numberOfBin <= 28))
                    mod = 1;
                // 11 -> 18 + 29 -> 36 => even is Red
                else if ((numberOfBin >= 11 && numberOfBin <= 18) || (numberOfBin >= 29 && numberOfBin <= 36))
                    mod = 0;

                var color = numberOfBin % 2 == mod ? "Red" : "Black";


                Tiles.Add(new Tile{Value = numberOfBin.ToString(), Color = color});
            }

            Tiles.Add(new Tile{Value = "00"});
        }

        public Tile SpinRoulette()
        {
            return Tiles[_roulette.SpinRoulette()];
        }
    }
}
