namespace kisa_gcs_service.Model;

public class DroneCommunication
{
    public string Address;

    public DroneCommunication(string address)
    {
        Address = address;
    }

    public override string ToString()
    {
        return Address;
    }

    public override bool Equals(object? obj)
    {
        return obj != null && obj.ToString() == ToString();
    }
    
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}

