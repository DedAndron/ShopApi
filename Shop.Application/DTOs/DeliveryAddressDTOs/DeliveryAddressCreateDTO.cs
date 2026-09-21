using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.DeliveryAddressDTOs;

public class DeliveryAddressCreateDTO
{
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Street { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [MaxLength(100)]
    public string? RecipientName { get; set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }
}