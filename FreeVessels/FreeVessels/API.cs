using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eleon.Modding;

namespace FreeVessel
{
    class API
    {
        public static void Alert(int Target, string Message, string Alert, float Time)
        {
            byte prio = 2;
            if (Alert == "red")
            {
                prio = 0;
            }
            else if (Alert == "yellow")
            {
                prio = 1;
            }
            else
            {
                prio = 2;
            }

            if (Target == 0)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_InGameMessage_AllPlayers, (ushort)Storage.CurrentSeqNr, new IdMsgPrio(Target, Message, prio, Time));
            }
            else if (Target < 999)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_InGameMessage_Faction, (ushort)Storage.CurrentSeqNr, new IdMsgPrio(Target, Message, prio, Time));
            }
            else if (Target > 999)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_InGameMessage_SinglePlayer, (ushort)Storage.CurrentSeqNr, new IdMsgPrio(Target, Message, prio, Time));
            }
        }

        public static int PlayerInfo(int playerID)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Player_Info, (ushort)Storage.CurrentSeqNr, new Id(playerID));
            return Storage.CurrentSeqNr;
        }

        public static int TextWindowOpen(string TargetPlayer, string Message, String ConfirmText, String CancelText)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            if (CancelText == null)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_ShowDialog_SinglePlayer, (ushort)Storage.CurrentSeqNr, new DialogBoxData()
                {
                    Id = Convert.ToInt32(TargetPlayer),
                    MsgText = Message,
                    NegButtonText = "Close"
                });
            }
            else
            {
                Storage.GameAPI.Game_Request(CmdId.Request_ShowDialog_SinglePlayer, (ushort)Storage.CurrentSeqNr, new DialogBoxData()
                {
                    Id = Convert.ToInt32(TargetPlayer),
                    MsgText = Message,
                    NegButtonText = "Close",
                    PosButtonText = "Save Waypoints"
                });
            }
            return Storage.CurrentSeqNr;
        }

        public static int Gents(string playfield)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_GlobalStructure_Update, (ushort)Storage.CurrentSeqNr, new PString(playfield));
            return Storage.CurrentSeqNr;
        }

        public static void ConsoleCommand(String Sendable)
        {
            Storage.GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)Storage.CurrentSeqNr, new PString(Sendable));
        }

        public static int Blocks(string Entity)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Structure_BlockStatistics, (ushort)Storage.CurrentSeqNr, new Id(Convert.ToInt32(Entity)));
            return Storage.CurrentSeqNr;
        }

        public static void Destroy(string Entity)
        {
            try
            {
                Storage.GameAPI.Game_Request(CmdId.Request_Entity_Destroy, (ushort)Storage.CurrentSeqNr, new PString(Entity));
            }
            catch { };
            try
            {
                Storage.GameAPI.Game_Request(CmdId.Request_Entity_Destroy2, (ushort)Storage.CurrentSeqNr, new PString(Entity));
            }
            catch { };
        }

        public static int ItemExchange(int Player, string Title, string Body, string Button, ItemStack[] Items)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Player_ItemExchange, (ushort)Storage.CurrentSeqNr, new ItemExchangeInfo(Player, Title, Body, Button, Items));
            return Storage.CurrentSeqNr;
        }

        public static int CreditChange(int PlayerID, int Credits)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Player_Info , (ushort)Storage.CurrentSeqNr, new IdCredits(PlayerID, Credits));
            return Storage.CurrentSeqNr;

        }

        public static void Marker(int Player, string Name, int x, int y, int z, bool Waypoint, int Timer, bool Destroy)
        {
            string command = command = "remoteex cl=" + Player + " \'marker add name=" + Name + " pos=" + x + "," + y + "," + z;
            if (Waypoint)
            {
                command = command + " W";
            }

            if ( Timer > 0 )
            {
                command = command + " expire=" + Timer;
            }
            else if (Destroy)
            {
                command = command + " WD";
            }

            //command = "remoteex cl=" + Player + " \'marker add name=" + Name + " pos=" + x + "," + y + "," + z;
            command = command + "\'";
            API.ConsoleCommand(command);
        }

        public static int EntityID()
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            MyEmpyrionMod.SeqNrStorage[Storage.CurrentSeqNr] = MyEmpyrionMod.DelayNewEntityIdDict[MyEmpyrionMod.CurrentTick];
            Storage.GameAPI.Game_Request(CmdId.Request_NewEntityId , (ushort)Storage.CurrentSeqNr, null);
            return Storage.CurrentSeqNr;
        }

        public static int Spawn(string EntityName, int EntityID, byte FactionGroup, int FactionID, string Playfield, PVector3 Pos, PVector3 Rot, string FileName, string Folder, string exportedEntityDat, byte Type, string TypeName)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Entity_Spawn, (ushort)Storage.CurrentSeqNr, new EntitySpawnInfo()
            {
                name = EntityName,
                forceEntityId = EntityID,
                factionGroup = FactionGroup,
                factionId = FactionID,
                
                playfield = Playfield,
                pos = Pos,
                rot = Rot,

                prefabName = FileName,
                prefabDir = Folder,
                exportedEntityDat = exportedEntityDat,

                type = Type,
                entityTypeName = TypeName,
            }
                );
            return Storage.CurrentSeqNr;
        }

        public static int SpawnforPlayer(string EntityName, int EntityID, int FactionID, string Playfield, PVector3 Pos, PVector3 Rot, string FileName, string EntityType)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            byte entityType = 0;
            //Undef = 0, BA = 2, CV = 3, SV = 4, HV = 5, AstVoxel = 7. use this OR 'entityTypeName'
            if (EntityType == "BA")
            {
                entityType = 2;
            }
            else if (EntityType == "CV")
            {
                entityType = 3;
            }
            else if (EntityType == "SV")
            {
                entityType = 4;
            }
            else if (EntityType == "HV")
            {
                entityType = 5;
            }
            else if (EntityType == "AstVoxel")
            {
                entityType = 7;
            }

            Storage.GameAPI.Game_Request(CmdId.Request_Entity_Spawn, (ushort)Storage.CurrentSeqNr, new EntitySpawnInfo()
            {
                name = EntityName,
                forceEntityId = EntityID,
                factionGroup = Convert.ToByte(1),
                factionId = FactionID,

                playfield = Playfield,
                pos = Pos,
                rot = Rot,

                prefabName = FileName,
                //prefabDir = "K:\\Empyrion\\Content\\Prefabs",

                type = entityType,
                entityTypeName = "type",
            }
                );
            return Storage.CurrentSeqNr;
        }


    }
}
