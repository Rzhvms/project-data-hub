using CoreLib.Api.Controllers.ControllerTypes;
using Microsoft.AspNetCore.Mvc;

namespace Api;

/// <summary>
/// Контроллер управления информацией о проектах.
/// </summary>
[Route("project")]
public class ProjectManageController : PublicControllerBase
{
    [HttpGet]
    public ActionResult GetSmth()
    {
        return Ok();
    }
}