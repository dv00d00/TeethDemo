namespace Teeth.Demo
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Model;
    using Model.Domain;
    using Mvvm;
    using Properties;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string newGroupName;
        private Group selectedGroup;

        public void Initialize(ViewContext context)
        {
            this.ViewContext = context;
        }

        public MainWindowViewModel()
        {
            this.AddGroupCommand = new RelayCommand(() =>
            {
                var result = this.ViewContext.CaptureSelection(this.NewGroupName);
                this.ViewContext.AddGroup(result);
                this.Groups.Add(result);
                this.NewGroupName = "";
            });

            this.DeleteGroupCommand = new RelayCommand<Group>(arg =>
            {
                this.ViewContext.DeleteGroup(arg.Id);
                this.Groups.Remove(arg);
            });
        }

        public ViewContext ViewContext { get; private set; }

        public string NewGroupName
        {
            get { return this.newGroupName; }
            set
            {
                if (value == this.newGroupName) return;
                this.newGroupName = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Group> Groups { get; } = new ObservableCollection<Group>();

        public Group SelectedGroup
        {
            get { return this.selectedGroup; }
            set
            {
                if (Equals(value, this.selectedGroup)) return;
                this.selectedGroup = value;
                this.OnPropertyChanged();
                this.ViewContext.HandleSelectGroup(value.Id);
            }
        }

        public ICommand AddGroupCommand { get; }

        public ICommand DeleteGroupCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}