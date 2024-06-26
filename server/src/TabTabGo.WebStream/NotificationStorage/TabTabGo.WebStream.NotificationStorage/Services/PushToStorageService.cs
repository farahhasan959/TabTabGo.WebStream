﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TabTabGo.Core.Data;
using TabTabGo.WebStream.Model;
using TabTabGo.WebStream.NotificationStorage.Entites;
using TabTabGo.WebStream.NotificationStorage.Repository;
using TabTabGo.WebStream.Services.Contract;

namespace TabTabGo.WebStream.NotificationStorage.Services
{
    /// <summary>
    /// it is the implemetation of IPushEvent and ISaveWebStreamMessage to save webstream messages and notification to database
    /// </summary>
    public class PushToStorageService : IPushEvent, ISaveWebStreamMessage
    {
        private readonly IUserConnections _userConnections;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRepository _notifications;
        private readonly INotificationUserRepository _users; 
        public PushToStorageService(IUserConnections userConnections, IUnitOfWork unitOfWork, INotificationRepository notifications, INotificationUserRepository users)
        {
            _userConnections = userConnections;
            _unitOfWork = unitOfWork;
            _users = users;
            _notifications = notifications;
        }
        public async Task PushAsync(IEnumerable<string> connectionIds, WebStreamMessage message, CancellationToken cancellationToken = default)
        {
            var userIds = await _userConnections.GetUsersIdsByConnectionIdsAsync(connectionIds, cancellationToken);
            await this.Save(userIds, message, cancellationToken); 
        }

        public async Task PushAsync(string connectionId, WebStreamMessage message, CancellationToken cancellationToken = default)
        {
            var userId = await _userConnections.GetUserIdByConnectionIdAsync(connectionId, cancellationToken);
            await this.Save(userId, message, cancellationToken); 
        }
        public Task PushToUserAsync(IEnumerable<string> userIds, WebStreamMessage message, CancellationToken cancellationToken = default)
        {
            return Save(userIds, message, cancellationToken);
        }

        public Task PushToUserAsync(string userId, WebStreamMessage message, CancellationToken cancellationToken = default)
        {
            return Save(userId, message, cancellationToken);
        }


        public async Task Save(IEnumerable<string> userIds, WebStreamMessage message, CancellationToken cancellationToken = default)
        {

            var notification = await _notifications.GetByKeyAsync(message.NotificationId, cancellationToken: cancellationToken);
            if (notification == null)
            {
                notification = new Notification()
                {
                    Id = message.NotificationId,
                    EventName = message.EventName,
                    Message = message.Data,
                };
                await _notifications.InsertAsync(notification, cancellationToken);
            }

            foreach (var userId in userIds.Distinct().ToList())
            {

                var user = new NotificationUser()
                {
                    NotifiedDateTime = DateTime.UtcNow,
                    NotificationId = notification.Id,
                    UserId = userId
                };
                await _users.InsertAsync(user, cancellationToken);
            }
        }

        public async Task Save(string userId, WebStreamMessage message, CancellationToken cancellationToken = default)
        {
            var notification = await _notifications.GetByKeyAsync(message.NotificationId, cancellationToken: cancellationToken);
            if (notification == null)
            {
                notification = new Notification()
                {
                    Id = message.NotificationId,
                    EventName = message.EventName,
                    Message = message.Data,
                };
                await _notifications.InsertAsync(notification, cancellationToken);
            }
            var user = new NotificationUser()
            {
                NotifiedDateTime = DateTime.UtcNow,
                NotificationId = notification.Id,
                UserId = userId
            };
            await _users.InsertAsync(user, cancellationToken);

        }
    }
}
