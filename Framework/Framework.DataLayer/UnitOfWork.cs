using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Domain;
using Framework.Domain.Events;
using Framework.Domain.Repositories;
using Framework.Infrastructure.Extensions;

namespace Framework.DataLayer
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbExtendedContext
    {
        private readonly IDbContextFactory<T> _contextFactory;
        private readonly T _context;
        private bool _disposed;
        private readonly IEventBusSyncWrapper _eventBus;

        public UnitOfWork(IDbContextFactory<T> contextFactory, IEventBusSyncWrapper eventBus)
        {
            _contextFactory = contextFactory;
            _eventBus = eventBus;
            _context = contextFactory.Create();
            Debug.WriteLine("UoW Created - {0}", GetHashCode());
        }

        public void Save()
        {
            if (_context == null)
                return;
            try
            {
                var roots = GetModifiedAggregateRoots();
                var events = GetDomainEventsToThrow(roots);
                _context.SaveChanges();
                PublishDomainEvents(events);
                MarkEventsAsCommitted(roots);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        private IEnumerable<IAggregateRoot> GetModifiedAggregateRoots()
        {
            return _context.ChangeTracker.Entries<IAggregateRoot>()
                .Select(po => po.Entity)
                .ToArray();
        }

        private IEnumerable<IEvent> GetDomainEventsToThrow(IEnumerable<IAggregateRoot> roots)
        {
            var eventsToThrow = new List<IEvent>();
            foreach (var entity in roots)
            {
                var events = entity.GetUncommittedChanges();
                eventsToThrow.AddRange(events);
            }
            return eventsToThrow;
        }

        private void MarkEventsAsCommitted(IEnumerable<IAggregateRoot> roots)
        {
            roots.ForEach(x => x.MarkChangesAsCommited());
        }

        private void PublishDomainEvents(IEnumerable<IEvent> events)
        {
            if (_eventBus == null) return;

            foreach (var @event in events)
            {
                _eventBus.Publish(@event);
            }
        }

        public Task<int> SaveAsync()
        {
            return Context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public DbExtendedContext Context => _context ?? _contextFactory.Create();

        public T TypedContext => (T) Context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Debug.WriteLine("UoF disposed - {0}", GetHashCode());
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}