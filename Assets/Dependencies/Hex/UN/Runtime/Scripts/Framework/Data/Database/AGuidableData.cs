using HexUN.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexUN.Data
{
    public abstract class AGuidableData : IGuidable
    {
        protected Guid _guid;

        public Guid Guid => _guid;

        public AGuidableData()
        {
            _guid = Guid.NewGuid();
        }
    }
}
