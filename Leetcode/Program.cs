using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;

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
            // KidsWithCandies(new[] { 2, 3, 5, 1, 3 }, 3).ToList().ForEach(Console.WriteLine);
            // Console.WriteLine(CanPlaceFlowers(new[] { 0, 0, 1, 0, 0 }, 1));
            // Console.WriteLine(ReverseVowels("leetcode"));
            // Console.WriteLine(ReverseWords("  hello world  "));
            // ProductExceptSelf(new[] { 1, 2, 3, 4 }).ToList().ForEach(Console.WriteLine);
            // Console.WriteLine(IncreasingTriplet(new[] { 1, 5, 0, 4, 1, 3 }));
            // Console.WriteLine(Compress(new[] { 'a', 'a', 'b', 'b', 'c', 'c', 'c' }));
            // var nums = new[] { 0, 1, 0, 3, 12 };
            // MoveZeroes(nums);
            // nums.ToList().ForEach(Console.WriteLine);
            // Console.WriteLine(IsSubsequence("abc", "ahbgdc"));
            // Console.WriteLine(MaxArea(new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 }));
            // Console.WriteLine(MaxOperations(new[] { 3, 1, 3, 4, 3 }, 4));
            // Console.WriteLine(FindMaxAverage(new[] { 1, 12, -5, -6, 50, 3 }, 4));
            // Console.WriteLine(MaxVowels("ibpbhixfiouhdljnjfflpapptrxgcomvnb", 33));
            // Console.WriteLine(LongestOnes(new[] { 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0 }, 2));
            // Console.WriteLine(LongestSubarray(new[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 }));
            // Console.WriteLine(LargestAltitude(new[] { -4, -3, -2, -1, 4, 3, 2 }));
            // Console.WriteLine(PivotIndex(new[] { -1, -1, 0, 1, 1, 0 }));
            // FindDifference(new[] { 1, 2, 3 }, new[] { 2, 4, 6 }).ToList()
            //     .ForEach(x => x.ToList().ForEach(Console.WriteLine));
            // Console.WriteLine(UniqueOccurrences(new[] { -3, 0, 1, -3, 1, 1, 1, -3, 10, 0 }));
            // Console.WriteLine(CloseStrings("abbzccca", "babzzczc"));
            // Console.WriteLine(EqualPairs(new[] { new[] { 11, 1 }, new[] { 1, 11 } }));
            // Console.WriteLine(RemoveStars("leet**cod*e"));
            // AsteroidCollision(new[] { 1, -2, -2, -2 }).ToList().ForEach(Console.WriteLine);
            Console.WriteLine(DecodeString("3[z]2[2[y]pq4[2[jk]e1[f]]]ef"));
        }

        private static string DecodeString(string s)
        {
            var repeat = 0;
            var subStr = new StringBuilder(s.Length);
            var subStack = new Stack<(int start, int repeat)>();

            foreach(var c in s)
            {
                switch (c)
                {
                    case '[':
                        subStack.Push((subStr.Length, repeat));
                        repeat = 0;
                        break;
                    case ']':
                    {
                        var (start, times) = subStack.Pop();

                        for(var length = subStr.Length - start; times > 1; times--)
                        {
                            subStr.Append(subStr, start, length);
                        }

                        break;
                    }
                    default:
                    {
                        if(char.IsDigit(c))
                        {
                            repeat = 10 * repeat + (c - '0');
                        }
                        else
                        {
                            subStr.Append(c);
                        }

                        break;
                    }
                }
            }

            return subStr.ToString();
        }

        private static int[] AsteroidCollision(int[] asteroids)
        {
            var result = new Stack<int>();
            foreach (var asteroid in asteroids)
            {
                if (result.Count == 0)
                {
                    result.Push(asteroid);
                    continue;
                }

                switch (result.Peek() > 0)
                {
                    case true when asteroid > 0:
                        result.Push(asteroid);
                        continue;
                    case true:
                    {
                        var lastPop = 0;
                        while (result.TryPeek(out var peek))
                        {
                            if (peek < 0)
                            {
                                result.Push(asteroid);
                                break;
                            }

                            if (Math.Abs(asteroid) > peek)
                            {
                                result.Pop();
                                continue;
                            }

                            if (Math.Abs(asteroid) == peek)
                            {
                                lastPop = result.Pop();
                            }

                            break;
                        }

                        if (result.Count == 0 && lastPop != Math.Abs(asteroid))
                        {
                            result.Push(asteroid);
                        }

                        continue;
                    }
                    default:
                        result.Push(asteroid);
                        continue;
                }
            }

            return result.Reverse().ToArray();
        }

        private static string RemoveStars(string s)
        {
            var result = new Stack<char>();
            foreach (var ch in s)
            {
                if (ch == '*')
                {
                    result.Pop();
                    continue;
                }

                result.Push(ch);
            }

            return string.Join("", result.Reverse());
        }

        private static int EqualPairs(int[][] grid)
        {
            var count = 0;
            var rows = new Dictionary<string, int>();

            foreach (var row in grid)
            {
                var key = string.Join(",", row);
                rows[key] = 1 + (rows.TryGetValue(key, out var qty) ? qty : 0);
            }

            for (var i = 0; i < grid.Length; i++)
            {
                var colArr = new int[grid.Length];
                for (var j = 0; j < grid.Length; j++)
                {
                    colArr[j] = grid[j][i];
                }

                count += rows.TryGetValue(string.Join(",", colArr), out var qty) ? qty : 0;
            }

            return count;
        }

        private static bool CloseStrings(string word1, string word2)
        {
            if (word1.Length != word2.Length)
            {
                return false;
            }

            var charCount1 = new int[26];
            var charCount2 = new int[26];

            foreach (var ch in word1)
            {
                charCount1[ch - 'a']++;
            }

            foreach (var ch in word2)
            {
                charCount2[ch - 'a']++;
            }

            for (var i = 0; i < 26; i++)
            {
                if ((charCount1[i] == 0) != (charCount2[i] == 0))
                {
                    return false;
                }
            }

            Array.Sort(charCount1);
            Array.Sort(charCount2);

            for (var i = 0; i < 26; i++)
            {
                if (charCount1[i] != charCount2[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static IList<IList<int>> FindDifference(int[] nums1, int[] nums2)
        {
            IList<IList<int>> result = new List<IList<int>> { new List<int>(), new List<int>() };

            var hashNums1 = new HashSet<int>(nums1);
            var hashNums2 = new HashSet<int>(nums2);

            foreach (var num in hashNums1.Where(num => !hashNums2.Contains(num)))
            {
                result[0].Add(num);
            }

            foreach (var num in hashNums2.Where(num => !hashNums1.Contains(num)))
            {
                result[1].Add(num);
            }

            return result;
        }

        private static int PivotIndex(int[] nums)
        {
            var leftSum = 0;
            var rightSum = nums[1..].Sum();
            for (var pivotIndex = 0; pivotIndex < nums.Length; pivotIndex++)
            {
                if (leftSum == rightSum)
                {
                    return pivotIndex;
                }

                leftSum += nums[pivotIndex];
                rightSum -= pivotIndex + 1 == nums.Length ? 0 : nums[pivotIndex + 1];
            }

            return -1;
        }

        private static int LargestAltitude(int[] gain)
        {
            var maxAltitude = 0;
            var altitude = 0;

            foreach (var alt in gain)
            {
                altitude += alt;
                maxAltitude = Math.Max(maxAltitude, altitude);
            }

            return maxAltitude;
        }

        private static int LongestSubarray(int[] nums)
        {
            var maxCount = 0;
            var zeros = 0;
            var left = 0;

            for (var right = 0; right < nums.Length; right++)
            {
                if (nums[right] == 0)
                {
                    zeros++;
                }

                while (zeros > 1)
                {
                    if (nums[left] == 0)
                    {
                        zeros--;
                    }

                    left++;
                }

                maxCount = Math.Max(maxCount, right - left);
            }

            return maxCount;
        }

        private static int LongestOnes(int[] nums, int k)
        {
            var count = 0;
            var maxCount = 0;
            var zeroIndex = 0;

            foreach (var num in nums)
            {
                if (num == 1)
                {
                    count++;
                    continue;
                }

                if (k > 0)
                {
                    k--;
                    count++;
                    continue;
                }

                maxCount = Math.Max(maxCount, count);

                for (; nums[zeroIndex] != 0; zeroIndex++)
                {
                    count--;
                }

                zeroIndex++;
            }

            maxCount = Math.Max(maxCount, count);

            return maxCount;
        }

        private static int MaxVowels(string s, int k)
        {
            var allVowels = new[] { 'a', 'e', 'i', 'o', 'u' };

            var vowels = 0;
            for (var i = 0; i < k; i++)
            {
                if (allVowels.Contains(s[i]))
                {
                    vowels++;
                }
            }

            var maxVowels = vowels;
            for (var i = k; i < s.Length; i++)
            {
                if (allVowels.Contains(s[i]))
                {
                    vowels++;
                }

                if (allVowels.Contains(s[i - k]))
                {
                    vowels--;
                }

                maxVowels = Math.Max(maxVowels, vowels);
            }

            return maxVowels;
        }

        private static double FindMaxAverage(int[] nums, int k)
        {
            var sum = 0;
            for (var i = 0; i < k; i++)
            {
                sum += nums[i];
            }

            double result = sum;
            for (var i = k; i < nums.Length; i++)
            {
                sum += nums[i] - nums[i - k];
                result = Math.Max(result, sum);
            }

            return result / k;
        }

        private static int MaxOperations(int[] nums, int k)
        {
            var numCounts = new Dictionary<int, int>();
            var operationCount = 0;

            foreach (var num in nums)
            {
                if (numCounts.ContainsKey(k - num) && numCounts[k - num] > 0)
                {
                    operationCount++;
                    numCounts[k - num]--;
                }
                else
                {
                    if (!numCounts.ContainsKey(num))
                    {
                        numCounts[num] = 0;
                    }

                    numCounts[num]++;
                }
            }

            return operationCount;
        }

        private static int MaxArea(int[] height)
        {
            var maxVolume = 0;

            for (int i = 0, j = height.Length - 1; i < j;)
            {
                maxVolume = Math.Max(maxVolume, Math.Min(height[i], height[j]) * (j - i));

                if (height[i] < height[j])
                {
                    i++;
                }
                else
                {
                    j--;
                }
            }

            return maxVolume;
        }

        private static bool IsSubsequence(string s, string t)
        {
            var subStr = new StringBuilder(s);
            for (var i = 0; i < t.Length && subStr.Length > 0; i++)
            {
                if (t[i] == subStr[0])
                {
                    subStr.Remove(0, 1);
                }
            }

            return subStr.Length == 0;
        }

        private static void MoveZeroes(int[] nums)
        {
            var nonZeroCount = 0;
            for (var i = 0; i < nums.Length; i++)
            {
                if (nums[i] != 0)
                {
                    nums[nonZeroCount++] = nums[i];
                }
            }

            for (var i = nonZeroCount; i < nums.Length; i++)
            {
                nums[i] = 0;
            }
        }

        private static int Compress(char[] chars)
        {
            var count = 1;
            var currentLetter = chars[0];
            var compressedString = new StringBuilder();

            for (var i = 1; i < chars.Length; i++)
            {
                if (chars[i] == currentLetter)
                {
                    count++;
                    continue;
                }

                compressedString.Append(currentLetter);
                if (count > 1)
                {
                    compressedString.Append(count.ToString());
                }

                currentLetter = chars[i];
                count = 1;
            }

            compressedString.Append(currentLetter);
            if (count > 1)
            {
                compressedString.Append(count.ToString());
            }

            for (var i = 0; i < compressedString.Length; i++)
            {
                chars[i] = compressedString[i];
            }

            return compressedString.Length;
        }

        private static bool IncreasingTriplet(int[] nums)
        {
            if (nums.Length < 3)
            {
                return false;
            }

            var smallest = int.MaxValue;
            var secondSmallest = int.MaxValue;

            foreach (var number in nums)
            {
                if (number <= smallest)
                {
                    smallest = number;
                }
                else if (number <= secondSmallest)
                {
                    secondSmallest = number;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private static int[] ProductExceptSelf(int[] nums)
        {
            var product = new int[nums.Length];
            int num = 1, i;

            for (i = 0; i < nums.Length; i++)
            {
                product[i] = num;
                num *= nums[i];
            }

            num = 1;

            for (i = nums.Length - 1; i >= 0; i--)
            {
                product[i] *= num;
                num *= nums[i];
            }

            return product;
        }

        private static string ReverseWords(string s)
        {
            return string.Join(' ', s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse());
        }

        private static string ReverseVowels(string s)
        {
            var allVowels = new[] { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
            var vowels = new Stack<char>();

            foreach (var symbol in s.Where(symbol => allVowels.Contains(symbol)))
            {
                vowels.Push(symbol);
            }

            var newString = new StringBuilder(s);
            for (var i = 0; i < newString.Length; i++)
            {
                if (allVowels.Contains(newString[i]))
                {
                    newString[i] = vowels.Pop();
                }
            }

            return newString.ToString();
        }

        private static bool CanPlaceFlowers(int[] flowerbed, int n)
        {
            for (var i = 0; i < flowerbed.Length && n > 0; i++)
            {
                if (flowerbed[i] == 1)
                {
                    continue;
                }

                if ((i == 0 || flowerbed[i - 1] == 0) &&
                    (i == flowerbed.Length - 1 || flowerbed[i + 1] == 0))
                {
                    flowerbed[i] = 1;
                    n--;
                }
            }

            return n == 0;
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