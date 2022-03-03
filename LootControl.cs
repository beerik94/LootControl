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
    public override Version Version => new Version(1, 1, 0, 0);

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
            int randomChance = random.Next(0, 100);
            if (lootItem.DropChance > randomChance)
            {
              int stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
              Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                lootItem.ItemID, stack, false, lootItem.PrefixID);

#if DEBUG
              Console.WriteLine(
                "LootControl - OnNPCKilled: BloodmoonDrop ItemID:{0} - Prefix: {1} - Amount:{2} - (X:{3}, Y:{4})",
                lootItem.ItemID, lootItem.PrefixID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
            }
          }
        }

        if (Main.eclipse && outTable.NPCLootEntries.ContainsKey(WorldState.Eclipse))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Eclipse])
          {
            int randomChance = random.Next(0, 100);
            if (lootItem.DropChance > randomChance)
            {
              int stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
              Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                lootItem.ItemID, stack, false, lootItem.PrefixID);

#if DEBUG
              Console.WriteLine(
                "LootControl - OnNPCKilled: EclipseDrop ItemID:{0} - Prefix: {1} - Amount:{2} - (X:{3}, Y:{4})",
                lootItem.ItemID, lootItem.PrefixID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
            }
          }
        }

        if (Main.moonPhase == 0 && !Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Fullmoon))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Fullmoon])
          {
            int randomChance = random.Next(0, 100);
            if (lootItem.DropChance > randomChance)
            {
              int stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
              Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                lootItem.ItemID, stack, false, lootItem.PrefixID);

#if DEBUG
              Console.WriteLine(
                "LootControl - OnNPCKilled: FullmoonDrop ItemID:{0} - Prefix: {1} - Amount:{2} - (X:{3}, Y:{4})",
                lootItem.ItemID, lootItem.PrefixID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
            }
          }
        }

        if (!Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Night))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Night])
          {
            int randomChance = random.Next(0, 100);
            if (lootItem.DropChance > randomChance)
            {
              int stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
              Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                lootItem.ItemID, stack, false, lootItem.PrefixID);

#if DEBUG
              Console.WriteLine(
                "LootControl - OnNPCKilled: NightDrop ItemID:{0} - Prefix: {1} - Amount:{2} - (X:{3}, Y:{4})",
                lootItem.ItemID, lootItem.PrefixID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
            }
          }
        }

        if (Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Day))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Day])
          {
            int randomChance = random.Next(0, 100);
            if (lootItem.DropChance > randomChance)
            {
              int stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
              Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                lootItem.ItemID, stack, false, lootItem.PrefixID);

  #if DEBUG
              Console.WriteLine(
                "LootControl - OnNPCKilled: DayDrop ItemID:{0} - Prefix: {1} - Amount:{2} - (X:{3}, Y:{4})",
                lootItem.ItemID, lootItem.PrefixID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
  #endif
            }
          }
        }

        if (outTable.NPCLootEntries.ContainsKey(WorldState.Normal))
        {
          foreach (Loot lootItem in outTable.NPCLootEntries[WorldState.Normal])
          {
            int randomChance = random.Next(0, 100);
            if (lootItem.DropChance > randomChance)
            {
              int stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
              Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                lootItem.ItemID, stack, false, lootItem.PrefixID);

#if DEBUG
              Console.WriteLine(
                "LootControl - OnNPCKilled: NormalDrop ItemID:{0} - Prefix: {1} - Amount:{2} - (X:{3}, Y:{4})",
                lootItem.ItemID, lootItem.PrefixID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
            }
          }
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