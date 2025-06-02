using HMS.Shared.Enums;

namespace HMS.Shared.DTOs
{
    public class UserWithTokenDto
    {
        public string Token { get; set; } = null!;

        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public UserRole Role { get; set; }

        public string Name { get; set; } = null!;

        public string CNP { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
