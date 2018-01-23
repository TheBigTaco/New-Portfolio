using System;
using Portfolio.Models;
namespace Portfolio.ViewModels
{
    public class PostComments
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; } = new Comment();
        public PostComments(Post post)
        {
            Post = post;
            Comment.PostId = post.PostId;
        }
    }
}
