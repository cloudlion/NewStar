using UnityEngine;
using System.Collections;
using System;
#if MVC
using MVC;
#endif
//using Item;
using GameUtil;

namespace Constant
{
    public static class Path
    {
        public static string MapFilePath = "map/";
        public static string TileMeshPrefabPath = "Art/Map_test/Prefab/";
        public static string BuildingPrefabPath = "Assets/BaseAssets/streaming/UI/building/";
        public static string MarchPrefabPath = "Assets/BaseAssets/streaming/UI/march/";

        public static string MenuPrefabPath = "Assets/BaseAssets/streaming/UI/";
        //public static string ActiveObjectPrefabPath = "Art/Map_test/Prefab/";
        //public static string ActiveObjectSpritePath = "BaseAssets/streaming/world_map/";

        public static string TerrainMeshesPath = "Assets/BaseAssets/streaming/world_map/Terrain/";
        public static string CoverMeshesPath = "Assets/BaseAssets/streaming/world_map/Cover/";

        public static string MapAtlasPath = "Assets/BaseAssets/streaming/world_map/MapAtlas/";
        public static string TileResourcesPath = "Assets/BaseAssets/streaming/world_map/TileResources/";
        public static string ActiveObjectSpritePath = "Assets/BaseAssets/streaming/world_map/ActiveObject/";
        public static string WorldMapUIPath = "Assets/BaseAssets/streaming/world_map/WorldMapUI/";


    }
    public static class CloudLock
    {
        public static string MiniMapPageLock = "MiniMapPageLock";
        public static string RealmPageLock = "RealmPageLock";
        public static string LandPageMapLock = "LandPageMapLock";
        public static string LevelChagendLock = "LevelChagendLock";
        public static string WorldMapLock = "WorldMapLock";
        public static string FromMiniMapToWorldMapLock = "FromMiniMapToWorldMapLock";
        public static string CityMapLock = "CityMapLock";
		public static string WaitCityDataLock = "WaitCityDataLock";
        public static string InstanceMapChangedLock = "InstanceMapChangedLock";
    }

	public static class EquipmentType
	{
		public const int WEAPON = 1;
		public const int CAP = 2;
		public const int CLOTHES = 3;
		public const int SHOES = 4;
	}

	public static class EquipmentQuality
	{
		public const int NORMAL = 1;
		public const int UNUSUAL = 2;
		public const int EXCELLENT = 3;
		public const int EPIC = 4;
		public const int LEGEND = 5;
	}

	public static class ItemCategory
	{
		public const int MATERIALS = 6;
		public const int PROP = 7;
	}

    public static class InGameGuideType
	{
		private const int BUFF_TOWER = 14;
		public static string FORMATION_INGAME = "formation_ingame";
		public static string SHAREBUFF_INGAME = "sharebuff_ingame";

		public static int[] inGameGuideArrays = new int[]{ BUFF_TOWER };
	}

    public static class Notification
    {
        public static class ProgressEvent
        {
            public static string BuildFinishEvent = "BuildFinish";
            public static string BuildUpdateEvent = "BuildUpdate";
            public static string BuildStartEvent = "BuildStartEvent";

            public static string TechFinishEvent = "TechFinish";
            public static string TechUpdateEvent = "TechUpdate";
            public static string TechStartEvent = "TechStartEvent";
			public static string TechFinishInstantEvent = "TechFinishInstantEvent";


            public static string TrainingFinishEvent = "TrainingFinish";
            public static string TrainingUpdateEvent = "TrainingUpdate";
            public static string TrainingStartEvent = "TrainingStartEvent";


            public static string RecruitFinishEvent = "RecruitFinish";
			public static string RecruitUpdateEvent = "RecruitUpdate";
            public static string RecruitStartEvent = "RecruitStartEvent";


            public static string HealFinishEvent = "HealFinish";
			public static string HealUpdateEvent = "HealUpdate";
            public static string HealStartEvent = "HealStartEvent";


            public static string MarchFinishEvent = "MarchFinish";
            public static string MarchUpdateEvent = "MarchUpdate";
            public static string MarchStartEvent = "MarchStartEvent";


            public static string PetMarchFinishEvent = "PetMarchFinish";
            public static string PetMarchUpdateEvent = "PetMarchUpdate";
            public static string PetMarchStartEvent = "PetMarchStartEvent";


            public static string WallRepaireFinishEvent = "WallRepaireFinishEvent";
			public static string WallRepaireUpdateEvent = "WallRepaireUpdateEvent";
            public static string WallRepaireStartEvent = "WallRepaireStartEvent";

