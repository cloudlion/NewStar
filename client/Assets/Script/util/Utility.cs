using UnityEngine;
using System.Collections;
using System;
#if MVC
using MVC;
#endif
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace GameUtil
{
    public static class Utility
    {
#if WORLDMAP
        public static class WorldMapUtility
        {
            public static void CalculateGameObjectStrings(WorldMap.WorldMapActiveObjectTileType tileType, int tileLevel, ref string typeName, ref string name, ref string whole, ref string typeWhole)
            {
                typeName = "";
                name = "";
                whole = "";
                typeWhole = "";

                WorldMap.WorldMapActiveObjectTileType t = tileType;
                int level = tileLevel;
                int type = (int)t;
                typeName = type.ToString();
                name = typeName + "_" + level.ToString();
                whole = name + "a" + WorldMap.WorldMapProxy.CurrentMapLevel.ToString();
                typeWhole = typeName + "a" + WorldMap.WorldMapProxy.CurrentMapLevel.ToString();
            }


            public static void CalculateGameObjectStrings<TILE>(TILE tile, ref string typeName, ref string name, ref string whole, ref string typeWhole) where TILE : WorldMap.WorldMapActiveObjectTile
            {
                typeName = "";
                name = "";
                whole = "";
                typeWhole = "";

                WorldMap.WorldMapActiveObjectTileType t = tile.TileType;
                int level = tile.Level;
                int type = (int)t;
                if (tile.TileType == WorldMap.WorldMapActiveObjectTileType.Player)
                    level = tile.CityLevel;
                typeName = type.ToString();
                name = typeName + "_" + level.ToString();
                whole = name + "a" + WorldMap.WorldMapProxy.CurrentMapLevel.ToString();
                typeWhole = typeName + "a" + WorldMap.WorldMapProxy.CurrentMapLevel.ToString();
            }

            public static Sprite CalculateGameObjectSprite(WorldMap.ActiveObjectRoot root, string typeName, string name, string whole, string typeWhole)
            {
                if (root.Sprites.ContainsKey(whole))
                    return root.Sprites[whole];
                if (root.Sprites.ContainsKey(typeWhole))
                    return root.Sprites[typeWhole];
                else if (root.Sprites.ContainsKey(name))
                    return root.Sprites[name];
                else if (root.Sprites.ContainsKey(typeName))
                    return root.Sprites[typeName];
                return null;
            }
            public static string CalculateGameObjectName(WorldMap.ActiveObjectRoot root, string typeName, string name, string whole, string typeWhole)
            {
                if (root.Sprites.ContainsKey(whole) && root.Sprites[whole] != null)
                    return root.Sprites[whole].name;
                if (root.Sprites.ContainsKey(typeWhole) && root.Sprites[typeWhole] != null)
                    return root.Sprites[typeWhole].name;
                else if (root.Sprites.ContainsKey(name) && root.Sprites[name] != null)
                    return root.Sprites[name].name;
                else if (root.Sprites.ContainsKey(typeName) && root.Sprites[typeName] != null)
                    return root.Sprites[typeName].name;
                return "";
            }


            public static Sprite CalculateGameObjectSprite<TILE>(TILE tile, WorldMap.ActiveObjectRoot root) where TILE : WorldMap.WorldMapActiveObjectTile
            {
                string typeName = "";
                string typeNamename = "";
                string typeNamewhole = "";
                string typeNametypeWhole = "";
                GameUtil.Utility.WorldMapUtility.CalculateGameObjectStrings<TILE>(tile, ref typeName, ref typeNamename, ref typeNamewhole, ref typeNametypeWhole);
                return GameUtil.Utility.WorldMapUtility.CalculateGameObjectSprite(root, typeName, typeNamename, typeNamewhole, typeNametypeWhole);
            }
            public static string CalculateGameObjectName<TILE>(TILE tile, WorldMap.ActiveObjectRoot root) where TILE : WorldMap.WorldMapActiveObjectTile
            {
                string typeName = "";
                string typeNamename = "";
                string typeNamewhole = "";
                string typeNametypeWhole = "";
                GameUtil.Utility.WorldMapUtility.CalculateGameObjectStrings<TILE>(tile, ref typeName, ref typeNamename, ref typeNamewhole, ref typeNametypeWhole);
                return GameUtil.Utility.WorldMapUtility.CalculateGameObjectName(root, typeName, typeNamename, typeNamewhole, typeNametypeWhole);
            }
            public static string CalculateGameObjectName(WorldMap.WorldMapActiveObjectTileType tileType, int tileLevel, WorldMap.ActiveObjectRoot root)
            {
                string typeName = "";
                string typeNamename = "";
                string typeNamewhole = "";
                string typeNametypeWhole = "";
                GameUtil.Utility.WorldMapUtility.CalculateGameObjectStrings(tileType, tileLevel, ref typeName, ref typeNamename, ref typeNamewhole, ref typeNametypeWhole);
                return GameUtil.Utility.WorldMapUtility.CalculateGameObjectName(root, typeName, typeNamename, typeNamewhole, typeNametypeWhole);
            }

            public static class FocusWhenOpeningUI
            {
                public static Bounds GetUIBounds(GameObject go)
                {
                    return new Bounds();
                }
                public static Vector4 GetScreenBounds(Bounds bounds)
                {
                    return new Vector4();
                }
                public static Vector2 GetScreenRalativeCenterPoint()
                {
                    return new Vector2();
                }

                public static WorldMap.WorldMapPosition  CalculateWorldMapMoveTargetCenter()
                {
                    return new WorldMap.WorldMapPosition();
                }

                //public static WorldMap.WorldMapPosition FocusToLeftScreenCenterWhenOpeningUI(GameObject go)
                //{
                //    return new WorldMap.WorldMapPosition();
                //}

            }

#region WorldMap Tile Encode
            public static string DecodeWorldMapTileType(byte a)
            {
                string s = Convert.ToString(a, 2);
                while (true)
                {
                    if (s.Length >= 8) break;
                    s = "0" + s;
                }
                return s;
            }

            public static byte EncodeWorldMapTileType(string s)
            {
                return Convert.ToByte(s, 2);
            }
#endregion
            public static void Parse(uint info, out uint left, out uint top, out uint type, out uint level)
            {
                left = info >> 23;
                top = info << 9 >> 23;
                type = info << 18 >> 25;
                level = info << 25 >> 25;
            }

#region world to coordinate
            public static int CalculateBlockID(WorldMap.WorldMapPosition pos,int width)
            {
                int x = pos.RalativeX / Constant.WorldMapActiveObject.ServerBlockWidth;
                int y = pos.RalativeY / Constant.WorldMapActiveObject.ServerBlockHeight;

                return y * width / Constant.WorldMapActiveObject.ServerBlockWidth + x + 1;
            }

            public static void CalculateTouchTileCoordinate(Camera camera, ref WorldMap.WorldMapPosition position)
            {
                Vector3 v3 = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Constant.WorldMapTerrain.WorldMapLeftTopPoint.y));
                GameUtil.Utility.WorldMapUtility.CalculatePosition(v3.x, v3.z, ref position);
            }
            public static void CalculateMiniMapTouchTileCoordinate(Camera camera, ref WorldMap.WorldMapPosition position)
            {
                Vector3 v3 = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Constant.WorldMapTerrain.WorldMapLeftTopPoint.y));
                GameUtil.Utility.WorldMapUtility.CalculateMiniMapPositionFromSpaceToWorldMapPosition(v3.x, v3.z, ref position);
            }
            public static void CalculateMiniMapTouchTileCoordinate(Camera camera, ref MiniMap.MiniMapPosition position)
            {
                Vector3 v3 = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Constant.WorldMapTerrain.WorldMapLeftTopPoint.y));
                GameUtil.Utility.WorldMapUtility.CalculateMiniMapPosition(v3.x, v3.z, ref position);

            }

            private static bool CalculatePosition(float x, float y,float originX,float originY,float width,float height, ref int posX,ref int posY)
            {
                bool changed = false;
                //y -= Constant.WorldMapTerrain.WorldMapLeftTopPoint.z;
                //x -= Constant.WorldMapTerrain.WorldMapLeftTopPoint.x;

                x -= originX;
                y -= originY;

                float u = x - 2 * y;
                u += width / 2;
                int ru = (int)(u / width);
                if (u < 0 && (float)ru - u != 0)
                    ru--;

                float v = x / 2 + y;
                v -= height / 2;
                v *= -1;
                int rv = (int)(v / height);
                if (v < 0 && (float)rv - v != 0)
                    rv--;


                if (posX != ru || posY != rv)
                    changed = true;
                posX = ru;
                posY = rv;

                return changed;

            }
            public static bool CalculatePosition(float x, float y, ref WorldMap.WorldMapPosition pos)
            {
				int posX = pos.X,posY= pos.Y;
				bool changed;
                changed = CalculatePosition(x, y,Constant.WorldMapTerrain.WorldMapLeftTopPoint.x,Constant.WorldMapTerrain.WorldMapLeftTopPoint.z, Constant.WorldMapTerrain.WorldMapTileWidth, Constant.WorldMapTerrain.WorldMapTileHeight, ref posX,ref posY);
				pos.X = posX;
				pos.Y = posY;
				return changed;
            }
    //        public static bool CalculatePosition(float x, float y, ref WorldMap.CoverPosition pos)
    //        {
				//bool changed = false;
    //            int posX = 0;
    //            int posY = 0;
    //            float originX = Constant.WorldMapTerrain.WorldMapLeftTopPoint.x;
    //            float originY = Constant.WorldMapTerrain.WorldMapLeftTopPoint.z + Constant.WorldMapTerrain.WorldMapTileHeight * (Constant.WorldMapCover.CoverHeight - 1) / (Constant.WorldMapCover.CoverHeight * 2);

    //            changed = CalculatePosition(x, y,originX,originY, Constant.WorldMapTerrain.WorldMapTileWidth / Constant.WorldMapCover.CoverWidth,
    //                Constant.WorldMapTerrain.WorldMapTileHeight / Constant.WorldMapCover.CoverHeight, ref posX,ref posY);

    //            //posX += Constant.WorldMapCover.CoverWidth / 2;
    //            //posY += Constant.WorldMapCover.CoverHeight / 2;

    //            pos.X = posX % Constant.WorldMapCover.CoverWidth;
    //            pos.Y = posY % Constant.WorldMapCover.CoverHeight;


    //            return changed;
    //        }

            public static bool CalculateMiniMapPositionFromSpaceToWorldMapPosition(float x, float y,ref WorldMap.WorldMapPosition pos)
            {
                pos = new WorldMap.WorldMapPosition();

                bool changed = false;
                int posX = 0;
                int posY = 0;
                float originX = Constant.WorldMiniMap.LeftTopPoint.x;
                float originY = Constant.WorldMiniMap.LeftTopPoint.z + Constant.WorldMiniMap.TileHeight / 2;

                changed = CalculatePosition(x, y, originX, originY, Constant.WorldMiniMap.TileWidth / WorldMap.WorldMapProxy.MapWidth,
                    Constant.WorldMiniMap.TileHeight / WorldMap.WorldMapProxy.MapHeight, ref posX, ref posY);
                pos.X = posX;
                pos.Y = posY;
                //pos.X = posX % Constant.WorldMapCover.CoverWidth;
                //pos.Y = posY % Constant.WorldMapCover.CoverHeight;

                return changed;
            }

            public static bool CalculateMiniMapPosition(float x, float y, ref MiniMap.MiniMapPosition pos)
            {
                pos = new MiniMap.MiniMapPosition();
                bool changed = false;
                int posX = 0;
                int posY = 0;
                float originX = Constant.WorldMiniMap.LeftTopPoint.x;
                float originY = Constant.WorldMiniMap.LeftTopPoint.z;

                changed = CalculatePosition(x, y, originX, originY, Constant.WorldMiniMap.TileWidth, Constant.WorldMiniMap.TileHeight, ref posX, ref posY);

                pos.X = posX;
                pos.Y = -posY;



                return changed;

            }
