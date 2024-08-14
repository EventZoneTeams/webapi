namespace EventZone.Repositories.Helper
{
    public static class CacheKeys
    {
        public static string EventCategories => "_EventCategories";
        public static string EventCategory(Guid id) => $"_EventCategory_{id}";
        public static string Events => "_Events";
        public static string Event(Guid id) => $"_Event_{id}";
    }
}
