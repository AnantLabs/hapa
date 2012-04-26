using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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