#nullable disable

using System.ComponentModel.DataAnnotations;

namespace InfinityElectronics.Models.Domain;

public class Category
{
    // Limit string size ie:
    [MaxLength(100)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}