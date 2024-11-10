using System;
using System.Collections.Generic;

namespace quiz.ModelDb
{
    public partial class Topic
    {
        public Topic()
        {
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
