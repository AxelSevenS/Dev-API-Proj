namespace ApiSevenet;

public record User
{
    public uint Id { get; set; } = 1;
    public string? Username { get; set; } = null;
    public string? Password { get; set; } = null;
    public bool Admin { get; set; } = false;
}