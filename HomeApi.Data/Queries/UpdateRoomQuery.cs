namespace HomeApi.Data.Queries;

public class UpdateRoomQuery
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int Area { get; set; }
    public bool GasConnected { get; set; }
    public int Voltage { get; set; }

    public UpdateRoomQuery(Guid? id, string name, int area, bool gasConnected, int voltage)
    {
        Id = id;
        Name = name;
        Area = area;
        GasConnected = gasConnected;
        Voltage = voltage;
    }
}