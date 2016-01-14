using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using RubyDll;
using VALUE = System.UInt32; // equivalent to C unsigned long 
using ID = System.UInt32; // equivalent to C unsigned long 

namespace SketchUpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dispatcher _rubyThreadDispatcher;

        public MainWindow(Dispatcher dispatcher)
        {
            _rubyThreadDispatcher = dispatcher;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>();

            progress.ProgressChanged += (a, b) => { Dispatcher.Invoke(() => ProgressBar.Value = b); };

            await LongRunOpAsync(progress);
        }

        public Task LongRunOpAsync(IProgress<int> progress)
        {
            return Task.Run(() =>
            {
                progress.Report(0);
                VALUE active_model = RubyFuncCall(() => Ruby.rb_eval_string("Sketchup.active_model"));
                for (int i = 1; i <= 10; i++)
                {
                    active_model =
                        RubyFuncCall(
                            () =>
                                Ruby.rb_eval_string("for i in " + (i*10 - 10) + ".." + (i*10) +
                                                    "; puts i.to_s; end;Sketchup.active_model"));
                    progress.Report(i*10);
                }
                RubyBeginActionCall(() => Ruby.rb_funcall(Ruby.rb_mKernel, Ruby.id_puts, active_model));
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string title = Title;
            RubyBeginActionCall(() => Ruby.rb_eval_string("puts 'from CsExtApp0 Wpf Window " + title + "'"));
        }

        private void RubyBeginActionCall(Action action)
        {
            _rubyThreadDispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        private TResult RubyFuncCall<TResult>(Func<TResult> func)
        {
            TResult result = _rubyThreadDispatcher.Invoke(func);

            DispatcherOperation operation = _rubyThreadDispatcher.BeginInvoke(DispatcherPriority.Normal, func);

            operation.Completed += (sender, args) => RubyCallCompleted<TResult>(operation, delegate { MessageBox.Show("End"); });

            return result;
        }

        private TResult RubyCallCompleted<TResult>(DispatcherOperation operation, Action<TResult> completed)
        {
            TResult result = (TResult) operation.Result;
            completed(result);

            return result;
        }
    }
}