namespace Common.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string JobPosition { get; set; }
        public string Image { get; set; }
        public int CustomerId { get; set; }
        public int UnitId { get; set; }
        public string Username { get; set; }
    }
}