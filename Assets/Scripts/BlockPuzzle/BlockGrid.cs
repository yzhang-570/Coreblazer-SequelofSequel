using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockGrid : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile bgTile;

    int xoffset, yoffset;
    int rows, cols;

    int currentDay;

    Dictionary<int, Vector2> solution;

    public void initGrid(Dictionary<int, Vector2> solution, int rows, int cols, int xoffset, int yoffset, int day)
    {
        this.xoffset = xoffset;
        this.yoffset = yoffset;
        this.rows = rows;
        this.cols = cols;
        currentDay = day;

        // map Block ID -> top-left corner's relative offset from the block w/ lowest ID
        this.solution = solution;

        // draw da tilemap
        tilemap.ClearAllTiles();
        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < cols; ++j) {
                Vector3Int cell = arrayToCell(new Vector3Int(i, j, 0));
                tilemap.SetTile(cell, bgTile);
            }
        }
    }

    public Vector3Int snapToGrid(Vector3 world) {
        Vector3 og = world;
        return new Vector3Int(Mathf.RoundToInt(og.x), Mathf.RoundToInt(og.y) - 1);
    }

    public Vector3Int worldToArray(Vector3 world) {
        Vector3Int conv = snapToGrid(world);
        return new Vector3Int(yoffset - conv.y, conv.x - xoffset);
    }

    public Vector3Int arrayToCell(Vector3Int grid) {
        return new Vector3Int(xoffset + grid.y, yoffset - grid.x);
    }

    public Vector3 arrayToWorld(int row, int col) {
        Vector3 og = tilemap.CellToWorld(arrayToCell(new Vector3Int(row, col)));
        return new Vector3(og.x - (currentDay == 1 ? 0.5f : 0), og.y);
    }

    public bool checkBlockPosition(Vector3 position, BlockType blockType) {
        Vector3Int off = worldToArray(position);
        return off.x >= 0 && off.y >= 0 && off.y + blockType.width <= cols && off.x + blockType.height <= rows;
    }

    public void addBlock(int id, Vector3 position, string blockType)
    {
        Vector3Int off = worldToArray(position);
        // TODO add to list
    }

    public void removeBlock(int id)
    {
        // TODO remove from list
    }

    public void updateBlock(int id, Vector3 position, string blockType) {
        removeBlock(id);
        addBlock(id, position, blockType);
    }

    public int getFirstMismatch() {
        // TODO iterate through blocks and find mismatch
        return -1;
    }
}