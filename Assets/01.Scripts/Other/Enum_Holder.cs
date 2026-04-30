public enum InGame_State
{
    READY,
    COUNTDOWN,
    ROUND_START,
    BATTLE,
    BOSS_READY,
    BOSS_BATTLE,
    ROUND_END,
    ABILITY_SELECT,
    VICTORY,
    DEATH
}
public enum Rarity
{
    Common,
    Rare,
    Hero,
    Legendary
}
public enum Item_Type
{
    Consumable, // 소모품
    Material, // 재료
    Other, // 기타 
    Equipment, // 장비 -> 아직 미정
    Rune,
    ALL,
    None
}
public enum DIRECTION
{
    Left,
    LeftUp, 
    LeftDown,
    Right,
    RightUp,
    RightDown,
    Up,
    Down, 
    None
}

public enum  Enemy_Type
{
    Core_Attacker,
    Melee_Attacker,
    Ranged_Attacker,
    Boss
}

public enum Enemy_State 
{
    Idle,
    Chase,
    Attack,
    Death
}

public enum Boss_Skill_Type 
{
    Teleport,
    Spawn_Slime,
    Spawn_Burst,
    None
}

public enum Scene_Button_Type
{
    Load_Scene,
    Reload_Scene
}

public enum Canvas_Layer
{
    Layer_01 = 0,
    Laver_02 = 1,
    Laver_03 = 2
}

public enum Effect_Type
{
    Buff,
    Shield
}

public enum Item_Slot_Type
{
    None,
    Shop,
    Inventory,
    Shop_Inventory,
    Equipped
}

public enum Player_Skill_Type 
{
    Passive,
    Active_Attack,
    Active_Buff
}

public enum Rune_Stat_Type
{
    Attack,
    HP,
    Mana,
    Speed,
    Stamina
}

public enum BGM_Type
{
    None,
    Title,
    Lobby,
    In_Game
}

public enum SFX_Type
{
    None = 0,

    Button_Click = 1,
    Button_Hover = 2,
    Ability_Select = 3,
    Button_Buy =4,
    Slot = 5,
    Item_Type_Button = 6,

    Warrior_Base_Attack = 50,
    Player_Hit = 51,
    Enemy_Hit = 52,
    Enemy_Death = 53,
    Enemy_Attack = 54,

    Rune_Upgrade_Success = 100,
    Rune_Upgrade_Fail = 101,

    Shop_Add_Button = 150,

    Use_Potion = 200,

    Boss_Skill_Bump = 300,

    Warrior_Skill_T = 400,

    Victory,
    Defeat
}

public enum Camera_Shake_Level
{
    Off,
    Weak,
    Strong
}