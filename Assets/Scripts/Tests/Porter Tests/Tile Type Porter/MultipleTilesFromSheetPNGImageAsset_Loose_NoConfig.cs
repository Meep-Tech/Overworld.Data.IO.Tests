using Meep.Tech.Data.IO.Tests;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class MultipleTilesFromSheetPNGImageAsset_Loose_NoConfig {
    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test = new(
        "Multiple Tile From Sheet PNG Image Asset -Loose, -NoConfig",
        new[] {
        $"./{TilePorterTestConstants.TileSheetPngFileName}"
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
          else if (Path.GetFileName(touchedFiles.First()) != TilePorterTestConstants.TileSheetPngFileName) {
            return new(false, $"Touched file: '{touchedFiles.First()}' instead of expexted dummy version of: {TilePorterTestConstants.TileSheetPngFileName}!");
          }

          /// test to make sure we made all the files.
          if (builtArchetypes.Count() != 105) {
            return new(false, "105 Archetypes expected from test sheet!");
          } /// test the first asset
          else if (builtArchetypes.First().Id.Name != Path.GetFileNameWithoutExtension(TilePorterTestConstants.TileSheetPngFileName) + " - 1") {
            return new(false, $"name: '{builtArchetypes.First().Id.Name}' does not equal expected: '{Path.GetFileNameWithoutExtension(TilePorterTestConstants.TileSheetPngFileName) + " - 1"}'");
          }
          else if (builtArchetypes.First().PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{builtArchetypes.First().PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (builtArchetypes.First().ResourceKey != "Test's Custom Assets::" + Path.GetFileNameWithoutExtension(TilePorterTestConstants.TileSheetPngFileName) + " - 1") {
            return new(false, $"resource key: '{builtArchetypes.First().ResourceKey}' does not equal expected: '{"Test's Custom Assets::" + Path.GetFileNameWithoutExtension(TilePorterTestConstants.TileSheetPngFileName) + " - 1"}'");
          }
          else if (builtArchetypes.Any(a => a.LinkArchetypeToTileDataOnSet)) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false with no config.");
          }
          else if (builtArchetypes.Any(a => !a.UseDefaultBackgroundAsInWorldTileImage)) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (builtArchetypes.Any(a => a.DefaultBackground.sprite.rect.width != 32 || a.DefaultBackground.sprite.rect.height != 32)) {
            return new(false, $"All tile producedby this sprite sheet should be 32x32 pixels.");
          } /// passed all tests
          else {
            return new(true);
          }
        }
      );
  }
}
