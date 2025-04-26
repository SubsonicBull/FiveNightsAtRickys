using UnityEngine;
using System.Collections.Generic;

public static class ActionMaster
{
    private static bool comicHidden = true;
    private static bool riceHidden = false;
    private static string song = "off";

    //Setter
    public static void SetComicHidden(bool b) { comicHidden = b; }
    public static void SetRiceHidden(bool b) { riceHidden = b; }

    public static void SetSong(string s) { song = s; }

    //Getter
    public static bool GetComicHidden() { return comicHidden; }
    public static bool GetRiceHidden() { return riceHidden; }

    public static string GetSong() { return song; }
}
