
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class BuildingPreviewSystem
{
  private Tilemap _previewTilemap; // 건물 미리보기 타일맵
  private Tilemap _validityTilemap; // 설치 가능성 타일맵
  private Tilemap _mainTilemap;
  private TileBase _validTile;
  private TileBase _invalidTile;
  private BuildingSO _selectedBuilding;
  private Vector3Int _previousCellPosition;

  public BuildingPreviewSystem(Tilemap previewTilemap, Tilemap validityTilemap, Tilemap mainTilemap, TileBase validTile, TileBase invalidTile)
  {
    this._previewTilemap = previewTilemap;
    this._validityTilemap = validityTilemap;
    this._mainTilemap = mainTilemap;
    this._validTile = validTile;
    this._invalidTile = invalidTile;
    _previousCellPosition = Vector3Int.zero; // 초기값 설정
  }

  public void PreviewUpdate()
  {
    if (_selectedBuilding != null)
    {
      Vector3Int currentCellPosition = GetCellPositionFromMouse();

      // 현재 셀 좌표가 이전 셀 좌표와 다를 때만 업데이트
      if (currentCellPosition != _previousCellPosition)
      {
        SetPreviewTile(currentCellPosition);
        SetValidityTile(currentCellPosition);
        _previousCellPosition = currentCellPosition; // 좌표 갱신
      }
    }
  }

  public void SetSelectedBuilding(BuildingSO building)
  {
    _selectedBuilding = building;
  }

  public Vector3Int GetCellPositionFromMouse()
  {
    // 특정 타일맵을 통해 월드 좌표를 셀 좌표로 변환
    Vector3 mouseWorldPosition = Mouse.current.position.ReadValue();
    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseWorldPosition);
    mouseWorldPos.z = 0; // 2D에서 Z축 값을 0으로 고정
    return _previewTilemap.WorldToCell(mouseWorldPos);
  }

  private void SetPreviewTile(Vector3Int position)
  {
    _previewTilemap.ClearAllTiles();

    if (_selectedBuilding == null) return;

    // 건물의 높이에서 1을 뺀 값을 Y 오프셋으로 사용하여 왼쪽 상단에 배치
    Vector3Int offset = new Vector3Int(-_selectedBuilding.Size.x / 2, _selectedBuilding.Size.y / 2 - 1, 0);
    // Vector3Int offset = new Vector3Int(0, _selectedBuilding.Size.y / 2 - 1, 0);
    Vector3Int topLeftPosition = position + offset;

    // 항상 표시되도록 하기 위해 canPlace 검사 생략
    TileBase previewTile = _selectedBuilding.Tile;  // 항상 유효한 타일로 설정

    _previewTilemap.SetTile(topLeftPosition, previewTile);
  }

  private void SetValidityTile(Vector3Int position)
  {
    _validityTilemap.ClearAllTiles();

    if (_selectedBuilding == null) return;

    // 건물의 좌측 상단 위치 계산 (미리보기와 동일한 방식)
    Vector3Int offset = new Vector3Int(-_selectedBuilding.Size.x / 2, _selectedBuilding.Size.y / 2 - 1, 0);
    Vector3Int topLeftPosition = position + offset;

    Tilemap tilemap = _mainTilemap;

    // 각 셀별로 설치 가능 여부를 판단하여 타일 설정
    for (int x = 0; x < _selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < _selectedBuilding.Size.y; y++)
      {
        Vector3Int cell = topLeftPosition + new Vector3Int(x, -y, 0);

        // 해당 셀에 타일이 있는지 확인
        bool cellCanPlace = tilemap.GetTile(cell) == null;

        // 설치 가능 여부에 따라 타일 선택
        TileBase tileToSet = cellCanPlace ? _validTile : _invalidTile;

        // 타일맵에 타일 설정
        _validityTilemap.SetTile(cell, tileToSet);
      }
    }
  }

  private bool CanPlaceBuilding(Vector3Int topLeftPosition)
  {
    Tilemap tilemap = _mainTilemap;
    for (int x = 0; x < _selectedBuilding.Size.x; x++)
    {
      for (int y = 0; y < _selectedBuilding.Size.y; y++)
      {
        // Y축 방향 조정 (-y)를 통해 아래 방향으로 검사
        Vector3Int cell = topLeftPosition + new Vector3Int(x, -y, 0);
        if (tilemap.GetTile(cell) != null)
        {
          return false; // 해당 위치에 타일이 이미 있음
        }
      }
    }
    return true; // 모든 위치가 비어 있음
  }


}
