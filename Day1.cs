// See https://aka.ms/new-console-template for more information
using AdventOfCode;
using System.Diagnostics;

class Day1 : Day
{
    public override long RunStep1(string input)
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

        int delta = 0;

        for (int i = 0; i < left.Count; i++)
        {
            delta += Math.Abs(left[i] - right[i]);
        }

        return delta;
    }

    public override long RunStep2(string input)
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

        int delta = 0;

        for (int i = 0; i < left.Count; i++)
        {
            delta += left[i] * right.Count(x => x == left[i]);
        }

        return delta;
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
        
        long step1 = RunStep1(testInput);
        Debug.Assert(step1 == expected1);
        long step2 = RunStep2(testInput);
        Debug.Assert(step2 == expected2);
        return step1 == expected1 && step2 == expected2;
    }
}
