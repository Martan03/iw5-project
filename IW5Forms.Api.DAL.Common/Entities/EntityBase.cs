namespace IW5Forms.Api.DAL.Common.Entities
{
    public abstract record EntityBase : IEntity
    {
        public required Guid Id { get; init; }
        public string? IdentityOwnerId { get; set; }
    }
}
