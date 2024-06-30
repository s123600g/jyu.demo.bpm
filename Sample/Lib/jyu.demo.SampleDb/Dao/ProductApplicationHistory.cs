using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace jyu.demo.SampleDb.Dao;

[Table("Product_Application_History")]
public partial class ProductApplicationHistory
{
    [Key]
    [Column(TypeName = "VARCHAR(255)")]
    public string Id { get; set; } = null!;

    [Column("Product_Name", TypeName = "VARCHAR(255)")]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "NUMERIC(10,4)")]
    public decimal Price { get; set; }

    [Column(TypeName = "NUMERIC(2)")]
    public decimal? Status { get; set; }

    [Column(TypeName = "VARCHAR(50)")]
    public string? Reviewer { get; set; }

    [Column("Process_Instance_Id", TypeName = "VARCHAR(255)")]
    public string? ProcessInstanceId { get; set; }
}
