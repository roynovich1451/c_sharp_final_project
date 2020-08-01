using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace createFourInARowDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog= FourInARowDB; 
                AttachDBFilename=c:\fourinrow\databases\fourinrowDB_roy_novich_oren_or.mdf; Integrated Security=True";
            //string connectTo = @"server = (localdb)\MSSQLLocalDb; database = c:\fourinrow\databases\fourinrowDB_roy_novich_oren_or; 
              // trusted_connection = true";
            using (var ctx = new FourInARowContext(connectString))
            {
                ctx.Database.Initialize(force: true);
            }
        }
    }
}
