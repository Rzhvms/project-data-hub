using CoreLib.Api.Controllers.ControllerTypes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProjectManage;

/// <summary>
/// Контроллер управления информацией о проектах.
/// </summary>
[Route("project")]
public class ProjectManageController : ClientControllerBase
{
    [HttpGet]
    public ActionResult GetSmth()
    {
        return Ok();
    }
}