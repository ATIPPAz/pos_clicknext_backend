using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using PosApi.Context;
using PosApi.Helpers;
using PosApi.Models;
using PosApi.Services;
using PosApi.ViewModels.UnitViewModel;
using System;











/*DataContainer<string> data = new DataContainer<string>();
data.data = "1";
var builder = WebApplication.CreateBuilder(args);

*//*builder.Services.AddControllers();*//*

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
*//*builder.Services.AddEndpointsApiExplorer();*/
/*builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});*//*
string connectionString = "Server=localhost;Port=3306;Database=pos;User Id=root;Password=Pan28060.";
// Add services to the container.
builder.Services.AddDbContext<posContext>(
        options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();
*//*//*app.UseDeveloperExceptionPage();*/
/*app.UseMvc();*
IServiceCollection services= new ServiceCollection();
services.AddScoped<WorkerA>();
services.AddScoped<ForeMan>();
services.AddTransient<WorkerB>();
services.AddTransient<Icar,Mazda>();
IServiceProvider root =  services.BuildServiceProvider();
Icar workerA =  new Mazda();
workerA.start();
WorkerB workerB =  root.GetRequiredService<WorkerB>();
workerB.id = 10;
ForeMan foreman = root.GetRequiredService<ForeMan>();
foreman.work();


/*workerB.work();
workerA.work();*/
/*
WorkerA workerA2 = root.GetRequiredService<WorkerA>();
;

WorkerA context;
using (var scope = root.CreateScope())
{
    context = scope.ServiceProvider.GetRequiredService<WorkerA>();
    *//*context.units.Add(new PosApi.Models.unit { unitName = "a" });
    context.SaveChanges();*//*
    context.id = 3;
    var context2 = scope.ServiceProvider.GetRequiredService<WorkerA>();
    bool isEqual = context == context2;
    ;
    using(var scope2 = root.CreateScope())
    {
        WorkerA context4 = scope2.ServiceProvider.GetRequiredService<WorkerA>();
    }
    ;
}








using (var scope = root.CreateScope())
{
    var context3 = scope.ServiceProvider.GetRequiredService<WorkerA>();

    bool isEqual = context3 == context;
    ;
}*//*

// Configure the HTTP request pipeline.
*//*  if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
*//*

class WorkerA {
    public int id { get; set; }
    public void work()
    {
        Console.WriteLine("A working . . ");
    }
}
class WorkerB
{
    
    public int id { get; set; }
    public void work()
    {
        Console.WriteLine("B working . . ");
    }
}
class ForeMan
{
    readonly WorkerA workerA;
    readonly WorkerB workerB;
    public ForeMan(WorkerA workerA,WorkerB workerB)
    {
        this.workerA = workerA;
        this.workerB = workerB;
    }
    public void work()
    {
        workerA.work();
        workerB.work();
    }
}
interface Icar
{
    void start();
}
class Toyota: Icar
{
    public void start()
    {
        Console.WriteLine("toyota starting..");
    }
}
class Mazda: Icar
{
    public void start()
    {
        Console.WriteLine("mazda starting..");
    }
}

class A
{
    public string one { get; set; }
}
class B : A
{
    public string two { get; set; }
}
class C
{
    public string two { get; set; }
    public string one { get; set; }
    public string three { get; set; }
}

class DataContainer<T>
{
    public T data { get; set; }
}
*/














var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(x => x.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});
// Add services to the container.
/*try
{*/
var connectString = builder.Configuration.GetConnectionString("mySql");

builder.Services.AddDbContext<posContext>(
        options =>
        {
            options.UseMySql(connectString, ServerVersion.AutoDetect(connectString));
        });
builder.Services.AddScoped<UnitRepository>();
builder.Services.AddScoped<ItemRepository>();
builder.Services.AddSingleton<ResponseHelper>();
builder.Services.AddScoped<ReceiptRepository>();
/*try
{
    ;
    try
    {
        throw new Exception("my name ");
    }
    catch (Exception ex)
    {
        throw;
    }
    ;
}
catch (Exception ex)
{
    ;
}
;
step1();
void step1()
{
   
        ;
        step2();
        ;
    
    ;
    void step2()
    {
        ;
        step3();
        ;
        void step3()
        {
            try
            {
                throw new Exception("my x");
                *//*throw;*//*
            }
            catch(Exception x)
            {
                
                throw;
            }
            ;
        }
    }
}*/
/*}*/
/*
catch (Exception E)
{
    var connectString = builder.Configuration.GetConnectionString("mySql2");

    builder.Services.AddDbContext<posContext>(
            options => options.UseMySql(connectString, ServerVersion.AutoDetect(connectString)));
}*/

/*using (var scope =  GetRequiredService<posContext>().CreateScope())
{
    context = scope.ServiceProvider.GetRequiredService<WorkerA>();
    *//*context.units.Add(new PosApi.Models.unit { unitName = "a" });
    context.SaveChanges(); *//*
    context.id = 3;
    var context2 = scope.ServiceProvider.GetRequiredService<WorkerA>();
    bool isEqual = context == context2;
    ;
    using (var scope2 = root.CreateScope())
    {
        WorkerA context4 = scope2.ServiceProvider.GetRequiredService<WorkerA>();
    }
    ;
}
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
