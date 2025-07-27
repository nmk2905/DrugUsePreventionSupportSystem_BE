using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Comment
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime? PostDate { get; set; }
        public int? BlogId { get; set; }
        public string BlogTitle { get; set; }
        public int? UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
