using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentQuestionController : Controller
    {
        private readonly IAssessmentQuestionService _service;

        public AssessmentQuestionController(IAssessmentQuestionService AssessmentQuestionService)
        {
            _service = AssessmentQuestionService;
        }
    }
}
