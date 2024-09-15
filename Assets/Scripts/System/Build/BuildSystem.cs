using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem
{
  private Tilemap _tilemap;
  private Tilemap _groundTilemap;
  private TileBase _transparentTile;
  public BuildingSO SelectedBuilding;

  public BuildSystem(Tilemap tilemap, Tilemap groundTilemap, TileBase transparentTile)
  {
    this._tilemap = tilemap;
    this._groundTilemap = groundTilemap;
    this._transparentTile = transparentTile;
  }

  public void SelectBuilding(BuildingSO building, BuildingPreviewSystem previewSystem)
  {
    SelectedBuilding = building;
    previewSystem.SetSelectedBuilding(SelectedBuilding);
  }

  public void PlaceBuilding(BuildingPreviewSystem previewSystem)
  {
    if (SelectedBuilding == null) return;

    Vector3Int cellPosition = previewSystem.GetCellPositionFromMouse();

    if (CanPlaceBuilding(cellPosition))
    {
      BuildingData.Create(SelectedBuilding, new Vector2(cellPosition.x, cellPosition.y));
    }
    else
    {
      Debug.Log("Invalid placement");
    }
  }

  private bool CanPlaceBuilding(Vector3Int position)
  {
    // 건물의 크기를 고려하여 오프셋 계산
    Vector3Int offset = new Vector3Int(-SelectedBuilding.Size.x / 2, SelectedBuilding.Size.y / 2 - 1, 0);
    Vector3Int topLeftPosition = position + offset;

    for (int x = 0; x < SelectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < SelectedBuilding.Size.y; y++)
      {
        // Y축 방향 조정 (-y)를 통해 아래 방향으로 검사
        Vector3Int cell = topLeftPosition + new Vector3Int(x, -y, 0);
        if (_tilemap.GetTile(cell) != null || _groundTilemap.GetTile(cell) == null)
        {
          return false; // 해당 위치에 타일이 이미 있음
        }
      }
    }
    return true; // 모든 위치가 비어 있음
  }


  public GameObject PlaceBuildingAt(Vector3Int position)
  {
    // Calculate top-left position with offset
    Vector3Int offset = new Vector3Int(-SelectedBuilding.Size.x / 2, SelectedBuilding.Size.y / 2 - 1, 0);
    Vector3Int topLeftPosition = position + offset;

    // Instantiate the building prefab at the correct world position
    Vector3 worldPosition = _tilemap.CellToWorld(topLeftPosition) + _tilemap.tileAnchor;
    GameObject instantiatedBuilding = GameObject.Instantiate(SelectedBuilding.Prefab, worldPosition, Quaternion.identity);
    instantiatedBuilding.transform.parent = _tilemap.transform; // Optional: Set parent to the Tilemap

    // Optionally, set a tile without a GameObject at the top-left position
    Tile tile = ScriptableObject.CreateInstance<Tile>();
    _tilemap.SetTile(topLeftPosition, tile);

    // Set transparent tiles for the rest of the area
    for (int x = 0; x < SelectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < SelectedBuilding.Size.y; y++)
      {
        Vector3Int cellPosition = topLeftPosition + new Vector3Int(x, -y, 0);

        if (cellPosition == topLeftPosition)
          continue;

        _tilemap.SetTile(cellPosition, _transparentTile);
      }
    }
    return instantiatedBuilding;
  }
}
