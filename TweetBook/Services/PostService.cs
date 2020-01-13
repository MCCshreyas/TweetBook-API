using System;
using System.Collections.Generic;
using System.Linq;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly List<Post> _posts;

        public PostService()
        {
            _posts = new List<Post>();
            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = $"Post name {i}"
                });
            }
        }
        
        public List<Post> GetPosts()
        {
            return _posts;
        }

        public Post GetPostById(Guid postId)
        {
            var post = _posts.FirstOrDefault(x => x.Id == postId);
            return post;
        }

        public bool UpdatePost(Post post)
        {
            var dbPost = GetPostById(post.Id);
            if (dbPost == null)
                return false;

            dbPost.Name = post.Name;
            return true;
        }

        public bool DeletePost(Guid postId)
        {
            var dbPost = GetPostById(postId);
            if (dbPost == null)
                return false;

            _posts.Remove(dbPost);
            return true;
        }
    }
}