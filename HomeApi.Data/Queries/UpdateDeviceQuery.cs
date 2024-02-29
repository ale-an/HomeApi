namespace HomeApi.Data.Queries
{
    /// <summary>
    /// Класс для передачи дополнительных параметров при обновлении устройства
    /// </summary>
    public class UpdateDeviceQuery(string newName, string newSerial)
    {
        public string NewName { get; } = newName;
        public string NewSerial { get; } = newSerial;
    }
}