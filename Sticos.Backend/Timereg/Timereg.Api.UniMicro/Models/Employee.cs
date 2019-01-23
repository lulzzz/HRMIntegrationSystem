namespace Timereg.Api.Unimicro.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string SocialSecurityNumber { get; set; }
        public BusinessRelationInfo BusinessRelationInfo { get; set; }
        public EmailData DefaultEmail { get; set; }
        public int? SubEntityID { get; set; }
        public SubEntity SubEntity { get; set; }
        public bool Deleted { get; set; }
        
    }
}