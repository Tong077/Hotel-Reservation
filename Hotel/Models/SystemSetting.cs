using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class SystemSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
