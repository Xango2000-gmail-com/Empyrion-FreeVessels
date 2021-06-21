using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eleon.Modding;

namespace FreeVessel
{
    class CommonFunctions
    {
        internal static void LogFile(string FileName, string FileData)
        {
            if (!System.IO.File.Exists(MyEmpyrionMod.ModPath + FileName))
            {
                //System.IO.File.Create(MyEmpyrionMod.ModPath + FileName);

                System.IO.File.AppendAllText(MyEmpyrionMod.ModPath + FileName,"");
            }
            string FileData2 = FileData + Environment.NewLine;
            System.IO.File.AppendAllText(MyEmpyrionMod.ModPath + FileName, FileData2);
        }

        internal static void Debug(string Data)
        {
            if (MyEmpyrionMod.debug)
            {
                LogFile("Debug.txt", Data);
            }
        }

        internal static int SeqNrGenerator(int LastSeqNr)
        {
            bool Fail = false;
            int CurrentSeqNr = Storage.CurrentSeqNr;
            do
            {
                if (LastSeqNr > 65530)
                {
                    LastSeqNr = 8000;
                }
                CurrentSeqNr = LastSeqNr + 1;
                if (MyEmpyrionMod.SeqNrStorage.ContainsKey(CurrentSeqNr)) { Fail = true; }
            } while (Fail == true);
            return CurrentSeqNr;
        }

        internal static string ArrayConcatenate(int start, string[] array)
        {
            string message = "";
            for (int i = start; i < array.Length; i++)
            {
                message = message + "\r\n";
                message = message + array[i];
            }
            return message;
        }

        public static List<PlayerYamlDB.PlayerData> Player(string Fragment, Dictionary<string, PlayerYamlDB.PlayerData> Playerdb)
        {
            List<PlayerYamlDB.PlayerData> Matches = new List<PlayerYamlDB.PlayerData> { };
            foreach (PlayerYamlDB.PlayerData Player in Playerdb.Values)
            {
                if (Convert.ToString(Player.EmpyrionID) == Fragment)
                {
                    Matches.Add(Player);
                    return Matches;
                }
                else if (Convert.ToString(Player.PlayerName) == Fragment)
                {
                    Matches.Add(Player);
                    return Matches;
                }
                else if (Convert.ToString(Player.PlayerName).ToLower() == Fragment.ToLower())
                {
                    Matches.Add(Player);
                }
                else if (Convert.ToString(Player.PlayerName).Contains(Fragment))
                {
                    Matches.Add(Player);
                }
                else if (Convert.ToString(Player.PlayerName).ToLower().Contains(Fragment.ToLower()))
                {
                    Matches.Add(Player);
                }
            }
            return Matches;
        }

        public static void FileReader(ushort ThisSeqNr, string File)
        {
            //Checks for simple errors
            string[] Script1 = System.IO.File.ReadAllLines(File);
            for (int i = 0; i < Script1.Count(); ++i)
            {

            }
        }

        public static string ChatmessageHandler(string[] Chatmessage, string Selector)
        {
            List<string> Restring = new List<string>(Chatmessage);
            string Picked = "";
            if (Selector.Contains('*'))
            {
                if (Selector == "1*")
                {
                    Restring.Remove(Restring[0]);
                    Picked = string.Join(" ", Restring.ToArray());
                }
                else if (Selector == "2*")
                {
                    Restring.Remove(Restring[1]);
                    Restring.Remove(Restring[0]);
                    Picked = string.Join(" ", Restring.ToArray());
                }
                else if (Selector == "3*")
                {
                    Restring.Remove(Restring[2]);
                    Restring.Remove(Restring[1]);
                    Restring.Remove(Restring[0]);
                    Picked = string.Join(" ", Restring.ToArray());
                }
                else if (Selector == "4*")
                {
                    Restring.Remove(Restring[3]);
                    Restring.Remove(Restring[2]);
                    Restring.Remove(Restring[1]);
                    Restring.Remove(Restring[0]);
                    Picked = string.Join(" ", Restring.ToArray());
                }
                else if (Selector == "5*")
                {
                    Restring.Remove(Restring[4]);
                    Restring.Remove(Restring[3]);
                    Restring.Remove(Restring[2]);
                    Restring.Remove(Restring[1]);
                    Restring.Remove(Restring[0]);
                    Picked = string.Join(" ", Restring.ToArray());
                }
            }
            else
            {

            }
            return Picked;
        }

