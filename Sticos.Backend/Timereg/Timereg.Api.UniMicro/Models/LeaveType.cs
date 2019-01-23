namespace Timereg.Api.Unimicro.Models
{
    public enum LeaveType
    {
        NotSet = 0, // Ikke valgt
        Leave = 1, // Permisjon
        LayOff = 2, // Permittering
        Leave_with_parental_benefit = 3, // Permisjon med foreldrepenger
        Military_service_leave = 4, // Permisjon ved militærtjenste
        Educational_leave = 5, // Utdanningspermisjon
        Compassionate_leave = 6, // Veldferdspermisjon
    }
}