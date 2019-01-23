
namespace Common.Api.Contracts.Employees
{
    public interface IEmployee
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string JobTitle { get; }
        string Phone { get; }
        string Email { get; }
        string Image { get; }
        int? UserId { get; }
        int? UnitId { get; }
        string NationalIdentificationNumber { get; }
    }
    public class Employee : IEmployee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public int? UserId { get; set; }
        public int? UnitId { get; set; }
        public string NationalIdentificationNumber { get; set; }
    }
}