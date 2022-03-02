using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LootControl
{
  public class Config
  {
    // int = NPC ID
    public Dictionary<int, LootTable> NPCLootTables;

    public Config()
    {
      NPCLootTables = new Dictionary<int, LootTable>();
    }

    public void WriteConfig(String configFile)
    {
      LootTable NewLT = new LootTable();
      NewLT.NPCLootEntries = new Dictionary<WorldState, List<Loot>>();
      List<Loot> LootEntries = new List<Loot>();
      LootEntries.Add(new Loot(73, 100, 1, 99));
      NewLT.NPCLootEntries.Add(WorldState.Normal, LootEntries);
      NPCLootTables.Add(1, NewLT);

      using (StreamWriter SW = new StreamWriter(configFile))
      {
        SW.Write(JsonConvert.SerializeObject(NPCLootTables, Formatting.Indented));
      }
    }

    public void ReadConfig(String configFile)
    {
      if (!File.Exists(configFile))
      {
        WriteConfig(configFile);
      }

      using (StreamReader SR = new StreamReader(configFile))
      {
        String JSONString = SR.ReadToEnd();
        NPCLootTables = JsonConvert.DeserializeObject<Dictionary<int, LootTable>>(JSONString);
      }
    }
  }
}