			public static string BlackSmithFinishEvent = "BlackSmithFinishEvent";
			public static string BlackSmithUpdateEvent = "BlackSmithUpdateEvent";
			public static string BlackSmithStartEvent = "BlackSmithStartEvent";


            public static string EnergyFinishEvent = "EnergyFinish";
            public static string EnergyUpdateEvent = "EnergyUpdate";
            public static string EnergyStartEvent = "EnergyStartEvent";


            public static string BuildCancelEvent = "BuildCancelEvent";
            public static string TechCancelEvent = "TechCancelEvent";



            public static string TrainCancelEvent = "TrainCancelEvent";
            public static string MarchCancelEvent = "MarchCancelEvent";
            public static string PetMarchCancelEvent = "PetMarchCancelEvent";
			public static string RecruitCancelEvent = "RecruitCancelEvent";
			public static string HealCancelEvent = "HealCancelEvent";

            public static string SpeedUpEvent = "SpeedUpEvent";
            public static string InstantFinishEvent = "InstantFinishEvent";

            public static string ScoutSpendTimeUpdateEvent = "ScoutSpendTimeUpdateEvent";


            
        }
        public class Gesture
        {
            public static string WorldMapClickEvent = "WorldMapClickEvent";
            public static string WorldMapPetAttackDragEvent = "WorldMapPetAttackDragEvent";
            public static string WorldMapPetAttackDragStartEvent = "WorldMapPetAttackDragStartEvent";
            public static string WorldMapPetAttackDragEndEvent = "WorldMapPetAttackDragEndEvent";

            public static string DragEvent = "DragEvent";
            public static string DragStartEvent = "DragStartEvent";
            public static string DragEndEvent = "DragEndEvent";
            public static string ClickShaBaKeEvent = "ClickShaBaKeEvent";


            public static string WorldMapClickWorldMapModeEvent = "WorldMapClickWorldMapModeEvent";
            public static string WorldMapClickPetAttackModeEvent = "WorldMapClickPetAttackModeEvent";
            public static string WorldMapClickWorldMapModePlainEvent = "WorldMapClickWorldMapModePlainEvent";
            public static string WorldMapClickWorldMapModeShaBaKeEvent = "WorldMapClickWorldMapModeShaBaKeEvent";
            public static string WorldMapClickWorldMapModePlainWithObjectEvent = "WorldMapClickWorldMapModePlainWithObjectEvent";

            public static string WorldMapClickWorldMapModePlayerEvent = "WorldMapClickWorldMapModePlayerEvent";
            public static string WorldMapClickWorldMapModeBossEvent = "WorldMapClickWorldMapModeBossEvent";
            public static string WorldMapClickWorldMapModeShaBaKeBossEvent = "WorldMapClickWorldMapModeShaBaKeBossEvent";
            public static string WorldMapClickWorldMapModeResEvent = "WorldMapClickWorldMapModeResEvent";
            public static string WorldMapClickWorldMapModeBuffEvent = "WorldMapClickWorldMapModeBuffEvent";
            public static string WorldMapClickWorldMapModeMonsterEvent = "WorldMapClickWorldMapModeMonsterEvent";
            public static string WorldMapClickWorldMapModeTourMonsterEvent = "WorldMapClickWorldMapModeTourMonsterEvent";

            public static string MiniMapClickEvent = "MiniMapClickEvent";

        }
        public class MainProcedure
        {
            public static string CityLoadedEvent = "CityLoadedEvent";
            public static string StartGamePlayEvent = "StartGamePlayEvent";
        }
        public static class PetAttack
        {
            public static string AddPetAttackPositionEvent = "SetPetAttackPositionsEvent";
            public static string ErasePetAttackPositionEvent = "ErasePetAttackPositionEvent";
            public static string SyncPetAttackPositionsEvent = "SyncPetAttackPositionsEvent";
            public static string SetPetAttackMainStatusEvent = "SetPetAttackMainStatusEvent";
            //public static string AddBuffEffectEvent = "AddBuffEffectEvent";
        }
        public static class March
        {
            public static string DrawMarchLineEvent = "DrawMarchLineEvent";
            public static string EraseMarchLineEvent = "EraseMarchLineEvent";

            public static string CreatePetModelEvent = "CreatePetModelEvent";
            public static string ErasePetModelEvent = "ErasePetModelEvent";
            public static string PlayPetModelActionEvent = "PlayPetModelActionEvent";

            public static string CreateArmyModelEvent = "CreateArmyModelEvent";
            public static string EraseArmyModelEvent = "EraseArmyModelEvent";
            public static string PlayArmyModelEvent = "EraseArmyModelEvent";

