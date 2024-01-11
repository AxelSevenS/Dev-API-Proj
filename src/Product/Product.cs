namespace ApiSevenet;

public record Product
{
    public uint Id { get; set; }
    public uint Author { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}