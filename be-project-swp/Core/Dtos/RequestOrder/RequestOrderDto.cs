namespace be_artwork_sharing_platform.Core.Dtos.RequestOrder
{
    public class RequestOrderDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Category_Artwork { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
