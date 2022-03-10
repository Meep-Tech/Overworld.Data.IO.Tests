using Meep.Tech.Data.IO.Tests;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTileFromPNGImageAsset_FolderAndLoose_NoConfig {
    const string TestTileFolderName = "Test-Tile-Folder";
    static readonly string SingleTileAssetWithinFolder
      = $"./{TestTileFolderName}/{TilePorterTestConstants.SingleTilePngFileName}";
    static readonly string ExtraneousTileAssetWithinFolder
      = $"./{TestTileFolderName}/{TilePorterTestConstants.RedSingleTilePngFileName}";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tile From PNG Image Asset -FolderAndLoose -NoConfig",
        new[] {
          $"./{TilePorterTestConstants.SecondSingleTilePngFileName}",
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
          /// check the file we touched.
          if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SecondSingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SecondSingleTilePngFileName}!");
          }
          if (touchedFiles.Count() != 2) {
            return new(false, $"Should've touched 2 file... touched {touchedFiles.Count()} instead!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 2) {
            return new(false, $"Expected 2 archetypes (bg and special) but produced: {builtArchetypes.Count()} instead!");
          }

          /// check the archetype we made.
          Tile.Type fromFolderType = builtArchetypes.First();
          if (fromFolderType.Id.Name != TestTileFolderName) {
            return new(false, $"name: '{fromFolderType.Id.Name}' does not equal expected: '{TestTileFolderName}'");
          }
          else if (fromFolderType.PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{fromFolderType.PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (fromFolderType.ResourceKey != $"Test's Custom Assets::__Single Tile From PNG Image Asset -FolderAndLoose -NoConfig/__dummy_op/__dummy_ip/__test/Test-Tile-Folder") {
            return new(false, $"resource key: '{fromFolderType.ResourceKey}' does not equal expected: '{$"Test's Custom Assets::__Single Tile From PNG Image Asset -FolderAndLoose -NoConfig/__dummy_op/__dummy_ip/__test/Test-Tile-Folder"}'");
          }
          else if (fromFolderType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false with no config.");
          }
          else if (!fromFolderType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (fromFolderType.DefaultBackground.sprite.rect.width != 32 || fromFolderType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }

          Tile.Type looseType = builtArchetypes.Last();
          if (looseType.Id.Name != Path.GetFileNameWithoutExtension(TilePorterTestConstants.SecondSingleTilePngFileName)) {
            return new(false, $"name: '{looseType.Id.Name}' does not equal expected: '{Path.GetFileNameWithoutExtension(TilePorterTestConstants.SecondSingleTilePngFileName)}'");
          }
          else if (looseType.PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{looseType.PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (looseType.ResourceKey != "Test's Custom Assets::" + Path.GetFileNameWithoutExtension(TilePorterTestConstants.SecondSingleTilePngFileName)) {
            return new(false, $"resource key: '{looseType.ResourceKey}' does not equal expected: '{"Test's Custom Assets::" + Path.GetFileNameWithoutExtension(TilePorterTestConstants.SecondSingleTilePngFileName)}'");
          }
          else if (looseType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false with no config.");
          }
          else if (!looseType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (looseType.DefaultBackground.sprite.rect.width != 32 || looseType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          /// passed all tests
          else {
            return new(true);
          }
        }
      );
  }
}
