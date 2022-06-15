using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReactiveUI;

namespace ReactiveUIWinformsBug
{
    public partial class Main : Form, IViewFor<Viewmodel>
    {
        private Viewmodel _vm;

        object IViewFor.ViewModel
        {
            get => _vm;
            set => _vm = (Viewmodel)value;
        }

        Viewmodel IViewFor<Viewmodel>.ViewModel
        {
            get => _vm;
            set => _vm = value;
        }

        public Main(Viewmodel vm)
        {
            _vm = vm;
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(_vm, v => v.Button1Colour, form => form.button1.BackColor));
                d(this.BindCommand(_vm, v => v.DisableButton1, form => form.button1));

                d(this.OneWayBind(_vm, v => v.Button2Colour, form => form.button2.BackColor));
                d(this.OneWayBind(_vm, v => v.Button2Enabled, form => form.button2.Enabled));
                d(this.BindCommand(_vm, v => v.DisableButton2, form => form.button2));
            });


        }
    }
}
