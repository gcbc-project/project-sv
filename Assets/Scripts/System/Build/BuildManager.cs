using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
  public Tilemap MainTilemap;
  public Tilemap PreviewTilemap;
  public Tilemap ValidityTilemap;
  public TileBase ValidTile;
  public TileBase InvalidTile;
  public TileBase TransparentTile;

  private BuildSystem _buildSystem;
  private BuildingPreviewSystem _buildingPreviewSystem;

  [field: SerializeField]
  public NavMeshSurface NavMeshSurface;

  [SerializeField] BuildingSO _selectedBuilding;

  void Awake()
  {
    _buildSystem = new BuildSystem(MainTilemap, TransparentTile);
    _buildingPreviewSystem = new BuildingPreviewSystem(PreviewTilemap, ValidityTilemap, MainTilemap, ValidTile, InvalidTile);
  }

  void Update()
  {
    if (_selectedBuilding != null)
    {
      _buildingPreviewSystem.PreviewUpdate();

      if (Mouse.current.leftButton.wasPressedThisFrame)
      {
        _buildSystem.PlaceBuilding(_buildingPreviewSystem);
        NavMeshSurface.BuildNavMeshAsync();
      }
    }
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
    _buildSystem.SelectBuilding(_selectedBuilding, _buildingPreviewSystem);
  }
}
