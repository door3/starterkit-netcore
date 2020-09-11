using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CsvHelper;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Models
{
    public class ReadCsvOptions
    {
        public Encoding Encoding { get; set; } = Encoding.Default;

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        public Action<CsvReader> BeforeParsingAction { get; set; }

        public Action<IList<string>> AfterParsingAction { get; set; }

        public Func<CsvHelperException, bool> OnReadingException { get; set; }

        public Action<bool, string[], int, ReadingContext> OnHeaderValidated { get; set; }

        public Func<ReadingContext, bool> BadDataFunc { get; set; }

        public ReadCsvOptions()
        {
        }

        public ReadCsvOptions(Encoding encoding)
        {
            Encoding = encoding.NotNull(nameof(encoding));
        }
    }
}
