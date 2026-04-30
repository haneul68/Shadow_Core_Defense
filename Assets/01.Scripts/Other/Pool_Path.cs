using System.Collections.Generic;
using UnityEngine;

public static class Pool_Path
{
    private static readonly Dictionary<Pool_ID, string> paths = new()
    {
        { Pool_ID.Enemy_Magma_Slime, "Enemy/Magma_Slime" },
        { Pool_ID.Enemy_Blue_Slime, "Enemy/Blue_Slime" },
        { Pool_ID.Enemy_Green_Slime, "Enemy/Green_Slime" },

        { Pool_ID.Boss_Vampires, "Boss/Boss_Vampires" },

        { Pool_ID.Melee_Hit_Box, "Other/Melee_Hit_Box" },
        { Pool_ID.Hit_Box_Spawn_Point, "Other/Hit_Box_Spawn_Point" },

        { Pool_ID.Enemy_HP_Bar, "UI/Enemy_HP_Bar" },
        { Pool_ID.Boss_HP_Bar, "UI/Boss_HP_Bar" },
        { Pool_ID.Ability_Slot, "UI/Ability_Slot" },
        { Pool_ID.Minimap_Enemy_Icon, "UI/Minimap_Enemy_Icon" },
        { Pool_ID.Minimap_Player_Icon, "UI/Minimap_Player_Icon" },
        { Pool_ID.Inv_Item_Slot, "UI/Slots/Inv_Item_Slot" },
        { Pool_ID.Inv_Add_Button, "UI/Slots/Inv_Add_Button" },
        { Pool_ID.Shop_Item_Slot, "UI/Slots/Shop_Item_Slot" },

        { Pool_ID.Character_Slot, "UI/Slots/Character_Slot" },
        { Pool_ID.Player_Skill_Slot, "UI/Slots/Player_Skill_Slot" },
        { Pool_ID.In_Game_Skill_Slot, "UI/Slots/In_Game_Skill_Slot" },
        { Pool_ID.End_Game_Item_Slot, "UI/Slots/End_Game_Item_Slot" },
        { Pool_ID.UI_Rune_Slot, "UI/Slots/UI_Rune_Slot" },
        { Pool_ID.UI_Rune_Material_Slot, "UI/Slots/UI_Rune_Material_Slot" },

        { Pool_ID.Shop_Item_Action_Panel, "UI/Shop_Item_Action_Panel" },
        { Pool_ID.Inv_Item_Action_Panel, "UI/Inv_Item_Action_Panel" },
        { Pool_ID.Character_Action_Panel, "UI/Character_Action_Panel" },
        { Pool_ID.Character_Purchase_Panel, "UI/Character_Purchase_Panel" },
        { Pool_ID.UI_Rune_Action_Panel, "UI/UI_Rune_Action_Panel" },
        { Pool_ID.Inv_Add_Action_Panel, "UI/Inv_Add_Action_Panel" },
        
        { Pool_ID.Damage_Text, "UI/Damage_Text" },

        { Pool_ID.UI_Inventory, "UI/#UI_Inventory" },
        { Pool_ID.UI_Shop, "UI/#UI_Shop" },
        { Pool_ID.UI_Character, "UI/#UI_Character" },
        { Pool_ID.UI_Des_PopUp, "UI/UI_Des_PopUp" },
        { Pool_ID.UI_Skill_Des_Popup, "UI/UI_Skill_Des_Popup" },
        { Pool_ID.UI_Text_Popup, "UI/UI_Text_Popup" },
        { Pool_ID.UI_Rune, "UI/#UI_Rune" },
        { Pool_ID.UI_Setting, "UI/#UI_Setting" },


        { Pool_ID.Blue_Slime_Attack_Effect, "Effect/Blue_Slime_Attack_Effect" },
        { Pool_ID.Boss_Boom_SKill_Effect, "Effect/Boss_Boom_SKill_Effect" },
        { Pool_ID.Warrio_Skill_Tornador, "Effect/Warrio_Skill_Tornador" },

        { Pool_ID.Warrior_Skill_RapidStrike_Cast, "Effect/Warrior_Skill_RapidStrike_Cast" },
        { Pool_ID.Warrior_Skill_RapidStrike_Hit, "Effect/Warrior_Skill_RapidStrike_Hit" }
    };

    public static string Get_Path(Pool_ID id)
    {
        return paths.TryGetValue(id, out var path) ? path : "";
    }
}

[System.Serializable]
public enum Pool_ID
{
    Enemy_Magma_Slime = 0,
    Enemy_Blue_Slime = 1,
    Enemy_Green_Slime = 2,

    Boss_Vampires = 101,

    Hit_Box_Spawn_Point = 151,
    Melee_Hit_Box = 152,

    Enemy_HP_Bar = 200,
    Boss_HP_Bar = 201,
    Ability_Slot = 202,
    Minimap_Enemy_Icon = 203,
    Minimap_Player_Icon = 204,
    Inv_Item_Slot = 205,
    Inv_Add_Button = 206,
    Shop_Item_Slot = 207,

    Character_Slot = 250,
    Player_Skill_Slot = 251,
    In_Game_Skill_Slot = 252,
    End_Game_Item_Slot = 253,
    UI_Rune_Slot = 254,
    UI_Rune_Material_Slot = 255,

    Shop_Item_Action_Panel = 300,
    Inv_Item_Action_Panel = 301,
    Character_Action_Panel = 302,
    Character_Purchase_Panel = 303,
    UI_Rune_Action_Panel = 304,
    Inv_Add_Action_Panel = 305,

    Damage_Text = 350,

    UI_Inventory = 400,
    UI_Shop = 401,
    UI_Character = 402,
    UI_Des_PopUp = 403,
    UI_Skill_Des_Popup = 404,
    UI_Text_Popup = 405,
    UI_Rune = 406,
    UI_Setting = 407,

    Blue_Slime_Attack_Effect = 500,
    Boss_Boom_SKill_Effect = 501,
    Warrio_Skill_Tornador = 502,

    Warrior_Skill_RapidStrike_Cast = 600,
    Warrior_Skill_RapidStrike_Hit = 601,

    None = 999

}