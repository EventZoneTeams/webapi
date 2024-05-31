using Domain.Entities;

namespace Repositories.Extensions
{
    public enum EventCategoryOrderBy
    {
        ASC,
        DESC
    }
    public static class EventCategoryExtension
    {
        public static IQueryable<EventCategory> Sort(this IQueryable<EventCategory> query, EventCategoryOrderBy eventCategoryOrderBy)
        {

            if (string.IsNullOrWhiteSpace(eventCategoryOrderBy.ToString())) return query.OrderBy(p => p.Title);

            query = eventCategoryOrderBy switch
            {
                EventCategoryOrderBy.ASC => query.OrderBy(p => p.Title),
                EventCategoryOrderBy.DESC => query.OrderByDescending(p => p.Title),
                _ => query.OrderBy(p => p.Title)
            };
            return query;
        }

        public static IQueryable<EventCategory> Search(this IQueryable<EventCategory> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }

    }
}
