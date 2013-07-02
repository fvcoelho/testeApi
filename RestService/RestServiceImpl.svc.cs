using Wallmart.Database;

namespace RestService
{
    public class RestServiceImpl : IRestServiceImpl
    {
        CityDAL c = new CityDAL();

        public string Add(City city)
        {
            c.Add(city);

            return "New City added !" + city.cityId.ToString();
        }

        public string Update(int id)
        {
            City oldCity = new City();
            c.UpdateById(oldCity);

            return "City updated !";
        }

        public string Delete(int id)
        {
            c.RemoveById(id);

            return "City deleted !";
        }

        public string Get(int id)
        {
            c.ListAll().Find(city => city.cityId == id);

            return "Get ok";
        }


    }
}