        public static Dictionary<string, string[]> CSVReader1(string File)
        {
            Dictionary<string, string[]> ItemDB = new Dictionary<string, string[]> { };
            string[] Line = System.IO.File.ReadAllLines(File);
            foreach (string Item in Line)
            {
                string[] itemArray = Item.Split(',');
                ItemDB.Add(itemArray[0], itemArray);
            }
            return ItemDB;
        }

        public static ItemStack[] ReadItemStacks(string File)
        {
            string[] bagLines = System.IO.File.ReadAllLines(File);
            int itemStackSize = bagLines.Count();
            ItemStack[] itStack = new ItemStack[itemStackSize];
            for (int i = 0; i < itemStackSize; ++i)
            {
                string[] bagLinesSplit = bagLines[i].Split(',');
                itStack[i] = new ItemStack(Convert.ToInt32(bagLinesSplit[1]), Convert.ToInt32(bagLinesSplit[2]));
                itStack[i].slotIdx = Convert.ToByte(bagLinesSplit[0]);
                itStack[i].ammo = Convert.ToInt32(bagLinesSplit[3]);
                itStack[i].decay = Convert.ToInt32(bagLinesSplit[4]);
            }
            return itStack;
        }

        public static void WriteItemStacks(string File, ItemStack[] ItemStacks, bool SuperStack)
        {
            if (SuperStack)
            {
                Dictionary<int, List<ItemStack>> Superstacker = new Dictionary<int, List<ItemStack>> { };
                foreach (ItemStack item in ItemStacks)
                {
                    Debug("1");
                    //int itemid = item.id;
                    List<ItemStack> StackList = new List<ItemStack> { };
                    if (Superstacker.Keys.Contains(item.id))
                    {
                        Debug("2");
                        StackList = Superstacker[item.id];
                        bool Matched = false;
                        for (int i = 0; i < StackList.Count; i++)
                        {
                            Debug("3");
                            if (item.decay == StackList[i].decay && item.ammo == StackList[i].ammo)
                            {
                                Debug("4");
                                ItemStack NewItemStack = new ItemStack
                                {
                                    count = item.count + StackList[i].count,
                                    id = item.id,
                                    ammo = item.ammo,
                                    decay = item.decay,
                                    slotIdx = item.slotIdx
                                };
                                Matched = true;
                                StackList[i] = NewItemStack;
                                Debug("5");
                            }
                        }
                        if (!Matched)
                        {
                            Debug("6");
                            StackList.Add(item);
                            Debug("7");
                        }
                    }
                    else
                    {
                        Debug("8");
                        StackList = new List<ItemStack>{ };
                        StackList.Add(item);
                        Debug("9");
                    }
                    Debug("10");
                    Superstacker[item.id] = StackList;
                    Debug("11");
                }
                ItemStack[] Final = new ItemStack[] { };
                foreach (List<ItemStack> ListedItemStack in Superstacker.Values)
                {
                    Debug("12");
                    foreach (ItemStack IndividualItemStack in ListedItemStack)
                    {
                        Debug("13");
                        LogFile(File, IndividualItemStack.slotIdx + "," + IndividualItemStack.id + "," + IndividualItemStack.count + "," + IndividualItemStack.decay + "," + IndividualItemStack.ammo);
                    }
                }
            }
            else
            { 
                foreach (ItemStack item in ItemStacks)
                {
                    Debug("14");
                    LogFile(File, item.slotIdx + "," + item.id + "," + item.count + "," + item.decay + "," + item.ammo);
                }
            }
        }

        public static Storage.ChatData SplitChat(string ChatMessage, Dictionary<string, PlayerYamlDB.PlayerData> PlayersDB)
        {

            string[] splitted = ChatMessage.Split(new[] { ' ' }, 3);
            List<PlayerYamlDB.PlayerData> playerdata = Player(splitted[1], PlayersDB);
            if (splitted.Count() == 3)
            {
                Storage.ChatData ChatData = new Storage.ChatData
                {
                    Command = splitted[0],
                    Player = playerdata[0],
                    Message = splitted[2]
                };
                return ChatData;
            }
            else
            {
                Storage.ChatData ChatData = new Storage.ChatData
                {
                    Command = splitted[0],
                    Player = playerdata[0],
                };
                return ChatData;
            }
        }

        public static string SplitChat2(string ChatMessage)
        {
            string[] splitted = ChatMessage.Split(new[] { ' ' }, 2);
            string message = splitted[1];
            return message;
        }

        public static string UnixTimeStamp()
        {
            string time = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            return time;
        }

    }
}

