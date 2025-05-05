using LPP;
using LPP.Bot;
using LPP.DAL;
using LPP.DAL.Context;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddBot();
builder.Services.AddDAL();

var host = builder.Build();

#if !DEBUG
    await host.Services.GetRequiredService<LPPContext>().Database.MigrateAsync();
#endif

host.Run();
