// See https://aka.ms/new-console-template for more information
using AdventOfCode;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

Day?[] days = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Day)))
            .OrderBy(type =>
            {
                var match = Regex.Match(type.Name, @"\d+$");
                return match.Success ? int.Parse(match.Value) : 0;
            })
            .Select(type => (Day)Activator.CreateInstance(type))
            .ToArray();

int numErrors = 0;
Console.OutputEncoding = Encoding.UTF8;
foreach (Day day in days)
{

    var watch = Stopwatch.StartNew();
    string input = day.ReadDayInput();
    Rune Ok = new Rune(0x2705);
    Rune NotOk = new Rune(0x274c);
    try
    {
        bool testOk = day.Test();
        var (step1, step2) = day.Run(input);
        Console.ForegroundColor = ConsoleColor.Green;
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine("{0}: {1} | {2} | Test {3} ({4}ms)", day.GetType().Name, step1, step2, testOk ? Ok : NotOk, elapsedMs);
        Console.ForegroundColor = ConsoleColor.White;
    }
    catch (Exception ex)
    {
        numErrors++;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("{0}: FAILURE {1}", day.GetType().Name, ex.Message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
return numErrors;
