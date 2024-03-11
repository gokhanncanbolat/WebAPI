using WebAPI.Utilities.Formatters;

namespace WebAPI.Extension
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(options =>
                   {
                       options.OutputFormatters
                       .Add(new CsvOutpuFormater());
                   });
        }
    }
}
