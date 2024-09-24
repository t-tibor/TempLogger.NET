using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaspiTempLogger.Services;

namespace RaspiTempLogger.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IBmeSensorModel _bmeSensorModel;

    public BmeMeasDataDto MeasurementData { get; set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        IBmeSensorModel bmeSensorModel
        )
    {
        _logger = logger;
        _bmeSensorModel = bmeSensorModel;
    }

    public async Task OnGet()
    {
        MeasurementData = await _bmeSensorModel.ReadSensor();
    }
}
