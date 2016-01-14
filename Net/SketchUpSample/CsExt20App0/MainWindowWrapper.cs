using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using RubyDll;

namespace CsExtTutorial
{
    internal partial class MainWindowWrapper
    {
        public string Name { get; set; }

        public MainWindow _window = null;

        public MainWindowWrapper(string name)
        {
            this.Name = name;
            InitializeWindow();
        }
        ~MainWindowWrapper()
        {
        }

        private static Object windowLock = new Object();
        public void InitializeWindow()
        {
            Dispatcher rubyThreadDispatcher = Dispatcher.CurrentDispatcher;

            Thread thread = new Thread(() =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                lock (windowLock)
                {
                    _window = new MainWindow(rubyThreadDispatcher);
                    _window.Title = this.Name;
                }

                _window.Closed += (sender2, e2) =>
                {
                    _window.Dispatcher.InvokeShutdown();
                };
                _window.Show();

                System.Windows.Threading.Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            thread.Start();

            for (int waited = 0; waited < 1000 && _window == null; waited += 10)
            {
                System.Threading.Thread.Sleep(10);
            }
        }

        public void ShowMainWindow()
        {
            if (_window == null) return;
            _window.Dispatcher.BeginInvoke((Action)(() =>
            {
                _window.Show();
                _window.Activate();
            }), System.Windows.Threading.DispatcherPriority.ContextIdle, null);
        }

    }
}
