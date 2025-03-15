using log4net.Config;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using SovosDocApi.Repositories;
using SovosDocApi.Repositories.Invoice;
using System.Configuration;
using Quartz;
using SovosDocApi.Quartz;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<SovosDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

var isStartJob = configuration.GetValue<bool>("EMailJob:Calistir");
var scheduler = configuration.GetValue<string>("EMailJob:ZamanTrigger");

if (isStartJob)
{
    builder.Services.AddQuartz(q =>
    {
        var jobKey = new JobKey("EmailJob");
        q.AddJob<SendEmailJob>(opts => opts.WithIdentity(jobKey));

        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("EmailJobTrigger")
            .WithCronSchedule(scheduler));
    });

    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
}


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
