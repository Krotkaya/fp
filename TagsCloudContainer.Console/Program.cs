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
            return LoadSettings(options)
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
            foreach (var e in errors)
                Console.Error.WriteLine(e.ToString());
            return 1;
        });


static Result<TagCloudSettings> ValidateOptions(CommandLineOptions o)
{
    if (o.Width <= 0 || o.Height <= 0)
        return Result.Fail<TagCloudSettings>("Invalid image size. Width and height must be positive");

    if (o.MinFontSize <= 0 || o.MaxFontSize <= 0)
        return Result.Fail<TagCloudSettings>("Invalid font size. Min/Max must be positive");

    if (o.MinFontSize > o.MaxFontSize)
        return Result.Fail<TagCloudSettings>("Invalid font size range. MinFontSize > MaxFontSize");

    if (string.IsNullOrWhiteSpace(o.InputFile))
        return Result.Fail<TagCloudSettings>("Input file is not specified");

    if (string.IsNullOrWhiteSpace(o.OutputFile))
        return Result.Fail<TagCloudSettings>("Output file is not specified");

    return Result.Ok(new TagCloudSettings
    {
        Width = o.Width,
        Height = o.Height,
        FontFamily = o.FontFamily,
        ColorScheme = o.ColorScheme,
        MinFontSize = o.MinFontSize,
        MaxFontSize = o.MaxFontSize,
        Algorithm = o.Algorithm,
        BoringWordsPath = o.BoringWordsFile
    });
}
 static Result<TagCloudSettings> LoadSettings(CommandLineOptions o)
  {
      if (string.IsNullOrWhiteSpace(o.SettingsFile))
          return ValidateOptions(o); // old CLI path

      if (!File.Exists(o.SettingsFile))
          return Result.Fail<TagCloudSettings>($"Settings file not found: {o.SettingsFile}");

      return Result.Of(() =>
          {
              var json = File.ReadAllText(o.SettingsFile);
              var settings = JsonSerializer.Deserialize<TagCloudSettings>(
                  json,
                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
              if (settings == null)
                  throw new InvalidOperationException("Settings file is empty or invalid.");
              return settings;
          }, $"Failed to read settings file: {o.SettingsFile}")
          .Then(ValidateSettings)
          .Then(settings => Result.Ok(OverrideBoringWords(settings, o.BoringWordsFile)));
  }

  static Result<TagCloudSettings> ValidateSettings(TagCloudSettings s)
  {
      if (s.Width <= 0 || s.Height <= 0)
          return Result.Fail<TagCloudSettings>("Invalid image size. Width and height must be positive");

      if (s.MinFontSize <= 0 || s.MaxFontSize <= 0)
          return Result.Fail<TagCloudSettings>("Invalid font size. Min/Max must be positive");

      if (s.MinFontSize > s.MaxFontSize)
          return Result.Fail<TagCloudSettings>("Invalid font size range. MinFontSize > MaxFontSize");

      if (string.IsNullOrWhiteSpace(s.FontFamily))
          return Result.Fail<TagCloudSettings>("Font family is not specified");

      return Result.Ok(s);
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

