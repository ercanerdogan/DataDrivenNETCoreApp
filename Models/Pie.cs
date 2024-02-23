using System.ComponentModel.DataAnnotations;

namespace BethanysPieShopAdmin.Models;

public class Pie
{
    public int PieId { get; set; }
    [Display(Name = "Name")]
    [Required]
    public string Name { get; set; } = string.Empty;
    [StringLength(100)]
    [Display(Name = "Short description")]
    public string? ShortDescription { get; set; }
    [StringLength(1000)]
    [Display(Name = "Long description")]
    public string? LongDescription { get; set; }
    [StringLength(1000)]
    [Display(Name = "Allergy Information")]
    public string? AllergyInformation { get; set; }
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPieOfTheWeek { get; set; }
    public bool InStock { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Ingredient>? Ingredients { get; set; }
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}