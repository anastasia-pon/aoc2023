using System.Text.RegularExpressions;

namespace aoc2023.Services;

public interface ISolutionService
{
    public int GetSolutionDay1(int part, string input);
    public int GetSolutionDay2(int part, string input);
    public int GetSolutionDay3(int part, string input);
    public int GetSolutionDay4(int part, string input);
    public int GetSolutionDay5(int part, string input);
}

public class SolutionService : ISolutionService
{
    public SolutionService()
    {
    }

    public int GetSolutionDay1(int part, string input)
    {
        Dictionary<string, int> textToDigitDictionary = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };
        
        var lines = input.Split("\n");
        var linesOfDigits = lines.Select(line =>
        {
            var pattern = @"[0-9]";
            if (part == 2)
            {
                pattern = @"[0-9]|one|two|three|four|five|six|seven|eight|nine";
            }

            var firstNumber = new Regex(pattern).Match(line).ToString();
            var lastNumber = new Regex(pattern, RegexOptions.RightToLeft).Match(line).ToString();
            
            if (firstNumber.Length == 0 || lastNumber.Length == 0) return 0;
            if (part == 2 && firstNumber.Length > 1)
            {
                firstNumber = textToDigitDictionary[firstNumber].ToString();
            }
            if (part == 2 && lastNumber.Length > 1)
            {
                lastNumber = textToDigitDictionary[lastNumber].ToString();
            }
            var doubleDigitString = firstNumber + lastNumber;
            
            return int.Parse(doubleDigitString);
        });

