using System;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;

namespace SketchUpSample
{
    internal partial class MainWindowWrapper
    {
        private static Object windowLock = new Object();
        public MainWindow _window;

        public MainWindowWrapper(string name)
        {
            Name = name;
            InitializeWindow();
        }

        public string Name { get; set; }

        ~MainWindowWrapper()
        {
        }

        public void InitializeWindow()
        {
            Dispatcher rubyThreadDispatcher = Dispatcher.CurrentDispatcher;

            Thread thread = new Thread(() =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                lock (windowLock)
                {
                    _window = new MainWindow(rubyThreadDispatcher);
                    _window.Title = Name;
                }

                _window.Closed += (sender2, e2) => { _window.Dispatcher.InvokeShutdown(); };
                _window.Show();

                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            thread.Start();

            for (int waited = 0; waited < 1000 && _window == null; waited += 10)
            {
                Thread.Sleep(10);
            }
        }

        public void ShowMainWindow()
        {
            if (_window == null) return;
            _window.Dispatcher.BeginInvoke((Action) (() =>
            {
                _window.Show();
                _window.Activate();
            }), DispatcherPriority.ContextIdle, null);
        }
    }
}