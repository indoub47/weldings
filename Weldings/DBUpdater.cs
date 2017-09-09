using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using Weldings.Properties;

namespace Weldings
{
    public static class DBUpdater
    {
        public static int DoPirmieji(List<WeldingInspection> wiList)
        {
            int count;
            using (OleDbConnection conn = new OleDbConnection(string.Format(Settings.Default.OleDbConnectionString, Settings.Default.AccessDbPath)))
            {
                OleDbTransaction trans = null;
                OleDbCommand cmd = new OleDbCommand
                {
                    Connection = conn
                };
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = trans;
                    count = InsertPirmieji(wiList, cmd);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        // Attempt to roll back the transaction.
                        trans.Rollback();
                        throw new DbUpdateException(Messages.Default.DBUpdateFailTransOK, ex, true, SheetType.pirmieji);
                    }
                    catch
                    {
                        throw new DbUpdateException(Messages.Default.DBUpdateFailTransFail, ex, false, SheetType.pirmieji);
                    }
                }
            }
            return count;
        }

        private static int InsertPirmieji(List<WeldingInspection> inspectionList, OleDbCommand cmd)
        {
            string sql = "INSERT INTO ssd (Linia, Kel, kilomrtras, piket, metras, siule, [saliginis kodas], suv_numer, suvirino, IFas, Pak_suv_data, I_pat_data, I_pat_aparat, I_pat_operator, Pastaba) "
            + "VALUES (@Linia, @Kel, @kilomrtras, @piket, @metras, @siule, @saliginis_kodas, @suv_numer, @suvirino, @IFas, @Pak_suv_data, @I_pat_data, @I_pat_aparat, @I_pat_operator, @Pastaba)";
            cmd.CommandText = sql;
            int count = 0;
            if (inspectionList.Count == 0) return count;
            foreach (WeldingInspection wi in inspectionList)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Linia", wi.Linija);
                cmd.Parameters.AddWithValue("@Kel", wi.Kelias);
                cmd.Parameters.AddWithValue("@kilomrtras", wi.Km);
                AddNullableParam(cmd.Parameters, wi.Pk, "@piket");
                AddNullableParam(cmd.Parameters, wi.M, "@metras");
                AddNullableParam(cmd.Parameters, wi.Siule, "@siule");
                cmd.Parameters.AddWithValue("@saliginis_kodas", wi.SalygKodas);
                AddNullableParam(cmd.Parameters, wi.Nr, "@suv_numer");
                cmd.Parameters.AddWithValue("@suvirino", wi.Suvirino);
                cmd.Parameters.AddWithValue("@IFas", Settings.Default.Ifas);
                cmd.Parameters.AddWithValue("@Pak_suv_data", wi.TikrinimoData);
                cmd.Parameters.AddWithValue("@I_pat_data", wi.TikrinimoData);
                cmd.Parameters.AddWithValue("@I_pat_aparat", wi.Aparatas);
                cmd.Parameters.AddWithValue("@I_pat_operator", wi.Operatorius);
                cmd.Parameters.AddWithValue("@Pastaba", wi.Pastaba);

                count += cmd.ExecuteNonQuery();
            }
            return count; 
        }
        

        public static int DoNepirmieji(List<WeldingInspection> wiList)
        {
            int count;
            using (OleDbConnection conn = new OleDbConnection(string.Format(Settings.Default.OleDbConnectionString, Settings.Default.AccessDbPath)))
            {
                OleDbTransaction trans = null;
                OleDbCommand cmd = new OleDbCommand
                {
                    Connection = conn
                };
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = trans;
                    count = UpdateNepirmieji(wiList, cmd);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        // Attempt to roll back the transaction.
                        trans.Rollback();
                        throw new DbUpdateException(Messages.Default.DBUpdateFailTransOK, ex, true, SheetType.nepirmieji);
                    }
                    catch
                    {
                        throw new DbUpdateException(Messages.Default.DBUpdateFailTransFail, ex, false, SheetType.nepirmieji);
                    }
                }
            }
            return count;
        }

        private static int UpdateNepirmieji(List<WeldingInspection> inspectionList, OleDbCommand cmd)
        {
            string patData = "", aparat = "", operatorius = ""; // įrašomi tinkamų stulpelių pavadinimai
            int count = 0;
            if (inspectionList.Count == 0) return count;
            foreach (WeldingInspection wi in inspectionList)
            {
                cmd.Parameters.Clear();
                if (wi.KelintasTikrinimas == Kelintas.papildomas)
                {
                    cmd.CommandText = "UPDATE ssd SET Pastaba = Pastaba + @pastaba WHERE number = @id;";
                    cmd.Parameters.AddWithValue("@pastaba", wi.Pastaba);
                    cmd.Parameters.AddWithValue("@id", wi.Id);
                }
                else
                {
                    switch (wi.KelintasTikrinimas)
                    {
                        case Kelintas.II:
                            patData = "II_pat_data";
                            aparat = "II_pat_aparat";
                            operatorius = "II_pat_operator";
                            break;
                        case Kelintas.III:
                            patData = "III_pat_data";
                            aparat = "III_pat_aparat";
                            operatorius = "III_pat_operaqtor";
                            break;
                        case Kelintas.IV:
                            patData = "IV_pat_data";
                            aparat = "IV_pat_aparat";
                            operatorius = "IV_pat_operator";
                            break;
                    }

                    cmd.CommandText = "UPDATE ssd SET " + 
                        patData + " = @patdata, " + 
                        aparat + " = @aparat, " + 
                        operatorius + " = @operator WHERE number = @id";

                    cmd.Parameters.AddWithValue("@patdata", wi.TikrinimoData);
                    cmd.Parameters.AddWithValue("@aparat", wi.Aparatas);
                    cmd.Parameters.AddWithValue("@operator", wi.Operatorius);
                    cmd.Parameters.AddWithValue("@id", wi.Id);
                }

                count += cmd.ExecuteNonQuery();
            }
            return count;
        }

        public static void AddNullableParam(OleDbParameterCollection parameters, long? value, string paramName)
        {
            if (value == null) parameters.AddWithValue(paramName, DBNull.Value);
            else parameters.AddWithValue(paramName, value);
        }
    }
}
