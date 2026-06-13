using CoreLib.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreLib.Api.Controllers.Audit;

/// <summary>
/// Контроллер для просмотра журнала аудита. Доступен только администраторам
/// </summary>
[ApiController]
[Route("api/audit")]
[Authorize(Roles = "Administrator")]
public class AuditController(IAuditService auditService) : ControllerBase
{
    /// <summary>
    /// Получить все записи аудита, отсортированные от новых к старым
    /// </summary>
    [HttpGet("logs")]
    [ProducesResponseType(typeof(IEnumerable<AuditLog>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLogsAsync([FromQuery] int? limit = null)
    {
        var logs = await auditService.GetLogsAsync(limit);
        return Ok(logs);
    }
}
