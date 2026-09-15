using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Vocabulary.Infrastructure.Persistence.Context;
using Vocabulary.Infrastructure.Data.Seed;
using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.Services;
using Vocabulary.Infrastructure.Persistence.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VocabularyDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserMeaningProgressRepository, UserMeaningProgressRepository>();
builder.Services.AddScoped<IUserQuestionProgressRepository, UserQuestionProgressRepository>();
builder.Services.AddScoped<VocabularySeedService>();
builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ILevelProgressService, LevelProgressService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider
        .GetRequiredService<VocabularySeedService>();

    await seedService.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
