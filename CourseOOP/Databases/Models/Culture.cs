namespace CourseOOP.Databases.Models
{
    public class Culture
    {
        public int Id { get; set; }
        public string CultureName { get; set; }
        public string? AuthorName { get; set; }
        public string? ParentVariety { get; set; }
        public int Productivity { get; set; }
        public string? Specification { get; set; }
        public int FrostResistance { get; set; }
        public int Immunity { get; set; }
        public string? SelectionFund { get; set; }
        public Culture() { }

        public Culture(string name, int productivity, int frostRes, int immunity)
        {
            CultureName = name;
            Productivity = productivity;
            FrostResistance = frostRes;
            Immunity = immunity;
        }
    }
}
