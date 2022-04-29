﻿using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class Order : BaseEntity
{
    [Key]
    public int OrderId { get; set; }
    [Required]
    public int OrderNumber { get; set; }
    [Required]
    public decimal Sum { get; set; }
    [Required]
    [MaxLength(30)]
    public string PhoneNumber { get; set; }
    [Required]
    [MaxLength(60)]
    public string Address { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<OrdersBooks> OrdersBook { get; set; }
}