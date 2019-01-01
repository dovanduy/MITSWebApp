namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class CreateTransactionResponseMessage
    {
        public string ResultCode { get; set; }
        public MessageResponseMessage[] Message { get; set; }
    }
}