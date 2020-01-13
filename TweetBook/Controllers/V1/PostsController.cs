using System;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetPosts());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute] Guid postId)
        {
            var post = _postService.GetPostById(postId);
                
            if (post != null)
            {
                return Ok(post);
            }
            
            return NotFound();
        }
        
        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Put([FromRoute] Guid postId, [FromBody] UpdatePostRequest updatePostRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = updatePostRequest.Name
            };
            var result = _postService.UpdatePost(post);
            if (result)
            {
                return Ok(post);
            }
            return NotFound();
        }
        
        [HttpDelete(ApiRoutes.Posts.Update)]
        public IActionResult Delete([FromRoute] Guid postId)
        {
            var result = _postService.DeletePost(postId);
            if (result)
            {
                return NoContent();
            }
            
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postRequest.Id
            };

            if (post.Id != Guid.Empty)
            {
                post.Id = Guid.NewGuid();
            }
            _postService.GetPosts().Add(post);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = baseUrl + ApiRoutes.Posts.Get.Replace("postId", post.Id.ToString());

            var response = new PostResponse
            {
                Id = post.Id
            };

            return Created(location, response);
        }
    }
}