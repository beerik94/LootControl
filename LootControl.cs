using System;
using System.IO;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace LootControl
{
  [ApiVersion(2, 1)]
  public class LootControl : TerrariaPlugin
  {
    public override string Author => "Beerik";
    public override string Description => "This plugin allows the user to control the loot tables of all NPCs";
    public override string Name => "Loot Control";
    public override Version Version => new Version(1, 3, 1, 0);

    private Config _config;
    private string _filePath = "";

    public LootControl(Main game) : base(game)
    {
      Order = 1;
    }

    public override void Initialize()
    {
      _filePath = Path.Combine(TShock.SavePath, "LootControl_Config.json");
      _config = new Config();
      _config.ReadConfig(_filePath);
      ServerApi.Hooks.NpcKilled.Register(this, OnNPCKilled);
      ServerApi.Hooks.NpcLootDrop.Register(this, OnNPCLootDrop);
      GeneralHooks.ReloadEvent += OnReloadEvent;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        ServerApi.Hooks.NpcKilled.Deregister(this, OnNPCKilled);
        ServerApi.Hooks.NpcLootDrop.Deregister(this, OnNPCLootDrop);
        GeneralHooks.ReloadEvent -= OnReloadEvent;
      }

      base.Dispose(disposing);
    }

    private void OnNPCKilled(NpcKilledEventArgs eventArgs)
    {
#if DEBUG
      Console.WriteLine("LootControl - OnNPCKilled: NPC ID: {0} - NPCArrayIndex: {1} - NPCPos: {2},{3}",
        eventArgs.npc.netID,
        eventArgs.npc.whoAmI, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif

      LootTable outTable;
      Random random = new Random();
      if (_config.NPCLootTables.TryGetValue(eventArgs.npc.netID, out outTable))
      {
        if (Main.bloodMoon && outTable.NPCLootEntries.ContainsKey(WorldState.Bloodmoon))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Bloodmoon])
          {
            DropLoot(lootItem, eventArgs, WorldState.Bloodmoon);
          }
        }

        if (Main.eclipse && outTable.NPCLootEntries.ContainsKey(WorldState.Eclipse))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Eclipse])
          {
            DropLoot(lootItem, eventArgs, WorldState.Eclipse);
          }
        }

        if (Main.moonPhase == 0 && !Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Fullmoon))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Fullmoon])
          {
            DropLoot(lootItem, eventArgs, WorldState.Fullmoon);
          }
        }

        if (!Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Night))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Night])
          {
            DropLoot(lootItem, eventArgs, WorldState.Night);
          }
        }

        if (Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Day))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Day])
          {
            DropLoot(lootItem, eventArgs, WorldState.Day);
          }
        }

        if (outTable.NPCLootEntries.ContainsKey(WorldState.Normal))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Normal])
          {
            DropLoot(lootItem, eventArgs, WorldState.Normal);
          }
        }
      }
    }

    // If successful returns stack size dropped else returns -1
    private void DropLoot(Loot pLootItem, NpcKilledEventArgs eventArgs, WorldState pState)
    {
#if DEBUG
      Console.WriteLine(
        "LootControl - OnNPCKilled: ExpertMode: {0} - MasterMode: {1} - LootClassic: {2} - LootExpert: {3} - LootMaster: {4}",
        Main.expertMode, Main.masterMode, pLootItem.DropInClassic, pLootItem.DropInExpert, pLootItem.DropInMaster);
#endif
      
      if (!Main.expertMode && !Main.masterMode && pLootItem.DropInClassic ||
          Main.expertMode && !Main.masterMode && pLootItem.DropInExpert ||
          Main.expertMode && Main.masterMode && pLootItem.DropInMaster)
      {
        Random random = new Random();
        double randomChance = random.NextDouble() * 100.0;
        if (pLootItem.DropChance > randomChance)
        {
          int stack = random.Next(pLootItem.Min_Stack, pLootItem.Max_Stack + 1);
          int newPrefix = (pLootItem.PrefixID < 0) ? random.Next(0, 84) : pLootItem.PrefixID;
          Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
            pLootItem.ItemID, stack, false, newPrefix);

#if DEBUG
          Console.WriteLine(
            "LootControl - OnNPCKilled: State: {0} - ItemID: {1} - Prefix: {2} - Amount: {3} - (X: {4}, Y: {5})",
            pState.ToString(), pLootItem.ItemID, newPrefix, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
        }
      }
    }
    
    private void OnNPCLootDrop(NpcLootDropEventArgs eventArgs)
    {
      LootTable outTable;
      if (_config.NPCLootTables.TryGetValue(eventArgs.NpcId, out outTable))
      {
        if (!outTable.ShouldDropDefaultLoot)
        {
          eventArgs.Handled = true;
        }
      }
    }

    private void OnReloadEvent(ReloadEventArgs eventArgs)
    {
      _config.ReadConfig(_filePath);
    }
  }
}