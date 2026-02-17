namespace Bam.Data.Dynamic
{
    public class DynamicDataSaveResultEventArgs: EventArgs
    {
        public DynamicDataSaveResult Result { get; set; } = null!;
        public FileInfo File { get; set; } = null!;
        //public Bam.Logging.Counters.Timer Timer { get; set; }
    }
}
