using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTileFromPNGImageAsset_Folder {
    const string TestTileFolderName = "Test-Tile-Folder";
    static readonly string FirstAlphabeticalTileAssetWithinFolder
      = $"./{TestTileFolderName}/{TilePorterTestConstants.SingleTilePngFileName}";
    static readonly string DesiredTileAssetWithinFolder
      = $"./{TestTileFolderName}/{TilePorterTestConstants.RedSingleTilePngFileName}";
    static readonly string ConfigWithinFolder
      = $"./{TestTileFolderName}/config.json";
    const string NameConfig = "Test-Tile";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tile From PNG Image Asset -Folder",
        new[] {
          ConfigWithinFolder,
          DesiredTileAssetWithinFolder,
          FirstAlphabeticalTileAssetWithinFolder
        }.ToHashSet(),
        config: JObject.FromObject(new {
          Name = NameConfig,
          DefaultImageFile = $"./{TilePorterTestConstants.RedSingleTilePngFileName}"
        }),
        options: new() {
          { PorterExtensions.PixelsPerTileImportOptionKey, 32 }
        },
        validateCreatedTypesAndProccessedFiles: (builtArchetypes, touchedFiles) => {
          PorterTester.PrintVerboseReport(builtArchetypes, touchedFiles);

          /// check the file we touched.
          if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.RedSingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.RedSingleTilePngFileName}!");
          }
          if (!touchedFiles.Any(tf => Path.GetFileName(tf) == "config.json")) {
            return new(false, $"Did not touch dummy config!");
          }
          if (touchedFiles.Count() != 2) {
            return new(false, $"Should've touched 2 files... touched {touchedFiles.Count()} instead!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 1) {
            return new(false, "More or less than one archetype was created!");
          }
          else if (builtArchetypes.First().Id.Name != NameConfig) {
            return new(false, $"name: '{builtArchetypes.First().Id.Name}' does not equal expected: '{NameConfig}'");
          }
          else if (builtArchetypes.First().PackageKey != PorterTester.DefaultPackageName) {
            return new(false, $"package name: '{builtArchetypes.First().PackageKey}' does not equal expected: '{PorterTester.DefaultPackageName}'");
          }
          else if (builtArchetypes.First().ResourceKey != $"{PorterTester.DefaultPackageName}::{NameConfig}") {
            return new(false, $"resource key: '{builtArchetypes.First().ResourceKey}' does not equal expected: '{PorterTester.DefaultPackageName}::{NameConfig}'");
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
          }
          var firstPixel = builtArchetypes.First().DefaultBackground.sprite.texture.GetPixels().First();
          if (firstPixel.r != 1) {
            return new(false, $"The tile file used should be the red tile. The first pixel found was: r:{firstPixel.r} g:{firstPixel.g} b:{firstPixel.b} a:{firstPixel.a}");
          }
          /// passed all tests
          else {
            return new(true);
          }
        }
      );
  }
}
