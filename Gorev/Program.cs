using GorevY.Data;
using GorevY.Middlewares;
using GorevY.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS ayarlarýný güncelle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite'ýn çalýþtýðý portu ekleyin
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Veritabaný ayarlarý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Maksimum 5 kez dene
                maxRetryDelay: TimeSpan.FromSeconds(10), // 10 saniyelik gecikme ile
                errorNumbersToAdd: null); // Belirli hata kodlarý ile sýnýrlama yapma
        }));

// JWT ayarlarý
var key = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(key) || key.Length < 32)
{
    throw new InvalidOperationException("JWT anahtarý yapýlandýrýlmamýþ veya yetersiz.");
}

var keyBytes = Encoding.UTF8.GetBytes(key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true, // Token süresi dolmuþsa reddet
        ClockSkew = TimeSpan.Zero // Token süresi doðrulama sýrasýnda esnek olmayacak
    };
});

// Servisleri ekle
builder.Services.AddScoped<KullaniciService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GorevY API", Version = "v1" });
});

var app = builder.Build();

// Orta katmanlarý ekle
app.UseCors("AllowSpecificOrigins"); // Yeni CORS politikasýný kullan
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>(); // Özel middleware'i burada kullanýyoruz

app.MapControllers();

// Swagger yapýlandýrmasý
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GorevY API V1");
    c.RoutePrefix = "swagger";
});

app.Run();
