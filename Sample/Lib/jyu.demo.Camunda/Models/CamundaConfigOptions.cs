using System.Text;

namespace jyu.demo.Camunda.Models;

public class CamundaConfigOptions
{
    public string?Protocol { get; set; }

    public string? Host { get; set; }

    public int? Port { get; set; }

    public string? ContextPath { get; set; }
}