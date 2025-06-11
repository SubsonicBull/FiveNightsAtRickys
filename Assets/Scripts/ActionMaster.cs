using UnityEngine;
using System.Collections.Generic;

public static class ActionMaster
{
    private static bool comicHidden = true;
    private static bool riceHidden = false;
    private static bool playerHidden = false;
    private static string song = "off";
    private static string hidingSpotName = "couch";
    private static bool isScreaming = false;

    private static int power = 100;


    //Setter
    public static void SetComicHidden(bool b) { comicHidden = b; }
    public static void SetRiceHidden(bool b) { riceHidden = b; }
    public static void SetPlayerHidden(bool b) { playerHidden = b; }
    public static void SetIsScreaming(bool b) { isScreaming = b; }

    public static void SetHidingSpot(string s) { hidingSpotName = s; }
    public static void SetSong(string s) { song = s; }
    public static void ResetPower() { power = 100; }

    public static void ConsumePower(int f)
    {
        if (power != 0)
        {
            if (power < 0)
            {
                power = 0;
            }
            else
            {
                power -= f;
            }
        }
    }

    //Getter
    public static bool GetComicHidden() { return comicHidden; }
    public static bool GetRiceHidden() { return riceHidden; }
    public static bool GetPlayerHidden() { return playerHidden; }
    public static bool GetIsScreaming() { return isScreaming; }

    public static string GetHidingSpot() { return hidingSpotName; }
    public static string GetSong() { return song; }
    public static int GetPower() { return power; }
}
