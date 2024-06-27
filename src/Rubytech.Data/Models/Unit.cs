namespace Rubytech.Data.Models
{
    public class Unit
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string FullName { get; set; }
    }
}