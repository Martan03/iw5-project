namespace IW5Forms.Common.Models
{
    public record AnswerModel
    {
        public required Guid Id { get; set; }
        
        // Users answer
        public required string Text { get; set; }

        // UserId is nullable, in case incognito mode is enabled
        public Guid? UserId { get; set; }
    }
}
