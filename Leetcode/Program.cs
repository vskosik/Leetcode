﻿using System;
using System.Collections.Generic;

namespace Leetcode
{
    internal abstract class Program
    {
        private static void Main()
        {
            //Console.WriteLine(ConvertTime("02:30", "04:35"));
            //Console.WriteLine(CoinChange(new[] { 186, 419, 83, 408 }, 6249));
            Console.WriteLine(ValidParentheses("]"));
        }

        private static bool ValidParentheses(string s)
        {
            var stack = new Stack<char>();
            var brackets = new Dictionary<char, char> { { '(', ')' }, { '{', '}' }, { '[', ']' } };

            foreach (var symbol in s)
            {
                if (brackets.ContainsKey(symbol))
                {
                    stack.Push(symbol);
                }
                else if (brackets.ContainsValue(symbol))
                {
                    if (stack.Count == 0 || brackets[stack.Pop()] != symbol)
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }

        private static int CoinChange(int[] coins, int amount)
        {
            var count = 0;
            Array.Sort(coins);
            Array.Reverse(coins);

            foreach (var coin in coins)
            {
                var tmp = amount / coin;
                amount -= tmp * coin;
                count += tmp;
            }

            if (amount != 0)
            {
                return -1;
            }

            return count;
        }

        private static int ConvertTime(string current, string correct)
        {
            var currentMinutes = int.Parse(current[..2]) * 60 + int.Parse(current[3..]);
            var correctMinutes = int.Parse(correct[..2]) * 60 + int.Parse(correct[3..]);

            var diff = correctMinutes - currentMinutes;
            var operators = new[] { 60, 15, 5, 1 };
            var count = 0;

            foreach (var op in operators)
            {
                count += diff / op;
                diff %= op;
            }

            return count;
        }
    }
}