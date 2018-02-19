using UnityEngine;
using System.Collections;




#if WORLDMAP
namespace GameUtil
{

    public class WorldMapViewPortSizeScaner
    {
        private Vector3 WorldMapLeftTopPoint;

        private GameUtil.Utility.WorldMapUtility.SpaceCalculator Calculator = null;

        public WorldMapViewPortSizeScaner(Vector3 point, GameUtil.Utility.WorldMapUtility.SpaceCalculator calculator)
        {
            WorldMapLeftTopPoint = point;
            Calculator = calculator;
        }

        public bool GetViewPortSizeInWorldMap(Camera camera, ref Vector4 v4)
        {
            bool changed = false;
            if(camera == null) return changed;

            Vector3 start = camera.ScreenToWorldPoint(new Vector3(0, 0, WorldMapLeftTopPoint.y));
            Vector3 end = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth,camera.pixelHeight, WorldMapLeftTopPoint.z ));
            Vector3 size = end - start;
            if (v4.x != start.x || v4.y != start.z || v4.w != size.x || v4.z != size.z)
                changed = true;
            v4.x = start.x;
            v4.y = start.z;
            v4.w = size.x;
            v4.z = size.z;


            return changed;
        }
        public void CalculateViewPortSizeInPosition(Vector4 v4, ref Vector4 pv4)
        {
            pv4.x = Calculator.CalculateX(v4.x, v4.y + v4.z);
            pv4.y = Calculator.CalculateY(v4.x + v4.w, v4.y + v4.z);
            pv4.w = Calculator.CalculateX(v4.x + v4.w, v4.y);
            pv4.z = Calculator.CalculateY(v4.x, v4.y);
        }

        public bool GetViewportCenterInWorldMap(Camera camera,ref Vector3 v3,ref Vector4 v4)
        {
            bool changed = false;
            if (camera == null) return changed;
            changed = GetViewPortSizeInWorldMap(camera, ref v4) || changed;
            if (v3.x != (v4.x + v4.w / 2) || (v3.z != v4.y + v4.z / 2))
                changed = true;

            v3.x = v4.x + v4.w / 2;
            v3.y = WorldMapLeftTopPoint.y;
            v3.z = v4.y + v4.z / 2;
            return changed;
        }

        public bool GetViewportCenterTileCoords(Camera camera, ref int posX, ref int posY, ref Vector3 v3, ref Vector4 v4)
        {
            bool changed = false;
            changed = GetViewportCenterInWorldMap(camera, ref v3, ref v4) || changed;
            changed = Calculator.CalculatePosition(v3.x, v3.z, ref posX, ref posY) || changed;
            return changed;
        }
        



    }
}
#endif