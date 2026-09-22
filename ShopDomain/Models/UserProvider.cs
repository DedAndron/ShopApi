using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopDomain.Models;

[Table("users_providers")]
public class UserProvider
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("provider_id")]
    public int ProviderId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("number_provider")]
    public string NumberProvider { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(ProviderId))]
    public Provider Provider { get; set; } = null!;
}