using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseOOP.Databases.Models
{
    internal class AddRequest
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public AddRequest(string text)
        {
            Text = text;
        }
    }
}
