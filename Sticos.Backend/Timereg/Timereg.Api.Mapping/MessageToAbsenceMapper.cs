using Sticos.Personal.MessageContracts;
using System.Collections.Generic;
using System.Linq;
using Timereg.Api.Contracts;

namespace Timereg.Api.Mapping
{
    public static class MessageToAbsenceMapper
    {
        private const int LunchInMinutes = 0;

         public static Absence CreateAbsenceFromMessage(IAbsenceMessageData message)
        {
            var absenceEntries = message.AbsenceEntries?.Select(entry => new AbsenceEntry
            {
                ExternalAbsenceCode = null,
                ExternalEntityId = null,
                ExternalId = null,
                IsFullDay = entry.IsFullDay,
                StartTime = entry.FromDate,
                EndTime = entry.ToDate,
                LocalAbsenceCode = (int)entry.AbsenceSubType
            }).ToList();
            absenceEntries = absenceEntries ?? new List<AbsenceEntry>();
            return new Absence
            {
                LunchInMinutes = LunchInMinutes,
                LocalId = message.AbsenceId,
                Description = "Imported from Sticos Personal",
                EmployeeId = message.EmployeeId,
                UnitId = message.UnitId,
                AbsenceEntries = absenceEntries
            };
        }
    }
}
