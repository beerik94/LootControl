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
    public override Version Version => new Version(1, 0, 0, 0);

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
      Console.WriteLine("LootControl - OnNPCKilled: NPC ID: {0} - NPCArrayIndex: {1} - NPCPos: {2},{3}", eventArgs.npc.netID,
        eventArgs.npc.whoAmI, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif

      LootTable outTable;
      var random = new Random();
      if (_config.NPCLootTables.TryGetValue(eventArgs.npc.netID, out outTable))
      {
        if (Main.bloodMoon && outTable.NPCLootEntries.ContainsKey(WorldState.Bloodmoon))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Bloodmoon])
            if (lootItem.AlwaysDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                eventArgs.npc.DropItemInstanced(eventArgs.npc.position, eventArgs.npc.Size, lootItem.ItemID, stack);
                
#if DEBUG
                Console.WriteLine("LootControl - OnNPCKilled: BloodmoonDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
              }
            }

        if (Main.eclipse && outTable.NPCLootEntries.ContainsKey(WorldState.Eclipse))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Eclipse])
            if (lootItem.AlwaysDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                eventArgs.npc.DropItemInstanced(eventArgs.npc.position, eventArgs.npc.Size, lootItem.ItemID, stack);
                
#if DEBUG
                Console.WriteLine("LootControl - OnNPCKilled: EclipseDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
              }
            }

        if (Main.moonPhase == 0 && !Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Fullmoon))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Fullmoon])
            if (lootItem.AlwaysDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                eventArgs.npc.DropItemInstanced(eventArgs.npc.position, eventArgs.npc.Size, lootItem.ItemID, stack);
                
#if DEBUG
                Console.WriteLine("LootControl - OnNPCKilled: FullmoonDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
              }
            }

        if (!Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Night))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Night])
            if (lootItem.AlwaysDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                eventArgs.npc.DropItemInstanced(eventArgs.npc.position, eventArgs.npc.Size, lootItem.ItemID, stack);
                
#if DEBUG
                Console.WriteLine("LootControl - OnNPCKilled: NightDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
              }
            }

        if (Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Day))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Day])
            if (lootItem.AlwaysDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                eventArgs.npc.DropItemInstanced(eventArgs.npc.position, eventArgs.npc.Size, lootItem.ItemID, stack);
                
#if DEBUG
                Console.WriteLine("LootControl - OnNPCKilled: DayDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
              }
            }

        if (outTable.NPCLootEntries.ContainsKey(WorldState.Normal))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Normal])
            if (lootItem.AlwaysDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                eventArgs.npc.DropItemInstanced(eventArgs.npc.position, eventArgs.npc.Size, lootItem.ItemID, stack);
                
#if DEBUG
                Console.WriteLine("LootControl - OnNPCKilled: NormalDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
              }
            }
      }
    }

    private void OnNPCLootDrop(NpcLootDropEventArgs eventArgs)
    {
#if DEBUG
      Console.WriteLine("LootControl - OnNPCLootDrop: NPC ID: {0} - NPCArrayIndex: {1} - NPCPos: {2},{3} - DroppedItem: {4}",
        eventArgs.NpcId, eventArgs.NpcArrayIndex, eventArgs.Position.X, eventArgs.Position.Y, eventArgs.ItemId);
#endif

      LootTable outTable;
      var random = new Random();
      if (_config.NPCLootTables.TryGetValue(eventArgs.NpcId, out outTable))
      {
        if (Main.bloodMoon && outTable.NPCLootEntries.ContainsKey(WorldState.Bloodmoon))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Bloodmoon])
            if (!lootItem.AlwaysDrop || lootItem.DoubleDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var newItem = TShock.Utils.GetItemById(lootItem.ItemID);
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                Item.NewItem(eventArgs.Source, eventArgs.Position, newItem.width, newItem.height, lootItem.ItemID,
                  stack, eventArgs.Broadcast, lootItem.PrefixID);

#if DEBUG
                Console.WriteLine("LootControl - OnNPCLootDrop: BloodmoonDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.Position.X, eventArgs.Position.Y);
#endif
                
                eventArgs.Handled = true;
              }
            }

        if (Main.eclipse && outTable.NPCLootEntries.ContainsKey(WorldState.Eclipse))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Eclipse])
            if (!lootItem.AlwaysDrop || lootItem.DoubleDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var newItem = TShock.Utils.GetItemById(lootItem.ItemID);
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                Item.NewItem(eventArgs.Source, eventArgs.Position, newItem.width, newItem.height, lootItem.ItemID,
                  stack, eventArgs.Broadcast, lootItem.PrefixID);

#if DEBUG
                Console.WriteLine("LootControl - OnNPCLootDrop: EclipseDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.Position.X, eventArgs.Position.Y);
#endif
                
                eventArgs.Handled = true;
              }
            }

        if (Main.moonPhase == 0 && !Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Fullmoon))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Fullmoon])
            if (!lootItem.AlwaysDrop || lootItem.DoubleDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var newItem = TShock.Utils.GetItemById(lootItem.ItemID);
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                Item.NewItem(eventArgs.Source, eventArgs.Position, newItem.width, newItem.height, lootItem.ItemID,
                  stack, eventArgs.Broadcast, lootItem.PrefixID);

#if DEBUG
                Console.WriteLine("LootControl - OnNPCLootDrop: FullmoonDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.Position.X, eventArgs.Position.Y);
#endif
                
                eventArgs.Handled = true;
              }
            }

        if (!Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Night))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Night])
            if (!lootItem.AlwaysDrop || lootItem.DoubleDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var newItem = TShock.Utils.GetItemById(lootItem.ItemID);
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                Item.NewItem(eventArgs.Source, eventArgs.Position, newItem.width, newItem.height, lootItem.ItemID,
                  stack, eventArgs.Broadcast, lootItem.PrefixID);

#if DEBUG
                Console.WriteLine("LootControl - OnNPCLootDrop: NightDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.Position.X, eventArgs.Position.Y);
#endif
                
                eventArgs.Handled = true;
              }
            }

        if (Main.dayTime && outTable.NPCLootEntries.ContainsKey(WorldState.Day))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Day])
            if (!lootItem.AlwaysDrop || lootItem.DoubleDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var newItem = TShock.Utils.GetItemById(lootItem.ItemID);
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                Item.NewItem(eventArgs.Source, eventArgs.Position, newItem.width, newItem.height, lootItem.ItemID,
                  stack, eventArgs.Broadcast, lootItem.PrefixID);

#if DEBUG
                Console.WriteLine("LootControl - OnNPCLootDrop: DayDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.Position.X, eventArgs.Position.Y);
#endif

                eventArgs.Handled = true;
              }
            }

        if (outTable.NPCLootEntries.ContainsKey(WorldState.Normal))
          foreach (var lootItem in outTable.NPCLootEntries[WorldState.Normal])
            if (!lootItem.AlwaysDrop || lootItem.DoubleDrop)
            {
              var randomChance = random.Next(0, 100);
              if (lootItem.DropChance > randomChance)
              {
                var newItem = TShock.Utils.GetItemById(lootItem.ItemID);
                var stack = random.Next(lootItem.Min_Stack, lootItem.Max_Stack + 1);
                Item.NewItem(eventArgs.Source, eventArgs.Position, newItem.width, newItem.height, lootItem.ItemID,
                  stack, eventArgs.Broadcast, lootItem.PrefixID);

#if DEBUG
                Console.WriteLine("LootControl - OnNPCLootDrop: NormalDrop ItemID:{0} - Amount:{1} - (X:{2}, Y:{3})",
                  lootItem.ItemID, stack, eventArgs.Position.X, eventArgs.Position.Y);
#endif

                eventArgs.Handled = true;
              }
            }

        if (!outTable.ShouldDropDefaultLoot) eventArgs.Handled = true;
      }
    }

    private void OnReloadEvent(ReloadEventArgs eventArgs)
    {
      _config.ReadConfig(_filePath);
    }
  }
}