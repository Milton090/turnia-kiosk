using Newtonsoft.Json;
using Services.Bixolon;
using Services.Constants;
using Services.Queue.Dtos;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Services.Queue
{
    public class QueueClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BixolonService _bixolonService;
        public DeviceConfigurationCheckin? _deviceConfigurationCheckin = null!;

        #region ServiceValues
        public bool ServiceReady { get; set; }
        public ServicesListDto _services { get; set; } = new ServicesListDto { };

        public DataAvailability ServicesAvailability { get; private set; } = DataAvailability.Unavailable;
        #endregion

        public QueueClientService(HttpClient httpClient, IHttpClientFactory httpClientFactory, BixolonService bixolonService)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HttpClient");
            _bixolonService = bixolonService;
        }

        public void SetHttpClientUrl(string url)
        {
            if (_httpClient != null && !string.IsNullOrWhiteSpace(url))
            {
                _httpClient.BaseAddress = new Uri(url);
            }
        }

        public bool SetDeviceKeyHeader(string deviceKey)
        {
            if (_httpClient != null)
            {
                var header = _httpClient.DefaultRequestHeaders
                    .Where(x => x.Key == ConstantDefinitions.DeviceKeyHeaderDefinition)
                    .FirstOrDefault();

                if (header.Key == null)
                {
                    if (_httpClient.DefaultRequestHeaders.TryAddWithoutValidation(ConstantDefinitions.DeviceKeyHeaderDefinition, deviceKey))
                    {
                        return true;
                    }
                }
                else
                {
                    if (_httpClient.DefaultRequestHeaders.Remove(ConstantDefinitions.DeviceKeyHeaderDefinition))
                    {
                        if (_httpClient.DefaultRequestHeaders.TryAddWithoutValidation(ConstantDefinitions.DeviceKeyHeaderDefinition, deviceKey))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


#region Get Configuration
        public async Task<(bool, DeviceConfigurationCheckin?, TimeSpan?)> GetCheckinDeviceConfiguration()
        {
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();

                var result = await _httpClient.PostAsync(ConstantDefinitions.RouteGetDeviceCheckinConfig, null);

                if (result.IsSuccessStatusCode)
                {
                    var r = await result.Content.ReadFromJsonAsync<DeviceConfigurationCheckin>();
                    stopWatch.Stop();
                    _deviceConfigurationCheckin = r;
                    return (true, r, stopWatch.Elapsed);
                }

                stopWatch.Stop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"QueueClientService: GetCheckinDeviceConfiguration failed, Exception: {ex.Message}");
            }
            return (false, null, stopWatch.Elapsed);
        }

        public async Task<(bool, ServicesListDto?, TimeSpan?)> GetServices()
        {
            var stopWatch = new Stopwatch();
            ServicesAvailability = DataAvailability.Unavailable;

            try
            {
                stopWatch.Start();

                var result = await _httpClient.GetAsync(ConstantDefinitions.RouteGetServices);
                if (result.IsSuccessStatusCode)
                {
                    var r = await result.Content.ReadFromJsonAsync<ServicesListDto>();

                    if (r == null)
                    {
                        return (false, null, stopWatch.Elapsed);
                    }

                    stopWatch.Stop();
                    _services = r;
                    ServicesAvailability = DataAvailabilityHelper.Evaluate(r.Services);
                    return (true, r, stopWatch.Elapsed);
                }

                stopWatch.Stop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"QueueClientService: GetServices failed, Exception: {ex.Message}");
            }
            return (false, null, stopWatch.Elapsed);
        }
        #endregion

        public async Task<(bool, Ticket?, string?)> CreateTicketAsync(AddTicketDto model)
        {
            try
            {
                var ticket = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

                var result = await _httpClient.PostAsync(ConstantDefinitions.RouteCreateTicket, ticket);
                if (result.IsSuccessStatusCode)
                {
                    var r = await result.Content.ReadFromJsonAsync<Ticket>();
                    return (true, r, null);
                }

                var reason = await result.Content.ReadAsStringAsync();
                return (false, null, string.IsNullOrWhiteSpace(reason) ? null : reason);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"QueueClientService: CreateTicketAsync failed, Exception: {ex.Message}");
            }
            return (false, null, null);
        }

        public bool PrintTicket(Ticket ticket)
        {
            try
            {
                var result = _bixolonService.PrintTicket(ticket);
                return result.Item1;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<(bool, TimeSpan?)> MarkTicketAsPrintedAsync(Ticket ticket, string deviceKey)
        {
            var stopWatch = new Stopwatch();

            try
            {
                var dto = new MarkTicketPrintDto()
                {
                    Id = ticket.Id,
                    DeviceKey = deviceKey
                };
                var dtoSerialized = new StringContent(JsonConvert.SerializeObject(dto), System.Text.Encoding.UTF8, "application/json");
                var url = ConstantDefinitions.RouteMarkTicketAsPrinted;
                url = url.Replace("{id}", ticket.Id.ToString());

                stopWatch.Start();
                var result = await _httpClient.PostAsync(url, dtoSerialized);
                if (result.IsSuccessStatusCode)
                {
                    stopWatch.Stop();
                    return (true, stopWatch.Elapsed);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"QueueClientService: MarkTicketAsPrintedAsync failed, Exception: {ex.Message}");
            }
            return (false, stopWatch.Elapsed);
        }
    }
}
