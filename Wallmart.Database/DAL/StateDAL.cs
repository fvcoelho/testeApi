using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;

namespace Wallmart.Database
{
    public enum StateRegionsEnum
    {
        Norte,
        Sul,
        Leste,
        Oeste
    }

    public class StateDAL
    {
        private string _connectionString;
        private string _fullPath;

        public StateDAL()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["wm"].ConnectionString;
            _fullPath = System.Configuration.ConfigurationManager.AppSettings["path"].ToString();

            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(_fullPath);
            if (!d.Exists)
                d.Create();
        }

        public StateDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public States ListAll()
        {
            const string METHOD = "ListAll";
            States r = new States();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.State.ListAll, cn, tr))
                    {
                        try
                        {
                            using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.SingleResult))
                            {
                                while (dr.Read())
                                    r.Add(GetStateFromReader(dr));

                                dr.Close();
                            }
                            // ok
                            tr.Commit();

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("{0}\r\n{1}\r\n{2}", "Erro executando o comando.", METHOD, ex.Message), ex);
                        }
                    }
                }
            }

            return r;
        }

        public States ListByName(string name)
        {
            const string METHOD = "ListByName";
            States r = new States();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.State.ListByName, cn, tr))
                    {
                        cm.Parameters.Add("name", SqlDbType.VarChar).Value = name;

                        try
                        {
                            using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.SingleResult))
                            {
                                while (dr.Read())
                                    r.Add(GetStateFromReader(dr));

                                dr.Close();
                            }
                            // ok
                            tr.Commit();

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("{0}\r\n{1}\r\n{2}", "Erro executando o comando.", METHOD, ex.Message), ex);
                        }
                    }
                }
            }

            return r;
        }

        public void RemoveById(int stateId)
        {
            const string METHOD = "RemoveById";

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.State.RemoveById, cn, tr))
                    {
                        cm.Parameters.Add("stateId", SqlDbType.VarChar).Value = stateId;

                        try
                        {
                            cm.ExecuteNonQuery();

                            Serialize(new State { stateId = stateId }, GetFullFileName(METHOD, stateId));

                            // ok
                            tr.Commit();

                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();

                            throw new Exception(String.Format("{0}\r\n{1}\r\n{2}", "Erro executando o comando.", METHOD, ex.Message), ex);
                        }
                    }
                }
            }
        }

        public State Add(State newState)
        {
            const string METHOD = "Add";
            State r = null;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.State.Add, cn, tr))
                    {
                        cm.Parameters.Add("name", SqlDbType.VarChar).Value = newState.name;
                        cm.Parameters.Add("country", SqlDbType.VarChar).Value = newState.country;
                        cm.Parameters.Add("abbreviation", SqlDbType.VarChar).Value = newState.abbreviation;
                        cm.Parameters.Add("regionId", SqlDbType.Int).Value = 1;

                        using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.SingleRow))
                        {

                            try
                            {
                                if (dr.Read())
                                {
                                    r = newState;
                                    r.stateId = Convert.ToInt32(dr.GetValue(0));
                                }
                                dr.Close();

                                Serialize(newState, GetFullFileName(METHOD, newState.stateId));

                                // ok
                                tr.Commit();

                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();

                                throw new Exception(String.Format("{0}\r\n{1}\r\n{2}", "Erro executando o comando.", METHOD, ex.Message), ex);
                            }
                        }
                    }
                }
            }

            return r;
        }

        public void UpdateById(State state)
        {
            const string METHOD = "UpdateById";

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.State.UpdateById, cn, tr))
                    {
                        cm.Parameters.Add("stateId", SqlDbType.Int).Value = state.stateId;
                        cm.Parameters.Add("name", SqlDbType.VarChar).Value = state.name;
                        cm.Parameters.Add("country", SqlDbType.VarChar).Value = state.country;
                        cm.Parameters.Add("abbreviation", SqlDbType.VarChar).Value = state.abbreviation;
                        cm.Parameters.Add("regionId", SqlDbType.Int).Value = 2;

                        try
                        {
                            cm.ExecuteNonQuery();

                            Serialize(state, GetFullFileName(METHOD, state.stateId));

                            // ok
                            tr.Commit();

                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();

                            throw new Exception(String.Format("{0}\r\n{1}\r\n{2}", "Erro executando o comando.", METHOD, ex.Message), ex);
                        }
                    }
                }
            }
        }

        private State GetStateFromReader(SqlDataReader dr)
        {
            State state = new State();
            state.stateId = dr.GetInt32(dr.GetOrdinal("stateId"));
            state.name = dr.GetString(dr.GetOrdinal("name"));
            state.abbreviation = dr.GetString(dr.GetOrdinal("abbreviation"));
            state.region = Enum.GetName(typeof(StateRegionsEnum), dr.GetInt32(dr.GetOrdinal("regionId")));
            state.country = dr.GetString(dr.GetOrdinal("country"));

            return state;
        }

        private void Serialize(State state, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(State));
            using (XmlWriter w = XmlWriter.Create(path))
                serializer.Serialize(w, state);

        }

        private string GetFullFileName(string method, int id)
        {
            return string.Concat(_fullPath, "\\State", method, "_", id.ToString(), ".xml");
        }

        private string GetRegionNameById(int regionId)
        {
            string r = "";

            //switch (regionId)
            //{
            //    case 1:
            //        r = "norte";
            //        break;
            //    case 2:
            //        r = "sul";
            //        break;
            //    case 3:
            //        r = "leste";
            //        break;
            //    case 4:
            //        r = "oeste";
            //        break;
            //    default:
            //        r = "erro";
            //        break;
            //}

            return r;
        }
    }
}
