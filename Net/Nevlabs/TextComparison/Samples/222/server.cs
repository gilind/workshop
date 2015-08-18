// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
////////////////////////////////////////////////////////////////////////////////
// Void
//    This class represents a Missing Variant
////////////////////////////////////////////////////////////////////////////////
namespace System {
    
    using System;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public sealed class DBNull : ISerializable, IConvertible {
    
        //Package private constructor
        private DBNull(){
        }
 
        private DBNull(SerializationInfo info, StreamingContext context) {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DBNullSerial"));
        }
        
        public static readonly DBNull Value = new DBNull();
 
        [System.Security.SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            UnitySerializationHolder.GetUnitySerializationInfo(info, UnitySerializationHolder.NullUnity, null, null);
        }
    
        public override String ToString() {
            return String.Empty;
        }
    }
}