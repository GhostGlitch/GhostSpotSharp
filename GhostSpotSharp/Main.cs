using Windows.Media.Control;
using TCS = Windows.Media.Control.GlobalSystemMediaTransportControlsSession;
using TCSProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties;
using TCSManager = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager;
using Windows.Media;
using System.Drawing;
using Windows.Storage.Streams;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using GhostSpotSharp;

TCSManager Manager = await TCSManager.RequestAsync();
IReadOnlyList<TCS> Sessions = Manager.GetSessions();
Dictionary<TCS, GhostProps> SeshMap = [];

foreach (TCS sesh in Sessions) {
    GhostProps props = new();
    try {
        await Task.Run(async () =>
        {
            TCSProperties tst = await sesh.TryGetMediaPropertiesAsync();
            await props.InitAsync(tst);
        }).ConfigureAwait(false);
    }
    catch (Exception ex) {
        Console.WriteLine(ex.ToString());
    }
    SeshMap[sesh] = props;
}
foreach (GhostProps props in SeshMap.Values) {
#pragma warning disable CS8604 // Possible null reference argument.
    GhostImg.DebugViewImage(props.Thumbnail, props.Title);
#pragma warning restore CS8604 // Possible null reference argument.
    foreach (var kvp in props) {
        if (kvp.Key == "Genres") {
            List<string>? genres = (List<string>)kvp.Value;
            Console.WriteLine($"Genres: {(genres.Count > 0 ? string.Join(", ", genres) : "")}");
        } else if (kvp.Key != "Thumbnail") {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
    Console.WriteLine();
}
