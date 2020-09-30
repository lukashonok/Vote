using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vote.Models
{
    public class CompromisingEvidenceFileModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CompromisingEvidence")]
        public CompromisingEvidenceModel CompromisingEvidenceId { get; set; }
        public byte[] File { get; set; }
    }
}
