using System.Data;
using System.Data.SqlServerCe;
using NHibernate.Driver;
using NHibernate.SqlTypes;

namespace FunnelWeb.Providers.Database.SqlCe
{

    /// <summary>
    /// Overridden Nhibernate SQL CE Driver,
    /// so that ntext fields are not truncated at 4000 characters
    /// </summary>
    public class FunnelWebSqlServerCeDriver : SqlServerCeDriver
    {
        protected override void InitializeParameter(IDbDataParameter dbParam,string name,SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            var stringType = sqlType as StringSqlType;
            if (stringType != null && stringType.LengthDefined && stringType.Length > 4000)
            {
                var parameter = (SqlCeParameter)dbParam;
                parameter.SqlDbType = SqlDbType.NText;
            }

        }
    }

}