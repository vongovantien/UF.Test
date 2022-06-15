using System.ComponentModel.DataAnnotations;

namespace UF.AssessmentProject.Model
{
    public class Partner
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string partnerrefno { get; set; }
        [Required]
        [MaxLength(50)]
        public string partnerkey { get; set; }
        public string partnerpassword { get; set; }
    }
}
