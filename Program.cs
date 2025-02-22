using ChatServerExample.HubWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddChatServerExampleServices();//uygulama servisleri
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//cors
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy

       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowCredentials()
       .SetIsOriginAllowed(x => true))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();
app.AddChatServerExampleMapHubs();

app.Run();
