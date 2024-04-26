﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using onlineshopping.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<onlineshoppingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("onlineshoppingContext") ?? throw new InvalidOperationException("Connection string 'onlineshoppingContext' not found.")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => options.LoginPath = new PathString("/User/Login"));
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();  

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=product}/{action=Productlist}/{id?}");

app.Run();
