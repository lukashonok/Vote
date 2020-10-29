using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class CompromisingEvidenceFileModel : Base
    {
        [ForeignKey("CompromisingEvidence")]
        public CompromisingEvidenceModel CompromisingEvidenceId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
