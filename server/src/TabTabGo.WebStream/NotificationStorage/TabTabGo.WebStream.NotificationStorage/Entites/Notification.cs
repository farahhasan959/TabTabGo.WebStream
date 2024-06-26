﻿using System;
using System.Collections.Generic;
using TabTabGo.Core.Entities;
namespace TabTabGo.WebStream.NotificationStorage.Entites
{
    public class Notification: IEntity
    {
        public Guid Id { get; set; }
        public object Message { get; set; }
        public string EventName { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsEnabled { get; set; } 
        public IDictionary<string, object> ExtraProperties { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
