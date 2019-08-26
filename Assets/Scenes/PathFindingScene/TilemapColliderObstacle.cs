using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scenes.PathFindingScene
{
    public class TilemapColliderObstacle : MovementObstacleBase
    {
        public Tilemap tilemap;

        public Vector2Int offset;

        private void Start()
        {
            tilemap.GetComponentInChildren<TilemapRenderer>().enabled = false;
        }

        public override bool IsBlocked(Vector2Int position)
        {
            position += offset;
            var v3 = new Vector3Int(position.x, position.y, 0);
            var tile = tilemap.GetTile(v3);
            return tile != null && tile.name.Equals("blocked");
        }
    }
}