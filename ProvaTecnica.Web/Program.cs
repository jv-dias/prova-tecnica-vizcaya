using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Data; 
using ProvaTecnica.Web.Components;
using ProvaTecnica.Web.Components.Account;
using ProvaTecnica.Core.Entities;
using ProvaTecnica.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddHttpClient();
//Regra de negócio: Implementar Cache nas consultas ao ViaCep
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ViaCepService>();

//Regra de negócio: Implementar serviço de gerenciamento de alunos
builder.Services.AddScoped<IAlunoService, AlunoService>();

//Regra de negócio: Implementar serviço de importação de CSV
builder.Services.AddScoped<ICsvImportService, CsvImportService>();

//Regra de negócio: Implementar serviço de gerenciamento de turmas
builder.Services.AddScoped<ITurmaService, TurmaService>();

//Regra de negócio: Implementar serviço de gerenciamento de matrículas
builder.Services.AddScoped<IMatriculaService, MatriculaService>();

//Regra de negócio: Implementar serviço de dashboard
builder.Services.AddScoped<IDashboardService, DashboardService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); 

// Adição necessária para lógica de auditoria funcionar
builder.Services.AddHttpContextAccessor();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuramos o Identity para usar o ApplicationDbContext
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.MapGet("/Logout", async (SignInManager<ApplicationUser> signInManager, HttpContext context) =>
{
    await signInManager.SignOutAsync();
    context.Response.Redirect("/");
});

try
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        await ProvaTecnica.Core.Data.Seed.SeedData.InitializeAsync(serviceProvider);
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Um erro ocorreu durante o processo de Seed do banco de dados.");
}

app.Run();