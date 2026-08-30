using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopDomain.Models;

public class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    public int OrderId { get; set; }
    [ForeignKey(nameof(OrderId))] 
    public Order Order { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("count")]
    public int Count { get; set; }
}
