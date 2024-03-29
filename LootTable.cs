﻿using System.Collections.Generic;

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
    public double DropChance = 100;
    public int Min_Stack = 0;
    public int Max_Stack = 1;
    public bool DropInClassic = true;
    public bool DropInExpert = false;
    public bool DropInMaster = false;

    public Loot(int pItemID, double pDropChance, int pMin_Stack, int pMax_Stack, int pPrefixID=0, bool pDropInClassic=true, bool pDropInExpert=false, bool pDropInMaster=false)
    {
      ItemID = pItemID;
      PrefixID = pPrefixID;
      DropChance = pDropChance;
      Min_Stack = pMin_Stack;
      Max_Stack = pMax_Stack;
      DropInClassic = pDropInClassic;
      DropInExpert = pDropInExpert;
      DropInMaster = pDropInMaster;
    }
  }
}