using be_artwork_sharing_platform.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_project_swp.Core.Entities
{
    [Table("payments")]
    public class Payment : BaseEntity<long>
    {
        public string User_Id { get; set; }
        public long Artwork_Id { get; set; }
        public decimal Total_Price { get; set; }
        // RelationShip
        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }

        [ForeignKey("Artwork_Id")]
        public Artwork Artworks { get; set; }
    }
}
