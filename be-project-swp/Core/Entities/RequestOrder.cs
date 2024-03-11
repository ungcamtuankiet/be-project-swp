using System.ComponentModel.DataAnnotations.Schema;

namespace be_artwork_sharing_platform.Core.Entities
{
    [Table("requestorders")]
    public class RequestOrder : BaseEntity<long>
    {
        public string FullName { get; set; }
        public string UserName_Sender { get; set; }
        public string UserId_Receivier { get; set; }
        public string Text {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Category_Artwork { get; set; }
    }
}
