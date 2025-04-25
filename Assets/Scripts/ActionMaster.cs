using UnityEngine;
using System.Collections.Generic;

public static class ActionMaster
{
    private static bool comicHidden = true;
    private static string song = "off";

    //Setter
    public static void SetComicHidden(bool b) { comicHidden = b; }

    public static void SetSong(string s) { song = s; }

    //Getter
    public static bool GetComicHidden() { return comicHidden; }

    public static string GetSong() { return song; }
}
