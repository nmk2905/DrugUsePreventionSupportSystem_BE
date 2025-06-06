using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }
    }
}
