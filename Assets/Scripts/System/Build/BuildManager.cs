using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
  public Tilemap MainTilemap;
  public Tilemap PreviewTilemap;
  public TileBase ValidTile;
  public TileBase InvalidTile;

  private BuildSystem _buildSystem;
  private BuildingPreviewSystem _buildingPreviewSystem;

  void Awake()
  {
    _buildSystem = new BuildSystem(MainTilemap);
    _buildingPreviewSystem = new BuildingPreviewSystem(PreviewTilemap, ValidTile, InvalidTile);
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
    Building building = new Building()
    {
      Name = "테스트",
      Size = new Vector2Int(2, 2),
    };
  }
}
