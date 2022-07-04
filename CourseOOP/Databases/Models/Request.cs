namespace CourseOOP.Databases.Models
{
    internal class Request
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public Request(string text)
        {
            Text = text;
        }
    }
}
