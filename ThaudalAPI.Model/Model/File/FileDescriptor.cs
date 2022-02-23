using System.ComponentModel.DataAnnotations;

namespace ThaudalAPI.Model.Model.File;

public class FileDescriptor
{
    [Key]
    public string FileName { get; set; }
    public string FileType { get; set; }
    public string Description { get; set; }    
    public string BlobIdentifier { get; set; }
}