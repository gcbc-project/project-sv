using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildManager : MonoSingleton<BuildManager>
{

  static BuildManager _instance;
  public static BuildManager Get(bool allowCreation = true)
  {
    if (_instance == null && allowCreation)
    {
      new BuildManager();
    }

    return _instance;
  }
  public Tilemap MainTilemap;
  public Tilemap PreviewTilemap;
  public Tilemap ValidityTilemap;
  public Tilemap GroundTilemap;
  public TileBase ValidTile;
  public TileBase InvalidTile;
  public TileBase TransparentTile;

  public BuildSystem BuildSystem;
  private BuildingPreviewSystem _buildingPreviewSystem;

  [field: SerializeField]
  public NavMeshSurface NavMeshSurface;

  [SerializeField] BuildingSO _selectedBuilding;

  void Awake()
  {
    Init();
    BuildSystem = new BuildSystem(MainTilemap, GroundTilemap, TransparentTile);
    _buildingPreviewSystem = new BuildingPreviewSystem(PreviewTilemap, ValidityTilemap, MainTilemap, GroundTilemap, ValidTile, InvalidTile);
  }

  private void Init()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else if (_instance != this)
    {
      Destroy(gameObject);
    }
  }


  void Update()
  {
    if (_selectedBuilding != null)
    {
      _buildingPreviewSystem.PreviewUpdate();

      if (Mouse.current.leftButton.wasPressedThisFrame)
      {
        BuildSystem.PlaceBuilding(_buildingPreviewSystem);
      }
    }
  }

  public void BuildNavMesh()
  {
    NavMeshSurface.BuildNavMeshAsync();
  }

  public BuildSystem GetBuildSystem()
  {
    return BuildSystem;
  }

  public BuildingPreviewSystem GetBuildingPreviewSystem()
  {
    return _buildingPreviewSystem;
  }

  public void TestBuild()
  {
    BuildSystem.SelectBuilding(_selectedBuilding, _buildingPreviewSystem);
  }
}