#endregion

#region Block

            public static WorldMap.WorldMapPosition CalculateBlockLeftTop(WorldMap.WorldMapPosition pos, int blockWidth, int blockheight)
            {
                WorldMap.WorldMapPosition leftTop = new WorldMap.WorldMapPosition();
                try
                {
                    int dx = 0;
                    int dy = 0;
                    if (pos.X % blockWidth != 0)
                        dx = blockWidth;
                    if (pos.Y % blockheight != 0)
                        dy = blockheight;

                    if (pos.X >= 0)
                        leftTop.X = pos.X / blockWidth * blockWidth;
                    else
                        leftTop.X = pos.X / blockWidth * blockWidth - dx;

                    if (pos.Y >= 0)
                        leftTop.Y = pos.Y / blockheight * blockheight;
                    else
                        leftTop.Y = pos.Y / blockheight * blockheight - dy;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    
                }
                return leftTop;
                //leftTop.Y = pos.Y / height * height;
                //leftTop.X = pos.X / width * width;
            }
            public static WorldMap.WorldMapPosition CalculateBlockLeftTop(int x, int y, int width, int height)
            {
                WorldMap.WorldMapPosition leftTop = new WorldMap.WorldMapPosition();
                int dx = 0;
                int dy = 0;
                if (x % width != 0)
                    dx = width;
                if (y % height != 0)
                    dy = height;

                if (x >= 0)
                    leftTop.X = x / width * width;
                else
                    leftTop.X = x / width * width - dx;

                if (y >= 0)
                    leftTop.Y = y / height * height;
                else
                    leftTop.Y = y / height * height - dy;

                return leftTop;
            }
            public static void CalculateBlockLeftTop(int x, int y, int width, int height, out int X, out int Y)
            {
                X = 0;
                Y = 0;
                if (x % width != 0)
                    X = width;
                if (y % height != 0)
                    Y = height;

                if (x >= 0)
                    X = x / width * width;
                else
                    X = x / width * width - X;

                if (y >= 0)
                    Y = y / height * height;
                else
                    Y = y / height * height - Y;
            }


#endregion

