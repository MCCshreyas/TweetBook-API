using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
                
            if (post != null)
            {
                return Ok(post);
            }
            
            return NotFound();
        }
        
        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Put([FromRoute] Guid postId, [FromBody] UpdatePostRequest updatePostRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = updatePostRequest.Name
            };
            var result = await _postService.UpdatePostAsync(post);
            if (result)
            {
                return Ok(post);
            }
            return NotFound();
        }
        
        [HttpDelete(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var result = await _postService.DeletePostAsync(postId);
            if (result)
            {
                return NoContent();
            }
            
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Name = postRequest.Name
            };

            var newPost = await _postService.CreatePostAsync(post);
            var postResponse = new PostResponse { Id = newPost.Id, Name = newPost.Name };
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = baseUrl + ApiRoutes.Posts.Get.Replace("postId", post.Id.ToString());
            return Created(location, postResponse);
        }
    }
}