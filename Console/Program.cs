using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Flogging.Core;
using Microsoft.SqlServer.Server;
using Roulette;
using Roulette.Bets;
using Roulette.Enums;
using Roulette.Exceptions;

namespace Console
{
    internal class Program
    {
        private const double START_CREDITS = 1000;
        private const int MAX_PLAYERS = 4;
        private const int MINIMUM_BET = 0;
        private const int TOTAL_LIMIT = 100;
        private static string _error;
        private static string _message;
        public static Game Game;

        private static void Main(string[] args)
        {
            StartGame();
        }
        
        private static void ShowPlayerStack()
        {
            System.Console.Clear();
            var longest = Game.Players.Max(p => p.Name.Length);

            int aantalKarakters = 50 + longest;

            var stakes = $" Turn {Game.TurnHistory.Count} ";
            System.Console.WriteLine(new string('=', aantalKarakters));
            System.Console.WriteLine("||" + new string('-', aantalKarakters / 2 - stakes.Length / 2 - 2) + stakes + new string('-', aantalKarakters / 2 - stakes.Length / 2 - 2 + aantalKarakters % 2 - stakes.Length % 2) + "||");
            System.Console.WriteLine("||" + new string('=', aantalKarakters - 4) + "||");

            foreach (var player in Game.Players)
            {
                var playerLine = "";
                playerLine = player.Name + new string(' ', longest + 10 - player.Name.Length) + "$" +
                             new string(' ', 10 - player.TotalCredits.ToString().Length) +
                             player.TotalCredits;
                var info = $"||  {playerLine}";
                System.Console.WriteLine(info + new string(' ', aantalKarakters - info.Length - 4) + "  ||");
            }


            System.Console.WriteLine(new string('=', aantalKarakters));

            WriteMessage();
            WriteError();

            System.Console.WriteLine();
        }

        private static void WriteError()
        {
            if (!string.IsNullOrEmpty(_error))
            {
                var d = Flogger.GetFlogDetail(_error, null);
                Flogger.WriteError(d);
                System.Console.WriteLine($"\n Error: {_error}");
                _error = "";
            }
        }

        private static void WriteMessage()
        {
            if (!string.IsNullOrEmpty(_message))
            {
                var d = Flogger.GetFlogDetail(_message, null);
                Flogger.WriteDiagnostic(d);
                System.Console.WriteLine($"\n {_message}");
                _message = "";
            }
        }

