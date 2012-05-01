using System.ComponentModel.DataAnnotations;

namespace Automation.Models
{
    public class ResultModel
    {
    }

    public class SnapshotModel
    {
        [Required]
        public string Id { get; set; }

        public string Snapshot { get; set; }
    }
}