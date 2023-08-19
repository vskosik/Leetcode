﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leetcode
{
    internal abstract class Program
    {
        private static void Main()
        {
            // Console.WriteLine(ConvertTime("02:30", "04:35"));
            // Console.WriteLine(CoinChange(new[] { 186, 419, 83, 408 }, 6249));
            // Console.WriteLine(ValidParentheses("]"));
            // Console.WriteLine(MaxProfit(new[] { 7, 1, 5, 3, 6, 4 }));
            // Console.WriteLine(Reverse(-123));
            // Console.WriteLine(MergeAlternately("abcd", "pq"));
            // Console.WriteLine(GcdOfStrings("ABCABCABC", "ABCABC"));
            KidsWithCandies(new[] { 2, 3, 5, 1, 3 }, 3).ToList().ForEach(Console.WriteLine);
        }

        private static IEnumerable<bool> KidsWithCandies(int[] candies, int extraCandies)
        {
            var maxCandies = candies.Max();

            return candies.Select(candy => candy + extraCandies >= maxCandies).ToList();
        }

        private static int Gcd(int a, int b)
        {
            return b == 0 ? a : Gcd(b, a % b);
        }

        private static string GcdOfStrings(string str1, string str2)
        {
            return str1 + str2 == str2 + str1
                ? str1[..Gcd(str1.Length, str2.Length)]
                : "";
        }

        private static string MergeAlternately(string word1, string word2)
        {
            var result = new StringBuilder(word1.Length + word2.Length);
            for (int i = 0, j = 0, k = 0; i < result.Capacity; i++)
            {
                if (i % 2 == 0)
                {
                    if (j >= word1.Length)
                    {
                        result.Append(word2[k..]);
                        break;
                    }

                    result.Append(word1[j++]);
                }
                else
                {
                    if (k >= word2.Length)
                    {
                        result.Append(word1[j..]);
                        break;
                    }

                    result.Append(word2[k++]);
                }
            }

            return result.Replace(" ", "").ToString();
        }

        private static int Reverse(int x)
        {
            long result = 0;
            while (x != 0)
            {
                result = result * 10 + x % 10;
                x /= 10;
            }

            if (result > int.MaxValue || result < int.MinValue)
            {
                return 0;
            }

            return (int)result;
        }

        private static int MaxProfit(int[] prices)
        {
            if (prices.Length == 0)
            {
                return 0;
            }

            var minPrice = prices[0];
            var maxProfit = 0;

            for (var i = 1; i < prices.Length; i++)
                if (prices[i] < minPrice)
                {
                    minPrice = prices[i];
                }
                else if (prices[i] - minPrice > maxProfit)
                {
                    maxProfit = prices[i] - minPrice;
                }

            return maxProfit;
        }

        private static bool ValidParentheses(string s)
        {
            var stack = new Stack<char>();
            var brackets = new Dictionary<char, char> { { '(', ')' }, { '{', '}' }, { '[', ']' } };

            foreach (var symbol in s)
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