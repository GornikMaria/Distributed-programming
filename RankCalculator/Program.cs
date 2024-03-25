using System;
using System.Linq;
using NATS.Client;
using System.Text;
using StackExchange.Redis;

namespace RankCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = connectionMultiplexer.GetDatabase();

            ConnectionFactory cf = new ConnectionFactory();
            using IConnection c = cf.CreateConnection();

            var s = c.SubscribeAsync("valuator.processing.rank", "rank_calculator", (sender, args) =>
            {
                string id = Encoding.UTF8.GetString(args.Message.Data);

                string textKey = "TEXT-" + id;
                string text = db.StringGet(textKey);

                string rankKey = "RANK-" + id;
                double rank = GetRank(text);
                db.StringSet(rankKey, rank);
                Console.WriteLine(rankKey);
            });

            s.Start();

            Console.WriteLine("start RankCalculator");
            Console.ReadLine();
        }

        static double GetRank(string text)
        {
            if (text == null) {
                return 0;
            }
        int notLetterCharsCount = text.Where(ch => !char.IsLetter(ch)).Count();
        return notLetterCharsCount / (double) text.Length;
        }
    }
}