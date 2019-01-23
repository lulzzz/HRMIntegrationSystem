namespace Common.Api.Contracts.Users
{
    public class User : IUser
    {
        public int? UserId { get; set; }
        public int? UnitId { get; set; }
        public int? CustomerId { get; set; }
        public string Email { get; set; }
        public string Mobilephone { get; set; }
        public string Workphone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsPersonalCustomerAdmin { get; set; }
    }
}
