using System;
using System.Collections.Generic;

namespace quiz.ModelDb
{
    public partial class UserQuestionProgress
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string UserId { get; set; } = null!;
        public Guid QuestionId { get; set; }
        public DateTime LastAnswered { get; set; }
        public bool IsCorrect { get; set; }
        public string? SelectedAnswerText { get; set; }
        public int Repetitions { get; set; }
        public int RepetitionInterval { get; set; }
        public DateTime NextDue { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
