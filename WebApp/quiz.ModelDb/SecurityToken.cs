using System;
using System.Collections.Generic;

namespace quiz.ModelDb
{
    public partial class SecurityToken
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = null!;
        public string? IpAddress { get; set; }
        public string MethodName { get; set; } = null!;
        public string MethodParametersHash { get; set; } = null!;
        public bool IsUsed { get; set; }
    }
}
