using System.Text.Json;
using Autofac;
using CommandLine;
using TagsCloudContainer.Console;
using TagsCloudContainer.Core.DI;
using TagsCloudContainer.Core.Models;
using ResultOf;

var parser = new Parser(with => with.HelpWriter = Console.Out);

return parser.ParseArguments<CommandLineOptions>(args)
    .MapResult(
        options =>
        {
            return ValidateCommandLineOptions(options)
                .Then(LoadSettings)
                .Then<TagCloudSettings, None>(settings =>
                {
                    var layoutOptions = new LayoutOptions
                    {
                        Width = settings.Width,
                        Height = settings.Height,
                        FontFamily = settings.FontFamily
                    };

                    return Result.Of(() =>
                        {
                            var builder = new ContainerBuilder();
                            builder.RegisterModule(new AutofacModule(settings));
                            builder.RegisterInstance(options).As<CommandLineOptions>();
                            builder.RegisterType<ConsoleClient>().SingleInstance();
                            var container = builder.Build();
                            return container.Resolve<ConsoleClient>();
                        }, "Failed to initialize DI container") 
                        .Then<ConsoleClient, None>(client =>
                            client.GenerateTagCloud(options.InputFile, options.OutputFile, layoutOptions));
                })
                .OnFail(err => Console.Error.WriteLine("Error: " + err))
                .IsSuccess ? 0 : 1;
        },
        errors =>
        {
            foreach (var e in errors) Console.Error.WriteLine(e.ToString());
            return 1;
        });

static Result<CommandLineOptions> ValidateCommandLineOptions(CommandLineOptions o)
{
    if (string.IsNullOrWhiteSpace(o.InputFile))
        return Result.Fail<CommandLineOptions>("Input file is not specified");

    return string.IsNullOrWhiteSpace(o.OutputFile) 
        ? Result.Fail<CommandLineOptions>("Output file is not specified") 
        : Result.Ok(o);
}

static TagCloudSettings CreateSettingsFromOptions(CommandLineOptions o)
{
    return new TagCloudSettings
    {
        Width = o.Width,
        Height = o.Height,
        FontFamily = o.FontFamily,
        ColorScheme = o.ColorScheme,
        MinFontSize = o.MinFontSize,
        MaxFontSize = o.MaxFontSize,
        Algorithm = o.Algorithm,
        BoringWordsPath = o.BoringWordsFile
    };
}

static Result<TagCloudSettings> LoadSettings(CommandLineOptions o)
{
    var validator = new TagCloudSettingsValidator();

    if (string.IsNullOrWhiteSpace(o.SettingsFile))
        return validator.Validate(CreateSettingsFromOptions(o)); 

    if (!File.Exists(o.SettingsFile))
        return Result.Fail<TagCloudSettings>($"Settings file not found: {o.SettingsFile}");

    return Result.Of(() => File.ReadAllText(o.SettingsFile),
            $"Failed to read settings file: {o.SettingsFile}")
        .Then(DeserializeSettings)
        .Then(settings => Result.Ok(OverrideBoringWords(settings, o.BoringWordsFile)))
        .Then(validator.Validate);

    static Result<TagCloudSettings> DeserializeSettings(string json)
    {
        try
        {
            var settings = JsonSerializer.Deserialize<TagCloudSettings>(
                json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return settings == null 
                ? Result.Fail<TagCloudSettings>("Settings file is empty or invalid.") 
                : Result.Ok(settings);
        }
        catch (Exception e)
        {
            return Result.Fail<TagCloudSettings>($"Failed to parse settings file: {e.Message}");
        }
    }
}

static TagCloudSettings OverrideBoringWords(TagCloudSettings s, string? path)
{
    if (string.IsNullOrWhiteSpace(path))
        return s;

    return new TagCloudSettings
    {
        Width = s.Width,
        Height = s.Height,
        FontFamily = s.FontFamily,
        ColorScheme = s.ColorScheme,
        MinFontSize = s.MinFontSize,
        MaxFontSize = s.MaxFontSize,
        Algorithm = s.Algorithm,
        BoringWordsPath = path
    };
}
