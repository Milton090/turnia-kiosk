namespace Services.Queue.Dtos
{
    public class DeviceConfigurationCheckin
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string DeviceKey { get; set; } = null!;
    }
}
