using System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BaseApiController:ControllerBase
{
    //base controller for other controllers to inherit from

}
