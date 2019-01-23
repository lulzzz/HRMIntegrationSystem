﻿using System;

namespace Common.Api.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}