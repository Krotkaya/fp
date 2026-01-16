using Autofac;
using CommandLine;
using TagsCloudContainer.Console;
using TagsCloudContainer.Core.DI;
using TagsCloudContainer.Core.Models;

var parser = new Parser(with => with.HelpWriter = Console.Out);

return parser.ParseArguments<CommandLineOptions>(args)
    .MapResult(
        options =>
        { 
            try
            {
                var settings = new TagCloudSettings
                {
                    Width = options.Width,
                    Height = options.Height,
                    FontFamily = options.FontFamily,
                    ColorScheme = options.ColorScheme,
                    MinFontSize = options.MinFontSize,
                    MaxFontSize = options.MaxFontSize,
                    Algorithm = options.Algorithm,
                    BoringWordsPath = options.BoringWordsFile
                };
        
                var builder = new ContainerBuilder();
                builder.RegisterModule(new AutofacModule(settings));
                builder.RegisterInstance(options).As<CommandLineOptions>();
                builder.RegisterType<ConsoleClient>().SingleInstance();
        
                var container = builder.Build();
        
                var layoutOptions = new LayoutOptions
                {
                    Width = options.Width,
                    Height = options.Height,
                    FontFamily = options.FontFamily
                };
        
                var client = container.Resolve<ConsoleClient>();
                client.GenerateTagCloud(options.InputFile, options.OutputFile, layoutOptions);
        
                Console.WriteLine("Tag cloud generated successfully!");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        },
        errors =>
        {
            foreach (var e in errors)
                Console.Error.WriteLine(e.ToString());
            return 1;
        });