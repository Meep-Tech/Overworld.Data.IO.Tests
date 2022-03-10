using Meep.Tech.Data.IO.Tests;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTileFromPNGImageAsset_Loose_NoConfig {
    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tile From PNG Image Asset -Loose -NoConfig",
        new[] {
          $"./{TilePorterTestConstants.SingleTilePngFileName}"
        }.ToHashSet(),
        options: new() {
          { PorterExtensions.PixelsPerTileImportOptionKey, 32 }
        },
        validateCreatedTypesAndProccessedFiles: (builtArchetypes, touchedFiles) => {
          PorterTester.PrintVerboseReport(builtArchetypes, touchedFiles);

          /// check the file we touched.
          if (touchedFiles.Count() != 1) {
            return new(false, "Touched more than one file!");
          }
          else if (Path.GetFileName(touchedFiles.First()) != TilePorterTestConstants.SingleTilePngFileName) {
            return new(false, $"Touched file: '{touchedFiles.First()}' instead of expexted dummy version of: {TilePorterTestConstants.SingleTilePngFileName}!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 1) {
            return new(false, "More than one archetype was created!");
          }
          else if (builtArchetypes.First().Id.Name != Path.GetFileNameWithoutExtension(TilePorterTestConstants.SingleTilePngFileName)) {
            return new(false, $"name: '{builtArchetypes.First().Id.Name}' does not equal expected: '{Path.GetFileNameWithoutExtension(TilePorterTestConstants.SingleTilePngFileName)}'");
          }
          else if (builtArchetypes.First().PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{builtArchetypes.First().PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (builtArchetypes.First().ResourceKey != "Test's Custom Assets::" + Path.GetFileNameWithoutExtension(TilePorterTestConstants.SingleTilePngFileName)) {
            return new(false, $"resource key: '{builtArchetypes.First().ResourceKey}' does not equal expected: '{"Test's Custom Assets::" + Path.GetFileNameWithoutExtension(TilePorterTestConstants.SingleTilePngFileName)}'");
          }
          else if (builtArchetypes.First().LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false with no config.");
          }
          else if (!builtArchetypes.First().UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (builtArchetypes.First().DefaultBackground.sprite.rect.width != 32 || builtArchetypes.First().DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          } /// passed all tests
          else {
            return new(true);
          }
        }
      );
  }
}
