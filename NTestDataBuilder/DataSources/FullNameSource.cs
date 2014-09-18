﻿using System.Collections.Generic;
using System.Linq;
using NTestDataBuilder.DataSources.FileData;
using NTestDataBuilder.DataSources.Generators;

namespace NTestDataBuilder.DataSources
{
    /// <summary>
    /// Free sample data for testing obtained from http://www.briandunning.com/sample-data/
    /// </summary>
    public class FullNameSource : DataSource<string>
    {
        /// <inheritdoc />
        public FullNameSource() 
            : base() { }

        /// <inheritdoc />
        public FullNameSource(IGenerator generator) 
            : base(generator) { }

        protected override IList<string> InitializeList()
        {
            return FileDataRepository.People
                .Select(person => person.FirstName + " " + person.LastName)
                .ToList();
        }
    }
}