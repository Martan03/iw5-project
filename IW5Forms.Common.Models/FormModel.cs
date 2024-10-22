namespace IW5Forms.Common.Models
{
    public record FormModel
    {
        public required Guid Id { get; set; }
        public required DateTime BeginTime { get; set; }
        public required DateTime EndTime { get; set; }

        // Incognito mode doesn't store UserId in Answers
        public required bool Incognito { get; set; }

        // SingleTry allows only one try per user
        public required bool SingleTry{ get; set; }

        // if SingleTry is true, UsersCompleted will store users which already completed the form
        public IList<UserModel>? UsersCompleted { get; set; }

        // stores Users which have access to this form
        public IList<UserModel> Users { get; set; } = new List<UserModel>();
        public IList<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        

    }
}
