using System;
using System.Drawing;
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

        private Color _button1Colour = Color.FromKnownColor(KnownColor.Control);
        public Color Button1Colour
        {
            get => _button1Colour;
            set => this.RaiseAndSetIfChanged(ref _button1Colour, value);
        }

        private Color _button2Colour = Color.FromKnownColor(KnownColor.Control);
        public Color Button2Colour
        {
            get => _button2Colour;
            set => this.RaiseAndSetIfChanged(ref _button2Colour, value);
        }

        public ReactiveCommand<Unit, Unit> DisableButton1 { get; private set; }

        public ReactiveCommand<Unit, Unit> DisableButton2 { get; private set; }


        public Viewmodel()
        {
            // Create command with CanExecute condition
            var btn1Enabled = this.WhenAnyValue(x => x.Button1Enabled);
            var newColour = 0;
            DisableButton1 = ReactiveCommand.Create(() =>
            {
                // Disable the command. This automatically disables the UI via the command binding
                // NOTE: execution happens on the taskpool thread, result returns on the UI thread.
                Button1Enabled = false;

                // Change button colour
                newColour += 50;
                Button1Colour = Color.FromArgb(255, newColour, 0);
                if (newColour > 255) newColour = 0;
            }, btn1Enabled);

            DisableButton2 = ReactiveCommand.Create(() =>
            {
                Observable.Start(() =>
                {
                    // this forces a refresh of the UI thread, but requires a UI thread to Execute correctly
                    Application.DoEvents();
                    // Set the Enabled state of the Button to false.
                    Button2Enabled = false;
                    Button2Colour = Color.FromArgb(255, newColour, 0);
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