            public static string ModelAnimationFinishEvent = "ModelAnimationFinishEvent";

            public static string StartFocusToMarchEvent = "FocusToMarchEvent";
            public static string FinishFocusToMarchEvent = "FinishFocusToMarchEvent";

            public static string ShowMarchUIEvent = "ShowMarchUIEvent";
            public static string CloseMarchUIEvent = "CloseMarchUIEvent";

            public static string ShowMarchBarSelectedEvent = "ShowMarchBarSelectedEvent";
            public static string ClearMarchBarSelectedEvent = "ClearMarchBarSelectedEvent";

            public static string SyncMarchEvent = "SyncMarchEvent";
            public static string KnightStatusChanged = "KnightStatusChanged"; 
            
        }
        public static class WorldMap
        {
            public static string FireTheCityEvent = "FireTheCityEvent";
            public static string CeaseFireTheCityEvent = "CeaseFireTheCityEvent";
            public static string TileRemovedEvent = "TileRemovedEvent";
            public static string CityDisappearEffectEvent = "CityDisappearEffectEvent";
            //public static string ModelEffectFinishEvent = "ModelEffectFinishEvent";
            public static string ProtectCityEvent = "ProtectCity";
            public static string DisplayActiveObjectEvent = "DisplayActiveObjectEvent";

            public static string RemoveTileEffectEvent = "RemoveTileEffectEvent";
            public static string RemoveTileBuffEvent = "RemoveTileBuffEvent";

            public static string AddTileEffectEvent = "AddTileEffectEvent";
            public static string AddTileBuffEvent = "AddTileBuffEvent";

            public static string KickTileEvent = "KickTileEvent";
            public static string OpenCityShareMenuEvent = "OpenCityShareMenuEvent";
            public static string PopCityShareMenuEvent = "PopCityShareMenuEvent";

            public static string WorldLoadedEvent = "WorldLoadedEvent";
            public static string BuffConnectionLineEvent = "BuffConnectionLineEvnet";
            public static string RemoveBuffConnectionLineEvent = "RemoveBuffConnectionLineEvent";

            public static string RefreshActiveObjectEvent = "RefreshActiveObjectEvent";
            public static string MoveSelfCityEvent = "MoveSelfCityEvent";

            public static string ForceRefreshEvent = "ForceRefreshEvent";
            public static string ForceRefreshTileEvent = "ForceRefreshTileEvent";
            public static string IsBlockInQueueEvent = "IsBlockInQueueEvent";
            public static string ClearWorldEvent = "ClearWorldEvent";
            public static string ClearWorldMapLevelEvent = "ClearWorldMapLevelEvent";
            public static string MarchReturnHomeMessageEvent = "MarchReturnHomeMessageEvent";
			public static string MarchReturnMessageEvent = "MarchReturnMessageEvent";
        }
#if MVC
        public class NotifyEvent : GameEngine.Event
        {
            public const string UI_Notify_Event = "UINotifyEvent";//

            public const int Type_Quest_Finished = 1;
            public const int Type_Refresh_Recommand_Quest = 2;

            public const int Status_Reach = 1;
            public const int Status_Nope = 2;

            public int type = Type_Quest_Finished;
            public int status = Status_Nope;

            public NotifyEvent(string name, int type, int status) : base(name)
            {
                this.type = type;
                this.status = status;
            }
        }


        public class ServerNotifyEvent : GameEngine.Event
        {
            public const string Server_Notify_Event = "Server_Notify_Event";
            public object o;

            public int type = 0; 

			public float showTimeClose;

			public ServerNotifyEvent(string name,int type,object o,float showTimeClose) : base(name)
            {
                this.o = o;
                this.type = type;
				this.showTimeClose = showTimeClose;
            }
        }
#endif
        public class ServerMessageConstant
        {
            public const int KickTile = 1;
        }

        public class TileInfoEvent
        {
            public static string RefreshResourceTileEvent = "RefreshResourceTileEvent";
            public static string RefreshTreasureTileEvent = "RefreshTreasureTileEvent";
            public static string RefreshBuffTileEvent = "RefreshBuffTileEvent";
            public static string RefreshShaBaKeEvent = "RefreshShaBaKeEvent";
            public static string RefreshShaBaKeBossEvent = "RefreshShaBaKeBossEvent";
            public static string RefreshBossEvent = "RefreshBossEvent";
            public static string RefreshBossHeadEvent = "RefreshBossHeadEvent";
            public static string RefreshPlayerEvent = "RefreshPlayerEvent";
            public static string RefreshPetHeadBossEvent = "RefreshPetHeadBossEvent";
            public static string RefreshPetBossEvent = "RefreshPetBossEvent";
            public static string RefreshTourBossEvent = "RefreshTourBossEvent";


        }

