using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Forms;
using ReactiveUI;

namespace ReactiveUIWinformsBug
{
    public class Viewmodel : ReactiveObject
    {
        private bool _button1Enabled = true;
        public bool Button1Enabled
        {
            get => _button1Enabled;
            set => this.RaiseAndSetIfChanged(ref _button1Enabled, value);
        }
        private bool _button2Enabled = true;
        public bool Button2Enabled
        {
            get => _button2Enabled;
            set => this.RaiseAndSetIfChanged(ref _button2Enabled, value);
        }


        public ReactiveCommand<Unit, Unit> DisableButton1 { get; private set; }

        public ReactiveCommand<Unit, Unit> DisableButton2 { get; private set; }


        public Viewmodel()
        {
            // Create command with CanExecute condition
            var btn1Enabled = this.WhenAnyValue(x => x.Button1Enabled);
            DisableButton1 = ReactiveCommand.Create(() =>
            {
                Observable.Start(() =>
                {
                    // Disable the command. This automatically disables the UI via the command binding
                    Button1Enabled = false;
                }, RxApp.MainThreadScheduler);
            }, btn1Enabled);

            DisableButton2 = ReactiveCommand.Create(() =>
            {
                Observable.Start(() =>
                {
                    // this forces a refresh of the UI thread
                    Application.DoEvents();
                    // Set the Enabled state of the Button to false.
                    Button2Enabled = false;
                }, RxApp.MainThreadScheduler);
            });

            // Create a timer to show that the UI status is reset when the CanExecute state is true
            Observable.Interval(TimeSpan.FromSeconds(5), RxApp.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    Button1Enabled = Button2Enabled = true;
                });
        }
    }
}
