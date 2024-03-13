using be_artwork_sharing_platform.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_project_swp.Core.Entities
{
    [Table("confirmationcodes")]
    public class ConfirmationCode : BaseEntity<long>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