        public class StateMachine
        {
            public static string FloatDataChangedEvent = "FloatDataChangedEvent";
            public static string IntDataChangedEvent = "IntDataChangedEvent";
            public static string BoolDataChangedEvent = "BoolDataChangedEvent";
            public static string FinishStateMachineEvent = "FinishStateMachineEvent";
        }

        public class Instance
        {
            public static string TeamListEvent = "TeamListEvent";
            public static string TeamInfoEvent = "TeamInfoEvent";
            public static string UserInfoEvent = "UserInfoEvent";
            public static string ChoiceEvent = "ChoiceEvent";
            public static string StartSceneEvent = "StartSceneEvent";
            public static string EndSceneEvent = "EndSceneEvent";
            public static string TileInfoEvent = "TileInfoEvent";
            public static string ChoiceListEvent = "ChoiceListEvent";

            public static string EnterWorldMapEvent = "EnterWorldMapEvent";
            public static string EnterInstanceEvent = "EnterInstanceEvent";
            public static string InstancesListEvent = "InstancesListEvent";
            public static string ILeftTeamEvent = "ILeftTeamEvent";

        }

        public static string GDSOKEvent = "gdsok";
        public static string WorldEvent = "WorldEvent";
        public static string ItemsChangeEvent = "ItemsChangeEvent";
        public static string UnlockNewKnightEvent = "UnLockNewKnightEvent";

        public static string EnergyChangedEvent = "EnergyChangedEvent";
        public static string BuildingLevelupEvent = "BuildingLevelupEvent";

        public static string SoulCollectEvent = "SoulCollectEvent";
        public static string SkillLevelupEvent = "SkillLevelupEvent";


        public static string WorldMapCenterChangedEvent = "WorldMapCenterChangedEvent";
        public static string RealmChangedEvent = "RealmChangedEvent";

        public static string DrawBuffTile = "DrawBuffTile";
        public static string EraseBuffTile = "EraseBuffTile";

        public static string ConfirmDialogEvent = "ConfirmDialogEvent";

        public static string ActiveObjectFlashEvent = "ActiveObjectFlashEvent";
        public static string TerrainFlashEvent = "TerrainFlashEvent";

        public static string ExcuteTileGameObjectEvent = "ExcuteTileGameObjectEvent";

        public static string MediatorOpenEvent = "MediatorOpenEvent";
        public static string MediatorPopEvent = "MediatorPopEvent";

        public static string DrawRedRectTerrainEvent = "DrawRedRectTerrainEvent";
        public static string EraseRedRectTerrainEvent = "EraseRedRectTerrainEvent";

        public static string ChangeWorldMapModeEvent = "ChangeWorldMapModeEvent";
        public static string FocusToTileEvent = "FocusToTileEvent";
        public static string FocusToMiniMapWorldTileEvent = "FocusToMiniMapWorldTileEvent";
        public static string FocusToMiniMapTileEvent = "FocusToMiniMapTileEvent";

        public static string MOVE_SCREEN = "move_screen";
        public static string WorldMapMoveEvent = "WorldMapMoveEvent";

        public static string UpdatePetAttackUIEvent = "UpdatePetAttackUIEvent";
        public static string ShowSelectedCityEffectEvent = "ShowSelectedCityEffectEvent";
        public static string HideSelectedCityEffectEvent = "HideSelectedCityEffectEvent";
        public static string HideAllMenuEvent = "HideAllMenuEvent";
        public static string HideAllTileMenuEvent = "HideAllTileMenuEvent";
        public static string DiamondChangedEvent = "DiamondChangedEvent";
        public static string ChangeActiveObjectTileEvent = "ChangeActiveObjectTileEvent";

        public static string ErrorBoxEvent = "ErrorBoxEvent";
        public static string MessageBoxEvent = "MessageBoxEvent";
        public static string BetTimeResult = "BetTimeResult";
        public static string SetKingEvent = "SetKingEvent";
        public static string SetBuffEvent = "SetBuffEvent";

        public static string ChangeMapLevelEvent = "ChangeMapLevelEvent";
        public static string WatchTowerNoticeEvent = "WatchTowerNoticeEvent";
        public static string WatchTowerListEvent = "WatchTowerListEvent";

        public static string MarchEffectEvent = "MarchEffectEvent";
        public static string EffectEvent = "EffectEvent";
        public static string AreaEffectEvent = "AreaEffectEvent";
        public static string EraseEffectEvent = "EraseEffectEvent";
        public static string EraseAreaEffectEvent = "EraseAreaEffectEvent";
        public static string ShakeCameraEvent = "ShakeCameraEvent";

