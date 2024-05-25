namespace Services.BusinessModels.ResponseModels
{
    public class ResponseGenericModel<TEntity>
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = "";
        public TEntity? Data { get; set; }

    }
}
