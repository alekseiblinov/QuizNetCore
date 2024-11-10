using System;
using System.Collections.Generic;

namespace quiz.ModelDb
{
    public partial class Question
    {
        public Question()
        {
            UserQuestionProgresses = new HashSet<UserQuestionProgress>();
        }

        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string Option01 { get; set; } = null!;
        public string? Option02 { get; set; }
        public string? Option03 { get; set; }
        public string? Option04 { get; set; }
        public string Answer { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual Topic Topic { get; set; } = null!;
        public virtual ICollection<UserQuestionProgress> UserQuestionProgresses { get; set; }
    }
}
