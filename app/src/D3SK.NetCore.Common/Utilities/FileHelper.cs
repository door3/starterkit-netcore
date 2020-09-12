using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Models;

namespace D3SK.NetCore.Common.Utilities
{
    public static class FileHelper
    {
        public static Task<IList<T>> ReadCsvAsync<T>(string path, string fileName, string relativePath = null,
            ReadCsvOptions options = null)
        {
            var file = FindFile(path, fileName, relativePath);
            return ReadCsvAsync<T>(file.FullName, options);
        }

        public static async Task<IList<T>> ReadCsvAsync<T>(string filePath, ReadCsvOptions options = null)
        {
            options ??= new ReadCsvOptions();

            using var reader = new StreamReader(filePath, options.Encoding);
            using var csv = new CsvReader(reader, options.CultureInfo, false);

            options.BeforeParsingAction?.Invoke(csv);
            var badDataFunc = options.BadDataFunc ?? (context => true);

            var good = new List<T>();
            var bad = new List<string>();
            var isRecordBad = false;

            csv.Configuration.BadDataFound = context =>
            {
                if (badDataFunc(context))
                {
                    isRecordBad = true;
                    bad.Add(context.RawRecord);
                }
            };

            csv.Configuration.ReadingExceptionOccurred = options.OnReadingException ?? (ex => false);
            csv.Configuration.HeaderValidated =
                options.OnHeaderValidated ?? ((isValid, headerNames, headerNameIndex, context) => { });
            
            while (await csv.ReadAsync())
            {
                var record = csv.GetRecord<T>();
                if (!isRecordBad)
                {
                    good.Add(record);
                }

                isRecordBad = false;
            }

            options.AfterParsingAction?.Invoke(bad);
            return good.ToList();
        }

        public static Task<IList<T>> ReadCsvAsync<T>(string filePath, Encoding encoding)
        {
            return ReadCsvAsync<T>(filePath, new ReadCsvOptions(encoding));
        }

        public static Task<IList<T>> ReadCsvAsync<T, TMap>(string filePath, ReadCsvOptions options = null)
            where TMap : ClassMap<T>
        {
            options ??= new ReadCsvOptions();
            var bpAction = options.BeforeParsingAction;
            options.BeforeParsingAction = csv =>
            {
                csv.Configuration.RegisterClassMap<TMap>();
                bpAction?.Invoke(csv);
            };
            return ReadCsvAsync<T>(filePath, options);
        }

        public static Task<IList<T>> ReadCsvAsync<T, TMap>(string filePath, Encoding encoding)
            where TMap : ClassMap<T>
        {
            return ReadCsvAsync<T, TMap>(filePath, new ReadCsvOptions(encoding));
        }

        public static bool FileExists(string path, string fileName, string relativePath = null)
        {
            return FindFile(path, fileName, relativePath) != null;
        }

        public static FileInfo FindFile(string path, string fileName, string relativePath = null)
        {
            while (true)
            {
                var filePath = Path.Combine(path, relativePath, fileName);
                if (File.Exists(filePath))
                {
                    return new FileInfo(filePath);
                }

                var newPath = Directory.GetParent(path)?.FullName;
                if (newPath.IsEmpty())
                {
                    return null;
                }

                path = newPath;
            }
        }

        public static string GenerateUniqueFileName(string fileName, string path)
        {
            var originalName = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            var nameCounter = 1;

            while (File.Exists(Path.Combine(path, fileName)))
            {
                fileName = $"{originalName}{'_'}{nameCounter++}{ext}";
            }

            return fileName;
        }

        public static async Task<string> SaveFileAsync(string path, string fileName, Stream stream)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            fileName = GenerateUniqueFileName(fileName, path);
            var filePath = Path.Combine(path, fileName);

            await using (var file = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(file);
            }

            return fileName;
        }

        public static string GetFileContentType(string filename)
        {
            var ext = Path.GetExtension(filename);
            return MimeTypes.GetMimeType(ext);
        }
    }
}
