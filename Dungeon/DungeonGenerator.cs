using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);

        foreach (Room room in RoomController.instance.loadedRooms)
            room.RemoveUnconnectedDoors();
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        Vector2Int start = new Vector2Int(0, 0);
        RoomController.instance.LoadRoom(ObtenerPuertasAdyacentes(start), 0, 0);
        foreach (Vector2Int roomLocation in rooms)
        {
            string nombreDeHabitacion = ObtenerPuertasAdyacentes(roomLocation);
            Debug.Log(nombreDeHabitacion);

            RoomController.instance.LoadRoom(nombreDeHabitacion, roomLocation.x, roomLocation.y);
            
        }
    }

    public string ObtenerPuertasAdyacentes(Vector2Int roomLocation)
    {
        List<Direction> puertasAdyacentes = new List<Direction>();

        if (dungeonRooms.Contains(new Vector2Int(roomLocation.x - 1, roomLocation.y)))
            puertasAdyacentes.Add(Direction.left);

        if (dungeonRooms.Contains(new Vector2Int(roomLocation.x, roomLocation.y + 1)))
            puertasAdyacentes.Add(Direction.up);

        if (dungeonRooms.Contains(new Vector2Int(roomLocation.x + 1, roomLocation.y)))
            puertasAdyacentes.Add(Direction.right);

        if (dungeonRooms.Contains(new Vector2Int(roomLocation.x, roomLocation.y - 1)))
            puertasAdyacentes.Add(Direction.down);

        // Imprime el contenido de la lista para verificar
        Debug.Log("Puertas adyacentes para la habitación en " + roomLocation + ": " + string.Join(", ", puertasAdyacentes));

        return AsignarNombreHabitacion(puertasAdyacentes);
    }

    private string AsignarNombreHabitacion(List<Direction> direcciones)
    {
        // Mapeo de combinaciones de direcciones a nombres de habitaciones
        Dictionary<List<Direction>, string> combinacionDeDireccionesAMapa = new Dictionary<List<Direction>, string>()
    {
        // 4 Puertas
        { new List<Direction> { Direction.left, Direction.up, Direction.right, Direction.down }, "4Doors" },
        // 3 Puertas
        { new List<Direction> { Direction.left, Direction.up, Direction.right }, "LeftTopRight1" },
        { new List<Direction> { Direction.left, Direction.right, Direction.down }, "LeftRightBottom1" },
        { new List<Direction> { Direction.left, Direction.up, Direction.down }, "LeftTopBottom1" },
        { new List<Direction> { Direction.up, Direction.right, Direction.down }, "TopRightBottom1" },
        // 2 Puertas
        { new List<Direction> { Direction.left, Direction.up }, "LeftTop1" },
        { new List<Direction> { Direction.up, Direction.right }, "TopRight1" },
        { new List<Direction> { Direction.right, Direction.down }, "RightBottom1" },
        { new List<Direction> { Direction.left, Direction.down }, "LeftBottom1" },
        { new List<Direction> { Direction.left, Direction.right }, "LeftRight1" },
        { new List<Direction> { Direction.up, Direction.down }, "TopBottom1" },
        // 1 Puerta
        { new List<Direction> { Direction.left }, "Left1" },
        { new List<Direction> { Direction.right }, "Right1" },
        { new List<Direction> { Direction.up }, "Top1" },
        { new List<Direction> { Direction.down }, "Bottom1" }
    };

        // Verifica en el mapeo qué nombre corresponde a estas direcciones
        foreach (var kvp in combinacionDeDireccionesAMapa)
        {
            if (ContieneDirecciones(direcciones, kvp.Key))
            {
                return kvp.Value;
            }
        }

        // Si no hay una asignación específica, devuelve un nombre por defecto o maneja de otra manera
        return "Empty";
    }

    private bool ContieneDirecciones(List<Direction> direcciones, List<Direction> direccionesRequeridas)
    {
        // Verifica si la lista de direcciones contiene todas las direcciones requeridas
        foreach (var direccion in direccionesRequeridas)
        {
            if (!direcciones.Contains(direccion))
            {
                return false;
            }
        }
        return true;
    }
}
