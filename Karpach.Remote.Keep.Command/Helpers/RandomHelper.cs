using System;

namespace Karpach.Remote.Keep.Command.Helpers
{
    public class RandomHelper
    {
        public string GetSessionId()
        {
            long random = LongRandom(1000000000, 9999999999, new Random());
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long timestamp = (long)t.TotalSeconds * 1000 + t.Milliseconds;
            return $"s--{timestamp}--{random}";
        }

        public string GetNodeId()
        {
            ulong random = LongRandom(new Random());
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long timestamp = (long)t.TotalSeconds * 1000 + t.Milliseconds;
            return $"{timestamp:x}.{random:x16}";
        }

        private long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }

        private ulong LongRandom(Random rand)
        {
            int part1 = rand.Next(int.MinValue, int.MaxValue);
            int part2 = rand.Next(int.MinValue, int.MaxValue);            
            ulong result = (ulong)part1 << 32 | (uint)part2;
            return result;
        }
    }
}