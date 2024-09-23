using Quartz;
using RaspiTempLogger.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddOptions<BmeConfig>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(BmeConfig)))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<IBmeSensorModel, BmeSensorModel>();

builder.Services.AddQuartzHostedService(q =>
{
	q.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
