using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DisableButton1 = ReactiveCommand.Create(() =>
            {
                Observable.Start(() =>
                {
                    Button1Enabled = false;
                }, RxApp.MainThreadScheduler);
            });
            DisableButton2 = ReactiveCommand.Create(() =>
            {
                Observable.Start(() =>
                {
                    Application.DoEvents();
                    Button2Enabled = false;
                }, RxApp.MainThreadScheduler);
            });
        }
    }
}