#region coordinate to world

            public class SpaceCalculator
            {
                private Vector3 WorldMapLeftTopPoint;
                private float WorldMapTileWidth;
                private float WorldMapTileHeight;
                private int NY = 1;

                public SpaceCalculator(Vector3 worldMapLeftTopPoint, float worldMapTileWidth, float worldMapTileHeight,int ny)
                {
                    this.WorldMapLeftTopPoint = worldMapLeftTopPoint;
                    this.WorldMapTileWidth = worldMapTileWidth;
                    this.WorldMapTileHeight = worldMapTileHeight;
                    NY = ny;
                }

                public int CalculateX(float x, float y)
                {
                    y -= WorldMapLeftTopPoint.z;
                    x -= WorldMapLeftTopPoint.x;

                    float u = x - 2 * y;
                    u += WorldMapTileWidth / 2;
                    int ru = (int)(u / WorldMapTileWidth);
                    if (u < 0 && (float)ru - u != 0)
                        ru--;
                    return ru;
                }
                public int CalculateY(float x, float y)
                {
                    y -= WorldMapLeftTopPoint.z;
                    x -= WorldMapLeftTopPoint.x;

                    float v = x / 2 + y;
                    v -= WorldMapTileHeight / 2;
                    v *= -1;
                    int rv = (int)(v / WorldMapTileHeight);
                    if (v < 0 && (float)rv - v != 0)
                        rv--;
                    rv *= NY;
                    return rv;
                }
                public bool CalculatePosition(float x, float y, ref int posX, ref int posY)
                {
                    bool r = WorldMapUtility.CalculatePosition(x, y, WorldMapLeftTopPoint.x, WorldMapLeftTopPoint.z, WorldMapTileWidth, WorldMapTileHeight, ref posX, ref posY);
                    posY *= NY;
                    return r;
                }

                public Vector2 CalculateSpace(int x, int y)
                {
                    y *= NY;
                    float x0 = WorldMapLeftTopPoint.x + (x - y) * WorldMapTileWidth / 2;
                    float y0 = WorldMapLeftTopPoint.z - (x + y) * WorldMapTileHeight / 2;
                    return new Vector2(x0, y0);
                }
                public  Vector2 CalculateSpace(float x, float y)
                {
                    y *= NY;
                    float x0 = WorldMapLeftTopPoint.x + (x - y) * WorldMapTileWidth / 2;
                    float y0 = WorldMapLeftTopPoint.z - (x + y) * WorldMapTileHeight / 2;
                    return new Vector2(x0, y0);
                }

                public Vector2 CalculateSpace(WorldMap.WorldMapPosition pos)
                {
                    return CalculateSpace(pos.X, pos.Y);
                }

                public Vector2 CalculateSpace(WorldMap.WorldMapArea area)
                {
                    return CalculateSpace(area.LeftTop);
                }

                public  Vector2 CalculateSpace(int x, int y, int width, int height)
                {
                    return CalculateSpace(x + (float)(width - 1) / 2f, y + (float)(height - 1) / 2f);
                }

            }

            public static SpaceCalculator TerrainCalculator = new SpaceCalculator(Constant.WorldMapTerrain.WorldMapLeftTopPoint, Constant.WorldMapTerrain.WorldMapTileWidth, Constant.WorldMapTerrain.WorldMapTileHeight,1);
            public static SpaceCalculator MinimapCalculator = new SpaceCalculator(Constant.WorldMiniMap.LeftTopPoint, Constant.WorldMiniMap.TileWidth, Constant.WorldMiniMap.TileHeight,-1);
            public static SpaceCalculator MinimapWorldCalculator = new SpaceCalculator(new Vector3(Constant.WorldMiniMap.LeftTopPoint.x, Constant.WorldMiniMap.LeftTopPoint.y, Constant.WorldMiniMap.LeftTopPoint.z + Constant.WorldMiniMap.TileHeight / 2 - Constant.WorldMiniMap.TileHeight / Constant.WorldMap.WorldMapHeight / 2) , Constant.WorldMiniMap.TileWidth / Constant.WorldMap.WorldMapWidth, Constant.WorldMiniMap.TileHeight/ Constant.WorldMap.WorldMapHeight, 1);
            public static SpaceCalculator RealmCalculator = new SpaceCalculator(Constant.WorldMapRealm.LeftTopPoint, Constant.WorldMapRealm.TileWidth, Constant.WorldMapRealm.TileHeight,-1);
            public static SpaceCalculator LandCalculator = new SpaceCalculator(Constant.WorldMapLand.LeftTopPoint, Constant.WorldMapLand.TileWidth, Constant.WorldMapLand.TileHeight,1);



            public static WorldMapViewPortSizeScaner TerrainScaner = new WorldMapViewPortSizeScaner(Constant.WorldMapTerrain.WorldMapLeftTopPoint, TerrainCalculator);
            public static WorldMapViewPortSizeScaner MinimapScaner = new WorldMapViewPortSizeScaner(Constant.WorldMiniMap.LeftTopPoint, MinimapCalculator);
            public static WorldMapViewPortSizeScaner RealmScaner = new WorldMapViewPortSizeScaner(Constant.WorldMapRealm.LeftTopPoint, RealmCalculator);
            public static WorldMapViewPortSizeScaner LandScaner = new WorldMapViewPortSizeScaner(Constant.WorldMapLand.LeftTopPoint, LandCalculator);


            public static Vector2 CalculateCoverSpace(int x, int y, int w, int z)
            {
                w = x * Constant.WorldMapCover.CoverWidth + w;
                z = y * Constant.WorldMapCover.CoverHeight + z;

                float originY = Constant.WorldMapTerrain.WorldMapLeftTopPoint.z + Constant.WorldMapTerrain.WorldMapTileHeight * (Constant.WorldMapCover.CoverHeight - 1) / (Constant.WorldMapCover.CoverHeight * 2);

                float x0 = Constant.WorldMapTerrain.WorldMapLeftTopPoint.x + (w - z) * Constant.WorldMapTerrain.WorldMapTileWidth / 2 / Constant.WorldMapCover.CoverWidth;
                float y0 = originY - 0 - (w + z) * Constant.WorldMapTerrain.WorldMapTileHeight / 2 / Constant.WorldMapCover.CoverHeight;

                return new Vector2(x0, y0);
            }

#endregion
#region Camera

            public class FocusCamera
            {
                private SpaceCalculator spaceCalculator = null;

                public FocusCamera(SpaceCalculator calculator)
                {
                    spaceCalculator = calculator;

                }
                public void FocusToSpace(Vector3 space, Camera camera)
                {
                    camera.transform.localPosition = new Vector3(space.x, camera.transform.localPosition.y, space.z);
                }

                public void FocusToTile(int x, int y, Camera camera)
                {
                    if (camera == null) return;
                    Vector2 v2 = spaceCalculator.CalculateSpace(x, y);
                    camera.transform.localPosition = new Vector3(v2.x, camera.transform.localPosition.y, v2.y);
                    TweenPosition tween = camera.gameObject.GetComponent<TweenPosition>();
                    if (tween == null) return;
                    tween.enabled = false;
                    tween.onFinished.Clear();
                }

                public void FocusToTile(WorldMap.WorldMapPosition pos, Camera camera)
                {
                    FocusToTile(pos.X, pos.Y, camera);
                }

                public void FocusToTileTransition(int x, int y, Camera camera, AnimationCurve curve, float duration = 1f, EventDelegate finish = null)
                {
                    if (camera == null) return;
                    Vector2 v2 = spaceCalculator.CalculateSpace(x, y);
                    Vector3 v3 = new Vector3(v2.x, camera.transform.localPosition.y, v2.y);
                    TweenPosition tween = camera.gameObject.GetComponent<TweenPosition>();
                    if (tween == null) return;
                    tween.enabled = true;
                    tween.animationCurve = curve;
                    tween.from = camera.transform.localPosition;
                    tween.to = v3;
                    tween.duration = duration;
                    tween.style = UITweener.Style.Once;
                    tween.onFinished.Clear();
                    tween.onFinished.Add(new EventDelegate(() =>
                    {
                        if (finish != null)
                            finish.Execute();
                        tween.enabled = false;
                        camera.transform.localPosition = tween.to;
                        tween.onFinished.Clear();
                    }));

                    tween.ResetToBeginning();
                    tween.PlayForward();
                }
            }

            public static FocusCamera TerrainFocusCamera = new FocusCamera(TerrainCalculator);
            public static FocusCamera MiniMapFocusCamera = new FocusCamera(MinimapCalculator);
            public static FocusCamera MiniMapWorldFocusCamera = new FocusCamera(MinimapWorldCalculator);

