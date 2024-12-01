// See https://aka.ms/new-console-template for more information
using AdventOfCode;
using System.Diagnostics;

class Day1 : Day
{
    public override (long, long) Run(string input)
    {
        var lines = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in lines)
        {
            var tuple = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            left.Add(int.Parse(tuple[0]));
            right.Add(int.Parse(tuple[1]));
        }

        left.Sort();
        right.Sort();

        int step1 = 0;
        int step2 = 0;

        for (int i = 0; i < left.Count; i++)
        {
            step1 += Math.Abs(left[i] - right[i]);
            step2 += left[i] * right.Count(x => x == left[i]);
        }

        return (step1, step2);
    }
    public override bool Test()
    {
        string testInput = @"
3   4
4   3
2   5
1   3
3   9
3   3
";
        
        long expected1 = 11, expected2 = 31;
        
        var (step1, step2) = Run(testInput);
        Debug.Assert(step1 == expected1);
        Debug.Assert(step2 == expected2);
        return step1 == expected1 && step2 == expected2;
    }
}
