namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class LineItem
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
    }
}