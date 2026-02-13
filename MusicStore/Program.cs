using MusicStore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<IGeneratorService, GeneratorService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://zonal-courage-production.up.railway.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowFrontend");


// app.UseHttpsRedirection();

app.MapControllers();
app.Run();