        return linesOfDigits.Sum();
    }
    
    public int GetSolutionDay2(int part, string input)
    {
        var redRegex = new Regex(@"(\d+)\sred");
        var greenRegex = new Regex(@"(\d+)\sgreen");
        var blueRegex = new Regex(@"(\d+)\sblue");
        var games = input.Split("\n");
        if (part == 1) return GetSolutionDay2Part1(games, redRegex, greenRegex, blueRegex);
        return GetSolutionDay2Part2(games, redRegex, greenRegex, blueRegex);
    }
    
    private int GetSolutionDay2Part1(string[] games, Regex redRegex, Regex greenRegex, Regex blueRegex)
    {
        var maxRed = 12;
        var maxGreen = 13;
        var maxBlue = 14;
        var gameRegex = new Regex(@"Game\s(\d+)");

        IEnumerable<int> possibleGames = games.Select(game =>
        {
            if (game.Length == 0) return 0;
            
            var rounds = game.Split(";");
            foreach (var round in rounds)
            {
                var redMatch = redRegex.Match(round);
                if (redMatch.Success)
                {
                    var numOfRed = int.Parse(redMatch.Groups[1].ToString());
                    if (numOfRed > maxRed) return 0;
                }
                
                var greenMatch = greenRegex.Match(round);
                if (greenMatch.Success)
                {
                    var numOfGreen = int.Parse(greenMatch.Groups[1].ToString());
                    if (numOfGreen > maxGreen) return 0;
                }
                
                var blueMatch = blueRegex.Match(round);
                if (blueMatch.Success)
                {
                    var numOfBlue = int.Parse(blueMatch.Groups[1].ToString());
                    if (numOfBlue > maxBlue) return 0;
                }
            }

            var gameMatch = gameRegex.Match(game).Groups[1].ToString();
            return int.Parse(gameMatch);
        });

        return possibleGames.Sum();
    }
    
    private int GetSolutionDay2Part2(string[] games, Regex redRegex, Regex greenRegex, Regex blueRegex)
    {
        var powerOfGames = games.Select(game =>
        {
            if (game.Length == 0) return 0;
            
            var rounds = game.Split(";");
            var redNum = 0;
            var greenNum = 0;
            var blueNum = 0;
            foreach (var round in rounds)
            {
                var redMatch = redRegex.Match(round);
                if (redMatch.Success)
                {
                    var redInRound = int.Parse(redMatch.Groups[1].ToString());
                    if (redInRound > redNum) redNum = redInRound;
                }
                
                var greenMatch = greenRegex.Match(round);
                if (greenMatch.Success)
                {
                    var greenInRound = int.Parse(greenMatch.Groups[1].ToString());
                    if (greenInRound > greenNum) greenNum = greenInRound;
                }
                
                var blueMatch = blueRegex.Match(round);
                if (blueMatch.Success)
                {
                    var blueInRound = int.Parse(blueMatch.Groups[1].ToString());
                    if (blueInRound > blueNum) blueNum = blueInRound;
                }
            }

            return redNum * greenNum * blueNum;
        });

        return powerOfGames.Sum();
    }
    
    public int GetSolutionDay3(int part, string input)
    {
        if (part == 1) return GetSolutionDay3Part1(input); 
        return GetSolutionDay3Part2(input); 
    }
    
    private int GetSolutionDay3Part1(string input)
    {
        var numberRegex = new Regex(@"(\d+)");
        var symbolRegex = new Regex(@"[^\d\.]");

        var lines = input.Split("\n");

        var partNumbers = new List<int>();

        foreach (var (line, lineIndex) in lines.Select((line, lineIndex) => ( line, lineIndex )))
        {
            var numberMatches = numberRegex.Matches(line);
            
            foreach (Match numberMatch in numberMatches)
            {
                var number = int.Parse(numberMatch.Groups[1].ToString());

                if (HasSymbol(numberMatch.Index, numberMatch.Value, line, symbolRegex))
                {
                    partNumbers.Add(number);
                    continue;
                }
                
                if (lineIndex > 0)
                {
                    var topLine = lines[lineIndex - 1];
                    var topLineNeighbors = topLine.Substring(numberMatch.Index, numberMatch.Value.Length);
                    if (HasSymbol(numberMatch.Index, topLineNeighbors, topLine, symbolRegex))
                    {
                        partNumbers.Add(number);
                        continue;
                    }
                }
                
                if (lineIndex < lines.Length - 1)
                {
                    var bottomLine = lines[lineIndex + 1];
                    if (bottomLine.Length < 1) continue;
                    var bottomLineNeighbors = bottomLine.Substring(numberMatch.Index, numberMatch.Value.Length);
                    if (HasSymbol(numberMatch.Index, bottomLineNeighbors, bottomLine, symbolRegex))
                    {
                        partNumbers.Add(number);
                    }
                }
            }
        }

        return partNumbers.Sum();
    }

    private bool HasSymbol(int numberStartsAt, string mainString, string line, Regex symbolRegex)
    {
        var mainStringWithMargins = mainString;
        if (numberStartsAt > 0)
        {
            var leftNeighbor = line[numberStartsAt - 1].ToString();
            mainStringWithMargins = leftNeighbor + mainString;
        }
        if (numberStartsAt + mainString.Length < 139)
        {
            var rightNeighbor = line[numberStartsAt + mainString.Length].ToString();
            mainStringWithMargins += rightNeighbor;
        }

        return symbolRegex.IsMatch(mainStringWithMargins);
    }
    
    private int GetSolutionDay3Part2(string input)
    {
        var starRegex = new Regex(@"\*");
        var numberRegex = new Regex(@"(\d+)");

        var lines = input.Split("\n");

        var gearRatios = new List<int>();

        foreach (var (line, lineIndex) in lines.Select((line, lineIndex) => ( line, lineIndex )))
        {
            var starMatches = starRegex.Matches(line);
            
            foreach (Match starMatch in starMatches)
            {
                var neighborNumbers = new List<int>();
                var starWithNeighborsSubstring = line.Substring(
                    Math.Max(0, starMatch.Index - 3), 
                    starMatch.Index + 3 > 139 ? 139 - starMatch.Index : 7);
                var starIndexInSubstring = starMatch.Index - 3 < 0 ? starMatch.Index : 3;
                
                var sideNumberMatches = numberRegex.Matches(starWithNeighborsSubstring);
                if (sideNumberMatches.Count > 0)
                {
                    foreach (Match sideNumberMatch in sideNumberMatches)
                    {
                        if (IsAdjacentToStar(starIndexInSubstring, sideNumberMatch))
                        {
                            neighborNumbers.Add(int.Parse(sideNumberMatch.Groups[1].ToString()));
                        }
                    }
                }

                if (lineIndex > 0)
                {
                    var topLine = lines[lineIndex - 1];
                    var topLineNeighbors = topLine.Substring(
                        Math.Max(0, starMatch.Index - 3), 
                        starMatch.Index + 3 > 139 ? 139 - starMatch.Index : 7);
                    var topNumberMatches = numberRegex.Matches(topLineNeighbors);
                    if (topNumberMatches.Count > 0)
                    {
                        foreach (Match topNumberMatch in topNumberMatches)
                        {
                            if (IsAdjacentToStar(starIndexInSubstring, topNumberMatch))
                            {
                                neighborNumbers.Add(int.Parse(topNumberMatch.Groups[1].ToString()));
                            }
                        }
                    }
                }
                
                if (lineIndex < lines.Length - 1)
                {
                    var bottomLine = lines[lineIndex + 1];
                    if (bottomLine.Length > 0)
                    {
                        var bottomLineNeighbors = bottomLine.Substring(
                            Math.Max(0, starMatch.Index - 3), 
                            starMatch.Index + 3 > 139 ? 139 - starMatch.Index : 7);
                        var bottomNumberMatches = numberRegex.Matches(bottomLineNeighbors);
                        if (bottomNumberMatches.Count > 0)
                        {
                            foreach (Match bottomNumberMatch in bottomNumberMatches)
                            {
                                if (IsAdjacentToStar(starIndexInSubstring, bottomNumberMatch))
                                {
                                    neighborNumbers.Add(int.Parse(bottomNumberMatch.Groups[1].ToString()));
                                }
                            }
                        }
                    }
                }
                
                if (neighborNumbers.Count == 2) gearRatios.Add(neighborNumbers[0] * neighborNumbers[1]);
            }
        }

        return gearRatios.Sum();
    }
    
    private bool IsAdjacentToStar(int starIndexInSubstring, Match numberMatch)
    {
        var startIndex = numberMatch.Index;
        var endIndex = numberMatch.Index + numberMatch.Value.Length - 1;
        return startIndex <= starIndexInSubstring + 1 && starIndexInSubstring - 1 <= endIndex;
    }
    
    public int GetSolutionDay4(int part, string input)
    {
        if (part == 1) return GetSolutionDay4Part1(input);
        return GetSolutionDay4Part2(input);
    }
    
    private int GetSolutionDay4Part1(string input)
    {
        var cards = input.Split("\n");
        var scores = new List<int>();

        foreach (var card in cards)
        {
            if (card.Length == 0) continue;
            var cardBody = card.Split(":")[1];
            var winningNumbers = cardBody.Split("|")[0];
            var availableNumbers = cardBody.Split("|")[1];
            var winningNumbersList = winningNumbers.Split(" ").Where(v => v != "").ToList();
            var availableNumbersList = availableNumbers.Split(" ").Where(v => v != "").ToList();
            var scoringNumbers = winningNumbersList.Intersect(availableNumbersList).ToList();
            switch (scoringNumbers.Count)
            {
                case 0:
                    continue;
                case 1:
                    scores.Add(1);
                    break;
                case > 1:
                {
                    var parsedNumbers = scoringNumbers.Select(int.Parse).ToList();
                    scores.Add((int)Math.Pow(2, parsedNumbers.Count - 1));
                    break;
                }
            }

            ;
        }
        return scores.Sum(); 
    }
    
    private int GetSolutionDay4Part2(string input)
    {
        var cards = input.Split("\n").Where(card => card != "");
        var cardDictionary = cards.ToDictionary(card => card.Split(":")[1], _ => 1);
        var totalCards = 0;
        
        foreach(var ((body, count), cardIndex) in cardDictionary.Select((value, i) => ( value, i )))
        {
            var winningNumbers = body.Split("|")[0];
            var availableNumbers = body.Split("|")[1];
            var winningNumbersList = winningNumbers.Split(" ").Where(v => v != "").ToList();
            var availableNumbersList = availableNumbers.Split(" ").Where(v => v != "").ToList();
            var scoringNumbers = winningNumbersList.Intersect(availableNumbersList).ToList().Count;
            if (scoringNumbers > 0)
            {
                for (var i = 1; i <= scoringNumbers; i++)
                {
                    var repeatedCard = cardDictionary.ElementAt(cardIndex + i);
                    cardDictionary[repeatedCard.Key] = repeatedCard.Value + count;
                }
            }
            totalCards += count;
        }
        
        return totalCards; 
    }
    
    public int GetSolutionDay5(int part, string input)
    {
        if (part == 1) return GetSolutionDay4Part1(input);
        return GetSolutionDay4Part2(input);
    }
}