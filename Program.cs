namespace adventofcode2023;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        await Day2();
    }

    static async Task Day1()
    {
        Console.WriteLine("Welcome to the DAY 1!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadLinesAsync(@"input/day1.txt");
        var calibrationValues = inputText.Select(l => int.Parse($"{l.First(c => int.TryParse(c.ToString(), out int f))}{l.Last(c => int.TryParse(c.ToString(), out int f))}"));
        var sumOfValues = await calibrationValues.SumAsync();
        Console.WriteLine($"Sum of calibration values: {sumOfValues}");

        var setOfValidationValues = new Dictionary<string, int> {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
            {"1", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9}
        };

        var listOfCalibrationValues = new List<int>();

        await foreach (var line in inputText)
        {
            var firstIndexes = setOfValidationValues.Select(x => new KeyValuePair<int, int>(line.IndexOf(x.Key, 0), x.Value));
            var lastIndexes = setOfValidationValues.Select(x => new KeyValuePair<int, int>(line.LastIndexOf(x.Key, line.Length - 1), x.Value));

            var lowestFirstIndex = firstIndexes.Where(x => x.Key is not -1).OrderBy(x => x.Key).First();
            var highesLastIndex = lastIndexes.Where(x => x.Key is not -1).OrderByDescending(x => x.Key).First();

            listOfCalibrationValues.Add(int.Parse($"{lowestFirstIndex.Value}{highesLastIndex.Value}"));
        }

        var sumofNewValues = listOfCalibrationValues.Sum();
        Console.WriteLine($"Sum of new calibration values: {sumofNewValues}");
    }

    static async Task Day2()
    {
        Console.WriteLine("Welcome to the DAY 2!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var games = File.ReadLinesAsync(@"input/day2.txt");
        var cubeColorAndAmounts = new Dictionary<string, int>{
            {"blue", 14},
            {"red", 12},
            {"green", 13}
        };
        var gameNumber = 0;
        var possibleGames = new List<int>();
        await foreach (var game in games)
        {
            gameNumber++;
            var strippedString = game[(game.IndexOf(':') + 1)..].TrimStart();
            var rounds = strippedString.Split("; ");
            var isPossible = true;          
            foreach (var round in rounds)
            {
                var dicesPerRound = round.Split(", ");
                foreach (var dice in dicesPerRound)
                {
                    var amountAndColor = dice.Split(' ');
                    isPossible = cubeColorAndAmounts.First(x => x.Key == amountAndColor[1]).Value >= int.Parse(amountAndColor[0]);
                    if (!isPossible)
                        break;
                }
                if (!isPossible)
                    break;
            }
            if (isPossible)
                possibleGames.Add(gameNumber);
        }

        var sumOfPossibleGames = possibleGames.Sum();
        Console.WriteLine($"Sum of possible games: {sumOfPossibleGames}");

        gameNumber = 0;
        var gamesPower = new List<int>();
        await foreach (var game in games)
        {
            gameNumber++;
            var strippedString = game[(game.IndexOf(':') + 1)..].TrimStart();
            var rounds = strippedString.Split("; ");
            var minDicesNeededPerColor = new Dictionary<string, int>
            {
                {"blue", 0},
                {"green", 0},
                {"red", 0}
            };
            foreach (var round in rounds)
            {
                var dicesPerRound = round.Split(", ");
                foreach (var dice in dicesPerRound)
                {
                    var amountAndColor = dice.Split(' ');
                    var minDice = minDicesNeededPerColor.First(x => x.Key == amountAndColor[1]);
                    if (minDice.Value < int.Parse(amountAndColor[0]))
                    {
                        minDicesNeededPerColor.Remove(minDice.Key);
                        minDicesNeededPerColor.Add(minDice.Key, int.Parse(amountAndColor[0]));
                    }
                }
            }
            gamesPower.Add(minDicesNeededPerColor["red"] * minDicesNeededPerColor["green"] * minDicesNeededPerColor["blue"]);
        }

        var sumOfPowerOfGames = gamesPower.Sum();
        Console.WriteLine($"Sum of power of games: {sumOfPowerOfGames}");
    }
}