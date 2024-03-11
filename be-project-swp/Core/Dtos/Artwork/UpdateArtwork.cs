namespace be_artwork_sharing_platform.Core.Dtos.Artwork
{
    public class UpdateArtwork
    {
        public string Name { get; set; }
        public string Category_Name { get; set; }
        public string? Description { get; set; }
        public string Url_Image { get; set; }
        public double Price { get; set; }
    }
}
