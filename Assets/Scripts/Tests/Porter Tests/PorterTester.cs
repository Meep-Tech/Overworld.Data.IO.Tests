using Meep.Tech.Collections.Generic;
using Meep.Tech.Data.IO;
using Meep.Tech.Data.IO.Tests;
using Newtonsoft.Json.Linq;
using Overworld.Data;
using Overworld.Data.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Overworld.Data.IO.Tests {

  public class PorterTester : MonoBehaviour {

    /// <summary>
    /// The expected default package name for testing.
    /// </summary>
    public const string DefaultPackageName
      = "Test's Custom Assets";

    static string ModFolderContainingFolder;
    static string DummyAssetsFolder;
    static bool EnableVerboseMode;

    #region Unity Inspector Set Values

    public bool VerboseModeEnabled
      = true;

    #endregion

    void Awake() {
      EnableVerboseMode = VerboseModeEnabled;
      ModFolderContainingFolder = Application.persistentDataPath;
      DummyAssetsFolder = Path.Combine(ModFolderContainingFolder, ModPorterContext.ModFolderName, "___test_dummy_assets");
    }

    // Start is called before the first frame update
    void Start() {

      User testUser = new("Test");

      Entity.Porter entityPorter
        = new(testUser);

      Entity.Icon.Porter entityIconPorter
        = new(testUser);

      Tile.Porter tilePorter
        = new(testUser);

      PorterTester<Tile.Type> tilePorterTester = new(
        ModFolderContainingFolder,
        tilePorter,
        new HashSet<string> {
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.SingleTilePngFileName),
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.SecondSingleTilePngFileName),
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.RedSingleTilePngFileName),
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.IgnoredSingleTilePngFileName),
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.SecondIgnoredSingleTilePngFileName),
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.SecondSingleTilePngFileName),
          Path.Combine(DummyAssetsFolder, TilePorterTestConstants.TileSheetPngFileName)
        }
      );

      var tilePorterTests = new[] {
        SingleTileFromPNGImageAsset_Loose.Test,
        SingleTileFromPNGImageAsset_Folder.Test,
        SingleTileFromPNGImageAsset_Loose_NoConfig.Test,
        SingleTileFromPNGImageAsset_Folder_NoConfig.Test,
        SingleTileFromPNGImageAsset_FolderAndLoose_NoConfig.Test,
        SingleTileFromPNGImageAsset_Loose_SpecialConfigValues.Test,
        SingleTilesFromMultiplePNGImageAsset_Loose.Test,
        SingleTilesFromMultiplePNGImageAsset_Loose_NoConfig.Test,
        SingleTilesFromMultiplePNGImageAsset_Loose_SpecialConfigValues.Test,
        SingleTilesFromMultiplePNGImageAsset_Loose_AssetFoundByConfigName.Test,
        SingleTilesFromMultiplePNGImageAsset_Loose_AssetFoundByConfigProperty.Test,
        MultipleTilesFromSheetPNGImageAsset_Loose_NoConfig.Test
      };

      Dictionary<string, PorterTester<Tile.Type>.Test.TestResult> results
        = tilePorterTester.Run(tilePorterTests);

      results.ForEach(r => Debug.Log($"{r.Key}:: {r.Value}"));
    }
    public static void PrintVerboseReport(IEnumerable<Tile.Type> builtArchetypes, IEnumerable<string> touchedFiles) {
      if (EnableVerboseMode) {
        touchedFiles.ForEach(tf => Debug.Log($"Touched File: {tf}"));
        Debug.Log($"Touched {touchedFiles.Count()} Files");
        builtArchetypes.ForEach(a => Debug.Log($"Built Archetype: {a}"));
        Debug.Log($"Built {builtArchetypes.Count()} Archetypes");
      }
    }
  }
}