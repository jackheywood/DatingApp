using DatingApp.Backend.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogUserActivityActionFilter))]
public class BaseApiController : ControllerBase;
