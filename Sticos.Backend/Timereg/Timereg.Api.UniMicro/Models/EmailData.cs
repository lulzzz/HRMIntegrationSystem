namespace Timereg.Api.Unimicro.Models
{
    public class EmailData
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }

    public class SubEntity
    {
        public int Id { get; set; }
        public string OrgNumber { get; set; }
        public int? SuperiorOrganizationID { get; set; }
        public BusinessRelationInfo BusinessRelationInfo { get; set; }
        public bool Deleted { get; set; }
    }
}