using Meep.Tech.Data.IO;
using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTilesFromMultiplePNGImageAsset_Loose_AssetFoundByConfigProperty {
    const string ConfigName = "Test-Single-Tile";
    const string ConfigPackageName = "Test-Package";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tiles From Multiple PNG Image Assets -Loose -AssetFoundByConfigProperty",
        new[] {
          $"./{TilePorterTestConstants.SecondSingleTilePngFileName}",
          $"./{TilePorterTestConstants.SingleTilePngFileName}"
        }.ToHashSet(),
        config: JObject.FromObject(new {
          Name = ConfigName,
          PackageName = ConfigPackageName,
          DefaultImageFile = $"./{TilePorterTestConstants.SecondSingleTilePngFileName}"
        }),
        options: new() {
          { PorterExtensions.PixelsPerTileImportOptionKey, 32 }
        },
        validateCreatedTypesAndProccessedFiles: (builtArchetypes, touchedFiles) => {
          PorterTester.PrintVerboseReport(builtArchetypes, touchedFiles);

          /// check the file we touched.
          if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SingleTilePngFileName}!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SecondSingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SecondSingleTilePngFileName}!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == ArchetypePorter.DefaultConfigFileName)) {
            return new(false, $"Did not touch config file: {ArchetypePorter.DefaultConfigFileName}!");
          }
          if (touchedFiles.Count() != 3) {
            return new(false, $"Should've touched 3 files... touched {touchedFiles.Count()} instead!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 2) {
            return new(false, $"Expected 2 archetypes (config and no-config) but produced: {builtArchetypes.Count()} instead!");
          }

          Tile.Type typeWithConfig = builtArchetypes.First();
          if (typeWithConfig.Id.Name != ConfigName) {
            return new(false, $"name: '{typeWithConfig.Id.Name}' does not equal expected: '{ConfigName}'");
          }
          else if (typeWithConfig.PackageKey != ConfigPackageName) {
            return new(false, $"package name: '{typeWithConfig.PackageKey}' does not equal expected: '{ConfigPackageName}'");
          }
          else if (typeWithConfig.ResourceKey != $"{ConfigPackageName}::{ConfigName}") {
            return new(false, $"resource key: '{typeWithConfig.ResourceKey}' does not equal expected: '{ConfigPackageName}::{ConfigName}'");
          }
          else if (typeWithConfig.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false.");
          }
          else if (!typeWithConfig.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (typeWithConfig.DefaultBackground.sprite.rect.width != 32 || typeWithConfig.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (typeWithConfig.DefaultHeight.HasValue) {
            return new(false, $"The tiles produced should not have a default height value.");
          }

          string noConfigTypeName = Path.GetFileNameWithoutExtension(TilePorterTestConstants.SingleTilePngFileName);
          Tile.Type bgType = builtArchetypes.Last();
          if (bgType.Id.Name != noConfigTypeName) {
            return new(false, $"name: '{bgType.Id.Name}' does not equal expected: '{noConfigTypeName}'");
          }
          else if (bgType.PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{bgType.PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (bgType.ResourceKey != $"{"Test's Custom Assets"}::{noConfigTypeName}") {
            return new(false, $"resource key: '{bgType.ResourceKey}' does not equal expected: '{"Test's Custom Assets"}::{noConfigTypeName}'");
          }
          else if (bgType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false.");
          }
          else if (!bgType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (bgType.DefaultBackground.sprite.rect.width != 32 || bgType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (bgType.DefaultHeight.HasValue) {
            return new(false, $"The tiles produced should not have a default height value.");
          }

          /// passed all tests
          return new(true);
        }
      );
  }
}
