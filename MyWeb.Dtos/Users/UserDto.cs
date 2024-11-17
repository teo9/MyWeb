namespace MyWeb.Dtos.Users
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }  
        public bool IsAdmin { get; set; } 
    }
}