#endregion

            public static int[] WorldPositionsToIDs(WorldMap.WorldMapPosition[] positions)
            {
                if (positions == null) return null;
                int[] ids = new int[positions.Length];
                WorldMap.TerrainProxy proxy = ProxyMgr.Instance.GetProxy<WorldMap.TerrainProxy>();
                for (int i = 0; i < positions.Length; i++)
                {
                    ids[i] = positions[i].ToID(proxy.Width);
                }
                return ids;
            }
            public static int[] WorldAreasToIDs(WorldMap.WorldMapArea[] areas)
            {
                if (areas == null) return null;
                int[] ids = new int[areas.Length];
                WorldMap.TerrainProxy proxy = ProxyMgr.Instance.GetProxy<WorldMap.TerrainProxy>();
                for (int i = 0; i < areas.Length; i++)
                {
                    ids[i] = areas[i].LeftTop.ToID(proxy.Width);
                }
                return ids;
            }

#region Line
            public static Mesh MakeLineMesh(Vector2[] positions, 
                float thick,
                Action<Vector3[], float> ajustAction, 
                Action<Vector3, Vector3, float, List<Vector3>> makeoneline)
            {
                if (positions == null) return null;
                Vector2[] v2s = new Vector2[positions.Length];
                for (int i = 0; i < positions.Length; i++)
                {
                    v2s[i] = TerrainCalculator.CalculateSpace(positions[i].x, positions[i].y);
                }
                Vector3[] v3s = new Vector3[positions.Length];
                for (int j = 0; j < positions.Length; j++)
                {
                    v3s[j].x = v2s[j].x;
                    v3s[j].y = Constant.WorldMapTerrain.PointYPosition;
                    v3s[j].z = v2s[j].y;
                }
                if (ajustAction != null)
                    ajustAction(v3s,thick);

                List<Vector3> vertices = new List<Vector3>();

                for (int i = 0; i < v3s.Length - 1; i++)
                {
                    if (makeoneline != null)
                        makeoneline(v3s[i], v3s[i + 1], thick, vertices);

                }
                Mesh mesh = new Mesh();

                mesh.vertices = vertices.ToArray();
                mesh.triangles = MakeRectMesh(v3s.Length - 1);
                //mesh.uv = MakeUV(v3s.Length - 1);
                return mesh;

            }
            //public static Mesh MakeLineMesh(Vector3[] positions, float thick, Action<Vector3, Vector3, float, List<Vector3>> makeoneline)
            //{
            //}
            private static float CalculateTilingY(Vector2[] v2s)
            {
                //Constant.March.MarchLineInterval
                if (v2s == null || v2s.Length != 2)
                    return 0f;
                float distance = Mathf.Sqrt((v2s[1].x - v2s[0].x) * (v2s[1].x - v2s[0].x) + (v2s[1].y - v2s[0].y) * (v2s[1].y - v2s[0].y));


                return distance / Constant.March.MarchLineInterval;
            }

            public static Mesh MakeMarchLineMesh(Vector2[] positions,
                                                float thick,
                                                Action<Vector3[], float> ajustAction,
                                                Action<Vector3, Vector3, float, List<Vector3>> makeoneline,
                                                out float tilingY)
            {
                tilingY = 1f;
                if (positions == null) return null;
                Vector2[] v2s = new Vector2[positions.Length];
                for (int i = 0; i < positions.Length; i++)
                {
                    v2s[i] = TerrainCalculator.CalculateSpace(positions[i].x, positions[i].y);
                }
                //v2s = FixV2sForMarchLine(v2s);
                tilingY = CalculateTilingY(v2s);
                Vector3[] v3s = new Vector3[v2s.Length];
                for (int j = 0; j < v2s.Length; j++)
                {
                    v3s[j].x = v2s[j].x;
                    v3s[j].y = Constant.WorldMapTerrain.PointYPosition;
                    v3s[j].z = v2s[j].y;
                }
                if (ajustAction != null)
                    ajustAction(v3s, thick);

                List<Vector3> vertices = new List<Vector3>();

                for (int i = 0; i < v3s.Length - 1; i++)
                {
                    if (makeoneline != null)
                        makeoneline(v3s[i], v3s[i + 1], thick, vertices);

                }
                Mesh mesh = new Mesh();

                mesh.vertices = vertices.ToArray();
                mesh.triangles = MakeRectMesh(v3s.Length - 1);
                mesh.uv = MakeUV(v3s.Length - 1);
                return mesh;

            }
            private static float InnerMultiple(Vector2 v1, Vector2 v2)
            {
                return v1.x * v2.x + v1.y * v2.y;
            }
            private static Vector2[] FixV2sForMarchLine(Vector2[] v2s)
            {
                if (v2s == null) return null;
                List<Vector2> list = new List<Vector2>();
                int index = 0;
                for (int i = 0; i < v2s.Length - 1; i++)
                {
                    Vector2 current = v2s[i];
                    Vector2 next = v2s[i + 1];
                    Vector2 dir = next - current;
                    Vector2 normal = dir.normalized;
                    index = 0;

                    while (normal != Vector2.zero)
                    {
                        Vector2 v2 = (current + normal * Constant.March.MarchLineInterval * index);
                        if (InnerMultiple(next - v2, normal) < 0f)
                        {
                            break;
                        }
                        list.Add(v2);
                        index++;
                    }
                    list.Add(next);
                }
                return list.ToArray();
            }

            public static void RectAjustAction(Vector3[] v3s,float thick)
            {
                v3s[0].z += Constant.WorldMapTerrain.WorldMapTileHeight / 2 - thick;
                v3s[1].x -= Constant.WorldMapTerrain.WorldMapTileWidth / 2 - thick;
                v3s[2].z -= Constant.WorldMapTerrain.WorldMapTileHeight / 2 - thick;
                v3s[3].x += Constant.WorldMapTerrain.WorldMapTileWidth / 2 - thick;
                v3s[4].z += Constant.WorldMapTerrain.WorldMapTileHeight / 2 - thick;
            }


            public static Vector2[] MakeUV(int lineCount)
            {
                Vector2 [] v2s = new Vector2[lineCount * 4];
                for (int i = 0; i < lineCount; i++)
                {
                    int t = 4 * i;
                    v2s[t] = new Vector2(1, 1);
                    v2s[t + 1] = new Vector2(0, 1);
                    v2s[t + 2] = new Vector2(0, 0);
                    v2s[t + 3] = new Vector2(1, 0);

                }
                
                return v2s;
            }

            public static void MakeRectOneLine(Vector3 start, Vector3 end, float thick, List<Vector3> vertices)
            {
                Vector3 at = end - start;
                Vector3 offsetS, offsetE;
                if (at.x * at.z < 0)
                {
                    offsetS = (new Vector3(1, 0, 0)) * thick * Mathf.Sqrt(at.x * at.x + at.z * at.z) / -at.z;
                    offsetE = (new Vector3(0, 0, 1)) * thick * Mathf.Sqrt(at.x * at.x + at.z * at.z) / at.x;
                }
                else
                {
                    offsetS = (new Vector3(0, 0, 1)) * thick * Mathf.Sqrt(at.x * at.x + at.z * at.z) / at.x;
                    offsetE = (new Vector3(1, 0, 0)) * thick * Mathf.Sqrt(at.x * at.x + at.z * at.z) / -at.z;
                }

                vertices.Add(start);
                vertices.Add(start + offsetS);
                vertices.Add(end + offsetE);
                vertices.Add(end);

            }
            public static void MakeOneLine(Vector3 start, Vector3 end, float thick, List<Vector3> vertices)
            {
                Vector3 at = end - start;
                Vector3 offset = (new Vector3(-at.z, 0, at.x)).normalized * thick / 2;

                vertices.Add(start - offset);
                vertices.Add(start + offset);
                vertices.Add(end + offset);
                vertices.Add(end - offset);
            }


            public static int[] MakeRectMesh(int lineCount)
            {
                int index = 0;
                int [] indices = new int[lineCount * 6];
                for (int i = 0; i < lineCount; i++)
                {
                    int t = 4 * i;
                    indices[index++] = t;
                    indices[index++] = t + 1;
                    indices[index++] = t + 2;

                    indices[index++] = t + 2;
                    indices[index++] = t + 3;
                    indices[index++] = t;
                }

                return indices;
            }

            public static Vector2[] CalculateRectPositions(int left, int top, int width, int height)
            {
                Vector2[] v2s = new Vector2[5];
                v2s[0].x = left;
                v2s[0].y = top;

                v2s[1].x = left;
                v2s[1].y = top + height - 1;

                v2s[2].x = left + width - 1;
                v2s[2].y = top + height - 1;

                v2s[3].x = left + width - 1;
                v2s[3].y = top;

                v2s[4].x = left;
                v2s[4].y = top;

                return v2s;
            }

