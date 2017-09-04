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

        public SkillProviderView SelectedEntry
        {
            set
            {
                SkillProviderControl = value.LogicProvider.Control; 
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SkillProviderView> Entries => GeneralSettings.ProviderWrappers;

        public int CombatRange => GeneralSettings.Instance.CombatRange;

        public Gui()
        {
            InitializeComponent();
            SkillLogicProvidersDataGrid.ItemsSource = Entries;
        }

        public void SetProviders(IEnumerable<SkillLogicProvider> providers)
        {
            Entries.Clear();
            providers.Select(provider => new SkillProviderView(provider)).ForEach(Entries.Add);
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
                foreach (var providerWrapper in GeneralSettings.ProviderWrappers)
                {
                    providerWrapper.UpdateSkillList();
                }
        }
    }
}
