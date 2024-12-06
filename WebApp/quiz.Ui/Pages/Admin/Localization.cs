
namespace quiz.Ui.Pages.Admin;

/// <summary>
/// Класс содержит информацию о записи прогноза погоды на день.
/// </summary>
public class WeatherForecast 
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }
    
    public string? Summary { get; set; }

    public string? Cloudiness { get; set; }
}

/// <summary>
/// Страница с переключателем языка UI и простым заполненным Grid для просмотра результата.
/// </summary>
public partial class Localization
{
    private List<WeatherForecast> Forecast { get; set; }

    private static readonly string[] CloudCover = new[] {
                                                            "Sunny", "Partly cloudy", "Cloudy", "Storm"
                                                        };

    readonly Tuple<int, string>[] ConditionsForForecast = new Tuple<int, string>[] {
                                                                                       Tuple.Create( 22 , "Hot"),
                                                                                       Tuple.Create( 13 , "Warm"),
                                                                                       Tuple.Create( 0 , "Cold"),
                                                                                       Tuple.Create( -10 , "Freezing")
                                                                                   };
    
    protected override async Task OnInitializedAsync() 
    {
        Forecast = await CreateForecast();
    }
    
    /// <summary>
    /// Генерация случайным образом данных прогноза погоды.
    /// </summary>
    private async Task<List<WeatherForecast>> CreateForecast() {
        var rng = new Random();
        DateTime startDate = DateTime.Now;
        return Enumerable.Range(1, 30).Select(index => {
                                                  var temperatureC = rng.Next(-10, 30);
                                                  return new WeatherForecast {
                                                                                 Date = startDate.AddDays(index),
                                                                                 TemperatureC = temperatureC,
                                                                                 Cloudiness = CloudCover[rng.Next(0, 4)],
                                                                                 Summary = ConditionsForForecast.First(c => c.Item1 <= temperatureC).Item2
                                                                             };
                                              }).ToList();
    }
}