#endregion

#region Rect

            public static bool IsInRect(int x,int y, int left,int top , int width, int height)
            {
                if (x < left) return false;
                if (x >= left + width) return false;
                if (y < top) return false;
                if (y >= top + height) return false;

                return true;
            }

            public static bool IsInRect(float x, float y, float left, float top, float width, float height)
            {
                if (x < left) return false;
                if (x >= left + width) return false;
                if (y < top) return false;
                if (y >= top + height) return false;

                return true;
            }



#endregion

#region Effect

            public static WorldMap.WorldMapPosition[] FilterBossPlayerCity(WorldMap.WorldMapPosition[] positions)
            {
                if (positions == null) return null;
                List<WorldMap.WorldMapPosition> ps = new List<WorldMap.WorldMapPosition>();
                WorldMap.WorldMapPlayersProxy proxy = ProxyMgr.Instance.GetProxy<WorldMap.WorldMapPlayersProxy>();
                KingsLand.KingsLandProxy kingProxy = ProxyMgr.Instance.GetProxy<KingsLand.KingsLandProxy>();

                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i] == WorldMap.WorldMapPosition.InValid)
                    {
                        ps.Add(WorldMap.WorldMapPosition.InValid);
                    }
                    else
                    {
                        if (proxy.GetBoss(positions[i]) != null || proxy.GetPlayer(positions[i]) != null)
                        {
                            ps.Add(positions[i]);
                        }
                        else if (kingProxy.IsInKingsLandPalaceArea(positions[i]))
                        {
                            ps.AddRange(kingProxy.GetKingsLandPalacePositions(positions[i].MapID));
                        }
                        else
                        {
                            //ps.Add(WorldMap.WorldMapPosition.InValid);
                        }
                    }
                }
                return ps.ToArray();
            }

#endregion
        }
#endif
#region Time
        public static int ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32((date - epoch).TotalSeconds);
        }

        public static long ToUnixTimeStamp(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }

        public static string TimeFormat(this int seconds)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long s = (long)seconds * 10000000L + origin.Ticks;
            DateTime date = new DateTime(s);
            return date.ToString();
        }
        public static string TimeFormat(this long seconds)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long s = seconds * 10000000L + origin.Ticks;
            DateTime date = new DateTime(s);
            return date.ToString();
        }
        public static string LongDateFormat(this long seconds)
        {
            DateTime date = new DateTime(seconds);
            return date.ToLongDateString();
        }

        public static string LongDateFormat(this int seconds)
        {
            DateTime date = new DateTime(seconds);
            return date.ToLongDateString();
        }


        public static string LongTimeFormat(this long seconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (seconds / 3600) % 24, (seconds / 60) % 60, seconds % 60);
        }
        public static string LongTimeFormat(this int seconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (seconds / 3600) % 24, (seconds / 60) % 60, seconds % 60);
        }


        public static string SecondsFormat(this int seconds)
		{
			return string.Format("{0:00}:{1:00}:{2:00}",(seconds/3600)%24,(seconds/60)%60,seconds%60);
		}

		public static string SecondsFormat(this long seconds)
		{
			return string.Format("{0:00}:{1:00}:{2:00}",(seconds/3600)%24,(seconds/60)%60,seconds%60);
		}

        public static string WholeSecondsFormat(this long seconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (seconds / 3600), (seconds / 60) % 60, seconds % 60);
        }
        public static string WholeSecondsFormat(this int seconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (seconds / 3600), (seconds / 60) % 60, seconds % 60);
        }

  //      public static string SystemTime(this int seconds)
		//{
		//	return DateTime.UtcNow.AddSeconds ( seconds - TimeProxy.CurrentServerTime).ToLocalTime().ToString();
		//}
        public static int GetLastTime(this int seconds)
        {
            int now = (seconds / 3600) % 24 * 3600 + (seconds / 60) % 60 * 60 + seconds % 60;
            return 24 * 60 * 60 - now;
        }
        public static int GetNowServerTime_Seconds()
        {
            DateTime date = DateTime.UtcNow;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32((date - epoch).TotalSeconds);
        }

        public static int GetMidNightServerTime_Seconds(DateTime date)
        {
            date = date.Date;//get midnight Time
    //        Logger.Log("date:"+date.ToString());
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32((date - epoch).TotalSeconds);
        }

        //GetLastOrNextSeveralDaysMidNightServerTime_Seconds
        public static int GetLONSDsMNST_Seconds(DateTime date,double daySpan)
        {
            DateTime newDate = date.AddDays(daySpan);
            newDate = newDate.Date;
    //        Logger.Log("date:"+ newDate.ToString());
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32((newDate - epoch).TotalSeconds);
        }

        //GetLastOrNextSeveralHoursServerTime_Seconds
        public static int GetLONSHsST_Seconds(DateTime date, double hourSpan)
        {
            DateTime newDate = date.AddHours(hourSpan);
     //       Logger.Log("date:" + newDate.ToString());
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int seconds =  Convert.ToInt32((newDate - epoch).TotalSeconds);
            int hours = (seconds / 3600);
            return hours*3600;
        }

        //GetNextMidNightServerTime_Seconds
        public static int GetNMNST_Seconds(DateTime date)
        {
            return GetLONSDsMNST_Seconds(date, 1.0d);
        }

        //GetLastMidNightServerTime_Seconds
        public static int GetLMNST_Seconds(DateTime date)
        {
            return GetLONSDsMNST_Seconds(date, 0.0d);
        }

        //GetNextHourServerTime_Seconds
        public static int GetNHST_Seconds(DateTime date)
        {
            return GetLONSHsST_Seconds(date, 1.0d);
        }

        //Get today left time
        //public static string GetTodayLeftTime()
        //{
        //    int days = TimeProxy.CurrentServerTime / 86400;
        //    int leftTimeSeconds = (days + 1) * 86400 - TimeProxy.CurrentServerTime;
        //    return leftTimeSeconds.SecondsFormat();
        //}

        public static DateTime GetServerTime_DateTime(int totalSeconds)
        {
            TimeSpan temp = new TimeSpan(0, 0, totalSeconds);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            DateTime shedule = epoch + temp;

            return shedule;
        }

        public static string GetServerTimeFormatYYYYMMDD_HHmmss(double totalSeconds)
        {    
            DateTime s = new DateTime(1970, 1, 1);
            s = s.AddSeconds(totalSeconds);
            return s.ToString("MM-dd-yyyy,HH:mm:ss");
        }

        public static string GetServerTimeFormatYYYYMMDD_HHmmss1(double totalSeconds)
        {
            DateTime s = new DateTime(1970, 1, 1);
            s = s.AddSeconds(totalSeconds);
            return s.ToString("MM/dd/yy HH:mm:ss");
        }

        public static string GetServerTimeFormatYYYYMMDD(double totalSeconds)
        {
            DateTime s = new DateTime(1970, 1, 1);
            s = s.AddSeconds(totalSeconds);
            return s.ToString("MM/dd/yy");
        }

        public static string testTime()
        {
            double dTime = (double)GetNowServerTime_Seconds();
            return GetServerTimeFormatYYYYMMDD_HHmmss(dTime);
        }

        /*
        ** type = 0,means general
        ** type = 1,means chat
        */
        public static string getPassTimeStr(long passTime, int type = 0)
        {
            if (passTime >= 24 * 3600)
            {
                long days = passTime / (24 * 3600);

                Dictionary<string, System.Object> pairs = new Dictionary<string, System.Object>();
                pairs["Time"] = days;
                return GameUtil.DI18n.T("General.Dago", pairs);
            }
            else if (passTime < 24 * 3600 && passTime >= 3600)
            {
                long hours = passTime / (3600);

                Dictionary<string, System.Object> pairs = new Dictionary<string, System.Object>();
                pairs["Time"] = hours;
                return GameUtil.DI18n.T("General.Hago", pairs);
            }
            else if (passTime < 3600 && passTime >= 60)
            {
                long minutes = passTime / (60);

                Dictionary<string, System.Object> pairs = new Dictionary<string, System.Object>();
                pairs["Time"] = minutes;
                return GameUtil.DI18n.T("General.Mago", pairs);
            }
            else if (passTime < 60 && passTime > 0)
            {
                long seconds = passTime;

                Dictionary<string, System.Object> pairs = new Dictionary<string, System.Object>();
                pairs["Time"] = seconds;
                return GameUtil.DI18n.T("General.Sago", pairs);
            }
            else
            {
                if(type == 0)
                    return GameUtil.DI18n.T("General.Online");
                else
                    return GameUtil.DI18n.T("General.Now");
            }
        }



