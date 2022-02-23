namespace ThaudalAPI.Model.Model.File;

public class FileUploadInfo
{
    public string FileName { get; set; }
    public string FileType { get; set; }
    public string Description { get; set; }
    public Stream fileContent { get; set; }
    public List<string> FileTags { get; set; }
}