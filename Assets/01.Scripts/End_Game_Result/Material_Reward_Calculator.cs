using System.Collections.Generic;
using UnityEngine;

public static class Material_Reward_Calculator
{
    private const int MAX_REWARD_TYPES = 4;

    public static List<Material_Reward_Data> Calculate(int clearedRound)
    {
        List<Material_Reward_Data> results = new List<Material_Reward_Data>();

        int bossClearCount = Get_Boss_Clear_Count(clearedRound);
        if (bossClearCount <= 0)
            return results;

        int totalDropCount = Get_Total_Drop_Count(bossClearCount);
        if (totalDropCount <= 0)
            return results;

        List<Material_Item> candidates = Get_All_Material_Items();
        if (candidates.Count == 0)
            return results;

        int typeCount = Mathf.Min(MAX_REWARD_TYPES, totalDropCount, candidates.Count);

        List<Material_Item> selectedTypes = Select_Distinct_Items_By_Weight(candidates, typeCount);
        if (selectedTypes.Count == 0)
            return results;

        Dictionary<Material_Item, int> rewardMap = Distribute_Counts(selectedTypes, totalDropCount);

        foreach (var pair in rewardMap)
        {
            if (pair.Key == null) continue;
            if (pair.Value <= 0) continue;

            results.Add(new Material_Reward_Data(pair.Key, pair.Value));
        }

        return results;
    }

    private static int Get_Boss_Clear_Count(int clearedRound)
    {
        if (clearedRound <= 0) return 0;
        return clearedRound / Round_Manager.Instance.BOSS_ROUND_INTERVAL;
    }

    private static int Get_Total_Drop_Count(int bossClearCount)
    {
        int total = 0;

        for (int i = 0; i < bossClearCount; i++)
        {
            total += Random.Range(1, 6); // 1~5
        }

        return total;
    }

    private static List<Material_Item> Get_All_Material_Items()
    {
        List<Material_Item> list = new List<Material_Item>();

        foreach (var pair in Base_Manager.Data_Mng.Item_Data)
        {
            if (pair.Value is Material_Item materialItem)
            {
                if (materialItem.item_Chance > 0f)
                    list.Add(materialItem);
            }
        }

        return list;
    }

    private static List<Material_Item> Select_Distinct_Items_By_Weight(List<Material_Item> candidates, int count)
    {
        List<Material_Item> pool = new List<Material_Item>(candidates);
        List<Material_Item> selected = new List<Material_Item>();

        for (int i = 0; i < count; i++)
        {
            Material_Item picked = Pick_One_By_Weight(pool);
            if (picked == null)
                break;

            selected.Add(picked);
            pool.Remove(picked);
        }

        return selected;
    }

    private static Dictionary<Material_Item, int> Distribute_Counts(List<Material_Item> selectedItems, int totalDropCount)
    {
        Dictionary<Material_Item, int> result = new Dictionary<Material_Item, int>();

        if (selectedItems == null || selectedItems.Count == 0 || totalDropCount <= 0)
            return result;

        for (int i = 0; i < selectedItems.Count; i++)
        {
            result[selectedItems[i]] = 1;
        }

        int remaining = totalDropCount - selectedItems.Count;
        if (remaining <= 0)
            return result;

        for (int i = 0; i < remaining; i++)
        {
            Material_Item picked = Pick_One_By_Weight(selectedItems);
            if (picked == null)
                continue;

            result[picked]++;
        }

        return result;
    }

    private static Material_Item Pick_One_By_Weight(List<Material_Item> list)
    {
        if (list == null || list.Count == 0)
            return null;

        float totalWeight = 0f;

        for (int i = 0; i < list.Count; i++)
        {
            totalWeight += Mathf.Max(0f, list[i].item_Chance);
        }

        if (totalWeight <= 0f)
            return null;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < list.Count; i++)
        {
            cumulative += Mathf.Max(0f, list[i].item_Chance);

            if (roll <= cumulative)
                return list[i];
        }

        return list[list.Count - 1];
    }
}