        private static void StartGame()
        {
            Game = new Game();
            Game.Table.MinimumBet = MINIMUM_BET;
            Game.Table.TotalLimit = TOTAL_LIMIT;

            System.Console.WriteLine("\r\n  ______                                 __                             \r\n /      \\                               /  |                            \r\n/$$$$$$  |  ______    ______    ______  $$ |   __   ______    _______   \r\n$$ |  $$/  /      \\  /      \\  /      \\ $$ |  /  | /      \\  /       |  \r\n$$ |      /$$$$$$  |/$$$$$$  |/$$$$$$  |$$ |_/$$/  $$$$$$  |/$$$$$$$/   \r\n$$ |   __ $$    $$ |$$ |  $$ |$$    $$ |$$   $$<   /    $$ |$$      \\   \r\n$$ \\__/  |$$$$$$$$/ $$ \\__$$ |$$$$$$$$/ $$$$$$  \\ /$$$$$$$ | $$$$$$  |  \r\n$$    $$/ $$       |$$    $$ |$$       |$$ | $$  |$$    $$ |/     $$/   \r\n $$$$$$/   $$$$$$$/  $$$$$$$ | $$$$$$$/ $$/   $$/  $$$$$$$/ $$$$$$$/    \r\n                    /  \\__$$ |                                          \r\n                    $$    $$/                                           \r\n                     $$$$$$/                                            \r\n _______                       __              __      __               \r\n/       \\                     /  |            /  |    /  |              \r\n$$$$$$$  |  ______   __    __ $$ |  ______   _$$ |_  _$$ |_     ______  \r\n$$ |__$$ | /      \\ /  |  /  |$$ | /      \\ / $$   |/ $$   |   /      \\ \r\n$$    $$< /$$$$$$  |$$ |  $$ |$$ |/$$$$$$  |$$$$$$/ $$$$$$/   /$$$$$$  |\r\n$$$$$$$  |$$ |  $$ |$$ |  $$ |$$ |$$    $$ |  $$ | __ $$ | __ $$    $$ |\r\n$$ |  $$ |$$ \\__$$ |$$ \\__$$ |$$ |$$$$$$$$/   $$ |/  |$$ |/  |$$$$$$$$/ \r\n$$ |  $$ |$$    $$/ $$    $$/ $$ |$$       |  $$  $$/ $$  $$/ $$       |\r\n$$/   $$/  $$$$$$/   $$$$$$/  $$/  $$$$$$$/    $$$$/   $$$$/   $$$$$$$/ \r\n                                                                        \r\n                                                                        \r\n                                                                        \r\n");
            System.Console.ReadLine();
            System.Console.Clear();

            var aantalSpelers = 0;

            while (aantalSpelers <= 0 || aantalSpelers > MAX_PLAYERS)
            {
                System.Console.Clear();
                System.Console.WriteLine("Welcome to Cegekas Roulette!");
                System.Console.WriteLine();
                System.Console.WriteLine($"Number of players [1-{MAX_PLAYERS}]:");
                Int32.TryParse(System.Console.ReadLine(), out aantalSpelers);
            }

            for (var i = 1; i <= aantalSpelers; i++)
            {
                System.Console.Clear();
                System.Console.WriteLine("Welcome to Cegekas Roulette!");
                System.Console.WriteLine();
                var nameValid = false;
                var name = "";
                do
                {
                    System.Console.WriteLine($"Player {i} enter name: ");
                    name = System.Console.ReadLine();
                    if (Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                    {
                        nameValid = true;
                    }
                    else
                    {
                        System.Console.WriteLine("Please enter a valid name! (not empty and only letters)");
                    }

                } while (!nameValid);

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
                    if (player.Strategy != null)
                    {
                        try
                        {
                            var bet = player.Strategy.ApplyStrategy();
                            ShowPlayerStack();
                            System.Console.WriteLine($"Current strategy is active: {player.Strategy}\nDo you want to continue with this strategy? (enter \"no\" to cancel, anything to continue)");
                            var proceed = System.Console.ReadLine();
                            if (proceed != null && proceed.Equals("no"))
                            {
                                player.Strategy = null;
                                StrategyBetQuestion(player);
                            }
                            else
                            {
                                
                                Game.PlayerPlaceBet(player, bet);
                            }
                        }
                        catch (RouletteException e)
                        {
                            _message = e.Message;
                            StrategyBetQuestion(player);
                        }
                    }
                    else
                    {
                        StrategyBetQuestion(player);
                    }
                }

                ShowPlayerStack();

                System.Console.Write("Spinning the wheel");
                for (var i = 0; i < 3; i++)
                {
                    System.Console.Write(".");
                    Thread.Sleep(500);
                }
                Game.PlayTurn();
                System.Console.WriteLine("\n\n");
                System.Console.WriteLine($"The winning number is {Game.CurrentTurn.WinningTile.Color} {Game.CurrentTurn.WinningTile.Value}!");

                foreach (var player in Game.Players)
                {
                    System.Console.WriteLine("\n" + Game.GetResultsPlayer(player));
                }

                System.Console.WriteLine("\n\n\nPress enter to continue, 'exit' to quit");
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
                var repeatBet = false;
                ShowPlayerStack();

                System.Console.WriteLine($"{player.Name}, which bet do you want to place? ");
                System.Console.WriteLine("[1] Single\n[2] Color\n[3] Column\n[4] Corner\n[5] Dozen\n[6] Even\n[7] Five\n[8] Half\n[9] Line\n[10] Split\n[11] Street\n[12] Repeat bets from last game\n[0] Continue\n\nEnter bet type:");

                var betType = System.Console.ReadLine();

                switch (betType?.ToLower())
                {
                    case "1":
                        SingleBet(player, ref bet);
                        break;
                    case "2":
                        ColorBet(player, ref bet);
                        break;
                    case "3":
                        ColumnBet(player, ref bet);
                        break;
                    case "4":
                        CornerBet(player, ref bet);
                        break;
                    case "5":
                        DozenBet(player, ref bet);
                        break;
                    case "6":
                        EvenBet(player, ref bet);
                        break;
                    case "7":
                        FiveBet(player, ref bet);
                        break;
                    case "8":
                        HalfBet(player, ref bet);
                        break;
                    case "9":
                        LineBet(player, ref bet);
                        break;
                    case "10":
                        SplitBet(player, ref bet);
                        break;
                    case "11":
                        StreetBet(player, ref bet);
                        break;
                    case "12":
                        repeatBet = true;
                        break;
                    case "0":
                        isDone = true;
                        break;
                    default:
                        _error = "Please enter a valid command";
                        break;
                }
                if (repeatBet)
                {
                    var placed = Game.RepeatBet(player);
                    _message = placed;
                    continue;
                }

                if (isDone || bet == null || !string.IsNullOrEmpty(_error)) continue;

                var amountValid = false;
                do
                {
                    var total = Game.CurrentTurn.Bets.Where(b => b.Player == player).ToList().Sum(b => b.Amount);

                    System.Console.WriteLine($"Enter amount to bet (MIN: {Game.Table.MinimumBet} MAX: {Game.Table.TotalLimit - total}):");
                    if (double.TryParse(System.Console.ReadLine(), out var amount))
                    {
                        bet.Amount = amount;
                        amountValid = true;
                    }
                    else
                    {
                        System.Console.WriteLine("Please enter a valid amount!");
                    }
                } while (!amountValid);

                try
                {
                    Game.PlayerPlaceBet(player, bet);

                    if (player.Strategy != null)
                    {
                        player.Strategy.OriginalAmount = bet.Amount;
                        player.Strategy.Bet = bet;
                        player.Strategy.CurrentTurn++;
                        _message = $"Placed bet {bet} on current strategy for player {player.Name}";
                        isDone = true;
                    }
                    else
                    {
                        _message = $"Placed bet for player {player.Name} ({bet})!";
                    }
                }
                catch (RouletteException e)
                {
                    _error = e.Message;
                }
            }
        }

