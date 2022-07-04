using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseOOP.Databases.Models
{
    internal class Favorite
    {
        public int Id { get; set; }
        public int CultureId { get; set; }
        public int UserId { get; set; }

        public Favorite(int userId, int cultureId)
        {
            UserId = userId;
            CultureId = cultureId;
        }
    }
}
