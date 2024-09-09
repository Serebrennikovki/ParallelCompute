using System.Diagnostics;

namespace ParallelCompute;

class Program
{
    static void Main(string[] args)
    {
        var sw = new Stopwatch();
        var amounts = new[] { 100000, 1000000, 10000000 };
        foreach (var amount in amounts)
        {
            sw.Start();
            long sum1 = 0;
            var sum12 = Enumerable.Range(0,amount).Aggregate(sum1, (x,y)=>x+y);;
            sw.Stop();
            Console.WriteLine($"Время выполнения последовательного вычисления {sw.Elapsed} , sum = {sum12} для {amount}");
            sw.Reset();
            sw.Start();
            long sum2 = 0;
            var sum21 = Enumerable.Range(0,amount).AsParallel().Aggregate(sum2, (x,y)=>x+y);
            sw.Stop();
            Console.WriteLine($"Время выполнения LINQ вычисления {sw.Elapsed} , sum = {sum21} для {amount}");
            sw.Reset();
            sw.Start();
            var res = new Result() { res = 0 };
            Parallel.For(0, amount, (i)=>
            {
                lock (res)
                {
                    res.res += i;
                } 
            }); 
            sw.Stop();
            Console.WriteLine($"Время выполнения паралелльного вычисления {sw.Elapsed}, sum = {res.res} для {amount}");
        }
        
        
    }

    private class Result()
    {
        public long res { get; set; }
    }
  
    
}