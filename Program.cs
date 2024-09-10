using System.Collections.Concurrent;
using System.Diagnostics;

namespace ParallelCompute;

class Program
{
    static void Main(string[] args)
    {
        var amounts = new[] { 100000, 1000000, 10000000 };
        foreach (var amount in amounts)
        {
            CalculateSerial(amount);
            CalculatePLINQ(amount);
            CalculateParallel(amount);
        }
    }

    private class Result()
    {
        public long Res { get; set; }
    }

    private static void CalculateSerial(int amount)
    {
        var sw = new Stopwatch();
        sw.Start();
        long sum1 = 0;
        var sum12 = Enumerable.Range(0,amount).Aggregate(sum1, (x,y)=>x+y);;
        sw.Stop();
        Console.WriteLine($"Время выполнения последовательного вычисления {sw.Elapsed} , sum = {sum12} для {amount}");
    }

    private static void CalculatePLINQ(int amount)
    {
        var sw = new Stopwatch();
        sw.Start();
        long sum2 = 0;
        var sum21 = Enumerable.Range(0,amount)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount)
            .Aggregate(sum2, (x,y)=>x+y);
        sw.Stop();
        Console.WriteLine($"Время выполнения PLINQ вычисления {sw.Elapsed} , sum = {sum21} для {amount}");
    }

    private static void CalculateParallel(int amount)
    {
        var initArr = Enumerable.Range(0, amount).ToArray();
        var partitions = 100;
        var results = new long[partitions];
        var sw = new Stopwatch();
        sw.Start();
        Parallel.For(0, partitions, (i) =>
        {
            long sum = 0;
            results[i] = initArr
                .Take(i * amount / partitions)
                .Skip((i - 1) * amount / partitions)
                .Aggregate(sum, (x,y)=>x+y);
        });
        var res = results.Sum();
        sw.Stop();
        Console.WriteLine($"Время выполнения паралелльного вычисления {sw.Elapsed}, sum = {res} для {amount}");
    }
  
    
}