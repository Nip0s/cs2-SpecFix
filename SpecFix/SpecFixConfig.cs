using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace SpecFix;

public class SpecFixConfig : BasePluginConfig
{
    public override int Version { get; set; } = 2;

    /// <summary>Master switch for the fix.</summary>
    [JsonPropertyName("Enabled")]
    public bool Enabled { get; set; } = true;

    /// <summary>Print a debug line to server console whenever a phantom pawn is removed.</summary>
    [JsonPropertyName("DebugLog")]
    public bool DebugLog { get; set; } = true;
}
