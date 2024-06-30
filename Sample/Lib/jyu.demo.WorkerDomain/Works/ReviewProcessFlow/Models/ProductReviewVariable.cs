using System.Text.Json.Serialization;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Models;

public class ProductReviewVariable
{
    public VariableDetail<string> Id { get; set; }

    public VariableDetail<string> ProductName { get; set; }

    public VariableDetail<decimal> Price { get; set; }
    
    public VariableDetail<string> Reviewer { get; set; }
    
    public VariableDetail<bool> IsApproval { get; set; }
}