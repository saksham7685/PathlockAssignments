using TaskManagerAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register our repository
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();

// Get frontend URL from configuration or use default
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:5173";

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add configuration from appsettings.Development.json if in development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
}

// Configure Kestrel to listen on specific ports
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5262); // HTTP
    // If you need HTTPS in development, uncomment and configure below
    // serverOptions.ListenLocalhost(7262, listenOptions =>
    // {
    //     listenOptions.UseHttps();
    // });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Only use HTTPS redirection in production
    app.UseHttpsRedirection();
}

// Use CORS with the default policy
app.UseCors();

// Add detailed error pages in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
