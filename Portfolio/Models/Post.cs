using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public int Truncate()
        {
            char[] splitChar = { '.','!','?' };
            string[] splitContent = this.Content.Split(splitChar);
            return splitContent[0].Length;
        }
    }
}
