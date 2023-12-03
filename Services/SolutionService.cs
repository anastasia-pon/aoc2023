using System.Text.RegularExpressions;

namespace aoc2023.Services;

public interface ISolutionService
{
    public int GetSolutionDay1(int part, string input);
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
}