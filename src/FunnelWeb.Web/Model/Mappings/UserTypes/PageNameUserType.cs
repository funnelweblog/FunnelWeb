using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model.Mappings.UserTypes
{
    [Serializable]
    public class PageNameUserType : IUserType
    {
        bool IUserType.Equals(object x, object y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var resultAsString = (string)NHibernateUtil.String.NullSafeGet(rs, names[0]);
            var result = (PageName)(resultAsString ?? string.Empty);
            return result;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            value = (PageName)(value ?? string.Empty);
            ((IDataParameter) cmd.Parameters[index]).Value = value.ToString();
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return target;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes
        {
            get 
            {
                return new[] {new StringSqlType()};
            }
        }

        public Type ReturnedType
        {
            get { return typeof (PageName); }
        }

        public bool IsMutable
        {
            get { return false; }
        }
    }
}