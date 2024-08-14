namespace EventZone.Repositories.Models
{
    public class VnpayPayResponse
    {

        public VnpayPayResponse()
        {

        }
        public VnpayPayResponse(string rspCode, string message)
        {
            RspCode = rspCode;
            Message = message;
        }
        public void Set(string rspCode, string message)
        {
            RspCode = rspCode;
            Message = message;
        }
        public string RspCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
