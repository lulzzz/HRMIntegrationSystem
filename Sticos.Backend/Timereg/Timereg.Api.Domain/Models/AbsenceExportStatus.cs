using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Timereg.Api.Domain.Models
{
    public enum AbsenceExportStatus
    {
        Unknown = 0,
        Initial = 5,
        Success = 10,
        Failed =  20,
        Obsolete= 30
    }
}
