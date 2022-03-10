using Meep.Tech.Data.IO;
using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTilesFromMultiplePNGImageAsset_Loose_NoConfig {
    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tiles From Multiple PNG Image Assets -Loose -NoConfig",
        new[] {
          $"./{TilePorterTestConstants.SecondSingleTilePngFileName}",
          $"./{TilePorterTestConstants.SingleTilePngFileName}"
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
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SecondSingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SecondSingleTilePngFileName}!");
          }
          else if (touchedFiles.Count() != 2) {
            return new(false, $"Should've touched 2 files... touched {touchedFiles.Count()} instead!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 2) {
            return new(false, $"Expected 2 archetypes (config and no-config) but produced: {builtArchetypes.Count()} instead!");
          }

          string firstTypeName = Path.GetFileNameWithoutExtension(TilePorterTestConstants.SingleTilePngFileName);
          Tile.Type firstType = builtArchetypes.First();
          if (firstType.Id.Name != firstTypeName) {
            return new(false, $"name: '{firstType.Id.Name}' does not equal expected: '{firstTypeName}'");
          }
          else if (firstType.PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{firstType.PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (firstType.ResourceKey != $"{"Test's Custom Assets"}::{firstTypeName}") {
            return new(false, $"resource key: '{firstType.ResourceKey}' does not equal expected: '{"Test's Custom Assets"}::{firstTypeName}'");
          }
          else if (firstType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false.");
          }
          else if (!firstType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (firstType.DefaultBackground.sprite.rect.width != 32 || firstType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (firstType.DefaultHeight.HasValue) {
            return new(false, $"The tiles produced should not have a default height value.");
          }

          string secondTypeName = Path.GetFileNameWithoutExtension(TilePorterTestConstants.SecondSingleTilePngFileName);
          Tile.Type bgType = builtArchetypes.Last();
          if (bgType.Id.Name != secondTypeName) {
            return new(false, $"name: '{bgType.Id.Name}' does not equal expected: '{secondTypeName}'");
          }
          else if (bgType.PackageKey != "Test's Custom Assets") {
            return new(false, $"package name: '{bgType.PackageKey}' does not equal expected: '{"Test's Custom Assets"}'");
          }
          else if (bgType.ResourceKey != $"{"Test's Custom Assets"}::{secondTypeName}") {
            return new(false, $"resource key: '{bgType.ResourceKey}' does not equal expected: '{"Test's Custom Assets"}::{secondTypeName}'");
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
