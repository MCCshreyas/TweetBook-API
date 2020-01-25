using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            _dataContext.Posts.Add(post);
            await _dataContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);
            return post;
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            var dbPost = await GetPostByIdAsync(post.Id);
            if (dbPost == null)
                return false;

            var newPost = new Post
            {
                Id = post.Id,
                Name = post.Name
            };
            
            _dataContext.Entry(post).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var dbPost = await GetPostByIdAsync(postId);
            if (dbPost == null)
                return false;

            _dataContext.Posts.Remove(dbPost);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}