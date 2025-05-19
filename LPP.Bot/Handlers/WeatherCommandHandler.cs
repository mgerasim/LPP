using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Newtonsoft.Json;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{

    public class Weather
    {
        [JsonProperty("station_id")]
        public int StationId { get; set; }

        [JsonProperty("date_utc")]
        public object DateUtc { get; set; }

        [JsonProperty("temperature_day_min")]
        public int TemperatureDayMin { get; set; }

        [JsonProperty("temperature_day_max")]
        public int TemperatureDayMax { get; set; }

        [JsonProperty("temperature_night_min")]
        public int TemperatureNightMin { get; set; }

        [JsonProperty("temperature_night_max")]
        public int TemperatureNightMax { get; set; }

        [JsonProperty("temperature_current")]
        public double TemperatureCurrent { get; set; }

        [JsonProperty("temperature_previus")]
        public double TemperaturePrevius { get; set; }

        [JsonProperty("temperature_checked")]
        public double TemperatureChecked { get; set; }

        [JsonProperty("temperature_dew")]
        public double TemperatureDew { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("fall_current")]
        public double FallCurrent { get; set; }

        [JsonProperty("wind_gusts")]
        public int WindGusts { get; set; }

        [JsonProperty("wind_speed")]
        public int WindSpeed { get; set; }

        [JsonProperty("wind_direction")]
        public int WindDirection { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("whether_code")]
        public int? WhetherCode { get; set; }

        [JsonProperty("wheather")]
        public string Wheather { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("cloud")]
        public int Cloud { get; set; }

        [JsonProperty("snow_height")]
        public int SnowHeight { get; set; }

        [JsonProperty("comfort")]
        public int Comfort { get; set; }

        [JsonProperty("station_name")]
        public string StationName { get; set; }

        [JsonProperty("organization")]
        public object Organization { get; set; }

        [JsonProperty("level_last")]
        public int? LevelLast { get; set; }

        [JsonProperty("level_last_date")]
        public string LevelLastDate { get; set; }

        [JsonProperty("level8")]
        public int? Level8 { get; set; }

        [JsonProperty("level_current_date")]
        public string LevelCurrentDate { get; set; }

        [JsonProperty("ice_level_last")]
        public object IceLevelLast { get; set; }

        [JsonProperty("level20")]
        public int? Level20 { get; set; }

        [JsonProperty("w_t")]
        public double? WT { get; set; }

        [JsonProperty("water_level_change")]
        public int? WaterLevelChange { get; set; }

        [JsonProperty("fallout")]
        public double? Fallout { get; set; }

        [JsonProperty("water_rate")]
        public double? WaterRate { get; set; }

        [JsonProperty("oy")]
        public int? Oy { get; set; }

        [JsonProperty("ny")]
        public int? Ny { get; set; }

        [JsonProperty("poima")]
        public int? Poima { get; set; }

        [JsonProperty("extream_now")]
        public string ExtreamNow { get; set; }

        [JsonProperty("extream_date_first")]
        public object ExtreamDateFirst { get; set; }

        [JsonProperty("previus_level")]
        public int? PreviusLevel { get; set; }

        [JsonProperty("extream_date_second")]
        public object ExtreamDateSecond { get; set; }

        [JsonProperty("trend")]
        public string Trend { get; set; }

        [JsonProperty("geoobject")]
        public string Geoobject { get; set; }

        [JsonProperty("water_dec_norm")]
        public int? WaterDecNorm { get; set; }

        [JsonProperty("five_days_levels")]
        public List<List<object>> FiveDaysLevels { get; set; }

        [JsonProperty("five_days_falls")]
        public List<object> FiveDaysFalls { get; set; }
    }

    public class WeatherCommand : IRequest
    {

    }

    public class WeatherCommandHandler : IRequestHandler<WeatherCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public WeatherCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(WeatherCommand request, CancellationToken cancellationToken)
        {
            var weather = await this.GerWeatherNowAsync();

            var keyboard = this.keyboardHandler.GetKeaboard();

            var text =  $@"
<b>Погода в Хабаровске 🌞 {DateTime.Now.Day:00}.{DateTime.Now.Month:00}.{DateTime.Now.Year}:</b> 

<i>🌡Температура <b>{weather[0].TemperatureCurrent}</b>°C:</i>

днём ☀ {weather[0].TemperatureDayMin}..{weather[0].TemperatureDayMax}°C
ночью 🌙 {weather[0].TemperatureNightMin}..{weather[0].TemperatureNightMax}°C

<i>🌬Ветер</i> {this.WindHuman(weather[0].WindDirection)} от {weather[0].WindSpeed} до {weather[0].WindGusts} м/с
                ";

            await this.userState.BotClient.SendMessage(
                chatId: this.userState.ChatId,
                text: text,
                parseMode: ParseMode.Html,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }

        public async Task<List<Weather>> GerWeatherNowAsync()
        {
            var url = $@"https://khabmeteo.ru/cgi/main_widget.php?type=1&city_id=77";

            var response = await this.GetAsync(url);

            return JsonConvert.DeserializeObject<List<Weather>>(response);
        }

        private async Task<string> GetAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        private string WindHuman(int windDirection)
        {
            var windDirections = new List<string> {
                "северный",
                "северо-восточный",
                "восточный",
                "юго-восточный",
                "южный",
                "юго-западный",
                "западный",
                "северо-западный"};

            int direction = (int)((windDirection + 22.5) / 45 % 8);

            return windDirections[direction];
        }
    }
}
