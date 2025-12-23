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
    options.AddPolicy("AllowBlazorWasm", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
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

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Srs.Client._Imports).Assembly);

app.Run();