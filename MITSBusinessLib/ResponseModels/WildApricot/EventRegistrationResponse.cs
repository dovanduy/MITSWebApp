using MITSBusinessLib.Models;

namespace MITSBusinessLib.ResponseModels.WildApricot
{
    public class EventRegistrationResponse
    {
        public int Id { get; set; }
        public string IsCheckedIn { get; set; }
        public bool IsPaid { get; set; }
        public EventRegistrationTypesResponse RegistrationType { get; set; }
        public string DisplayName { get; set; }
    }
}