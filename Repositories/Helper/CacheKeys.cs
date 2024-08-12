namespace Repositories.Helper
{
    public static class CacheKeys
    {
        public static string EventCategories => "_EventCategories";
        public static string EventCategory(Guid id) => $"_EventCategory_{id}";
    }
}
