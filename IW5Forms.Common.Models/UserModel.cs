namespace IW5Forms.Common.Models
{
    public enum UserRoles
    {
        User,
        Admin
    }
    public record UserModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }

        // Role is either normal user (User) or Admin
        public required UserRoles Role { get; set; }
        
        // Forms contain all accessible forms by this user and bool whether the form was filled by the user
        public IDictionary<FormModel, bool> Forms { get; set; } = new Dictionary<FormModel, bool>();
    }
}
