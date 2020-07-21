using System;

namespace Listat.Models
{
    public class StatisticQuery
    {
        public string? ID { get; set; }

        public DateTimeOffset? CreationTime { get; set; }

        public DateTimeOffset? ModificationTime { get; set; }

        public string? Payload { get; set; }

        public string? Category { get; set; }

        public string? Uri { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;
    }
}
