﻿using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students
        private readonly Dictionary<ulong, ulong> _cache = new Dictionary<ulong, ulong>
        {
            [0] = 0,
            [1] = 1,
            [2] = 1,
        };
        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (!_cache.TryGetValue(n, out ulong result))
            {
                return result;
            }
            _cache[n] = RecursiveWithMemoization(n - 2) + RecursiveWithMemoization(n - 1);
            return _cache[n];
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n < 2)
            {
                return n;
            }
            ulong temp1 = 1, tmp2 = 1;
            for (ulong i = 0; i < n; i++)
            {
                var temp = temp1 + tmp2;
                temp1 = tmp2;
                tmp2 = temp;
            }
            return tmp2;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
