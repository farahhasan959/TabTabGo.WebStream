﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TabTabGo.Core.Models;
using TabTabGo.WebStream.NotificationStorage.Entites;
using TabTabGo.WebStream.NotificationStorage.Module;
using TabTabGo.WebStream.NotificationStorage.Repository;

namespace TabTabGo.WebStream.NotificationStorage.EFCore.Repositories
{
    class EfNotificationRepository(DbContext context) : TabTabGo.Data.EF.Repositories.GenericRepository<Notification, Guid>(context), INotificationRepository
    {
        public Task<List<Notification>> FindByCriteria(List<Expression<Func<Notification, bool>>> criteria, string orderBy, bool isDesc, CancellationToken cancellationToken = default)
        {
            IQueryable<Notification> query = context.Set<Notification>().AppleyCriteria(criteria);
            return query.OrderBy(orderBy, isDesc).ToListAsync(cancellationToken);
        }

        public Task<PageList<Notification>> FindByCriteria(List<Expression<Func<Notification, bool>>> criteria, string orderBy, bool isDesc, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        {
            IQueryable<Notification> query = context.Set<Notification>().AppleyCriteria(criteria);
            return new PageingListBuilder<Notification>(query, pageNumber, pageSize, orderBy, isDesc).BuildWithFullCountAsync(cancellationToken);
        }

        public List<Notification> FindByCriteriaAsync(List<Expression<Func<Notification, bool>>> criteria, string orderBy, bool isDesc)
        {
            IQueryable<Notification> query = context.Set<Notification>().AppleyCriteria(criteria);
            return query.OrderBy(orderBy, isDesc).ToList();
        }

        public PageList<Notification> FindByCriteriaAsync(List<Expression<Func<Notification, bool>>> criteria, string orderBy, bool isDesc, int pageSize, int pageNumber)
        {
            IQueryable<Notification> query = context.Set<Notification>().AppleyCriteria(criteria);
            return new PageingListBuilder<Notification>(query, pageNumber, pageSize, orderBy, isDesc).BuildWithFullCount();
        }

        public List<Notification> FindByUserId(string userId)
        {
            return context.Set<NotificationUser>().Where(s => s.UserId.Equals(userId)).Select(s => s.Notification).ToList();

        }

        public Task<List<Notification>> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return context.Set<NotificationUser>().Where(s => s.UserId.Equals(userId)).Select(s => s.Notification).ToListAsync(cancellationToken);
        }
    }
} 