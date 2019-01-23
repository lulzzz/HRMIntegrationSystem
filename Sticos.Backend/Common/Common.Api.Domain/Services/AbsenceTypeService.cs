using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Interfaces;
using domain = Common.Api.Domain.Entities;
using Sticos.Personal.MessageContracts.Enums;
using Common.Api.Domain.Entities;
using System.Linq;

namespace Common.Api.Domain.Services
{
    public class AbsenceTypeService : IAbsenceTypeService
    {
        public async Task<IEnumerable<domain.AbsenceType>> GetAbsenceTypes(SearchQueryAbsenceType query)
        {
            var absenceTypes = new List<domain.AbsenceType>()
            {
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.SelfdeclarationSick, Order = 1},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.SelfdeclarationChildminderSick, Order = 2},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.SelfdeclarationChildSick, Order = 3},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.Sickleave, Order = 4},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.SickleaveChild, Order = 5},
                   
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.Vacation, Order = 10},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.Timeoff, Order = 11},
                  
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.SocialLeave, Order = 20},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.ParentalLeave, Order = 21},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.CareLeave, Order = 22},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.EducationalLeave, Order = 23},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.NursingLeave, Order = 24},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.CaregivingLeave, Order = 25},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.MilitaryLeave, Order = 26},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.OtherLeave, Order = 27},
                    
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.CourseTravel, Order = 30},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.MeetingTravel, Order = 31},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.CustomervisitTravel, Order = 32},
                new domain.AbsenceType{ SpecificValue = AbsenceSubType.OtherTravel, Order = 33},
            };

            if(query.AbsenceTypesIds.Count > 0)
            {
                var absenceTypesQuery = absenceTypes.Where(e => query.AbsenceTypesIds.Contains(int.Parse(e.Value)));
                return absenceTypesQuery;
            }
            return absenceTypes;
        }
    }
}
