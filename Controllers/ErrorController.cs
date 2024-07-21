using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentSystemApi.Controllers {
    [ApiController]
    public class ErrorController : ControllerBase {

        [Route("/error")]
        [HttpGet]
        public IActionResult Error() {
            return Problem();
        }
    }
}
