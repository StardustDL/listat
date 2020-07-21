using System;
using System.Collections.Generic;
using System.Text;

namespace Listat.Models
{
    public class Statistic
    {
        public string Id { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Payload { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;
    }
}
