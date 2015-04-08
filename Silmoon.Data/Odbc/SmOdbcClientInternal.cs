using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.Odbc
{
    public abstract class SmOdbcClientInternal : SqlCommonTemplate
    {
        SmOdbcClient _dataSource;
        public SmOdbcClient DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
    }
}
