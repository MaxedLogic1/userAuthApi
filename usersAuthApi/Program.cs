using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
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
builder.Services.AddScoped<IHeadTailGameRepository, HeadTailGameRepository>();
builder.Services.AddScoped<IPlayerAndGameRepository, PlayerAndGameRepository>();
builder.Services.AddScoped<IFlappyBirdGameRepository, FlappyBirdGameRepository>();
builder.Services.AddScoped<IPlayerTotalAmountRepository, PlayerTotalAmountRepository>();



// Connections String 
builder.Services.AddDbContext<userDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnetionString")));

//Add Cores to all and specfic users
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy
            .AllowAnyOrigin()     
            .AllowAnyHeader()    
            .AllowAnyMethod();    
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//Enable Cores 
app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionMiddleware>();
//app.UseMiddleware<ValidateModelAttribute>();
app.Run();
