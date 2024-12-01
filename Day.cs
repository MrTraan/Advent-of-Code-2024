using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal abstract class Day
    {
        public int DayNumber()
        {
            var match = Regex.Match(GetType().Name, @"\d+$");
            return match.Success ? int.Parse(match.Value) : 0;
        }

        public abstract long RunStep1(string input);
        public abstract long RunStep2(string input);

        public abstract bool Test();

        // Common utility functions
        public string ReadDayInput()
        {
            string filePath = Path.Combine("Resources", $"InputDay{DayNumber()}.txt");
            string content = File.ReadAllText(filePath);
            return content;
        }

        public bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public IEnumerable<string> ReadLines(string input)
        {
            using (System.IO.StringReader reader = new System.IO.StringReader(input))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public int[] ParseNumberList(ReadOnlySpan<char> input)
        {
            int numNumbers = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (IsDigit(input[i]))
                {
                    numNumbers++;
                    while (i < input.Length && IsDigit(input[i]))
                    {
                        i++;
                    }
                }
            }

            var res = new int[numNumbers];
            int resIdx = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (IsDigit(input[i]))
                {
                    int j = i + 1;
                    while (j < input.Length && IsDigit(input[j]))
                    {
                        j++;
                    }
                    res[resIdx++] = int.Parse(input.Slice(i, j - i));
                    i = j;
                }
            }
            Debug.Assert(resIdx == res.Length);
            return res;
        }

        static public (string, string) SplitInTwo(ReadOnlySpan<char> input, char delimiter)
        {
            int pivot = input.IndexOf(delimiter);
            var first = input.Slice(0, pivot);
            var second = input.Slice(pivot + 1);
            return (first.ToString(), second.ToString());
        }

        public static IEnumerable<TReturn> ParallelForEachWithProgressBar<TSource, TReturn>(IEnumerable<TSource> iterable, Func<TSource, TReturn> op)
        {
            var bag = new ConcurrentBag<TReturn>();

            var cancel = new CancellationToken();
            var watch = Stopwatch.StartNew();

            var task = Parallel.ForEachAsync(iterable, async (param, cancel) =>
            {
                TReturn res = op(param);
                bag.Add(res);
            });

            int total = iterable.Count();
            while (true)
            {
                DisplayProgressBar(bag.Count, total, watch);

                if (task.IsCompleted)
                {
                    Debug.Assert(bag.Count == iterable.Count());
                    break;
                }
                Thread.Sleep(50);
            }

            return bag;
        }

        static void DisplayProgressBar(int current, int total, Stopwatch watch)
        {
            unsafe
            {
                Console.CursorLeft = 0;
                int barLength = 50;
                int progress = (int)(((double)current / total) * barLength);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < progress && i < barLength; i++)
                {
                    sb.Append('#');
                }
                for (int i = progress; i < barLength; i++)
                {
                    sb.Append(' ');
                }

                Console.Write($"[{sb}] {current}/{total} {watch.Elapsed.ToString()}");
            }
        }
    }
}

