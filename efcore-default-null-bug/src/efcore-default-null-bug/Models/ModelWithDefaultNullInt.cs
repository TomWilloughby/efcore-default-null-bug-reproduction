namespace efcore_default_null_bug.Models
{
    public class ModelWithDefaultNullInt
    {
        public int Id { get; set; }
        public bool? Bval { get; set; } = null;
        public string Sval { get; set; } = null;
        public int? Ival { get; set; } = null;
    }
}
