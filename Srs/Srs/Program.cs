using Business_Access.Interfaces;
using Business_Access.Services;
using Microsoft.EntityFrameworkCore;
using Srs.Client;
using Srs.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<SrsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IEvaluationPeriod, EvaluationPeriodService>();
builder.Services.AddScoped<IEvaluation, EvaluationServices>();
builder.Services.AddScoped<IProfessorEvaluation, ProfessorEvaluationServices>();
builder.Services.AddScoped<IGSDean, GSDeanService>();
builder.Services.AddScoped<IVPGSEvaluation, VPGSEvaluationService>();
builder.Services.AddScoped<IHODEvaluation, HODEvaluationService>();
builder.Services.AddScoped<IDean, DeanServices>();
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddScoped<IHRService, HRService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHttpClient();

//allow  cors policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("https://localhost:7223") // Your Blazor app URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");

app.MapControllers();

app.UseAntiforgery();

// Serve static files from the client wwwroot
app.UseStaticFiles();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Srs.Client._Imports).Assembly);

app.Run();