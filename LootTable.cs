using System.Collections.Generic;

namespace LootControl
{
  public enum WorldState
  {
    Normal,
    Day,
    Night,
    Eclipse,
    Fullmoon,
    Bloodmoon
  }
  
  public class LootTable
  {
    public Dictionary<WorldState, List<Loot>> NPCLootEntries;
    public bool ShouldDropDefaultLoot = false;

    public LootTable()
    {
      NPCLootEntries = new Dictionary<WorldState, List<Loot>>();
    }
  }

  public class Loot
  {
    public int ItemID;
    public int PrefixID;
    public int DropChance = 100;
    public int Min_Stack = 0;
    public int Max_Stack = 1;
    public bool AlwaysDrop = false;
    public bool DoubleDrop = false;

    public Loot(int pItemID, int pDropChance, int pMin_Stack, int pMax_Stack, bool pAlwaysDrop=false, bool pDoubleDrop=false, int pPrefixID=0)
    {
      ItemID = pItemID;
      PrefixID = pPrefixID;
      DropChance = pDropChance;
      Min_Stack = pMin_Stack;
      Max_Stack = pMax_Stack;
      AlwaysDrop = pAlwaysDrop;
      DoubleDrop = pDoubleDrop;
    }
  }
}