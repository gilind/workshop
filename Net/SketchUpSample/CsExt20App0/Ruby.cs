using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;  // for DllImport
using System.Text;
using VALUE = System.UInt32; // equivalent to C unsigned long 
using ID = System.UInt32;  // equivalent to C unsigned long 

namespace RubyDll
{
    public static class Ruby
    {

        /// the path of ruby dll
        /// We are using the one loaded by SketchUp.
        /// It is at the same directory as SketchUp excution (SketchUp.exe).
        /// for SketchUp 2013, the default folder is "C:\Program Files (x86)\SketchUp\SketchUp 2013"
        //public const string RubyDll = @"C:\Program Files (x86)\SketchUp\SketchUp 2013\msvcrt-ruby18";
        public const string RubyDll = @"msvcrt-ruby200";
        
        //
        // 
        //
        public static Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }
        private static Encoding encoding = Encoding.UTF8;

        // 
        // convert string to byte[]
        //
        private static byte[] ToCBytes(string str)
        {
            return Encoding.GetBytes(str + '\0');
        }

        //
        // Q*
        //
        public const VALUE Qfalse = (VALUE)0;
        public const VALUE Qtrue = (VALUE)2;
        public const VALUE Qnil = (VALUE)4;
        public const VALUE Qundef = (VALUE)6;

        //
        // T_*
        // 
        public const int T_NONE   = 0x00;
        
        public const int T_OBJECT = 0x01;
        public const int T_CLASS  = 0x02;
        public const int T_MODULE = 0x03;
        public const int T_FLOAT  = 0x04;
        public const int T_STRING = 0x05;
        public const int T_REGEXP = 0x06;
        public const int T_ARRAY  = 0x07;
        public const int T_HASH   = 0x08;
        public const int T_STRUCT = 0x09;
        public const int T_BIGNUM = 0x0a;
        public const int T_FILE   = 0x0b;
        public const int T_DATA   = 0x0c;
        public const int T_MATCH  = 0x0d;
        public const int T_COMPLEX  = 0x0e;
        public const int T_RATIONAL = 0x0f;
        
        public const int T_NIL    = 0x11;
        public const int T_TRUE   = 0x12;
        public const int T_FALSE  = 0x13;
        public const int T_SYMBOL = 0x14;
        public const int T_FIXNUM = 0x15;

        public const int T_UNDEF  = 0x1b;
        public const int T_NODE   = 0x1c;
        public const int T_ICLASS = 0x1d;
        public const int T_ZOMBIE = 0x1e;

        public const int T_MASK   = 0x1f;

        //
        // Symbol
        //
        private static VALUE rb_cObject_ = Qundef;
        public static VALUE rb_cObject
        {
            get
            {
                if (rb_cObject_ == Qundef)
                    rb_cObject_ = rb_eval_string("Object");
                return rb_cObject_;
            }
        }
        private static VALUE rb_mKernel_ = Qundef;
        public static VALUE rb_mKernel
        {
            get
            {
                if (rb_mKernel_ == Qundef)
                    rb_mKernel_ = rb_eval_string("Kernel");
                return rb_mKernel_;
            }
        }
        private static ID id_puts_ = Qundef;
        public static ID id_puts
        {
            get
            {
                if (id_puts_ == Qundef)
                    id_puts_ = rb_intern("puts");
                return id_puts_;
            }
        }
        
//  rb_eException      = rb_define_class("Exception", rb_cObject);
//  rb_eSystemExit     = rb_define_class("SystemExit", rb_eException);
//  rb_eFatal          = rb_define_class("fatal", rb_eException);
//  rb_eSignal         = rb_define_class("SignalException", rb_eException);
//  rb_eInterrupt      = rb_define_class("Interrupt", rb_eSignal);
//  rb_eStandardError = rb_define_class("StandardError", rb_eException);
//  rb_eTypeError     = rb_define_class("TypeError", rb_eStandardError);
//  rb_eArgError      = rb_define_class("ArgumentError", rb_eStandardError);
//  rb_eIndexError    = rb_define_class("IndexError", rb_eStandardError);
//  rb_eKeyError      = rb_define_class("KeyError", rb_eIndexError);
//  rb_eRangeError    = rb_define_class("RangeError", rb_eStandardError);
        private static VALUE rb_eException_ = Qundef;
        public static VALUE rb_eException
        {
            get
            {
                if (rb_eException_ == Qundef)
                    rb_eException_ = rb_eval_string("Exception");
                return rb_eException_;
            }
        }
        private static VALUE rb_eArgError_ = Qundef;
        public static VALUE rb_eArgError
        {
            get
            {
                if (rb_eArgError_ == Qundef)
                    rb_eArgError_ = rb_eval_string("ArgumentError");
                return rb_eArgError_;
            }
        }
        private static VALUE rb_eTypeError_ = Qundef;
        public static VALUE rb_eTypeError
        {
            get
            {
                if (rb_eTypeError_ == Qundef)
                    rb_eTypeError_ = rb_eval_string("TypeError");
                return rb_eTypeError_;
            }
        }



        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern ID rb_intern(byte[] name);
        public static ID rb_intern(string name)
        {
            return rb_intern(ToCBytes(name));
        }
        [DllImport(RubyDll, EntryPoint = "rb_to_id", CallingConvention = CallingConvention.Cdecl)]
        private static extern ID rb_to_id_(VALUE symbol);
        public static ID rb_to_id(VALUE symbol)
        {
            Check_Type(symbol, T_SYMBOL);
            return rb_to_id_(symbol);
        }

