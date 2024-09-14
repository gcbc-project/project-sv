using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem
{
  private Tilemap _tilemap;
  private TileBase _transparentTile;
  private BuildingSO _selectedBuilding;

  public BuildSystem(Tilemap tilemap, TileBase transparentTile)
  {
    this._tilemap = tilemap;
    this._transparentTile = transparentTile;
  }

  public void SelectBuilding(BuildingSO building, BuildingPreviewSystem previewSystem)
  {
    _selectedBuilding = building;
    previewSystem.SetSelectedBuilding(_selectedBuilding);
  }

  public void PlaceBuilding(BuildingPreviewSystem previewSystem)
  {
    if (_selectedBuilding == null) return;

    Vector3Int cellPosition = previewSystem.GetCellPositionFromMouse();

    if (CanPlaceBuilding(cellPosition))
    {
      PlaceBuildingAt(cellPosition);
    }
    else
    {
      Debug.Log("Invalid placement");
    }
  }

  private bool CanPlaceBuilding(Vector3Int position)
  {
    // 건물의 크기를 고려하여 오프셋 계산
    Vector3Int offset = new Vector3Int(-_selectedBuilding.Size.x / 2, _selectedBuilding.Size.y / 2 - 1, 0);
    Vector3Int topLeftPosition = position + offset;

    for (int x = 0; x < _selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < _selectedBuilding.Size.y; y++)
      {
        // Y축 방향 조정 (-y)를 통해 아래 방향으로 검사
        Vector3Int cell = topLeftPosition + new Vector3Int(x, -y, 0);
        if (_tilemap.GetTile(cell) != null)
        {
          return false; // 해당 위치에 타일이 이미 있음
        }
      }
    }
    return true; // 모든 위치가 비어 있음
  }


  private void PlaceBuildingAt(Vector3Int position)
  {
    // 오프셋 적용하여 좌측 상단 위치 계산
    Vector3Int offset = new Vector3Int(-_selectedBuilding.Size.x / 2, _selectedBuilding.Size.y / 2 - 1, 0);
    Vector3Int topLeftPosition = position + offset;

    // 좌측 상단 타일에만 건물 타일 배치
    Tile tempTile = ScriptableObject.CreateInstance<Tile>();
    tempTile.gameObject = _selectedBuilding.Prefab;
    _tilemap.SetTile(topLeftPosition, tempTile);

    // 나머지 영역은 투명 타일로 설정하여 차있음을 표시
    for (int x = 0; x < _selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < _selectedBuilding.Size.y; y++)
      {
        Vector3Int cellPosition = topLeftPosition + new Vector3Int(x, -y, 0);

        // 좌측 상단 타일은 이미 설정했으므로 건너뜁니다
        if (cellPosition == topLeftPosition)
          continue;

        // 투명 타일로 설정 (null 또는 투명한 타일 리소스)
        _tilemap.SetTile(cellPosition, _transparentTile);
      }
    }
  }

}
