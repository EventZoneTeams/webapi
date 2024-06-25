using Domain.Entities;

namespace Repositories.Extensions
{
    public static class EventExtension
    {
        public enum EventOrderBy
        {
            ASC,
            DESC
        }
        public static IQueryable<Event> Sort(this IQueryable<Event> query, EventOrderBy orderBy)
        {

            if (string.IsNullOrWhiteSpace(orderBy.ToString())) return query.OrderBy(p => p.Name);

            query = orderBy switch
            {
                EventOrderBy.ASC => query.OrderBy(p => p.Name),
                EventOrderBy.DESC => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }

        public static IQueryable<Event> Search(this IQueryable<Event> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm) || p.Location.ToLower().Contains(lowerCaseSearchTerm) || p.University.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Event> Filter(this IQueryable<Event> query, int? categoryId)
        {
            if (categoryId != null)
            {
                query = query.Where(p => p.EventCategoryId == categoryId);
            }
            return query;
        }

        public static IQueryable<Event> FilterByUserId(this IQueryable<Event> query, int? userId)
        {
            if (userId != null)
            {
                query = query.Where(p => p.UserId == userId);
            }
            return query;
        }

        public static IQueryable<Event> FilterByEventDate(this IQueryable<Event> query, DateTime? eventStartDate, DateTime? eventEndDate)
        {
            if (eventStartDate != null)
            {
                query = query.Where(p => p.EventStartDate >= eventStartDate);
            }
            if (eventEndDate != null)
            {
                query = query.Where(p => p.EventEndDate <= eventEndDate);
            }
            return query;
        }

        public static IQueryable<Event> FilterByLocation(this IQueryable<Event> query, string location)
        {
            if (string.IsNullOrEmpty(location)) return query;

            var lowerCaseLocation = location.Trim().ToLower();

            return query.Where(p => p.Location.ToLower().Contains(lowerCaseLocation));
        }

        public static IQueryable<Event> FilterByUniversity(this IQueryable<Event> query, string university)
        {
            if (string.IsNullOrEmpty(university)) return query;

            var lowerCaseUniversity = university.Trim().ToLower();

            return query.Where(p => p.University.ToLower().Contains(lowerCaseUniversity));
        }

        public static IQueryable<Event> FilterByStatus(this IQueryable<Event> query, string eventStatus)
        {
            if (!string.IsNullOrWhiteSpace(eventStatus))
            {
                query = query.Where(p => p.Status.ToLower() == eventStatus.ToString().ToLower());
            }
            return query;
        }
    }
}