        // 
        // rb_num2*
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 rb_num2int(VALUE val);

        //rb_ *2num
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern VALUE rb_int2inum(System.Int32 val);

        //
        // rb_eval*
        // 
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_eval_string(byte[] scriptString);
        public static VALUE rb_eval_string(string scriptString)
        {
            return rb_eval_string(ToCBytes(scriptString));
        }

        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern VALUE rb_eval_string_protect(byte[] script, ref VALUE state); // (5)
        public static VALUE rb_eval_string_protect(string script, ref VALUE state) // (6)
        {
            return rb_eval_string_protect(Encoding.UTF8.GetBytes(script + '\0'), ref state);
        }

        //
        // Callbacks
        //
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate VALUE RubyValueFunc0(VALUE self);
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate VALUE RubyValueFunc1(VALUE self, VALUE arg1);
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate VALUE RubyValueFunc2(VALUE self, VALUE arg1, VALUE arg2);
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate VALUE RubyValueFunc3(VALUE self, VALUE arg1, VALUE arg2, VALUE arg3);
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate VALUE RubyValueFuncM1(int argc, VALUE[] argv, VALUE obj);
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate void  RUBY_DATA_FUNC(IntPtr self);

        //
        // module, class
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern VALUE rb_define_module(string name);

        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern VALUE rb_define_class_under(VALUE module, string name, VALUE super);

        //
        // module method
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_module_function_(VALUE module, string name, RubyValueFunc0 func, int argc);
        public static void rb_define_module_function(VALUE module, string name, RubyValueFunc0 func, int agrc)
        {
            MethodDelegates.Add(func);
            rb_define_module_function_(module, name, func, agrc);
        }

        //
        // singleton Methods
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_singleton_method(VALUE obj, string name, RubyValueFunc0 func, int argc);
        public static void rb_define_singleton_method(VALUE obj, string name, RubyValueFunc0 func)
        {
            MethodDelegates.Add(func);
            rb_define_singleton_method(obj, name, func, 0);
        }

