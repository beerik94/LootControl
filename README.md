# LootControl
## TShock plugin to control the NPC loot tables
### Based of LootTableEditor by OLink all credits to him for original code (https://github.com/Olink/LootTableEditor).
This has more options than the original LootTableEditor.  
Will probably build a config creator at some point to ease the use of this plugin.  
Support for NPC variants

Supported world states:
- Normal
- Day
- Night
- Fullmoon
- Bloodmoon
- Eclipse

Example config below(when used make sure to delete // COMMENTS since JSON doesn't support comments):

```json
{
  "1": { // NPC ID
    "NPCLootEntries": {
      "Normal": [ // World State
        {
          "ItemID": 85, // ID of the item that you want to drop
          "PrefixID": 0, // ID of the prefix you want to give the item
          "DropChance": 100, // Chance of the item to drop, 0 being no chance and 100 will always drop it
          "Min_Stack": 1, // Minimum amount that will be dropped
          "Max_Stack": 10, // Maximum amount that will be dropped
          "AlwaysDrop": false, // If set to "true" this makes sure that the item is always dropped and not just when Terraria allows items to be dropped, Needs to be "true" if you use it for variants aka negative NPC IDs
          "DoubleDrop": false // If set to "true" the item will be dropped twice IF the NPC is not a variant aka negative NPC ID
        },
		    {
          "ItemID": 73, // ID of the item that you want to drop
          "PrefixID": 0, // ID of the prefix you want to give the item
          "DropChance": 100, // Chance of the item to drop, 0 being no chance and 100 will always drop it
          "Min_Stack": 1, // Minimum amount that will be dropped
          "Max_Stack": 10, // Maximum amount that will be dropped
          "AlwaysDrop": true, // If set to "true" this makes sure that the item is always dropped and not just when Terraria allows items to be dropped, Needs to be "true" if you use it for variants aka negative NPC IDs
          "DoubleDrop": true // If set to "true" the item will be dropped twice IF the NPC is not a variant aka negative NPC ID
        },
      ],
      "Bloodmoon": [ // World State
        {
          "ItemID": 85, // ID of the item that you want to drop
          "PrefixID": 0, // ID of the prefix you want to give the item
          "DropChance": 100, // Chance of the item to drop, 0 being no chance and 100 will always drop it
          "Min_Stack": 1, // Minimum amount that will be dropped
          "Max_Stack": 10, // Maximum amount that will be dropped
          "AlwaysDrop": false, // If set to "true" this makes sure that the item is always dropped and not just when Terraria allows items to be dropped, Needs to be "true" if you use it for variants aka negative NPC IDs
          "DoubleDrop": false // If set to "true" the item will be dropped twice IF the NPC is not a variant aka negative NPC ID
        },
		    {
          "ItemID": 73, // ID of the item that you want to drop
          "PrefixID": 0, // ID of the prefix you want to give the item
          "DropChance": 100, // Chance of the item to drop, 0 being no chance and 100 will always drop it
          "Min_Stack": 1, // Minimum amount that will be dropped
          "Max_Stack": 10, // Maximum amount that will be dropped
          "AlwaysDrop": true, // If set to "true" this makes sure that the item is always dropped and not just when Terraria allows items to be dropped, Needs to be "true" if you use it for variants aka negative NPC IDs
          "DoubleDrop": true // If set to "true" the item will be dropped twice IF the NPC is not a variant aka negative NPC ID
        },
      ]
    },
    "ShouldDropDefaultLoot": false // If set to "true" the NPC will also drop their default loot defined by Terraria itself
  }
}
```
