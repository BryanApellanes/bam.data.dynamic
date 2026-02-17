 using Bam.Data.Repositories;

 namespace Bam.Data.Dynamic.Data
{
    /// <summary>
    /// Represents the original document used
    /// to generate dynamic types.  Not all
    /// DynamicTypeDescriptors will have a 
    /// RootDocument.
    /// </summary>
    [Serializable]
    public class RootDocument: RepoData
    {
        public string FileName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ContentHash { get; set; } = null!;
    }
}
