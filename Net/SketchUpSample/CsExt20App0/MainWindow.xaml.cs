using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using RubyDll;
using VALUE = System.UInt32; // equivalent to C unsigned long 
using ID = System.UInt32;  // equivalent to C unsigned long 

namespace CsExtTutorial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dispatcher _rubyThreadDispatcher = null;
        public MainWindow(Dispatcher dispatcher)
        {
            this._rubyThreadDispatcher = dispatcher;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>();

            progress.ProgressChanged += (a, b) =>
            {
                this.Dispatcher.Invoke(() => this.ProgressBar.Value = b);
            };

            await this.LongRunOpAsync(progress);
        }

        public Task LongRunOpAsync(IProgress<int> progress)
        {
            return Task.Run(() =>
            {
                progress.Report(0);
                VALUE active_model = RubyFuncCall<VALUE>(() => Ruby.rb_eval_string("Sketchup.active_model"));
                for (int i = 1; i <= 10; i++)
                {
                    active_model = RubyFuncCall<VALUE>(() => Ruby.rb_eval_string("for i in " + (i * 10 - 10) + ".." + (i * 10) + "; puts i.to_s; end;Sketchup.active_model"));
                    progress.Report(i * 10);
                }
                RubyBeginActionCall(() => Ruby.rb_funcall(Ruby.rb_mKernel, Ruby.id_puts, active_model));
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string title = this.Title;
            RubyBeginActionCall(() => Ruby.rb_eval_string("puts 'from CsExtApp0 Wpf Window " + title + "'"));
        }

        private void RubyBeginActionCall(Action action)
        {
            this._rubyThreadDispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }
        private TResult RubyFuncCall<TResult>(Func<TResult> func)
        {

            TResult result = this._rubyThreadDispatcher.Invoke(func);
            
            DispatcherOperation operation = this._rubyThreadDispatcher.BeginInvoke(DispatcherPriority.Normal, func);

            operation.Completed += (sender, args) => RubyCallCompleted<TResult>(operation, delegate { MessageBox.Show("End"); });

            return result;
        }


        private TResult RubyCallCompleted<TResult>(DispatcherOperation operation, Action<TResult> completed)
        {
            TResult result = (TResult)operation.Result;
            completed(result);

            return result;
        }

    }
}
