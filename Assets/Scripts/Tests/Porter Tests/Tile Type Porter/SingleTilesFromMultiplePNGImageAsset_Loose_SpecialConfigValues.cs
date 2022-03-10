using Meep.Tech.Data.IO;
using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTilesFromMultiplePNGImageAsset_Loose_SpecialConfigValues {
    const string ConfigName = "Test-Single-Tile";
    const string ConfigPackageName = "Test-Package";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tiles From Multiple PNG Image Assets -Loose -SpecialConfigValues",
        new[] {
          $"./{TilePorterTestConstants.SecondSingleTilePngFileName}",
          $"./{TilePorterTestConstants.SingleTilePngFileName}"
        }.ToHashSet(),
        config: JObject.FromObject(new {
          Name = ConfigName,
          PackageName = ConfigPackageName,
          Height = 2
        }),
        options: new() {
          { PorterExtensions.PixelsPerTileImportOptionKey, 32 }
        },
        validateCreatedTypesAndProccessedFiles: (builtArchetypes, touchedFiles) => {
          PorterTester.PrintVerboseReport(builtArchetypes, touchedFiles);

          /// check the file we touched.
          if (touchedFiles.Count() != 3) {
            return new(false, $"Should've touched 3 files... touched {touchedFiles.Count()} instead!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SingleTilePngFileName}!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == TilePorterTestConstants.SecondSingleTilePngFileName)) {
            return new(false, $"Did not touch dummy version of file: {TilePorterTestConstants.SecondSingleTilePngFileName}!");
          }
          else if (!touchedFiles.Any(tf => Path.GetFileName(tf) == ArchetypePorter.DefaultConfigFileName)) {
            return new(false, $"Did not touch config file: {ArchetypePorter.DefaultConfigFileName}!");
          }

          /// check the archetype we made.
          if (builtArchetypes.Count() != 3) {
            return new(false, $"Expected 3 archetypes (config bg, config special, and no-config) but produced: {builtArchetypes.Count()} instead!");
          }

          Tile.Type bgTypeWithConfig = builtArchetypes.First(b => b.Id.Name == ConfigName + " (BG)");
          if (bgTypeWithConfig.Id.Name != ConfigName + " (BG)") {
            return new(false, $"name: '{bgTypeWithConfig.Id.Name}' does not equal expected: '{ConfigName} (BG)'");
          }
          else if (bgTypeWithConfig.PackageKey != ConfigPackageName) {
            return new(false, $"package name: '{bgTypeWithConfig.PackageKey}' does not equal expected: '{ConfigPackageName}'");
          }
          else if (bgTypeWithConfig.ResourceKey != $"{ConfigPackageName}::{ConfigName}") {
            return new(false, $"resource key: '{bgTypeWithConfig.ResourceKey}' does not equal expected: '{ConfigPackageName}::{ConfigName}'");
          }
          else if (bgTypeWithConfig.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false for the bg type.");
          }
          else if (!bgTypeWithConfig.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (bgTypeWithConfig.DefaultBackground.sprite.rect.width != 32 || bgTypeWithConfig.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (bgTypeWithConfig.DefaultHeight.HasValue) {
            return new(false, $"The tiles produced should not have a default height value.");
          }

          Tile.Type specialConfigType = builtArchetypes.First(b => b.Id.Name == ConfigName);
          if (specialConfigType.Id.Name != ConfigName) {
            return new(false, $"name: '{specialConfigType.Id.Name}' does not equal expected: '{ConfigName}'");
          }
          else if (specialConfigType.PackageKey != ConfigPackageName) {
            return new(false, $"package name: '{specialConfigType.PackageKey}' does not equal expected: '{ConfigPackageName}'");
          }
          else if (specialConfigType.ResourceKey != $"{ConfigPackageName}::{ConfigName}") {
            return new(false, $"resource key: '{specialConfigType.ResourceKey}' does not equal expected: '{ConfigPackageName}::{ConfigName}'");
          }
          else if (!specialConfigType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be true for the special type.");
          }
          else if (!specialConfigType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (specialConfigType.DefaultBackground.sprite.rect.width != 32 || specialConfigType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (!specialConfigType.DefaultHeight.HasValue || specialConfigType.DefaultHeight.Value != 2) {
            return new(false, $"The speical should have a height of 2. It is {specialConfigType.DefaultHeight?.ToString() ?? "null"} instead");
          }

          string noConfigTypeName = Path.GetFileNameWithoutExtension(TilePorterTestConstants.SecondSingleTilePngFileName);
          Tile.Type bgType = builtArchetypes.First(b => b.Id.Name == noConfigTypeName);
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
            return new(false, $"LinkArchetypeToTileDataOnSet should be false for the no config type.");
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
