using System;
using System.Collections.Generic;

namespace quiz.ModelDb
{
    public partial class LogRecord
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
