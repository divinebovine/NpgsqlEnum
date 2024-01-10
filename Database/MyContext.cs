using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace npgsqlEnum;

public enum Color
{
    Red = 0b000001,
    Yellow = 0b000010,
    Orange = 0b000100,
    Blue = 0b001000,
    Magenta = 0b010000,
    Green = 0b100000
}

public class Entry
{
    public int Id { get; set; }
    public Color Color { get; set; }
    public Entry()
    { }
    public Entry(int id, Color color)
    {
        Id = id;
        Color = color;
    }
}

public class MyContext : DbContext
{
    public DbSet<Entry> Entries { get; set; } = null!;

    public MyContext(DbContextOptions<MyContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .HasPostgresEnum<Color>();

        builder.Entity<Entry>()
            .Property(x => x.Color)
            .HasConversion<Color>();
    }
}
