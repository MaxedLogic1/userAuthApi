using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Middleware;
using usersAuthApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Repository
builder.Services.AddScoped<IGameRepository, GameRepository>();    
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IFundRepository, FundRepository>();
builder.Services.AddScoped<IBubbleGameRepository, BubbleGameRepository>();

// Connections String 
builder.Services.AddDbContext<userDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnetionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<globalExceptionMiddleware>();
app.Run();
