using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int x, y;

    public bool isOccupied => occupants.Count > 0;
    [SerializeField] private List<GridCellOccupant> occupants = new List<GridCellOccupant>();

    public void Set(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public GridCell GetNeighbour(Vector2Int neighbouringDirection)
    {
        return GridGenerator._Instance.GridCells[x + neighbouringDirection.x, y + neighbouringDirection.y];
    }

    public bool HasObstructionOccupant()
    {
        foreach (GridCellOccupant occupant in occupants)
        {
            if (occupant.isObstruction)
                return true;
        }
        return false;
    }

    public bool HasInstantLossOccupant()
    {
        foreach (GridCellOccupant occupant in occupants)
        {
            if (occupant.IsInstantLoss)
                return true;
        }
        return false;
    }

    public bool IsOccupiedByObstruction()
    {
        return isOccupied && HasObstructionOccupant();
    }

    public void AddOccupant(GridCellOccupant occupant)
    {
        occupants.Add(occupant);
    }

    public void TriggerEvents()
    {
        for (int i = 0; i < occupants.Count;)
        {
            GridCellOccupant occupant = occupants[i];
            if (occupant.HasEvents)
                occupant.TriggerEvents();
            if (occupant) // Occupant has not destroyed itself
                i++;
        }
    }

    public bool RemoveOccupant(GridCellOccupant occupant)
    {
        bool containedOccupant = occupants.Contains(occupant);
        occupants.Remove(occupant);
        return containedOccupant;
    }

    public GridCellOccupant SpawnOccupant(GridCellOccupant obj)
    {
        GridCellOccupant occupant = Instantiate(obj, transform.position, Quaternion.identity);
        occupants.Add(occupant);
        occupant.SetToCell(this);
        return occupant;
    }
}
