namespace Shop.Application.DTOs.DeliveryAddressDTOs;

public class DeliveryAddressReadDTO
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public string? RecipientName { get; set; }
    public string? PhoneNumber { get; set; }
}