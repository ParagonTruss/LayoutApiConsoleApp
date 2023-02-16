using ParagonApi;
using ParagonApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var layoutDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "layout.json");

var layoutDataContents = File.ReadAllText(layoutDataFilePath);
var layoutData = JsonSerializer.Deserialize<LayoutData>(layoutDataContents, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters =
    {
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    },
});

using var connection = await Paragon.Connect(ParagonApi.Environments.Environment.Localhost);

// Set this to the GUID of your project.
var projectGuid = Guid.Parse("");

Console.WriteLine("Uploading layout data...");

foreach (var roofContainer in layoutData.RoofContainers)
{
    await connection.Projects.AddRoofContainer(projectGuid, roofContainer);
}

foreach (var bearingEnvelope in layoutData.BearingEnvelopes)
{
    await connection.Projects.AddBearingEnvelope(projectGuid, bearingEnvelope);
}

Console.WriteLine("Done!");

class LayoutData
{
    public List<RoofContainer> RoofContainers { get; set; }
    public List<BearingEnvelope> BearingEnvelopes { get; set; }
}
