﻿using System.IO;
using Shouldly;
using TestStack.Dossier.DataSources.Dictionaries;
using Xunit;

namespace TestStack.Dossier.Tests.DataSources.Dictionaries
{
    public class CachedFileDictionaryRepositoryIntegrationTests
    {
        [Fact]
        public void GivenAnExternalFileDictionaryExists_WhenRetrievingWords_ThenExternalDictionaryIsUsed()
        {
            var sut = new CachedFileDictionaryRepository();

            var result = sut.GetWordsFrom("SampleDictionaryFile");

            result.Count.ShouldBe(2);
            result[0].ShouldBe("File first word");
        }

        [Fact]
        public void GivenAResourceDictionaryAndNoExternalFileDictionary_WhenRetrievingWords_ThenResourceDictionaryIsUsed()
        {
            var sut = new CachedFileDictionaryRepository();

            var result = sut.GetWordsFrom("GeoContinent");

            result.Count.ShouldBe(7);
            result[0].ShouldBe("Asia");
        }

        [Fact]
        public void GivenNoResourceDictionaryAndNoExternalFileDictionary_WhenRetrievingWords_ThenFileNotFoundException()
        {
            var sut = new CachedFileDictionaryRepository();
            Should.Throw<FileNotFoundException>(() => sut.GetWordsFrom("NonExistentDictionary"));
        }

    }
}