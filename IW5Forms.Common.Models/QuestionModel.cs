namespace IW5Forms.Common.Models
{
    public enum QuestionType
    {
        Text,
        Select,
        Number
    }
    public record QuestionModel
    {
        public required Guid Id { get; set; }
        public required QuestionType QuestionType { get; set; }

        // text of the question (the actual question)
        public required string Text { get; set; }
        
        // further description of the question (optional)
        public string? Description { get; set; }
        public IList<AnswerModel> Answers { get; set; } = new List<AnswerModel>();
    }

}