        private static void StrategyBetQuestion(Player player)
        {
            ShowPlayerStack();
            System.Console.WriteLine($"{player.Name}, how do you want to place a bet?\n[1] Bet\n[2] Strategy\n[0] Continue without bet\n\nEnter answer:");
            var answer = System.Console.ReadLine();
            switch (answer)
            {
                case "1":
                    PlaceBet(player);
                    break;
                case "2":
                    CreateStrategy(player);
                    break;
                case "0":
                    break;
                default:
                    _error = "Please enter a valid command";
                    StrategyBetQuestion(player);
                    break;
            }
        }

        private static void CreateStrategy(Player player)
        {
            ShowPlayerStack();
            Strategy strategy = null;
            System.Console.WriteLine($"What kind of strategy do you want to use?\n[1] Martingale\n[2] Bet without strategy\n[0] Return to previous menu");
            var answer = System.Console.ReadLine();
            switch (answer)
            {
                case "1":
                    strategy = new MartingaleStrategy();
                    break;
                case "2":
                    PlaceBet(player);
                    break;
                case "0":
                    StrategyBetQuestion(player);
                    break;
                default:
                    _error = "Please enter a valid command";
                    CreateStrategy(player);
                    break;
            }

            if (strategy != null)
            {
                AssignStrategyToPlayer(player, strategy);
            }
        }

