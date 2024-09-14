using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem
{
  private Tilemap tilemap;
  private BuildingSO selectedBuilding;

  public BuildSystem(Tilemap tilemap)
  {
    this.tilemap = tilemap;
  }

  public void SelectBuilding(BuildingSO building, BuildingPreviewSystem previewSystem)
  {
    selectedBuilding = building;
    previewSystem.SetSelectedBuilding(selectedBuilding);
  }

  public void PlaceBuilding(BuildingPreviewSystem previewSystem)
  {
    if (selectedBuilding == null) return;

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
    for (int x = 0; x < selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < selectedBuilding.Size.y; y++)
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

  private void PlaceBuildingAt(Vector3Int position)
  {
    for (int x = 0; x < selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < selectedBuilding.Size.y; y++)
      {
        Vector3Int cell = position + new Vector3Int(x, y, 0);
        tilemap.SetTile(cell, selectedBuilding.Tile);
      }
    }
  }
}
