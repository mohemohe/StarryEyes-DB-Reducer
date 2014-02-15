using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Krile_DB_Reducer
{
    class Program
    {
        static string _DBdir = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Krile\";
        static string _DBfile = "krile.db";

        static void Main(string[] args)
        {
            ulong DataCount;
            ulong TargetID;
            string TargetDT;

            uint LeaveData = 10000;

            double d;
            if (args.Length == 2 && args[0].ToLower() == "-d" && double.TryParse(args[1], out d))
            {
                LeaveData = (uint)d;
            }

            Console.WriteLine("Reduce start..." + "\n");

            if ((DataCount = GetTotalRecords("Status")) > LeaveData)
            {
                using (var sqlc = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
                {
                    sqlc.Open();
                    using (SQLiteCommand cmd = sqlc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Status ORDER BY julianday(CreatedAt)";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT CreatedAt FROM Status LIMIT 1 OFFSET " + (DataCount - LeaveData);
                        TargetDT = cmd.ExecuteScalar().ToString();

                        DateTime dt = DateTime.Parse(TargetDT);
                        TargetDT = dt.ToString("yyyy-MM-dd HH:mm:ss");

                        cmd.CommandText = "DELETE FROM Status WHERE julianday(CreatedAt) < julianday((datetime('" + TargetDT + "')))";
                        cmd.ExecuteNonQuery();
                    }
                    sqlc.Close();
                }

                Console.WriteLine("Status reduced.");
            }

            if ((DataCount = GetTotalRecords("StatusEntity")) > LeaveData)
            {
                using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
                {
                    con.Open();
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM StatusEntity ORDER BY Id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT Id FROM StatusEntity LIMIT 1 OFFSET " + (DataCount - LeaveData);
                        TargetID = Convert.ToUInt64(cmd.ExecuteScalar());

                        cmd.CommandText = "DELETE FROM StatusEntity WHERE Id < " + TargetID;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                Console.WriteLine("StatusEntity reduced.");
            }

            using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
            {
                con.Open();
                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserUrlEntity";
                    cmd.ExecuteNonQuery();
                }
                con.Close();

                Console.WriteLine("UserUrlEntity reduced.");
            }

            if ((DataCount = GetTotalRecords("Favorites")) > LeaveData)
            {
                using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
                {
                    con.Open();
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Favorites ORDER BY Id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT Id FROM Favorites LIMIT 1 OFFSET " + (DataCount - LeaveData);
                        TargetID = Convert.ToUInt64(cmd.ExecuteScalar());

                        cmd.CommandText = "DELETE FROM Favorites WHERE Id < " + TargetID;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                Console.WriteLine("Favorites reduced.");
            }

            if ((DataCount = GetTotalRecords("Retweets")) > LeaveData)
            {
                using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
                {
                    con.Open();
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Retweets ORDER BY Id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT Id FROM Retweets LIMIT 1 OFFSET " + (DataCount - LeaveData);
                        TargetID = Convert.ToUInt64(cmd.ExecuteScalar());

                        cmd.CommandText = "DELETE FROM Retweets WHERE Id < " + TargetID;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                Console.WriteLine("Retweets reduced.");
            }

            using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
            {
                con.Open();
                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM User";
                    cmd.ExecuteNonQuery();
                }
                con.Close();

                Console.WriteLine("User reduced.");
            }

            Console.WriteLine("VACUUM start...");
            using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
            {
                con.Open();
                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "VACUUM";
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            Console.WriteLine("VACUUM done.");

            Console.WriteLine("\n" + "Reduced!");
        }

        static ulong GetTotalRecords(string TableName)
        {
            UInt64 count;

            using (var con = new SQLiteConnection("Data Source=" + _DBdir + _DBfile))
            {
                con.Open();
                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM " + TableName + "";
                    count = Convert.ToUInt64(cmd.ExecuteScalar());
                }
                con.Close();
            }

            return count;
        }
    }
}
