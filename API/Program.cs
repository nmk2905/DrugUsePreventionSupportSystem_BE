using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.DBContext;
using Microsoft.EntityFrameworkCore;
using Services.Mappers;
using API.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseCategory, CourseCategoryService>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IConsultantService, ConsultantService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICourseRegisterService, CourseRegisterService>();
builder.Services.AddScoped<ICourseQuestionService, CourseQuestionService>();
builder.Services.AddScoped<ICourseQuestionOptionService, CourseQuestionOptionService>();
builder.Services.AddScoped<IUserAnswerService, UserAnswerService>();

builder.Services.AddScoped<IUserAssessmentService, UserAssessmentService>();
builder.Services.AddScoped<IAssessmentQuestionService, AssessmentQuestionService>();
builder.Services.AddScoped<IAssessmentOptionService, AssessmentOptionService>();
builder.Services.AddScoped<IAgeGroupService, AgeGroupService>();
builder.Services.AddScoped<IRiskLevelService, RiskLevelService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();


builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseCategoryRepository>();
builder.Services.AddScoped<CourseRegisterRepository>();
builder.Services.AddScoped<CourseQuestionRepository>();
builder.Services.AddScoped<CourseQuestionOptionRepository>();
builder.Services.AddScoped<UserAnswerRepository>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile));

builder.Services.AddDbContext<Drug_use_prevention_systemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
    

//
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());

    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = new PathString("/Account/Login");
    options.AccessDeniedPath = new PathString("/Account/Forbidden");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    ////JWT Config
    option.DescribeAllParametersInCamelCase();
    option.ResolveConflictingActions(conf => conf.First());
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//Listener gate 80 for the container
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(80);
//});

var app = builder.Build();

//Always enable Swagger (for Docker container)
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
