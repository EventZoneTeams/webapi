namespace EventZone.Repositories.Helper
{
    public static class CacheKeys
    {
        public static string EventCategories => "_EventCategories";

        public static string EventCategory(Guid id) => $"_EventCategory_{id}";

        public static string Events => "_Events";

        public static string Event(Guid id) => $"_Event_{id}";

        public static string EventProducts => "_EventProducts";

        public static string EventProduct(Guid id) => $"_EventProduct_{id}";

        public static string EventOrders => "_EventOrders";

        public static string EventOrder(Guid id) => $"_EventOrder_{id}";

        public static string EventTickets => "_EventTickets";

        public static string EventTicket(Guid id) => $"_EventTicket_{id}";
    }
}