        //
        // alloc methods
        //
        [DllImport(RubyDll, EntryPoint = "rb_define_alloc_func", CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_alloc_func_(VALUE klass, RubyValueFunc0 func);
        public static void rb_define_alloc_func(VALUE klass, RubyValueFunc0 allocFunc)
        {
            MethodDelegates.Add(allocFunc);
            rb_define_alloc_func_(klass, allocFunc);
        }

        //
        // member methods
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_method(VALUE klass, string name, RubyValueFunc0 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_method(VALUE klass, string name, RubyValueFunc1 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_method(VALUE klass, string name, RubyValueFunc2 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_method(VALUE klass, string name, RubyValueFunc3 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_method(VALUE klass, string name, RubyValueFuncM1 func, int argc);
        
        public static void rb_define_method(VALUE klass, string name, RubyValueFunc0 func)
        {
            MethodDelegates.Add(func);
            rb_define_method(klass, name, func, 0);
        }
        public static void rb_define_method(VALUE klass, string name, RubyValueFunc1 func)
        {
            MethodDelegates.Add(func);
            rb_define_method(klass, name, func, 1);
        }
        public static void rb_define_method(VALUE klass, string name, RubyValueFunc2 func)
        {
            MethodDelegates.Add(func);
            rb_define_method(klass, name, func, 2);
        }
        public static void rb_define_method(VALUE klass, string name, RubyValueFunc3 func)
        {
            MethodDelegates.Add(func);
            rb_define_method(klass, name, func, 3);
        }
        public static void rb_define_method(VALUE klass, string name, RubyValueFuncM1 func)
        {
            MethodDelegates.Add(func);
            rb_define_method(klass, name, func, -1);
        }


        //
        // member private methods
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_private_method(VALUE klass, string name, RubyValueFunc0 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_private_method(VALUE klass, string name, RubyValueFunc1 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_private_method(VALUE klass, string name, RubyValueFunc2 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_private_method(VALUE klass, string name, RubyValueFunc3 func, int argc);
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_define_private_method(VALUE klass, string name, RubyValueFuncM1 func, int argc);

        public static void rb_define_private_method(VALUE klass, string name, RubyValueFunc0 func)
        {
            MethodDelegates.Add(func);
            rb_define_private_method(klass, name, func, 0);
        }
        public static void rb_define_private_method(VALUE klass, string name, RubyValueFunc1 func)
        {
            MethodDelegates.Add(func);
            rb_define_private_method(klass, name, func, 1);
        }
        public static void rb_define_private_method(VALUE klass, string name, RubyValueFunc2 func)
        {

            MethodDelegates.Add(func);
            rb_define_private_method(klass, name, func, 2);
        }
        public static void rb_define_private_method(VALUE klass, string name, RubyValueFunc3 func)
        {

            MethodDelegates.Add(func);
            rb_define_private_method(klass, name, func, 3);
        }
        public static void rb_define_private_method(VALUE klass, string name, RubyValueFuncM1 func)
        {

            MethodDelegates.Add(func);
            rb_define_private_method(klass, name, func, -1);
        }

        //
        // exceptions
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_raise(VALUE exception, byte[] message);
        public static void rb_raise(VALUE exception, string message)
        {
            rb_raise(exception, ToCBytes(message));
        }

        //
        // string
        //
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern VALUE rb_str_new2(byte[] str);
        public static VALUE rb_str_new2(string str)
        {
            return rb_str_new2(ToCBytes(str));
        }
        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr rb_string_value_cstr(ref VALUE v_ptr); // (1)
        public static string StringValuePtr(VALUE v) // (2)
        {
            int length = 0;
            IntPtr ptr = rb_string_value_cstr(ref v); // (3)
            unsafe
            {
                byte* p = (byte*)ptr; // (4)
                while (*p != 0) // (5)
                {
                    length++;
                    p++;
                }
            }
            byte[] bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length); // (6)
            return Encoding.UTF8.GetString(bytes);
        }

        //
        // funcall
        //
        [DllImport(RubyDll, EntryPoint = "rb_funcall", CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_funcall_(VALUE recv, ID mid, int argc);
        [DllImport(RubyDll, EntryPoint = "rb_funcall", CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_funcall_(VALUE recv, ID mid, int argc, VALUE arg1);
        [DllImport(RubyDll, EntryPoint = "rb_funcall", CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_funcall_(VALUE recv, ID mid, int argc, VALUE arg1, VALUE arg2);
        [DllImport(RubyDll, EntryPoint = "rb_funcall", CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_funcall_(VALUE recv, ID mid, int argc, VALUE arg1, VALUE arg2, VALUE arg3);
        [DllImport(RubyDll, EntryPoint = "rb_funcall", CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_funcall_(VALUE recv, ID mid, int argc, VALUE arg1, VALUE arg2, VALUE arg3, VALUE arg4);
        [DllImport(RubyDll, EntryPoint = "rb_funcall", CallingConvention = CallingConvention.Cdecl)]
        private static extern VALUE rb_funcall_(VALUE recv, ID mid, int argc, VALUE arg1, VALUE arg2, VALUE arg3, VALUE arg4, VALUE arg5);
        public static VALUE rb_funcall(VALUE recv, ID mid)
        {
            return rb_funcall_(recv, mid, 0);
        }
        public static VALUE rb_funcall(VALUE recv, ID mid, VALUE arg1)
        {
            return rb_funcall_(recv, mid, 1, arg1);
        }
        public static VALUE rb_funcall(VALUE recv, ID mid, VALUE arg1, VALUE arg2)
        {
            return rb_funcall_(recv, mid, 2, arg1, arg2);
        }
        public static VALUE rb_funcall(VALUE recv, ID mid, VALUE arg1, VALUE arg2, VALUE arg3)
        {
            return rb_funcall_(recv, mid, 3, arg1, arg2, arg3);
        }
        public static VALUE rb_funcall(VALUE recv, ID mid, VALUE arg1, VALUE arg2, VALUE arg3, VALUE arg4)
        {
            return rb_funcall_(recv, mid, 4, arg1, arg2, arg3, arg4);
        }
        public static VALUE rb_funcall(VALUE recv, ID mid, VALUE arg1, VALUE arg2, VALUE arg3, VALUE arg4, VALUE arg5)
        {
            return rb_funcall_(recv, mid, 5, arg1, arg2, arg3, arg4, arg5);
        }

        //
        // ruby structs
        //
        [StructLayout(LayoutKind.Sequential)]
        public struct RBasic
        {
            public uint flags;
            public VALUE klass;

            internal RBasic(uint flags, VALUE klass)
            {
                this.flags = flags;
                this.klass = klass;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RData
        {
            public RBasic basic;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public RUBY_DATA_FUNC dmark;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public RUBY_DATA_FUNC dfree;
            public IntPtr data;

            internal RData(RBasic basic,
                RUBY_DATA_FUNC dmark,
                RUBY_DATA_FUNC dfree,
                IntPtr data)
            {
                this.basic = basic;
                this.dmark = dmark;
                this.dfree = dfree;
                this.data = data;
            }
        }


        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void rb_check_type(VALUE val, int type);
        public static void Check_Type(VALUE val, int type)
        {
            rb_check_type(val, type);
        }

        public static IntPtr DATA_PTR(VALUE dta)
        {
            //return ((RData*)dta)->data;
            RData dtaVal = (RData)Marshal.PtrToStructure((IntPtr)dta, typeof(RData));
            return dtaVal.data;
        }

        public static void SET_DATA_PTR(VALUE dta, IntPtr dataPtr)
        {
            int offset = Marshal.SizeOf(typeof(Ruby.RData)) - Marshal.SizeOf(typeof(IntPtr));
            Marshal.WriteIntPtr((IntPtr)dta, offset, dataPtr);
        }

        public static Delegate DATA_FREEFUNC(VALUE dta)
        {
            //return ((RData*)dta)->data;
            RData dtaVal = (RData)Marshal.PtrToStructure((IntPtr)dta, typeof(RData));
            return dtaVal.dfree;
        }

        [DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        public static extern VALUE rb_data_object_alloc(VALUE klass, IntPtr datap, Delegate dmark, Delegate dfree);

        public static VALUE Data_Wrap_Struct(VALUE klass, Delegate mark, Delegate free, IntPtr sval)
        {
            return rb_data_object_alloc(klass, sval, mark, free);
        }

        public static IntPtr Data_Get_Struct(VALUE obj)
        {
            Check_Type(obj, T_DATA);
            return DATA_PTR(obj);
        }

        private static List<Delegate> MethodDelegates = new List<Delegate>();

        /*
         * ruby.h
         * 
         * 
         * struct RData {
         *     struct RBasic basic;
         *     void (*dmark) _((void*));
         *     void (*dfree) _((void*));
         *     void *data;
         * };
         * 
         * #define DATA_PTR(dta) (RDATA(dta)->data)
         * 
         * typedef void (*RUBY_DATA_FUNC) _((void*));
         * 
         * VALUE rb_data_object_alloc _((VALUE,void*,RUBY_DATA_FUNC,RUBY_DATA_FUNC));
         * 
         * #define Data_Wrap_Struct(klass,mark,free,sval)\
         *     rb_data_object_alloc(klass,sval,(RUBY_DATA_FUNC)mark,(RUBY_DATA_FUNC)free)
         * 
         * #define Data_Make_Struct(klass,type,mark,free,sval) (\
         *     sval = ALLOC(type),\
         *     memset(sval, 0, sizeof(type)),\
         *     Data_Wrap_Struct(klass,mark,free,sval)\
         * )
         * 
         * #define Data_Get_Struct(obj,type,sval) do {\
         *     Check_Type(obj, T_DATA); \
         *     sval = (type*)DATA_PTR(obj);\
         * } while (0)
         * 
         * 
         */

        //[DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr ruby_xmalloc(int size);

        //[DllImport("msvcrt.dll")]
        //public static extern IntPtr memset(IntPtr dest, int c, int count);

        //[DllImport(RubyDll, CallingConvention = CallingConvention.Cdecl)]
        //public static extern VALUE((T*) xmalloc(sizeof (T)));

        //public static IntPtr ALLOC(Type type)
        //{
        //     return ruby_xmalloc(Marshal.SizeOf(type));
        //}

    }
}
