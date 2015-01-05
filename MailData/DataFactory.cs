using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Common.Utils;
using MailData.Model;

namespace MailData
{
    public class DataFactory : IDisposable
    {
        private OrmLiteConnectionFactory _dbFactory;
        private bool _dispose = false;
        private IDbConnection _db;

        public DataFactory(string connectionString)
        {
            var _dbFactory = new OrmLiteConnectionFactory(connectionString, false, SqlServerDialect.Provider);
            _db = _dbFactory.Open();
        }

        public void Initialize()
        {
            _db.CreateTableIfNotExists<MailDetails>();
        }

        public void AddMailMessage(MailDetails mailDetails)
        {
            _db.Insert<MailDetails>(mailDetails);
        }

        public List<MailDetails> GetMailByStatus(int status)
        {
            return _db.Select<MailDetails>().Where(n => n.Status == status).ToList();
        }

        public void UpdateMailStatus(int id, int status)
        {
            //_db.Update(new MailDetails{ Status = status}, n => n.Id == id);
            var mailDetails = _db.Select<MailDetails>().Single(n => n.Id == id);
            mailDetails.Status = status;
            _db.Update(mailDetails);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_dispose)
                return;

            if (disposing)
            {
                _db.Close();
            }

            _dispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
