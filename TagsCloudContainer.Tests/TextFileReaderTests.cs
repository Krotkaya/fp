using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Core.Infrastructure.Reading;

namespace TagsCloudContainer.Tests;
[TestFixture]
public class TextFileReaderTests
{
    private TextFileReader _reader;

    [SetUp]
    public void SetUp() => _reader = new TextFileReader();

    [Test]
    public void CanRead_ShouldReturnTrue_ForTxtFiles()
    {
        _reader.CanRead("test.txt").Should().BeTrue();
    }

    [Test]
    public void CanRead_ShouldReturnFalse_ForNonTxtFiles()
    {
        _reader.CanRead("test.doc").Should().BeFalse();
    }

    [Test]
    public void ReadWords_ShouldReturnAllLines_FromExistingFile()
    {
        using var temp = new TempFile(new[] { "word1", "word2", "word3" });

        var words = _reader.ReadWords(temp.Path);

        words.IsSuccess.Should().BeTrue();
        words.GetValueOrThrow().Should().Equal("word1", "word2", "word3");
    }

    [Test]
    public void ReadWords_ShouldReturnFail_ForNonExistingFile()
    {
        var result = _reader.ReadWords("non_existing_file.txt");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("File not found");

    }

    [Test]
    public void ReadWords_ShouldSkipEmptyLines()
    {
        using var temp = new TempFile(new[] { "word1", "", "word2", "   ", "word3" });

        var words = _reader.ReadWords(temp.Path);

        words.IsSuccess.Should().BeTrue();
        words.GetValueOrThrow().Should().Equal("word1", "word2", "word3");

    }

    private sealed class TempFile : IDisposable
    {
        public string Path { get; }

        public TempFile(IEnumerable<string> lines)
        {
            Path = System.IO.Path.GetTempFileName();
            File.WriteAllLines(Path, lines);
        }
        public void Dispose()
        {
            if (File.Exists(Path))
                File.Delete(Path);
        }
    }
}
