namespace IW5Forms.Api.DAL.Common.Entities
{
    public abstract record EntityBase
    {
        public required Guid Id { get; init; }
    }
}
