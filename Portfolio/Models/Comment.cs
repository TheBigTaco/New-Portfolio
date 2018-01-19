using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table("comments")]
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Author { get; set; }
        public string ContentBody { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
