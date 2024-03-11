namespace be_artwork_sharing_platform.Core.Dtos.RequestOrder
{
    public class ReceiveRequestDto
    {
        public string FullName_Sender { get; set; }
        public string Email_Sender { get; set; }
        public string PhoneNo_Sender { get; set; }
        public string Category_Artwork { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
