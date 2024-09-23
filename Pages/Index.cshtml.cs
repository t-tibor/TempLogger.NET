using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaspiTempLogger.Services;

namespace RaspiTempLogger.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IBmeSensorModel bmeSensorModel;

    public double Temperature { get; set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        IBmeSensorModel bmeSensorModel
        )
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        var dto = await bmeSensorModel.ReadSensor();
        this.Temperature = dto.TempDegC;
    }
}
