using System;
using Avalonia.Controls;
using MGS2_CheatTrainer_V2.Models;

namespace MGS2_CheatTrainer_V2.Views;

public partial class StringDetailView : UserControl
{
    public Mgs2Strings.Mgs2String GameString
    {
        get;
        set
        {
            field = value;
            Content = value.Tag;
            Name = value.Tag;
        }
    }

    [Obsolete("Do not use this method, it does not work. Use StringDetailView(Mgs2Strings.Mgs2String gameString) instead")]
    public StringDetailView()
    {
        throw new Exception("Cannot instantiate a StringDetailView without an Mgs2String.");
    }
    
    public StringDetailView(Mgs2Strings.Mgs2String gameString)
    {
        InitializeComponent();
        GameString = gameString;
    }
}