using FinancialCalculator.Services;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Web.Profiles;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICreditService, CreditService>();
builder.Services.AddTransient<IRefinanceService, RefinanceService>();
builder.Services.AddTransient<ILeaseService, LeaseService>();
builder.Services.AddAutoMapper(typeof(CreditMappingProfile));
builder.Services.AddAutoMapper(typeof(RefinanceMappingProfile));
builder.Services.AddAutoMapper(typeof(LeaseMappingProfile));
     
WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
