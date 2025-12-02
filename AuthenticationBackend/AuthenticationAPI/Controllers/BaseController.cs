using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {

    }
}
