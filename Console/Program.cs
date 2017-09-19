using System;
using System.Linq;
using System.Threading;
using Roulette;
using Roulette.Bets;
using Roulette.Enums;
using Roulette.Exceptions;

namespace Console
{
    internal class Program
    {
        private const double START_CREDITS = 1000;
        public static Game Game;

        private static void Main(string[] args)
        {
            StartGame();
        }

        private static void StartGame()
        {
            Game = new Game();

            var aantalSpelers = 0;

            while (aantalSpelers <= 0)
            {
                System.Console.WriteLine("Number of players:");
                Int32.TryParse(System.Console.ReadLine(), out aantalSpelers);
            }

            for (int i = 1; i <= aantalSpelers; i++)
            {
                System.Console.WriteLine($"Player {i} enter name: ");
                var name = System.Console.ReadLine();
                Game.AddPlayer(new Player(Game, START_CREDITS, name));
            }

            PlayTurn();
            
        }

        private static void PlayTurn()
        {
            while (true)
            {
                Game.StartTurn();

                foreach (var player in Game.Players)
                {
                    System.Console.WriteLine($"{player.Name}, place your bets (Current credits: {player.TotalCredits}): ");
                    PlaceBet(player);

                }

                System.Console.Write("Spinning the wheel");
                for (int i = 0; i < 3; i++)
                {
                    System.Console.Write(".");
                    Thread.Sleep(500);
                }
                Game.PlayTurn();
                System.Console.WriteLine();
                System.Console.WriteLine($"The winning number is {Game.CurrentTurn.WinningTile.Color} {Game.CurrentTurn.WinningTile.Value}!");

                foreach (var player in Game.Players)
                {
                    System.Console.WriteLine(Game.GetResultsPlayer(player));
                }

                System.Console.WriteLine("Press enter to continue, 'exit' to quit");
                switch (System.Console.ReadLine())
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        System.Console.WriteLine("Starting new round!");
                        continue;
                }
            }
        }

        private static void PlaceBet(Player player)
        {
            var isDone = false;
            Bet bet = null;

            while (!isDone)
            {
                System.Console.WriteLine("Enter bet type:");
                var betType = System.Console.ReadLine();

                switch (betType?.ToLower())
                {
                    case "single":
                        SingleBet(player, ref bet);

                        break;
                    case "color":
                        ColorBet(player, ref bet);
                        break;
                    case "column":
                        ColumnBet(player, ref bet);
                        break;
                    case "corner":
                        CornerBet(player, ref bet);
                        break;
                    case "dozen":
                        DozenBet(player, ref bet);
                        break;
                    case "even":
                        EvenBet(player, ref bet);
                        break;
                    case "five":
                        FiveBet(player, ref bet);
                        break;
                    case "half":
                        HalfBet(player, ref bet);
                        break;
                    case "line":
                        LineBet(player, ref bet);
                        break;
                    case "split":
                        SplitBet(player, ref bet);
                        break;
                    case "street":
                        StreetBet(player, ref bet);
                        break;
                    case "done":
                        isDone = true;
                        break;
                    default:
                        System.Console.WriteLine("Unknown input, try again!");
                        break;
                }

                if (isDone || bet == null) continue;
                System.Console.WriteLine("Enter amount to bet:");
                var amount = double.Parse(System.Console.ReadLine() ?? throw new InvalidOperationException());

                bet.Amount = amount;

                System.Console.WriteLine(!Game.PlayerPlaceBet(player, bet)
                    ? $"Placing bet for player {player.Name} failed!"
                    : $"Placed bet for player {player.Name} (Current credits: {player.TotalCredits})!");
            }
        }

        private static void SingleBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter tile to bet on:");
            var value = System.Console.ReadLine();
            var tile = GetTileByValue(value);
            if (tile != null)
            {
                bet = new SingleBet(player, tile);
            }
            else
            {
                System.Console.WriteLine("Invalid tile, please try again!");
            }
        }

        private static void ColorBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter color to bet on:");
            var value = System.Console.ReadLine()?.ToLower();
            if (value == "red" || value == "black")
            {
                bet = new ColorBet(player, value);
            }
            else
            {
                System.Console.WriteLine("Invalid color, please try black or red!");
            }
        }

        private static void ColumnBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter column to bet on:");
            var value = System.Console.ReadLine()?.ToLower();

            if (Enum.TryParse(value, out Column column))
            {
                bet = new ColumnBet(player, column);
            }
            else
            {
                System.Console.WriteLine("Invalid column, please try 1, 2 or 3!");
            }
        }

        private static void CornerBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter corner numbers to bet on:");
            var value = System.Console.ReadLine()?.ToLower().Split(null);

            if (value?.Length == 4)
            {
                try
                {
                    bet = new CornerBet(player, GetTileByValue(value[0]), GetTileByValue(value[1]), GetTileByValue(value[2]), GetTileByValue(value[3]));
                }
                catch (RouletteException r)
                {
                    System.Console.WriteLine(r.Message);
                }
            }
            else
            {
                System.Console.WriteLine("Invalid corner, please try again!");
            }
        }

        private static void DozenBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which number of dozen to bet on:");
            var value = System.Console.ReadLine();

            if (Enum.TryParse(value, out Column column))
            {
                bet = new DozenBet(player, column);
            }
            else
            {
                System.Console.WriteLine("Invalid dozen, please try again!");
            }
        }

        private static void EvenBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter even/odd to bet on:");
            var value = System.Console.ReadLine();

            if (Enum.TryParse(value, out Even even))
            {
                bet = new EvenBet(player, even);
            }
            else
            {
                System.Console.WriteLine("Invalid entry, please try again!");
            }
        }

        private static void FiveBet(Player player, ref Bet bet)
        {
            bet = new FiveBet(player);
        }

        private static void HalfBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which half to bet on:");
            var value = System.Console.ReadLine();

            if (Enum.TryParse(value, out Half half))
            {
                bet = new HalfBet(player, half);
            }
            else
            {
                System.Console.WriteLine("Invalid half, please try again!");
            }
        }

        private static void LineBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which number of row to bet on:");
            var value = System.Console.ReadLine();
            if (Int32.TryParse(value, out int number) && number > 0 && number < 12)
            {
                bet = new LineBet(player, number);
            }
            else
            {
                System.Console.WriteLine("Invalid number of row, please try again!");
            }
        }

        private static void SplitBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter split numbers to bet on:");
            var value = System.Console.ReadLine()?.ToLower().Split(null);

            if (value?.Length == 2)
            {
                try
                {
                    bet = new SplitBet(player, GetTileByValue(value[0]), GetTileByValue(value[1]));
                }
                catch (RouletteException r)
                {
                    System.Console.WriteLine(r.Message);
                }
            }
            else
            {
                System.Console.WriteLine("Invalid split, please try again!");
            }
        }

        private static void StreetBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which number of street to bet on:");
            var value = System.Console.ReadLine();
            if (Int32.TryParse(value, out int number) && number > 0 && number < 11)
            {
                bet = new StreetBet(player, number);
            }
            else
            {
                System.Console.WriteLine("Invalid number of street, please try again!");
            }
        }

        private static Tile GetTileByValue(string value)
        {
            return Game.Table.Tiles.FirstOrDefault(t => t.Value == value);
        }
    }
}
