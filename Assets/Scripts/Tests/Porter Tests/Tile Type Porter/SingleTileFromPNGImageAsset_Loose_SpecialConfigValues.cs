using Meep.Tech.Data.IO;
using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Overworld.Data.IO.Tests {
  public static class SingleTileFromPNGImageAsset_Loose_SpecialConfigValues {
    const string ConfigName = "Test-Single-Tile";
    const string ConfigPackageName = "Test-Package";

    public static readonly PorterTester<Tile.Type>.ImportWithConfigAndDummyFilesTest Test
      = new(
        "Single Tile From PNG Image Asset -Loose -SpecialConfigValues",
        new[] {
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
          if (builtArchetypes.Count() != 2) {
            return new(false, $"Expected 2 archetypes (bg and special) but produced: {builtArchetypes.Count()} instead!");
          }

          Tile.Type specialType = builtArchetypes.First(b => !b.Id.Name.Contains("BG"));
          if (specialType.Id.Name != ConfigName) {
            return new(false, $"name: '{specialType.Id.Name}' does not equal expected: '{ConfigName}'");
          }
          else if (specialType.PackageKey != ConfigPackageName) {
            return new(false, $"package name: '{specialType.PackageKey}' does not equal expected: '{ConfigPackageName}'");
          }
          else if (specialType.ResourceKey != $"{ConfigPackageName}::{ConfigName}") {
            return new(false, $"resource key: '{specialType.ResourceKey}' does not equal expected: '{ConfigPackageName}::{ConfigName}'");
          }
          else if (!specialType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be true for the special type.");
          }
          else if (!specialType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (specialType.DefaultBackground.sprite.rect.width != 32 || specialType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (!specialType.DefaultHeight.HasValue || specialType.DefaultHeight.Value != 2) {
            return new(false, $"The special tile produced should have a default height value of 2. {specialType.DefaultHeight?.ToString() ?? "null"} found instead.");
          }

          Tile.Type bgType = builtArchetypes.First(b => b.Id.Name.Contains("BG"));
          if (bgType.Id.Name != ConfigName + " (BG)") {
            return new(false, $"name: '{bgType.Id.Name}' does not equal expected: '{ConfigName} (BG)'");
          }
          else if (bgType.PackageKey != ConfigPackageName) {
            return new(false, $"package name: '{bgType.PackageKey}' does not equal expected: '{ConfigPackageName}'");
          }
          else if (bgType.ResourceKey != $"{ConfigPackageName}::{ConfigName}") {
            return new(false, $"resource key: '{bgType.ResourceKey}' does not equal expected: '{ConfigPackageName}::{ConfigName}'");
          }
          else if (bgType.LinkArchetypeToTileDataOnSet) {
            return new(false, $"LinkArchetypeToTileDataOnSet should be false for the BG type.");
          }
          else if (!bgType.UseDefaultBackgroundAsInWorldTileImage) {
            return new(false, $"UseDefaultBackgroundAsInWorldTileImage should be true on all archetypes produced.");
          }
          else if (bgType.DefaultBackground.sprite.rect.width != 32 || bgType.DefaultBackground.sprite.rect.height != 32) {
            return new(false, $"The tile produced by this sprite sheet should be 32x32 pixels.");
          }
          else if (bgType.DefaultHeight.HasValue) {
            return new(false, $"The BG tile produced should not have a default height value.");
          }

          /// passed all tests
          return new(true);
        }
      );
  }
}
