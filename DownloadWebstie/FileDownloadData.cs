using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadWebstie
{
    public class FileDownloadData
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Url { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string FileName { get; set; }
    }
}
