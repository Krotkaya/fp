using Autofac;
using SkiaSharp;
using TagsCloudContainer.Core.Infrastructure.Analysis;
using TagsCloudContainer.Core.Infrastructure.Coloring;
using TagsCloudContainer.Core.Infrastructure.Layout;
using TagsCloudContainer.Core.Infrastructure.Preprocessing;
using TagsCloudContainer.Core.Infrastructure.Reading;
using TagsCloudContainer.Core.Infrastructure.Rendering;
using TagsCloudContainer.Core.Models;

namespace TagsCloudContainer.Core.DI;
public class AutofacModule(TagCloudSettings settings) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(settings).As<TagCloudSettings>();
        
        builder.RegisterType<TextFileReader>().As<IFileTextReader>();
        builder.RegisterType<DocxTextReader>().As<IFileTextReader>();

        builder.RegisterType<MultiFormatTextReader>()
            .As<ITextReader>().SingleInstance();
        
        builder.Register<IBoringWordsProvider>(_ =>
                string.IsNullOrWhiteSpace(settings.BoringWordsPath)
                    ? new DefaultBoringWordsProvider()
                    : new FileBoringWordsProvider(settings.BoringWordsPath))
            .SingleInstance();
        
        builder.Register<ITextPreprocessor>(c =>
        {
            var boringWords = c.Resolve<IBoringWordsProvider>();
            return new CompositePreprocessor([
                new LowerCaseNormalizer(),
                new BoringWordsFilter(boringWords)
            ]);
        }).SingleInstance();
        
        builder.RegisterType<WordFrequencyAnalyzer>().As<IWordFrequencyAnalyzer>().SingleInstance();
        
        builder.Register<IFontSizeCalculator>(_ 
            => new LinearFontSizeCalculator(settings.MinFontSize, settings.MaxFontSize)).SingleInstance();
        
        builder.RegisterType<RandomColorScheme>()
            .Keyed<IColorScheme>(ColorSchemeType.Random)
            .SingleInstance();

        builder.Register(_ => new GradientColorScheme(SKColors.Blue, SKColors.LightBlue))
            .Keyed<IColorScheme>(ColorSchemeType.Gradient)
            .SingleInstance();

        builder.Register<IColorScheme>(ctx =>
                ctx.ResolveKeyed<IColorScheme>(settings.ColorScheme))
            .SingleInstance();
        
        builder.RegisterType<SpiralTagCloudAlgorithm>()
            .Keyed<ITagCloudAlgorithm>(TagCloudAlgorithmType.Spiral)
            .SingleInstance();

        builder.RegisterType<TightSpiralTagCloudAlgorithm>()
            .Keyed<ITagCloudAlgorithm>(TagCloudAlgorithmType.Tight)
            .SingleInstance();

        builder.Register<ITagCloudAlgorithm>(ctx =>
                ctx.ResolveKeyed<ITagCloudAlgorithm>(settings.Algorithm))
            .SingleInstance();

        builder.RegisterType<PngRenderer>().As<ITagCloudRenderer>().SingleInstance();
    }
}
