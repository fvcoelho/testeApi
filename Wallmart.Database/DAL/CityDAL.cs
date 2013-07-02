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

    public class CityDAL
    {
        private string _connectionString;
        private string _fullPath;

        public CityDAL()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["wm"].ConnectionString;
            _fullPath = System.Configuration.ConfigurationManager.AppSettings["path"];
        }

        public CityDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Cities ListAll()
        {
            const string METHOD = "ListAll";
            Cities r = new Cities();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.City.ListAll, cn, tr))
                    {
                        try
                        {
                            using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.SingleResult))
                            {
                                while (dr.Read())
                                    r.Add(GetCityFromReader(dr));

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

        public Cities ListByName(string name)
        {
            const string METHOD = "ListByName";
            Cities r = new Cities();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.City.ListByName, cn, tr))
                    {
                        cm.Parameters.Add("name", SqlDbType.VarChar).Value = name;

                        try
                        {
                            using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.SingleResult))
                            {
                                while (dr.Read())
                                    r.Add(GetCityFromReader(dr));

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

        public void RemoveById(int cityId)
        {
            const string METHOD = "RemoveById";

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.City.RemoveById, cn, tr))
                    {
                        cm.Parameters.Add("cityId", SqlDbType.VarChar).Value = cityId;

                        try
                        {
                            cm.ExecuteNonQuery();

                            Serialize(new City { cityId = cityId }, GetFullFileName(METHOD, cityId));

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

        public City Add(City newCity)
        {
            const string METHOD = "Add";
            City r = null;
            State s = newCity.state;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.City.Add, cn, tr))
                    {
                        cm.Parameters.Add("name", SqlDbType.VarChar).Value = newCity.name;
                        cm.Parameters.Add("isCapital", SqlDbType.Bit).Value = newCity.isCapital;
                        cm.Parameters.Add("stateId", SqlDbType.Int).Value = s.stateId;

                        using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            try
                            {
                                if (dr.Read())
                                {
                                    r = newCity;
                                    r.cityId = Convert.ToInt32(dr.GetValue(0));
                                }
                                dr.Close();

                                Serialize(newCity, GetFullFileName(METHOD, newCity.cityId));

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

        public void UpdateById(City city)
        {
            const string METHOD = "Update";

            State s = city.state;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlTransaction tr = cn.BeginTransaction())
                {
                    using (SqlCommand cm = new SqlCommand(Scripts.City.UpdateById, cn, tr))
                    {
                        cm.Parameters.Add("cityId", SqlDbType.Int).Value = city.cityId;
                        cm.Parameters.Add("name", SqlDbType.VarChar).Value = city.name;
                        cm.Parameters.Add("isCapital", SqlDbType.Bit).Value = city.isCapital;
                        cm.Parameters.Add("stateId", SqlDbType.Int).Value = s.stateId;

                        try
                        {
                            cm.ExecuteNonQuery();

                            Serialize(city, GetFullFileName(METHOD, city.cityId));

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

        private City GetCityFromReader(SqlDataReader dr)
        {
            City city = new City();
            city.cityId = dr.GetInt32(dr.GetOrdinal("cityId"));
            city.name = dr.GetString(dr.GetOrdinal("name"));
            city.isCapital = dr.GetBoolean(dr.GetOrdinal("isCapital"));

            return city;
        }

        private void Serialize(City state, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(City));
            using (XmlWriter w = XmlWriter.Create(path))
                serializer.Serialize(w, state);

        }

        private string GetFullFileName(string method, int id)
        {
            return string.Concat(_fullPath, "\\City", method, "_", id.ToString(), ".xml");
        }
    }
}
