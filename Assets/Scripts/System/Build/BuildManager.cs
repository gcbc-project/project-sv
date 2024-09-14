using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
  public Tilemap MainTilemap;
  public Tilemap PreviewTilemap;
  public Tilemap ValidityTilemap;
  public TileBase ValidTile;
  public TileBase InvalidTile;

  private BuildSystem _buildSystem;
  private BuildingPreviewSystem _buildingPreviewSystem;

  [SerializeField] BuildingSO _testBuildingSO;

  void Awake()
  {
    _buildSystem = new BuildSystem(MainTilemap);
    _buildingPreviewSystem = new BuildingPreviewSystem(PreviewTilemap, ValidityTilemap, ValidTile, InvalidTile);
  }

  void Update()
  {
    _buildingPreviewSystem.PreviewUpdate();
  }

  public BuildSystem GetBuildSystem()
  {
    return _buildSystem;
  }

  public BuildingPreviewSystem GetBuildingPreviewSystem()
  {
    return _buildingPreviewSystem;
  }

  public void TestBuild()
  {
    _buildSystem.SelectBuilding(_testBuildingSO, _buildingPreviewSystem);
  }
}
