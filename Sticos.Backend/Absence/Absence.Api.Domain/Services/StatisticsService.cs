
using Absence.Api.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Absence.Api.Domain.Models;
using Shared.Domain.Interfaces;
using Shared.Domain.Models;

namespace Absence.Api.Domain.Services
{
    public class StatisticsService : IStatisticsService
    {
        public async Task<IChart> GetStatistics(int id)
        {
            if (id == (int)AbsenceStatistics.AbsencesGroupedLast4Year)
            {
                return new ChartData
                {
                    Series = new List<IChartSerie>
                    {
                        new ChartSerie
                        {
                            Name = "2018",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)AbsenceChartValueType.TimeOff).ToString(), Value = 6},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SickLeave).ToString(),   Value = 10},
                                new ChartValue {Name = ((int)AbsenceChartValueType.Leave).ToString(),    Value = 7},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SelfCertification).ToString(),  Value = 6},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2017",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)AbsenceChartValueType.TimeOff).ToString(), Value = 13},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SickLeave).ToString(),   Value = 14},
                                new ChartValue {Name = ((int)AbsenceChartValueType.Leave).ToString(),    Value = 9},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SelfCertification).ToString(),  Value = 30},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2016",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)AbsenceChartValueType.TimeOff).ToString(), Value = 2},
                                new ChartValue {Name =  ((int)AbsenceChartValueType.SickLeave).ToString(),   Value = 1},
                                new ChartValue {Name = ((int)AbsenceChartValueType.Leave).ToString(),    Value = 18},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SelfCertification).ToString(),  Value = 1},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2015",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)AbsenceChartValueType.TimeOff).ToString(), Value = 7},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SickLeave).ToString(),   Value = 2},
                                new ChartValue {Name = ((int)AbsenceChartValueType.Leave).ToString(),    Value = 6},
                                new ChartValue {Name = ((int)AbsenceChartValueType.SelfCertification).ToString(),  Value = 1},
                            }
                        }
                    }
                };
            }
            else if (id == (int)AbsenceStatistics.VacationBankStatisticsLast4Year)
            {
                return new ChartData
                {
                    Series = new List<IChartSerie>
                    {
                        new ChartSerie
                        {
                            Name = "2018",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)VacationBankChartValueType.Total).ToString(), Value = 30},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Spent).ToString(), Value = 10},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.NotSpent).ToString(), Value = 7},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Pending).ToString(), Value = 13},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2017",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)VacationBankChartValueType.Total).ToString(), Value = 40},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Spent).ToString(), Value = 2 },
                                new ChartValue {Name =  ((int)VacationBankChartValueType.NotSpent).ToString(), Value = 26},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Pending).ToString(), Value = 12},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2016",
                            Values = new List<IChartValue>
                            {
                                new ChartValue {Name = ((int)VacationBankChartValueType.Total).ToString(), Value = 25},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Spent).ToString(), Value = 5},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.NotSpent).ToString(), Value = 20},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Pending).ToString(), Value = 0},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2015",
                            Values = new List<IChartValue>
                            {
                               new ChartValue {Name = ((int)VacationBankChartValueType.Total).ToString(), Value = 20},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Spent).ToString(), Value = 5},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.NotSpent).ToString(), Value =5},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Pending).ToString(), Value = 10},
                            }
                        },
                        new ChartSerie
                        {
                            Name = "2014",
                            Values = new List<IChartValue>
                            {
                               new ChartValue {Name = ((int)VacationBankChartValueType.Total).ToString(), Value = 22},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Spent).ToString(), Value = 7},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.NotSpent).ToString(), Value =13},
                                new ChartValue {Name =  ((int)VacationBankChartValueType.Pending).ToString(), Value = 2},
                            }
                        },
                    }
                };
            }

            return null;
        }

    }
}
