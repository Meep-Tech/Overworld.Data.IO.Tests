using Meep.Tech.Data.IO;
using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTileFromPNGImageAsset_Loose {
    const string ConfigName = "Test-Single-Tile";
    const string ConfigPackageName = "Test-Package";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tile From PNG Image Asset -Loose",
        new[] {
          $"./{TilePorterTestConstants.SingleTilePngFileName}"
        }.ToHashSet(),
        config: JObject.FromObject(new {
          Name = ConfigName,
          PackageName = ConfigPackageName
        }),
        options: new() {
          { PorterExtensions.PixelsPerTileImportOptionKey, 32 }
        },
        validateCreatedTypesAndProccessedFiles: (builtArchetypes, touchedFiles) => {
          PorterTester.PrintVerboseReport(builtArchetypes, touchedFiles);

          /// check the file we touched.
          if (touchedFiles.Count() != 2) {
            return new(false, $"Should've touched 2 files... touched {touchedFiles.Count()} instead!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SingleTilePngFileName}!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == ArchetypePorter.DefaultConfigFileName)) {
            return new(false, $"Did not touch config file: {ArchetypePorter.DefaultConfigFileName}!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 1) {
            return new(false, "More than one archetype was created!");
          }
          else if (builtArchetypes.First().Id.Name != ConfigName) {
            return new(false, $"name: '{builtArchetypes.First().Id.Name}' does not equal expected: '{ConfigName}'");
          }
          else if (builtArchetypes.First().PackageKey != ConfigPackageName) {
            return new(false, $"package name: '{builtArchetypes.First().PackageKey}' does not equal expected: '{ConfigPackageName}'");
          }
          else if (builtArchetypes.First().ResourceKey != $"{ConfigPackageName}::{ConfigName}") {
            return new(false, $"resource key: '{builtArchetypes.First().ResourceKey}' does not equal expected: '{ConfigPackageName}::{ConfigName}'");
          }
          else if (builtArchetypes.First().LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false with no config.");
          }
          else if (!builtArchetypes.First().UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (builtArchetypes.First().DefaultBackground.sprite.rect.width != 32 || builtArchetypes.First().DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (builtArchetypes.First().DefaultHeight.HasValue) {
            return new(false, $"The tile produced should not have a default height value.");
          } /// passed all tests
          else {
            return new(true);
          }
        }
      );
  }
}
