namespace HomeApi.Contracts.Models.Rooms;

public class PutRoomRequest
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int Area { get; set; }
    public bool GasConnected { get; set; }
    public int Voltage { get; set; }
}