﻿namespace Timereg.Api.Contracts
{
    public class HourBalance
    {
        public int ExpectedMinutes { get; set; }
        public int ActualMinutes { get; set; }

        public double HourBalanceInMinutes => ActualMinutes - ExpectedMinutes;
        public double HourBalanceInHours => HourBalanceInMinutes / 60;
    }
}
