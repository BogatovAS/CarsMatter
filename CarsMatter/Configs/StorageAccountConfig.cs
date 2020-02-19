using System.ComponentModel.DataAnnotations;

namespace CarsMatter.Configs
{
    public class StorageAccountConfig
    {
        [Required]
        public string ConnectionString { get; set; }

        public string DevConnectionString { get; set; }
    }
}
