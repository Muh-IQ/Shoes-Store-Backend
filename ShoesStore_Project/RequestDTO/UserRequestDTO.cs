namespace ShoesStore_Project.RequestDTO
{
    public class UserRequestDTO
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public IFormFile image { get; set; } 
    }
}
