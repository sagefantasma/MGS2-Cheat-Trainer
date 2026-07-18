using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MGS2_CheatTrainer_V2.Models;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace MGS2_CheatTrainer_V2.Views;

public partial class BossesTabView : UserControl
{
    private readonly Mgs2MemoryManager _memoryManager;
    private CancellationTokenSource? _cts;
    public static event EventHandler<string>? UpdateStatusBar;
    public static event EventHandler<Constants.Boss>? ActiveBoss;
    
    public BossesTabView()
    {
        InitializeComponent();
        _memoryManager = App.Services.GetRequiredService<Mgs2MemoryManager>();
        MainWindow.TabActivated += OnTabEntered;
    }

    private static void BossIsActive(Constants.Boss boss)
    {
        ActiveBoss?.Invoke(null, boss);
    }
    
    private static void RequestStatusBarUpdate(object? obj, string message)
    {
        UpdateStatusBar?.Invoke(null, message);
    }
    
    private void OnTabEntered(object? sender, Tab activated)
    {
        if (activated == Tab.Bosses)
        {
            _cts = new CancellationTokenSource();
            CancellationToken cancellationToken = _cts.Token;
            Task.Run(() => PeriodicTask.Run(CheckForBosses, TimeSpan.FromSeconds(1), cancellationToken), cancellationToken);
        }
        else
        {
            _cts?.Cancel();
        }
    }

    private void CheckForBosses()
    {
        Dispatcher.UIThread.Post(()=>
        {
            try
            {
                Stage currentStage = _memoryManager.GetStage();
                if (currentStage.AreaCode == StageNames.TankerStages.OlgaFight.AreaCode)
                {
                    BossIsActive(Constants.Boss.Olga);
                    RequestStatusBarUpdate(null, "Olga fight detected!");
                    OlgaDetailView.IsActive = true;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.SeaDockFortune.AreaCode)
                {
                    BossIsActive(Constants.Boss.Fortune);
                    //RequestStatusBarUpdate(null, "Fortune fight detected!"); //NOTE: uncomment if Fortune gets fixed
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false; //NOTE: if the bug with Fortune values gets fixed, set this to true
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.HeliportBomb.AreaCode)
                {
                    BossIsActive(Constants.Boss.Fatman);
                    RequestStatusBarUpdate(null, "Fatman fight detected!");
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = true;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.ShellsConnectingBridge.AreaCode)
                {
                    BossIsActive(Constants.Boss.Harrier);
                    //RequestStatusBarUpdate(null, "Harrier fight detected!"); //NOTE: Uncomment if Harrier gets fixed
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive =
                        false; //NOTE: If the bug with modifying Harrier HP gets fixed, set this to true 
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.Shell2FiltrationChamber2.AreaCode)
                {
                    BossIsActive(Constants.Boss.Vamp);
                    RequestStatusBarUpdate(null, "Vamp 1 fight detected!");
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = true;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.OilFenceVamp.AreaCode)
                {
                    BossIsActive(Constants.Boss.VampSnipe);
                    RequestStatusBarUpdate(null, "Vamp 2 fight detected!");
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = true;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.Rectum.AreaCode)
                {
                    BossIsActive(Constants.Boss.Ray1);
                    RequestStatusBarUpdate(null, "RAYs fight detected!");
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = true;
                    SolidusDetailView.IsActive = false;
                }
                else if (currentStage.AreaCode == StageNames.PlantStages.FederalHall.AreaCode)
                {
                    BossIsActive(Constants.Boss.Solidus);
                    //RequestStatusBarUpdate(null, "Solidus fight detected!"); //NOTE: uncomment if Solidus fight gets fixed
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive =
                        false; //NOTE: If the bug with modifying Solidus values gets fixed, set this to true 
                }
                else
                {
                    RequestStatusBarUpdate(null, "Monitoring for boss fight...");
                    OlgaDetailView.IsActive = false;
                    FortuneDetailView.IsActive = false;
                    FatmanDetailView.IsActive = false;
                    HarrierDetailView.IsActive = false;
                    Vamp1DetailView.IsActive = false;
                    Vamp2DetailView.IsActive = false;
                    RaysDetailView.IsActive = false;
                    SolidusDetailView.IsActive = false;
                }
            }
            catch (Exception e)
            {
                RequestStatusBarUpdate(null, "Not currently in-game, not looking for bosses...");
                OlgaDetailView.IsActive = false;
                FortuneDetailView.IsActive = false;
                FatmanDetailView.IsActive = false;
                HarrierDetailView.IsActive = false;
                Vamp1DetailView.IsActive = false;
                Vamp2DetailView.IsActive = false;
                RaysDetailView.IsActive = false;
                SolidusDetailView.IsActive = false;
                //TODO: do smart things here.
            }
        });
    }
}