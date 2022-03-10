using Meep.Tech.Data.IO.Tests;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTileFromPNGImageAsset_Folder_NoConfig {
    const string TestTileFolderName = "Test-Tile-Folder";
    static readonly string SingleTileAssetWithinFolder
      = $"./{TestTileFolderName}/{TilePorterTestConstants.SingleTilePngFileName}";
    static readonly string ExtraneousTileAssetWithinFolder
      = $"./{TestTileFolderName}/{TilePorterTestConstants.RedSingleTilePngFileName}";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tile From PNG Image Asset -Folder -NoConfig",
        new[] {
          ExtraneousTileAssetWithinFolder,
          SingleTileAssetWithinFolder
        }.ToHashSet(),
        options: new() {
          { PorterExtensions.PixelsPerTileImportOptionKey, 32 }
        },
        validateCreatedTypesAndProccessedFiles: (builtArchetypes, touchedFiles) => {
          PorterTester.PrintVerboseReport(builtArchetypes, touchedFiles);

          /// check the file we touched.
          if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SingleTilePngFileName}!");
          }
          if (touchedFiles.Count() != 1) {
            return new(false, $"Should've touched 1 file... touched {touchedFiles.Count()} instead!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 1) {
            return new(false, "More than one archetype was created!");
          }
          else if (builtArchetypes.First().Id.Name != TestTileFolderName) {
            return new(false, $"name: '{builtArchetypes.First().Id.Name}' does not equal expected: '{TestTileFolderName}'");
          }
          else if (builtArchetypes.First().PackageKey != PorterTester.DefaultPackageName) {
            return new(false, $"package name: '{builtArchetypes.First().PackageKey}' does not equal expected: '{PorterTester.DefaultPackageName}'");
          }
          else if (builtArchetypes.First().ResourceKey != $"Test's Custom Assets::__Single Tile From PNG Image Asset -Folder -NoConfig/__dummy_op/__dummy_ip/__test/Test-Tile-Folder") {
            return new(false, $"resource key: '{builtArchetypes.First().ResourceKey}' does not equal expected: 'Test's Custom Assets::__Single Tile From PNG Image Asset -Folder -NoConfig/__dummy_op/__dummy_ip/__test/Test-Tile-Folder'");
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
          else if (builtArchetypes.First().DefaultBackground.sprite.texture.GetPixels().First().r == 1) {
            return new(false, $"The tile file used should not be the red tile.");
          }
          /// passed all tests
          else {
            return new(true);
          }
        }
      );
  }
}
