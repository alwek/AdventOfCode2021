﻿// Inspired by answer found in https://www.reddit.com/r/adventofcode/comments/zkmyh4/2022_day_13_solutions/
using System.Text.Json;

namespace AdventOfCode2022
{
    internal static partial class Day13
    {
        public static void Run(string path)
        {
            Console.WriteLine("Day Thirteen");
            List<string> input = FileHelper.ReadFileAsList(path);
            
            Solve(input);
        }

        private static void Solve(List<string> input)
        {
            input = input.Where(x => !string.IsNullOrEmpty(x)).ToList();
            int indices = 0, index = 1;

            for(int i = 0; i < input.Count; i += 2) {
                JsonElement first = JsonSerializer.Deserialize<JsonElement>(input[i]);
                JsonElement second = JsonSerializer.Deserialize<JsonElement>(input[i + 1]);

                indices += Compare(first, second) < 0 ? index : 0;
                index++;
            }

            Console.WriteLine(indices);
        }

        private static int Compare(JsonElement first, JsonElement second) =>
            (first.ValueKind, second.ValueKind) switch {
                (JsonValueKind.Number, JsonValueKind.Number) => first.GetInt32() - second.GetInt32(),
                (JsonValueKind.Number, _) => DoCompare(JsonSerializer.Deserialize<JsonElement>($"[{first.GetInt32()}]"), second),
                (_, JsonValueKind.Number) => DoCompare(first, JsonSerializer.Deserialize<JsonElement>($"[{second.GetInt32()}]")),
                _ => DoCompare(first, second),
            };

        private static int DoCompare(JsonElement first, JsonElement second) {
            int result;
            JsonElement.ArrayEnumerator firstEnumerator = first.EnumerateArray();
            JsonElement.ArrayEnumerator secondEnumerator = second.EnumerateArray();

            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
                if ((result = Compare(firstEnumerator.Current, secondEnumerator.Current)) != 0)
                    return result;

            return first.GetArrayLength() - second.GetArrayLength();
        }
    }
}