#endregion
        public static GameObject AddChild(GameObject parent, GameObject child)
        {
            if (child == null) return null;
            GameObject go = GameObject.Instantiate(child) as GameObject;
            Vector3 v3 = child.transform.localPosition;
            //Vector3 vr = child.transform.localEulerAngles;
            //Vector3 vs = child.transform.localScale;
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            if (go != null && parent != null)
            {
                go.transform.parent = parent.transform;
                //t.localPosition = Vector3.zero;
                //t.localRotation = Quaternion.identity;
                //t.localScale = Vector3.one;
                go.layer = parent.layer;
                go.transform.localPosition = v3;
                //go.transform.localEulerAngles = vr;
                //go.transform.localScale = vs;
            }
            return go;

        }


        public static GameObject FindChild(GameObject parent, string name)
        {
            if (parent == null) return null;

            int n = parent.transform.childCount;
            for (int i = 0; i < n; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (child != null && child.gameObject.name == name)
                    return child.gameObject;
            }

            return null;
        }



        /// <summary>
        /// ui string helper
        /// </summary>
        public static class UIStringHelper
        {
            public static string GetBuildingTitle(int buildingType)
            {
                string key = "Building.Title" + buildingType;
                return DI18n.T(key);
            }
            public static string GetBuildingDescription(int buildingType)
            {
                string key = "Building.Desc" + buildingType;
                return DI18n.T(key);
            }
            public static string GetBuildingRequirement(int id)
            {
                string key = "Requirement" + id;
                return DI18n.T(key);
            }
        }



        

		//public static string GetBossName(int boss, int bossCity, int tileType)
		//{
		//	Dictionary<string, object> dict = new Dictionary<string, object> ();

		//	int level = 0;
		//	string name = string.Empty;

		//	if(boss < 10000) 
		//	{
		//		level = boss;
		//	}
		//	else  //pet boss
		//	{
		//		level = boss%100;
		//		int petType =  (boss/100)%100;
		//		name = DI18n.T("Pet.Name"+petType);
		//	}
			//switch ((WorldMap.WorldMapActiveObjectTileType)tileType)
			//{
			//case WorldMap.WorldMapActiveObjectTileType.MonsterHead:
			//	name = GameUtil.DI18n.T("Tile.BossCenter1");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.LittleMonster:
			//	name = GameUtil.DI18n.T("Tile.BossCity1");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.DogBossHead:
			//	name = GameUtil.DI18n.T("Tile.BossCenter4");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.LittleDogBoss:
			//	name = GameUtil.DI18n.T("Tile.BossCity4");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.HandBossHead:
			//	name = GameUtil.DI18n.T("Tile.BossCenter2");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.LittleHandBoss:
			//	name = GameUtil.DI18n.T("Tile.BossCity2");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.TreeBossHead:
			//	name = GameUtil.DI18n.T("Tile.BossCenter3");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.LittleTreeBoss:
			//	name = GameUtil.DI18n.T("Tile.BossCity3");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.TourLittleDog:
			//	name = GameUtil.DI18n.T("Tile.BossCity4");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.TourLittleTree:
			//	name = GameUtil.DI18n.T("Tile.BossCity3");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.TourDogHead:
			//	name = GameUtil.DI18n.T("Tile.BossCenter4");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.TourTreeHead:
			//	name = GameUtil.DI18n.T("Tile.BossCenter3");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.PetSmallDragonHead:
			//case WorldMap.WorldMapActiveObjectTileType.PetSmallDragon:
			//	name = DI18n.T("Pet.Name8");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.PetKylinHead:
			//case WorldMap.WorldMapActiveObjectTileType.PetKylin:
			//	name = DI18n.T("Pet.Name1");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.PetTaurenHead:
			//case WorldMap.WorldMapActiveObjectTileType.PetTauren:
			//	name = DI18n.T("Pet.Name4");
			//	break;
			//case WorldMap.WorldMapActiveObjectTileType.PetDragonHead:
			//case WorldMap.WorldMapActiveObjectTileType.PetDragon:
		//		name = DI18n.T("Pet.Name2");
		//		break;

		//	default:
		//		break;
		//	}

		//	dict.Add ("Num", level);
		//	dict.Add("Name", name);

		//	return DI18n.T("Mail.Boss", dict);
		//}

		//public static int GetBossLevel(int boss)
		//{
		//	int level = 0;

		//	if(boss < 100) 
		//	{
		//		level = boss;
		//	}
		//	else  //pet boss
		//	{
		//		level = boss%100;
		//	}
		//	return level;
		//}

		public static bool IsPetBoss(int boss)
		{
			return boss > 100;
		}

		//public static bool IsMonster(int tileType)
		//{
		//	switch ((WorldMap.WorldMapActiveObjectTileType)tileType)
		//	{
			
		//	case WorldMap.WorldMapActiveObjectTileType.PetSmallDragonHead:
		//	case WorldMap.WorldMapActiveObjectTileType.PetSmallDragon:
		//	case WorldMap.WorldMapActiveObjectTileType.PetKylinHead:
		//	case WorldMap.WorldMapActiveObjectTileType.PetKylin:
		//	case WorldMap.WorldMapActiveObjectTileType.PetTaurenHead:
		//	case WorldMap.WorldMapActiveObjectTileType.PetTauren:
		//	case WorldMap.WorldMapActiveObjectTileType.PetDragonHead:
		//	case WorldMap.WorldMapActiveObjectTileType.PetDragon:
		//		return true;
		//	default:
		//		return false;
		//	}
		//}

		public static int GetPetBossType(int boss)
		{
			if (boss < 100)
				return 0;
			else
				return (boss/100)%100;
		}

		public static string RealmName(this int num)
		{
			return DI18n.T("General.Realm") + Data_Settings.REALM_PREFIX + num.ToString();
		}

        public static void SerializeTroops(Dictionary<int,long> troops)
        {
            if (troops == null) return;
            if (troops.Count <= 0) return;

            PlayerPrefs.SetInt("TroopsNum", troops.Count);
            int i = 0;
            foreach (KeyValuePair<int, long> pair in troops)
            {
                PlayerPrefs.SetString("TroopsKey" + i.ToString(), pair.Key.ToString());
                PlayerPrefs.SetString("TroopsValue" + i.ToString(), pair.Value.ToString());
                i++;
            }

        }

        public static Dictionary<int, long> DeserializeTroops()
        {
            Dictionary<int, long> dic = new Dictionary<int, long>();

            int n  = PlayerPrefs.GetInt("TroopsNum", 0);
            for (int i = 0; i < n; i++)
            {
                int key = Int32.Parse(PlayerPrefs.GetString("TroopsKey" + i.ToString(), "0"));
                int value = Int32.Parse(PlayerPrefs.GetString("TroopsValue" + i.ToString(), "0"));
                dic[key] = value;
            }
            return dic;
        }

    }

	public static class StringFormatter
	{
		public static string FormatTime(long time)
		{
			TimeSpan t = TimeSpan.FromSeconds(time);
            string result = "";
            if (t.Seconds > 0)
            {
                result = t.Seconds + "S " + result;
            }   
			if(t.Minutes > 0){
				result = t.Minutes + "M " + result;
			}
			if(t.Hours > 0){
				result = t.Hours + "H " + result;
			}
			if(t.Days > 0){
				result = t.Days + "D " + result;
			}
			return result;
		}
		
		public static string FormatTime2(int time)
		{
			TimeSpan t = TimeSpan.FromSeconds(time);
			string result = "";
			
			if(t.Days > 0){
				result = result + t.Days + "D";
			}
			if(t.Hours > 0){
				result = result + t.Hours + "H";
			}
			else if(t.Minutes > 0){
				result = result + t.Minutes + "M";
			}
			else if(t.Seconds > 0)
			{
				result = result + t.Seconds + "S";	
			}
			
			return result;
		}
		public static string FormatTime3(long time,bool stillShowSeconds = true){
			if(time < 0) return "0S";
			TimeSpan t = TimeSpan.FromSeconds(time);
			string result = "";
			long hours = 0;
			
			if(t.Days > 0){
				hours = t.Days*24;
			}
			if(t.Hours>0 || hours > 0){
				result += t.Hours + hours + "H ";
			}
			if(t.Minutes>0){
				result += t.Minutes + "M ";
			}
			if(t.Seconds > 0 && stillShowSeconds){
				result += t.Seconds + "S ";
			}
			
			return result;
		}
		
		public static string FormatTime5D(int time){
			TimeSpan t = TimeSpan.FromSeconds (time);
			string result = "";
			int days = 0;
			int hours = 0;
			int minutes = 0;
			int seconds = 0;
			if (t.Days > 0) {
				days = t.Days;
			}
			if (t.Hours > 0) {
				hours = t.Hours;
			}
			if (t.Minutes > 0) {
				minutes = t.Minutes;
			}
			if (t.Seconds > 0) {
				seconds = t.Seconds;
			}
			if (days > 0) {
				result = days + "D " + hours + "H " + minutes + "M";
			} else {
				result = hours + "H " + minutes + "M " + seconds + "S";
			}
			return result;
		}
		
		
		public static string FormatTime5(int time,bool stillShowSecondes = true){
			TimeSpan t = TimeSpan.FromSeconds (time);
			string result = "";
			int hours = 0;
			int minutes = 0;
			int seconds = 0;
			if (t.Days > 0) {
				hours = t.Days * 24;
			}
			if (t.Hours > 0) {
				hours += t.Hours;
			}
			if (t.Minutes > 0) {
				minutes = t.Minutes;
			}
			if (t.Seconds > 0) {
				seconds = t.Seconds;
			}
			if (stillShowSecondes) {
				result = hours + "H " + minutes + "M " + seconds + "S";
			} else {
				if (hours > 0) {
					result = hours + "H " + minutes + "M";
				} else {
					result = minutes + "M " + seconds + "S";
				}
			}
			return result;
		}
		
		public static string FormatTime4(int time, uint units = TimeUnits.ALL){
			TimeSpan t = TimeSpan.FromSeconds(time);
			
			string result = "";
			
			if(Convert.ToBoolean(units & TimeUnits.DAYS))
				result += t.Days + "D ";
			
			if(Convert.ToBoolean(units & TimeUnits.HOURS))
				result += t.Hours + "H ";
			
			if(Convert.ToBoolean(units & TimeUnits.MINUTES))
				result += t.Minutes + "M ";
			
			if(Convert.ToBoolean(units & TimeUnits.SECONDS))
				result += t.Seconds + "S ";
			
			return result;
		}
		
		public static string FormatTime6(long time, int unitsCnt = 4){
			if(0 == time)
				return "0S";
			
			TimeSpan t = TimeSpan.FromSeconds(time);
			unitsCnt = Math.Max(1, unitsCnt);
			unitsCnt = Math.Min(4, unitsCnt);
			
			string units = "DHMS";
			int[]  count = new int[]{t.Days, t.Hours, t.Minutes, t.Seconds};
			
			int begin = 0;
			for (int i=0; i<4; i++)
			{
				if (count[i] > 0)
				{
					begin = i;
					break;
				}
			}
			
			string re = "";
			for (int i = begin; i<4 && unitsCnt>0; i++, unitsCnt--)
			{
				if ((i==3 || unitsCnt==1) && count[i] == 0)
					break;
				
				re += (count[i] + "" + units[i] + " ");
			}
			
			return re;
		}
		
		public static string FormatTime6T(long time, int unitsCnt = 4){
			return FormatTime3 (time);
			TimeSpan t = TimeSpan.FromSeconds(time);
			unitsCnt = Math.Max(1, unitsCnt);
			unitsCnt = Math.Min(4, unitsCnt);
			
			string[] units = new string[]{DI18n.T("common.day"), DI18n.T("common.hour"), DI18n.T("common.minute"), DI18n.T("common.second")};
			int[]  count = new int[]{t.Days, t.Hours, t.Minutes, t.Seconds};
			
			int begin = 0;
			for (int i=0; i<4; i++)
			{
				if (count[i] > 0)
				{
					begin = i;
					break;
				}
			}
			
			string re = "";
			for (int i = begin; i<4 && unitsCnt>0; i++, unitsCnt--)
			{
				if ((i==3 || unitsCnt==1) && count[i] == 0)
					break;
				
				re += (count[i] + " " + units[i] + " ");
			}
			
			return re;
		}
		
		public static string FormatTime7(int time,bool showDays = false){
			if(time < 0) return "0S";
			TimeSpan t = TimeSpan.FromSeconds(time);
			
			if(showDays&&t.Days>0){
				return t.Days + "D ";
			}
			
			string result = "";
			int hours = 0;
			
			if(t.Days > 0){
				hours = t.Days*24;
			}
			if(t.Hours>0 || hours > 0){
				result = t.Hours + hours + "H ";
			}
			else if(t.Minutes>0){
				result = t.Minutes + "M ";
			}
			else if(t.Seconds > 0){
				result = t.Seconds + "S ";
			}
			
			return result;
		}
		
		public static string FormatTime8(long time, int unitsCnt = 4){
			TimeSpan t = TimeSpan.FromSeconds(time);
			unitsCnt = Math.Max(1, unitsCnt);
			unitsCnt = Math.Min(4, unitsCnt);
			
			string units = "DHMS";
			int[]  count = new int[]{t.Days, t.Hours, t.Minutes, t.Seconds};
			
			int begin = -1;
			for (int i=0; i<5-unitsCnt; i++)
			{
				if (count[i] > 0)
				{
					begin = i;
					break;
				}
			}
			if (begin == -1)
			{
				begin = 4-unitsCnt;
			}
			
			string re = "";
			for (int i = begin; i<4 && unitsCnt>0; i++, unitsCnt--)
			{
				re += (count[i] + "" + units[i] + " ");
			}
			
			return re;
		}
		
		public static string FormatTime9(int time){
			TimeSpan t = TimeSpan.FromSeconds (time);
			string result = "";
			int days = 0;
			int hours = 0;
			int minutes = 0;
			int seconds = 0;
			if (t.Days > 0) {
				days = t.Days;
			}
			if (t.Hours > 0) {
				hours = t.Hours;
			}
			if (t.Minutes > 0) {
				minutes = t.Minutes;
			}
			if (t.Seconds > 0) {
				seconds = t.Seconds;
			}
			if(days > 0)
			{
				result = days + "D " + hours + "H";
			}
			else if(hours > 0)
			{
				result = hours + "H " + minutes + "M";
			}
			else if(minutes > 0)
			{
				result = minutes + "M " + seconds + "S";
			}
			else
			{
				result = seconds + "S";
			}
			return result;
		}

		public static string FormatTime10(int time, uint units = TimeUnits.ALL){
			TimeSpan t = TimeSpan.FromSeconds(time);
			
			string result = "";
			
//			if(Convert.ToBoolean(units & TimeUnits.DAYS))
//				result += t.Days + "D ";
			
			if(Convert.ToBoolean(units & TimeUnits.HOURS))
				result += t.Hours + ": ";
			
			if(Convert.ToBoolean(units & TimeUnits.MINUTES))
				result += t.Minutes + ": ";
			
			if(Convert.ToBoolean(units & TimeUnits.SECONDS))
				result += t.Seconds;
			
			return result;
		}
		
		public static string Substring(string s, int n)
		{
			if(s.Length <= n)
			{
				return s;	
			}
			else
			{
				string temp = s.Substring(0, n);
				return temp + "...";
			}
		}
		
		
		public static string ChangeNumberToString(int num)
		{
			if(num >=100)
			{
				return "99+";
			}
			else
			{
				return num + "";
			}
		}


		public static string FormateWorldCoordinate(int x, int y, int world)
		{
			string str = "(" + x + ", " +  y + ", " + world + ")";
			return str;

		}

		public static string FormateWorldCoordinate(string x, string y, int world)
		{
			string str = "(" + x + ", " +  y + ", " + world + ")";
			return str;
			
		}


		public static string FormatSimple(this int num)
		{
			if (num >= 1000f)
				return string.Format("{0:0,0}", num);
			else
				return num.ToString();
		}

		public static string FormatSimple(this long num)
		{
			if (num >= 1000f)
				return string.Format("{0:0,0}", num);
			else
				return num.ToString();
		}

		public static string FormatSimple(this float num)
		{
			return num.ToString ("F2");
		}

		public static string FormatMailTime(this int num)
		{
			return num.ToString ();
		}

		public class TimeUnits {
			public const uint ALL = DAYS | HOURS | MINUTES | SECONDS;
			
			public const uint DAYS = 0x0001;
			public const uint HOURS = 0x0002;
			public const uint MINUTES = 0x0004;
			public const uint SECONDS = 0x0008;
		}
		public static string FormatSimpleK(this int num)
		{
			if (num >= 1000f) {
				int first = num / 1000;
				string f = first > 1000 ? string.Format ("{0:0,0}", first) : first.ToString();

				int left = num % 1000;
				if (left != 0) {
					float left_ = left / 1000f;
					string l = left_.ToString ("F2");
					return f + "." + l.Split ('.') [1] + "K";
				} else {
					return f + "K";
				}
			}
			else
				return num.ToString();
		}
    }

	public static class Validations
	{
		
		public static bool ValidStringLength (string str, int min, int max)
		{
			if (str.Length >= min && str.Length <= max) 
				return true;
			return false;
		}
		
		public static bool ValidStringByRegexp (string str, string pattern, System.Text.RegularExpressions.RegexOptions options= System.Text.RegularExpressions.RegexOptions.IgnoreCase)
		{
			if (System.Text.RegularExpressions.Regex.IsMatch (str, pattern, options)) {
				return true;	
			}
			return false;
		}
		
		public static bool ValidOnlyNumberInString (string str)
		{
			if (ValidStringByRegexp(str, "\\d+")) {
				return true;
			}
			return false;
		}
		
		public static bool ValidInt (string str)
		{
			int result = 0;
			return int.TryParse(str, out result);
		}
        //		public static void OnMapCoordInput(UIInput input, string prevstr, string curstr)
        //		{
        //			int num = 0;
        //			
        //			if (string.IsNullOrEmpty(curstr))
        //			{
        //				// Empty
        //			}
        //			else if (int.TryParse(curstr, out num))
        //			{
        //				
        //				if (num >= MapConstants.ROWS)
        //				{
        //					num = MapConstants.ROWS - 1;
        //				}
        //				else if (num < 0)
        //				{
        //					num = 0;
        //				}
        //				
        //				input.text = num + "";
        //			}
        //			else
        //			{
        //				if (int.TryParse(prevstr, out num) || string.IsNullOrEmpty(prevstr))
        //				{
        //					input.text = prevstr;
        //				}
        //				else
        //				{
        //					input.text = "0";
        //				}
        //			}
        //		}


        /**
 * 检测是否有emoji字符
 * @param source
 * @return 一旦含有就抛出
 */
        public static bool containsEmoji(String source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }

            int len = source.Length;

            for (int i = 0; i < len; i++)
            {
                char codePoint = source[i];

                if (isEmojiCharacter(codePoint))
                {
                    //do nothing，判断到了这里表明，确认有表情字符
                    return true;
                }
            }

            return false;
        }

        private static bool isEmojiCharacter(char codePoint)
        {
            return (codePoint == 0x0) ||
                    (codePoint == 0x9) ||
                    (codePoint == 0xA) ||
                    (codePoint == 0xD) ||
                    ((codePoint >= 0x20) && (codePoint <= 0xD7FF)) ||
                    ((codePoint >= 0xE000) && (codePoint <= 0xFFFD)) ||
                    ((codePoint >= 0x10000) && (codePoint <= 0x10FFFF));
        }

        /**
         * 过滤emoji 或者 其他非文字类型的字符
         * @param source
         * @return
         */
        public static String filterEmoji(String source)
        {

            if (!containsEmoji(source))
            {
                return source;//如果不包含，直接返回
            }
            //到这里铁定包含
            StringBuilder buf = null;

            int len = source.Length;

            for (int i = 0; i < len; i++)
            {
                char codePoint = source[i];

                if (isEmojiCharacter(codePoint))
                {
                    if (buf == null)
                    {
                        buf = new StringBuilder(source.Length);
                    }

                    buf.Append(codePoint);
                }
                else
                {
                }
            }

            if (buf == null)
            {
                return source;//如果没有找到 emoji表情，则返回源字符串
            }
            else
            {
                if (buf.Length== len)
                {//这里的意义在于尽可能少的toString，因为会重新生成字符串
                    buf = null;
                    return source;
                }
                else
                {
                    return buf.ToString();
                }
            }

        }

    }


}