        public static string TranversePetGameObjectEvent = "TranversePetGameObjectEvent";

        //public static string GeneralShopBuyLimitRefresh = "GeneralShopBuyLimitRefresh";
        public static string AllianceShopBuyLimitRefresh = "AllianceShopBuyLimitRefresh";
        public static string AllianceShopRefresh = "AllianceShopRefresh";
        public static string AllianceStatusRefresh = "AllianceStatusRefresh";

        public static string UpdateKnightExperenceEvent = "UpdateKnightExperenceEvent";
        public static string UpdatePetExperenceEvent = "UpdateKnightExperenceEvent";

        public static string FTEEvent = "FTEEvent";
        public static string SendTileInfoEvent = "SendTileInfoEvent";
        public static string SelfCityArrived = "SelfCityArrived";
        public static string ShareTileEvent = "ShareTileEvent";

		public static string SHOWHELPPOP = "show_help_pop";

        public static string UpdateResource = "UpdateResource";
        public static string FTEReadyEvent = "FTEReadyEvent";
        public static string RefreshUserInfoEvent = "RefreshUserInfoEvent";

        public static string RefreshNewDayComeEvent = "RefreshNewDayComeEvent";
		public static string TranverseTerrainEvent = "TranverseTerrain";

        public static string NewDayEvent = "NewDayEvent";
        public static string RefreshOnlineTimeEvent = "RefreshOnlineTimeEvent";

        public static string ActiveObjectInVisibleEvent = "ActiveObjectInVisibleEvent";

        public static string RefreshItemsEvent = "RefreshItemsEvent";
        public static string ArmyControllerLoadFinishEvent = "ArmyControllerLoadFinishEvent";
        public static string RealmFocusHomeEvent = "RealmFocusHomeEvent";
        public static string RecommendEvent = "RecommendEvent";

        public static string LockCloudEvent = "LockCloudEvent";
        public static string UnLockCloudEvent = "UnLockCloudEvent";
        public static string UnActiveCloudEvent = "UnActiveCloudEvent";
        public static string CloudHoldingEvent = "CloudHoldingEvent";
        public static string AfterCloudHoldingEvent = "AfterCloudHoldingEvent";

        public static string ShowErrorTipsEvent = "ShowErrorTipsEvent";

        public static string LoginTipsEvent = "LoginTipsEvent";
        public static string LoadingPercentEvent = "LoadingPercentEvent";
        public static string OpenMenuEvent = "OpenMenuEvent";

        public static string TournamentEventChagnedEvent = "TournamentEventChagnedEvent";
        public static string ClickAllianceHelpEvent = "ClickAllianceHelpEvent";
    }
    public static class KingsLand
    {
        public static int PalaceTop = 148;
        public static int PalaceLeft = 148;
        public static int PalaceBottom = 151;
        public static int PalaceRight = 151;
        public static int PalaceWidth = 4;
        public static int PalaceHeight = 4;

        public static int LandTop = 140;
        public static int LandLeft = 140;
        public static int LandBottom = 159;
        public static int LandRight = 159;

        public static int PageSize = 5;
        public static int MaxRealmBuffNum = 200;
        public static int DataDespairTime = 360;

        public static float BetTimeMediatorRefreshInterval = 5f;
        public static float AllianceListRefreshInterval = 5f;

    }
    public static class March
    {
        public static float MarchLineInterval = 36f;
        public static float MarchLineThick = 3f;
        public static float MarchSpeedLow = 1f;
        public static float MarchSpeedHigh = 5f;

        public static float PetYPosition = 3f;
        public static float ArmyYPosition = 3f;

        public static float ArmyAttackTime = 1f;
        public static float PetAttackTime = 0.8f;
    }
    public static class WorldMiniMap
    {
        public static Vector3 LeftTopPoint = new Vector3(0, 0, 0);
        public static int TilePixelWidth = 256;
        public static int TilePixelHeight = 128;
        public static float TileWidth = 2560f;
        public static float TileHeight = 1280f;

    }
    public static class WorldMapTerrain
    {

        public static Vector3 WorldMapLeftTopPoint = new Vector3(0, 0, 0);
        public static int TilePixelWidth = 256;
        public static int TilePixelHeight = 128;
        public static float WorldMapTileWidth = 25.6f;
        public static float WorldMapTileHeight = 12.8f;
        public static float TileReloadTime = 100f;
        public static int TerrainCapacity = 6;

