
using System.ComponentModel.DataAnnotations;


namespace F12Website.Models
{
    public class ContactFormModel
    {
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        [Required, MaxLength(80)]
        public string Subject { get; set; }
        [Required, MaxLength(500, ErrorMessage = "Message cannot exceed 500 characters")]

        public string Message { get; set; }
        [Required(ErrorMessage = "Captcha token not received")]
        public string Token { get; set; }
    }
}