        private static void AssignStrategyToPlayer(Player player, Strategy strategy)
        {
            var amountValid = false;
            do
            {
                System.Console.WriteLine("Enter amount of turns to use the bet:");
                if (int.TryParse(System.Console.ReadLine(), out var amount))
                {
                    strategy.AmountOfTurns = amount;
                    amountValid = true;
                }
                else
                {
                    System.Console.WriteLine("Please enter a valid amount!");
                }
            } while (!amountValid);
            player.Strategy = strategy;
            PlaceBet(player);
        }

        private static void SingleBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter tile to bet on [00, 0-36]:");
            var value = System.Console.ReadLine();
            var tile = GetTileByValue(value);
            if (tile != null)
            {
                bet = new SingleBet(player, tile);
            }
            else
            {
                _error = "Invalid tile, please enter valid tile number!";
            }
        }

        private static void ColorBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter color to bet on:");
            System.Console.WriteLine("[1] Red");
            System.Console.WriteLine("[2] Black");
            var value = System.Console.ReadLine()?.ToLower();
            switch (value)
            {
                case "1":
                    bet = new ColorBet(player, "red");
                    break;
                case "2":
                    bet = new ColorBet(player, "black");
                    break;
                default:
                    _error = "Invalid color, please try [1] for Red or [2] for Black!";
                    break;
            }
        }

        private static void ColumnBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter column to bet on:");
            System.Console.WriteLine("[1] First");
            System.Console.WriteLine("[2] Second");
            System.Console.WriteLine("[3] Third");
            var value = System.Console.ReadLine()?.ToLower();
            int valueInt;

            if (Int32.TryParse(value, out valueInt) && Enum.IsDefined(typeof(Column), valueInt))
            {
                Column column = (Column)Enum.ToObject(typeof(Column), valueInt);
                bet = new ColumnBet(player, column);
            }
            else
            {
                _error = "Invalid column, please try [1], [2] or [3]!";
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
                    _error = r.Message;
                }
            }
            else
            {
                _error = "Invalid corner, please try again!";
            }
        }

        private static void DozenBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which number of dozen to bet on:");
            var value = System.Console.ReadLine();

            if(Enum.IsDefined(typeof(Column), int.Parse(value)))
            {
                Column column = (Column)Enum.ToObject(typeof(Column), int.Parse(value));
                bet = new DozenBet(player, column);
            }
            else
            {
                _error = "Invalid dozen, please try again!";
            }
        }

        private static void EvenBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter even/odd to bet on:");
            var value = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(System.Console.ReadLine());

            if (Enum.TryParse(value, out Even even))
            {
                bet = new EvenBet(player, even);
            }
            else
            {
                _error = "Invalid entry, please try again!";
            }
        }

        private static void FiveBet(Player player, ref Bet bet)
        {
            bet = new FiveBet(player);
        }

        private static void HalfBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which half to bet on:");
            var value = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(System.Console.ReadLine());

            if (Enum.TryParse(value, out Half half))
            {
                bet = new HalfBet(player, half);
            }
            else
            {
                _error = "Invalid half, please try again!";
            }
        }

        private static void LineBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which number of row to bet on:");
            var value = System.Console.ReadLine();
            if (int.TryParse(value, out var number) && number > 0 && number < 12)
            {
                bet = new LineBet(player, number);
            }
            else
            {
                _error = "Invalid number of row, please try again!";
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
                    _error = r.Message;
                }
            }
            else
            {
                _error = "Invalid split, please try again!";
            }
        }

        private static void StreetBet(Player player, ref Bet bet)
        {
            System.Console.WriteLine("Enter which number of street to bet on:");
            var value = System.Console.ReadLine();
            if (int.TryParse(value, out var number) && number > 0 && number < 11)
            {
                bet = new StreetBet(player, number);
            }
            else
            {
                _error = "Invalid number of street, please try again!";
            }
        }

        private static Tile GetTileByValue(string value)
        {
            return Game.Table.Tiles.FirstOrDefault(t => t.Value == value);
        }

    }
}
