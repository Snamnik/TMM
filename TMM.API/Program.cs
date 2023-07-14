using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var environmentName = builder.Environment.IsProduction() ? "" : $".{builder.Environment.EnvironmentName}";
builder.Configuration.AddJsonFile($"Authentication/admins{environmentName}.json", false, true);
builder.Configuration.AddJsonFile($"appsettings{environmentName}.json", false, true);

var services = builder.Services;
var configuration = builder.Configuration;
//register built-in services
services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    })
    .AddMvcOptions(options =>
{
    options.Filters.AddService<ExceptionHandler>();
    options.Filters.AddService<LoggingFilter>();
    options.Filters.AddService<InputValidationFilter>();
});

services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


services.AddHttpContextAccessor();
services.AddAuthorization();
services.AddLocalization();
services.AddCors(options => { options.AddPolicy("TMMCorsPolicy", builder => { builder.WithOrigins(configuration["Cors:Origins"].Split(";")).AllowAnyHeader().AllowAnyMethod(); }); });

//register third-party services
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c => { var filePath = Path.Combine(AppContext.BaseDirectory, "TMM.API.xml"); c.IncludeXmlComments(filePath); });
services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblies(new List<Assembly> { typeof(IApplication).Assembly, typeof(IAPI).Assembly });
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(IApplication).Assembly, typeof(IAPI).Assembly));

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger());

//register app services
services.AddDatabase(configuration).AddAuthentication(configuration).AddApiServices();

//app pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture("en-GB").AddSupportedCultures(new string[] { "en-GB" });
app.UseRequestLocalization(localizationOptions);

app.UseCors("TMMCorsPolicy");

app.UseAuthorization();
app.MapControllers();

app.Run();
