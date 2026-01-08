using System.Text;

namespace NewWordsApp
{
    internal class Program
    {
        static void Main()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            if (now.Hour != 13)
            {
                Console.WriteLine("不是确定的时间");
                return;
            }
            var wordsFile = "words.txt";
            var usedFile = "used_words.txt";
            var mdFile = "symbols";

            var allWords = ReadLines(wordsFile);
            var usedWords = File.Exists(usedFile)
                ? ReadLines(usedFile)
                : new HashSet<string>();

            var available = allWords.Except(usedWords).ToList();


            if (!available.Any())
            {
                Console.WriteLine("❌ 所有单词已用完");
                return;
            }

            var random = new Random();
            var word = available[0];
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            File.AppendAllText(
                mdFile,
             word + Environment.NewLine,
                Encoding.UTF8
            );

            File.AppendAllText(
                usedFile,
                word + Environment.NewLine,
                Encoding.UTF8
            );

            Console.WriteLine($"✅ 今日单词：{word}");
        }

        static HashSet<string> ReadLines(string path)
        {
            return File.ReadAllLines(path, Encoding.UTF8)
                       .Select(l => l.Trim())
                       .Where(l => !string.IsNullOrEmpty(l))
                       .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
