namespace H_application.DTOs.SystemSettingsDto
{
    public class SystemSettingsDtoCreate
    {



        public string? Key { get; set; }


        public string? Value { get; set; }

        public string? Category { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
