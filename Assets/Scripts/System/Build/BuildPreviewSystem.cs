
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class BuildingPreviewSystem
{
  private Tilemap _previewTilemap;
  private TileBase _validTile;
  private TileBase _invalidTile;
  private Building _selectedBuilding;

  public BuildingPreviewSystem(Tilemap previewTilemap, TileBase validTile, TileBase invalidTile)
  {
    this._previewTilemap = previewTilemap;
    this._validTile = validTile;
    this._invalidTile = invalidTile;
  }

  public void UpdatePreview(Building building)
  {
    _selectedBuilding = building;
    Vector3Int mouseCellPosition = GetCellPositionFromMouse();
    UpdatePreviewGrid(mouseCellPosition);
  }

  public Vector3Int GetCellPositionFromMouse()
  {
    Vector3 mouseWorldPosition = Mouse.current.position.ReadValue();
    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseWorldPosition);
    return Camera.main.GetComponentInChildren<Tilemap>().WorldToCell(mouseWorldPos);
  }

  private void UpdatePreviewGrid(Vector3Int position)
  {
    _previewTilemap.ClearAllTiles();

    if (_selectedBuilding == null) return;

    bool canPlace = CanPlaceBuilding(position);
    TileBase previewTile = canPlace ? _validTile : _invalidTile;

    for (int x = 0; x < _selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < _selectedBuilding.Size.y; y++)
      {
        Vector3Int cell = position + new Vector3Int(x, y, 0);
        _previewTilemap.SetTile(cell, previewTile);
      }
    }
  }

  private bool CanPlaceBuilding(Vector3Int position)
  {
    Tilemap tilemap = Camera.main.GetComponentInChildren<Tilemap>();
    for (int x = 0; x < _selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < _selectedBuilding.Size.y; y++)
      {
        Vector3Int cell = position + new Vector3Int(x, y, 0);
        if (tilemap.GetTile(cell) != null)
        {
          return false;
        }
      }
    }
    return true;
  }
}
