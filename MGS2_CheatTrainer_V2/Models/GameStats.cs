namespace MGS2_CheatTrainer_V2.Models;

public class GameStats
{
    public enum ModifiableStats
    {
        Alerts,
        Continues,
        DamageTaken,
        Kills,
        MechsDestroyed,
        Rations,
        Saves,
        Shots
    }

    public short Alerts;
    public short Continues;
    public short DamageTaken;
    public short Kills;
    public short MechsDestroyed;
    public int PlayTime;
    public short Rations;
    public short Saves;
    public short Shots;
    public short SpecialItems;

    public override string ToString()
    {
        return $"Alerts: {Alerts} -- Continues: {Continues} -- DamageTaken: {DamageTaken} -- Kills: {Kills} -- " +
               $"MechsDestroyed: {MechsDestroyed} -- PlayTime: {PlayTime} -- Rations: {Rations} -- Saves: {Saves} -- " +
               $"Shots: {Shots} -- SpecialItems: {SpecialItems}";
    }
}