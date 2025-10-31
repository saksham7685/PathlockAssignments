var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var corsPolicyName = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:5173",
                    "https://localhost:5173"
                )
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                .WithHeaders("Content-Type", "Authorization")
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add request logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

builder.Services.AddControllers();

// Register our services
builder.Services.AddSingleton<WorkPlanner.API.Services.SchedulerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

// Important: CORS must be before routing and authorization
app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", () => "Work Planner API is running!");
});

app.Run();
