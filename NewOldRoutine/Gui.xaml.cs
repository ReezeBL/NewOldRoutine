using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using Loki.Common;
using Loki.Game;
using NewOldRoutine.DataModels;

namespace NewOldRoutine
{
    /// <summary>
    /// Логика взаимодействия для NewOldGui.xaml
    /// </summary>
    public partial class Gui : UserControl, INotifyPropertyChanged
    {
        private UserControl skillProviderControl;

        public UserControl SkillProviderControl
        {
            get => skillProviderControl;
            set
            {
                if(skillProviderControl != null)
                    ProviderGuiGrid.Children.Remove(skillProviderControl);
                skillProviderControl = value;
                if (skillProviderControl != null)
                    ProviderGuiGrid.Children.Add(skillProviderControl);
                OnPropertyChanged();
            }
        }

        public SkillLogicEntry SelectedEntry
        {
            set
            {
                SkillProviderControl = value.LogicProvider.Control; 
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SkillLogicEntry> Entries { get; } = new ObservableCollection<SkillLogicEntry>();

        public Gui()
        {
            InitializeComponent();
            this.SkillLogicProvidersDataGrid.ItemsSource = Entries;
        }

        public void SetProviders(IEnumerable<SkillLogicProvider> providers)
        {
            Entries.Clear();
            providers.Select(provider => new SkillLogicEntry(provider)).ForEach(Entries.Add);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshProviders(object sender, RoutedEventArgs e)
        {
            if(LokiPoe.StateManager.IsInGameStateActive)
                SetProviders(GeneralSettings.Providers);
        }
    }
}
