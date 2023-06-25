using System.Diagnostics;

Trace.Listeners.Add(new TextWriterTraceListener("./App_Data/data_trace.log"));
Trace.AutoFlush = true;
Trace.WriteLine($"Started at {DateTime.Now}:");
Trace.Indent();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();