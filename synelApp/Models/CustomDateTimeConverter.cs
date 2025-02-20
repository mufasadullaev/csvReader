using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

public class CustomDateTimeConverter : ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParseExact(text, new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" },
                                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return date; //return converted date string to datetime
        }

        throw new TypeConverterException(this, memberMapData, text, row.Context,
            $"Unable to convert '{text}' to DateTime");
    }

    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        throw new NotImplementedException();
    }
}