        public static int TerrainUnitDataBits = 8;

        public static int TerrainBlockWidth = 5;
        public static int TerrainBlockHeight = 5;

        public static int BufferCircles = 3;
        public static int MemeryBuffTiles = (BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * 1;
        public static int MaxDidplayedTile = (BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * TerrainBlockWidth * TerrainBlockHeight;

        public static float MinScaleFactor = 0.76f;
        public static float MaxScaleFactor = 1.5f;

        public static float LineYPosition = 1f;
        public static float TerrainPointYPosition = 1.2f;
        public static float PointYPosition = 1.5f;
        public static float MainPointYPosition = 2f;
        public static float MainPointYStatusPosition = 10f;
        public static float LineThick = 0.5f;

        public static int SendViewPortInterval = 3;

        public static int EdgeLevel = 1;
        public static int EdgeMapID = 301;

        public static int MaxPower = 1000;
        public static int PowerPerBlock = 20;
        public static int PowerPerTile = 1;
        public static int PowerPerCreateBlock = 50;
    }
    public static class WorldMapCover
    {
        public static int CoverWidth = 10;
        public static int CoverHeight = 10;
        public static int BufferCircles = 3;
        public static int MemeryBuffTiles = (BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * 1;// * Constant.WorldMapCover.CoverWidth * Constant.WorldMapCover.CoverHeight;
        public static int MaxDidplayedTile = 200;//(BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * TerrainBlockWidth * TerrainBlockHeight + TerrainBlockWidth * TerrainBlockHeight;

        public static int MaxPower = 400;
        public static int PowerPerBlock = 20;
        public static int PowerPerTile = 1;
        public static int PowerPerCreateBlock = 50;

    }
    public static class WorldMap
    {
        public static int WorldMapWidth = 300;
        public static int WorldMapHeight = 300;

        public static string FireOnCity = "FireOnCity";
		public static string BuffOnCity = "BuffOnCity";
        public static string CityDisappear = "CityDisapear";
        public static string TileOnCity = "TileOnCity";
		public static string BuffTile = "BuffTile";
        public static string MarchEffect = "MarchEffect";
        public static string Effect = "Effect";
        public static string KickTile = "KickTile";
        public static string ShowCity = "ShowCity";
        public static string ShowCityBefore = "ShowCityBefore";
        public static string ChatTransferInspire = "ChatTransferInspire";

        public static string CommonBossConnection = "CommonBossConnection";
        public static string MassBossConnection = "MassBossConnection";
    }
    public static class WorldMapActiveObject
    {
        public static int BufferCircles = 2;

        public static int BlockWidth = 5;
        public static int BlockHeight = 5;

        public static int BufferNumbersOfServerBlock = 20;
        public static int ServerBlockWidth = 5;
        public static int ServerBlockHeight = ServerBlockWidth;

        public static int MemeryBuffTiles = (BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * BufferNumbersOfServerBlock;
        public static int MaxDidplayedTile = 150;//(BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * BlockWidth * BlockHeight;
        public static int MaxDidplayedPlayerTile = 120;
        public static int MaxDidplayedBossTile = 50;
        public static int MaxDidplayedResourceTile = 40;
        public static int MaxDidplayedBuffTile = 8;
        public static int MaxDidplayedPetTile = 9;
        public static int MaxDidplayedTourTile = 24;
        public static int MaxDidplayedShaBaKeTile = 1;

        public static int MaxPower = 10000;
        public static int PowerPerBlock = 20;
        public static int PowerPerTile = 1;
        public static int PowerPerCreateBlock = 50;

        public static int FreshViewportWidth = 10;
        public static int FreshViewportHeight = 10;

        public static int ActiveObjectExpireTime = 10;
    }
    public static class WorldMapRealm
    {
        public static Vector3 LeftTopPoint = new Vector3(0, 0, 0);
        public static int TilePixelWidth = 512;
        public static int TilePixelHeight = 512;
        public static float TileWidth = 512f;
        public static float TileHeight = 512f;
    }
    public static class WorldMapLand
    {
        public static Vector3 LeftTopPoint = new Vector3(0, 0, 0);
        public static int TilePixelWidth = 512;
        public static int TilePixelHeight = 512;
        public static float TileWidth = 512f;
        public static float TileHeight = 512f;
    }

    public static class WorldMapBuffTower
    {
        public static int BufferCircles = 0;
        public static int BufferNumbersOfServerBlock = 30;

        public static int BlockWidth = 1;
        public static int BlockHeight = 1;
        public static int MemeryBuffTiles = 100;//(BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * BlockWidth * BlockHeight * BufferNumbersOfServerBlock;
        public static int MaxPower = 1000;
    }
    public static class WorldMapMask
    {
        public static int BufferCircles = 3;
        public static int BufferNumbersOfServerBlock = 30;

        public static int BlockWidth = 5;
        public static int BlockHeight = 5;
        public static int MemeryBuffTiles = (BufferCircles * 2 + 1) * (BufferCircles * 2 + 1)  * BufferNumbersOfServerBlock;
        public static int MaxDidplayedTile = (BufferCircles * 2 + 1) * (BufferCircles * 2 + 1) * 2;

    }

    public static class TerrainEditor
    {
        public static int ShowCoordinateWidth = 20;
        public static int ShowCoordinateHeight = 20;
        public static string TemplateFilePath = "map/template/";
        public static int TemplateLevel = 100;
    }
    public static class Build
    {
        public static bool IsFakeData = false;
    }
    public static class Item
    {
        public static int DefaultBuyLimit = 100;
    }
    public static class Instance
    {
        public static int MarchBaseIndex = 100;
        public static int TeamPageNumPerPage = 10;

		public static string ScoreKeyword = "UI_fubenhuobi";
        public static string DonateKeyword = "UI_MyBadages";

        public static int Prepared = 1;
        public static int Unprepared = 2;

        public static float TeamListRefreshTime = 10f;

        public static int FTEInstanceType = 1;
    }

    public static class Builder
    {
        public static string BuilderName = "builder";
        public static float ProgressBarRefreshTimeInterval = 1f;
        public static string ProgressBarName = "BuildingProgressBar";
        public static string MarchProgressBarName = "MarchProgressBar";

    }
    public static class Knight
    {
        public static int KnightsTotalNumber = 5;
    }
    public static class Factor
    {
        public static int GDSTimeFactor = 100;
        public static int MinTime = 10;
        public static Vector3 PetAttackMainStatusOffset = new Vector3(0f,50f,0f);
        public static int ChatTransferInspireTime = 5;

        public static int ShaBakeTimerInterVal = 100;
    }
    public static class Name
    {
        public static string PetAttackPositionObjectName = "PetAttackPositionObjectName";
        public static string PetAttackMainPositionObjectName = "PetAttackMainPositionObjectName";
        public static string PetAttackMainPositionTerrainObjectName = "PetAttackMainPositionTerrain";
        public static string PetAttackPositionTerrainObjectName = "PetAttackPositionTerrainObjectName";
        public static string PetAttackMainStatusActiveObjectName = "PetAttackMainStatusActiveObjectName";
        public static string ProtectionShell = "ProtectionShell";
        public static string PetAttackRangeTileName = "PetAttackRangeTileName";

        public static string PetAttackMarchTargetPositionName = "PetAttackMarchTargetPositionName";

    }
    public class Chat
    {
        public static string GlobalKey = "General.Global";
        public static string RealmKey = "Chat.Land";
        //{
        //    get
        //    {
        //        if (ProxyMgr.Instance == null) return "";
        //        return ProxyMgr.Instance.GetProxy<ChatProxy>().getRealmTabText();
        //    }
        //}
        public static string AllianceKey = "General.Clan";
        public static string PrivateKey = "Chat.Private";
        public static string AllKey = "General.All";
        public static string[] Keys = new string[] { GlobalKey, RealmKey, AllianceKey, AllKey };
        public static int GetIndex(string key)
        {
            string realm = RealmKey;
            for (int i = 0; i < Keys.Length; i++)
            {
                if (key == GameUtil.DI18n.T(Keys[i]))
                    return i;
                else if (key == realm)
                    return 1;
            }
            return -1;
        }
    }


    public static class FTE
    {
        public static int WorldMapLevel = 0;
        public static int MapWidth = 20;
        public static int MapHeight = 20;
        public static int MapID = 1;
        public static int MaxMapID = 9;
        public static int MapTag = 0;

        public static int MarchID = 1;

        public static int EnermyUserID = 2;
        public static int FriendUserID = 2;
        public static int MarchLineUserID = 2;

    }

    public static class QUEST
    {
        public enum QUEST_SCOPE
        {
            none = 0,
            build = 1,
            troop = 2,
            pet = 3,
            prop = 4,
            tech = 5,
            alliance = 6,
            resource = 7,
            speedup = 8,
            fight = 9,
            teleport = 10,
            glory =11,
			bindAccount = 14,
            allgear = 15,
            gear_lv= 16,
			count,
            gear_rare = 17,
        }

        public static int getQuestScopeNum()
        {
			return (int)QUEST_SCOPE.count;
        }
    }

    public static class SOUND
    {
        //public static string Wav_Scout = "wav_sendmarch";
        //public static string Wav_Scout_Fail = "wav_scout_fail";
        //public static string Wav_Scout_Success = "wav_marchvictory";

        //public static string Wav_March_Start = "wav_sendmarch";
        //public static string Wav_March_Succuess = "wav_marchvictory";

        //public static string Wav_Pet_Summon = "wav_petsummon";
        //public static string Wav_Pet_Minotaur = "wav_minotaur";

        public enum MarchSoundName
        {
            wav_sendmarch = 0,
            wav_scout_fail = 1,
            wav_marchvictory = 2,
            wav_petsummon = 3,
            wav_minotaur = 4,
            wav_gold = 5,
            wav_wood = 6,
            wav_stone = 7,
            wav_ore = 8,
            wav_start_building = 9,
            wav_finishbuilding = 10,
            wav_techstart = 11,
            wav_techfinish = 12,
            wav_open_barrack = 13,
            wav_open_alchemy = 14,
            wav_open_castle  = 15,
            wav_open_cottage  = 16,
            wav_open_hospital = 17,
            wav_open_knightshall =18,
            wav_open_ore = 19,
            wav_open_quarry = 20,
            wav_open_sawmill = 21,
            wav_open_storehouse = 22,
            wav_open_watchtower = 23,
            wav_open_prison = 24,
            wav_open_petsummoner = 25,
            wav_open_clan= 26,
            wav_open_miracle = 27,
            wav_openpage1 = 28,
            wav_success = 29,
        }

		public enum BlackSmith
		{ 
			wav_open_blacksmith = 0,
			wav_forge = 1,
			wav_forgefinish = 2,
			wav_gearget = 3,
		}

        public static string wav_tavern = "wav_tavern";

        //public static string  Wav_Gold = "wav_gold";
        //public static string Wav_Wood = "wav_wood";
        //public static string Wav_Stone = "wav_stone";
        //public static string Wav_Ore = "wav_ore";

        //public static string Wav_Building_Start = "wav_start_building";
        //public static string Wav_Building_Finish = "wav_finishbuilding";

        //public static string Wav_Tech_Start = "wav_techstart";
        //public static string Wav_Tech_Finish = "wav_techfinish";

        //public static string Wav_Open_Barrack = "wav_click_barrack";//
        //public static string Wav_Open_Alchemy = "wav_open_alchemy";//
        //public static string Wav_Open_Castle = "wav_open_castle";//
        //public static string Wav_Open_Cottage = "wav_open_cottage";//
        //public static string Wav_Open_Hospital = "wav_open_hospital";//
        //public static string Wav_Open_Knightshall = "wav_open_knightshall";//
        //public static string Wav_Open_Ore = "wav_open_ore";//
        //public static string Wav_Open_Quarry = "wav_open_quarry";//
        //public static string Wav_Open_Sawmill = "wav_open_sawmill";//
        //public static string Wav_Open_Storehouse = "wav_open_storehouse";//
        //public static string Wav_Open_Watchtower = "wav_open_watchtower";//
        //public static string Wav_Open_Prison = "wav_open_prison";//
        //public static string Wav_Open_Petsummoner = "wav_open_petsummoner";//
        //public static string Wav_Open_Clan = "wav_open_clan";//
        //public static string Wav_Open_Miracle = "wav_open_miracle";//
        

        //public static string Wav_Open_UI_Default = "wav_openpage1";

        //public static string Wav_Success = "wav_success";
    }
#if MVC
    public class PlaySoundEvent : GameEngine.Event
    {
        public const string PLAY_MARCH_SOUND = "PLAY_MARCH_SOUND";
        public const string PLAY_OTHER_SOUND = "PLAY_OTHER_SOUND";

        public const string SND_PLAY = "SndPlay";
        public const string SND_LOOP = "SndLoop";
        public const string SND_PAUSE = "SndPause";

        public enum SoundPlayMode
        {
            SND_PLAY = 0,
            SND_LOOP = 1,
            SND_PAUSE = 2
        }

        public SoundPlayMode mode;
        public SOUND.MarchSoundName soundName;
        public string otherSoundName;

        public PlaySoundEvent(string name, SOUND.MarchSoundName soundName, SoundPlayMode mode) : base(name)
        {
            this.soundName = soundName;
            this.mode = mode;
        }

        public PlaySoundEvent(string name, string otherSoundName, SoundPlayMode mode = SoundPlayMode.SND_PLAY) : base(name)
        {
            this.otherSoundName = otherSoundName;
            this.mode = mode;
        }
    }
#endif
    


}
