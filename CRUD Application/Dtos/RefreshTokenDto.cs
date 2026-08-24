using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Dtos
{
    public class RefreshTokenDto
    {
        
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

}
