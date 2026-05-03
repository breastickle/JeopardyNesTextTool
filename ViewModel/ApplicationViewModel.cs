using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using JeopardyNesTextTool.Model;

namespace JeopardyNesTextTool.ViewModel
{
    public class ApplicationViewModel: INotifyPropertyChanged
    {

        private object _selectedBlock;
        private string _scriptFilePath;

        public string ScriptFilePath
        {
            get => _scriptFilePath;
            set
            {
                _scriptFilePath = value;
                OnPropertyChanged("ScriptFilePath");
            }
        }

        public Config ViewModelConfig { get; }
        public CommandsManager CommandsManager { get; }
        public List<StructuredTextBlock> ModelBlocks { get; set; } = new();

        public ObservableCollection<ViewModelGroup> ViewModelGroups { get; set; } = new();

        public object SelectedBlock
        {
            get => _selectedBlock;
            set
            {
                _selectedBlock = value;
                OnPropertyChanged("SelectedBlock");
            }
        }


        public ApplicationViewModel()
        {
            ViewModelConfig = new Config();
            CommandsManager = new CommandsManager(this);
        }


        private ICommand _selectedItemChangedCommand;

        public ICommand SelectedItemChangedCommand => _selectedItemChangedCommand ??= new RelayCommand(SelectedItemChanged);

        private void SelectedItemChanged(object selectedObject)
        {
            if (selectedObject is ViewModelGroup group)
            {
                SelectedBlock = group;
            }
            else if (selectedObject is ViewModelElement element)
            {
                SelectedBlock = element.ModelObject;
            }
            else
            {
                SelectedBlock = null;
            }
        }

        private DispatcherTimer _recalcTimer;

        public void NotifyTextChanged()
        {
            if (_recalcTimer == null)
            {
                _recalcTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(75) };
                _recalcTimer.Tick += OnRecalcTimerTick;
            }
            _recalcTimer.Stop();
            _recalcTimer.Start();
        }

        private void OnRecalcTimerTick(object sender, EventArgs e)
        {
            _recalcTimer.Stop();
            if (ModelBlocks != null && ModelBlocks.Count > 0)
            {
                GroupSizeCalculator.Recalculate(this);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public abstract class ViewModelElement : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public object ModelObject { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum BudgetState { Green, Yellow, Orange, Red }

    public class ViewModelGroup : ViewModelElement
    {
        public Group ConfigGroup { get; set; }
        public ObservableCollection<ViewModelBlock> ViewModelBlocks { get; set; }

        public uint ExpectedBytes => ConfigGroup?.InsertRange.Size ?? 0;

        private uint _actualBytes;
        public uint ActualBytes
        {
            get => _actualBytes;
            set
            {
                if (_actualBytes == value) return;
                _actualBytes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SlackBytes));
                OnPropertyChanged(nameof(BudgetState));
            }
        }

        public int SlackBytes => (int)ExpectedBytes - (int)ActualBytes;

        public BudgetState BudgetState
        {
            get
            {
                if (ExpectedBytes == 0) return BudgetState.Green;
                var ratio = (double)ActualBytes / ExpectedBytes;
                if (ratio > 1.0) return BudgetState.Red;
                if (ratio >= 0.99) return BudgetState.Orange;
                if (ratio >= 0.90) return BudgetState.Yellow;
                return BudgetState.Green;
            }
        }
    }

    public class ViewModelBlock : ViewModelElement
    {
        public ObservableCollection<ViewModelTopic> ViewModelTopics { get; set; }

        private uint _actualBytes;
        public uint ActualBytes
        {
            get => _actualBytes;
            set
            {
                if (_actualBytes == value) return;
                _actualBytes = value;
                OnPropertyChanged();
            }
        }
    }

    public class ViewModelTopic : ViewModelElement
    {
        public ObservableCollection<ViewModelQuestion> ViewModelQuestions { get; set; }

    }

    public class ViewModelQuestion : ViewModelElement
    {
        public Question ModelQuestion { get; set; }
    }
    public class PronounsNames : ObservableCollection<string>
    {
        public PronounsNames()
        {
            Add("WHO IS");
            Add("WHO IS THE");
            Add("WHO ARE");
            Add("WHO ARE THE");
            Add("WHAT IS");
            Add("WHAT IS THE");
            Add("WHAT IS A");
            Add("WHAT IS AN");
            Add("WHAT ARE");
            Add("WHAT ARE THE");
            Add("WHO WAS");
            Add("WHAT WAS");
            Add("WHAT WAS THE");
            Add("WHO WERE");
            Add("WHO WERE THE");
            Add("WHO WAS A");
        }
    }
}
