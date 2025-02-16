namespace Bam.Data.Dynamic
{
    public class DynamicDataSaveResultEventArgs: EventArgs
    {
        public DynamicDataSaveResult Result { get; set; }
        public FileInfo File { get; set; }
        //public Bam.Logging.Counters.Timer Timer { get; set; }
    }
}
