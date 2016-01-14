using RGiesecke.DllExport;
using RubyDll;
using VALUE = System.UInt32;
using ID = System.UInt32;


namespace SketchUpSample
{
    public static class CsExt
    {
        private const string ModuleName = "IHL_CsR20ExtApp0";
        private static VALUE mCsExt;

        [DllExport]
        public static VALUE Init_ihl_csr20extapp0()
        {
            mCsExt = Ruby.rb_define_module(ModuleName);

            #region define MainWindowWrapper

            MainWindowWrapper.klass = Ruby.rb_define_class_under(mCsExt, "MainWindow", Ruby.rb_cObject);
            Ruby.rb_define_alloc_func(MainWindowWrapper.klass, MainWindowWrapper.Allocate);
            Ruby.rb_define_method(MainWindowWrapper.klass, "initialize", MainWindowWrapper.Initialize);
            Ruby.rb_define_method(MainWindowWrapper.klass, "name", MainWindowWrapper.GetName);
            Ruby.rb_define_method(MainWindowWrapper.klass, "name=", MainWindowWrapper.SetName);

            #endregion

            //test
            //Ruby.rb_eval_string("for i in 0..90000; a = CsExtApp0::MainWindowWrapper.new 'a'+i.to_s; a.name = 'n'+i.to_s end");

            return mCsExt;
        }
    }
}