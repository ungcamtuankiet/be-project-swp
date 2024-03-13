using be_artwork_sharing_platform.Core.Constancs;
using System.ComponentModel.DataAnnotations;

namespace be_project_swp.Core.Dtos.Auth;
public class RequestCodeReq
{
    public string Email { get; set; } = null!;
}

public class ResetPasswordReq
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
    [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter and 1 number")]
    public string Password { get; set; } = null!;
}