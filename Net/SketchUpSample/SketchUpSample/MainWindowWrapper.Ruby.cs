using System;
using System.Runtime.InteropServices;
using RubyDll;
using VALUE = System.UInt32;
using ID = System.UInt32;

namespace SketchUpSample
{
    internal partial class MainWindowWrapper
    {
        public static VALUE klass = Ruby.Qundef;

        public static VALUE GetName(VALUE self)
        {
            IntPtr pointer = Ruby.Data_Get_Struct(self);
            GCHandle handle = GCHandle.FromIntPtr(pointer);
            MainWindowWrapper instance = (MainWindowWrapper) handle.Target;
            if (instance != null)
            {
                return Ruby.rb_str_new2(instance.Name);
            }
            return Ruby.Qnil;
        }

        public static VALUE SetName(VALUE self, VALUE val)
        {
            IntPtr pointer = Ruby.Data_Get_Struct(self);
            GCHandle handle = GCHandle.FromIntPtr(pointer);
            MainWindowWrapper instance = (MainWindowWrapper) handle.Target;
            if (instance != null)
            {
                Ruby.Check_Type(val, Ruby.T_STRING);
                instance.Name = Ruby.StringValuePtr(val);
                return val;
            }
            return Ruby.Qnil;
        }

        public static VALUE Allocate(VALUE klass)
        {
            try
            {
                VALUE obj = Ruby.Data_Wrap_Struct(klass, null, FreeFunc, IntPtr.Zero);
                return obj;
            }
            catch (Exception exception)
            {
                Ruby.rb_raise(Ruby.rb_eException, exception.Message);
                return Ruby.Qnil;
            }
        }

        public static void Deallocate(IntPtr obj)
        {
            if (obj == IntPtr.Zero)
            {
                return;
            }
            GCHandle handle = GCHandle.FromIntPtr(obj);
            try
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            catch (Exception exception)
            {
                Ruby.rb_raise(Ruby.rb_eException, exception.Message);
            }
        }

        public static VALUE Initialize(int nargs, VALUE[] args, VALUE self)
        {
            try
            {
                if (nargs > 1)
                {
                    Ruby.rb_raise(Ruby.rb_eArgError, "wrong number of arguments");
                    return Ruby.Qnil;
                }
                string name = "";
                if (nargs == 1)
                {
                    VALUE arg0 = args[0];
                    Ruby.Check_Type(arg0, Ruby.T_STRING);
                    name = Ruby.StringValuePtr(arg0);
                }
                MainWindowWrapper instance = new MainWindowWrapper(name);
                GCHandle handle = GCHandle.Alloc(instance, GCHandleType.Normal);
                IntPtr pointer = GCHandle.ToIntPtr(handle);
                Ruby.SET_DATA_PTR(self, pointer);

                return self;
            }
            catch (Exception exception)
            {
                Ruby.rb_raise(Ruby.rb_eException, exception.Message);
                return Ruby.Qnil;
            }
        }

        #region Ruby Free Function

        private static Delegate _freeFunc;

        public static Delegate FreeFunc
        {
            get
            {
                if (_freeFunc == null)
                    SetFreeFunc(Deallocate);
                return _freeFunc;
            }
        }

        private static void SetFreeFunc(Ruby.RUBY_DATA_FUNC free)
        {
            _freeFunc = free;
        }

        #endregion
    }
}