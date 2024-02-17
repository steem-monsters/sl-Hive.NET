using sl_Hive.Attributes;
using sl_Hive.Models;
using System.Reflection;

namespace sl_Hive.Engine
{
    public class SignatureSerializer
    {
        private static void WriteVarint32(MemoryStream oStream, uint n)
        {
            while (n >= 0x80)
            {
                oStream.WriteByte(Convert.ToByte(0x80 | n & 0x7F));
                n = n >> 7;
            }
            oStream.WriteByte(Convert.ToByte(n));
        }
        private void AddToStream(MemoryStream oStream, object obj)
        {
            byte[] buf;

            if (obj == null) return;
            switch (obj)
            {
                case bool value:
                    oStream.WriteByte((byte)(value ? 1 : 0));
                    break;
                case byte value:
                    oStream.WriteByte(value);
                    break;
                case short value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case ushort value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case int value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case uint value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case long value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case ulong value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case float value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case double value:
                    buf = BitConverter.GetBytes(value);
                    oStream.Write(buf, 0, buf.Length);
                    break;

                case byte[] value:
                    oStream.Write(value, 0, value.Length);
                    break;
                case string value:
                    if (string.IsNullOrEmpty(value))
                    {
                        oStream.WriteByte(0);
                        break;
                    }
                    buf = System.Text.Encoding.UTF8.GetBytes(value);
                    uint len = Convert.ToUInt32(buf.Length);
                    WriteVarint32(oStream, len);
                    oStream.Write(buf, 0, (int)len);
                    break;
                case DateTime value:
                    buf = BitConverter.GetBytes(Convert.ToUInt32(value.Ticks / 10000000 - 62135596800)); //01.01.1970
                    oStream.Write(buf, 0, buf.Length);
                    break;
                case object[] value:
                    WriteVarint32(oStream, (uint)value.Length);
                    if (value.Length == 0) break;
                    foreach (object item in value)
                    {
                        Serialize(oStream, item);
                    }
                    break;
                case Dictionary<string, ushort> value:
                    WriteVarint32(oStream, (uint)value.Count);
                    if (value.Count == 0) break;
                    foreach (KeyValuePair<string, ushort> item in value)
                    {
                        Serialize(oStream, item);
                    }
                    break;

                case object value:
                    Serialize(oStream, obj);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public byte[] Serialize(object obj)
        {
            using (MemoryStream oStream = new())
            {
                Serialize(oStream, obj);
                return oStream.ToArray();
            }
        }
        public void Serialize(MemoryStream oStream, object obj)
        {
            Type oType = obj.GetType();
            if (obj is IOperationID)
            {
                WriteVarint32(oStream, (uint)((IOperationID)obj).opid);
            }
            if (oType.Namespace == "System")
                AddToStream(oStream, obj);
            else
            {
                FieldInfo[] oFields = oType.Name == "KeyValuePair`2" ? (FieldInfo[])oType.GetTypeInfo().DeclaredFields : oType.GetFields();
                foreach (FieldInfo oField in oFields)
                {
                    if (!(oType.Name == "Transaction" && (oField.Name == "signatures" || oField.Name == "txid")))
                    {
                        if (oField.FieldType.Namespace == "System.Collections.Generic" && oField.GetValue(obj) == null)
                            WriteVarint32(oStream, 0);
                        else
                        {
                            if (Attribute.IsDefined(oField, typeof(OptionalField)))
                            {
                                if (oField.GetValue(obj) == null)
                                {
                                    oStream.WriteByte(0);
                                    continue;
                                }
                                else
                                    oStream.WriteByte(1);
                            }
                            if (oField.GetValue(obj) == null)
                                throw new ArgumentNullException(oField.Name);
                            AddToStream(oStream, oField.GetValue(obj));
                        }
                    }
                }
            }
        }
    }
}

