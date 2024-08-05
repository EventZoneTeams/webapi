using Domain.Entities;

namespace Domain.Extensions
{
    public static class EventOrderExtension
    {

        public static IQueryable<EventOrder> Sort(this IQueryable<EventOrder> query)
        {
            return query.OrderBy(p => p.CreatedAt);
        }

        public static IQueryable<EventOrder> Search(this IQueryable<EventOrder> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Id.ToString().ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<EventOrder> FilterEventId(this IQueryable<EventOrder> query, Guid? eventId)
        {
            if (eventId != null)
            {
                query = query.Where(p => p.EventId == eventId);
            }
            return query;
        }

        public static IQueryable<EventOrder> FilterByEventOrderDate(this IQueryable<EventOrder> query, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate != null)
            {
                query = query.Where(p => p.CreatedAt >= fromDate);
            }
            if (toDate != null)
            {
                query = query.Where(p => p.CreatedAt <= toDate);
            }
            return query;
        }

        public static IQueryable<EventOrder> FilterByStatus(this IQueryable<EventOrder> query, string EventOrderStatus)
        {
            if (!string.IsNullOrWhiteSpace(EventOrderStatus))
            {
                query = query.Where(p => p.Status.ToLower() == EventOrderStatus.ToString().ToLower());
            }
            return query;
        }
    }
}
