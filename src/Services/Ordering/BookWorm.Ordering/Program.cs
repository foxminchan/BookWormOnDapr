using BookWorm.Ordering.Extensions;
using BookWorm.ServiceDefaults;
using BookWorm.SharedKernel.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.UseOpenApi();

app.UseCloudEvents();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

var apiVersionSet = app.NewApiVersionSet().HasApiVersion(new(1, 0)).ReportApiVersions().Build();

app.MapEndpoints(apiVersionSet);

app.UseHttpsRedirection();

app.Run();
