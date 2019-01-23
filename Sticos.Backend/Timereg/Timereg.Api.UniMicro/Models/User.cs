namespace Timereg.Api.Unimicro.Models
{
    public class User
    {
        public int Id;
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public int UnitId { get; set; }
    }
}
