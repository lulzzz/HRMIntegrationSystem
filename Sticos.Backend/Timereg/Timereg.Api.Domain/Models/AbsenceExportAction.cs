using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Timereg.Api.Domain.Models
{
    public enum AbsenceExportAction
    {
        Unknown = 0,
        Create = 10,
        Update = 20,
        Delete = 30
    }
}
