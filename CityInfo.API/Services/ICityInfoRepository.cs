using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        //IQueryable<City> GetCities(); We can either use IQueryable or IEnumerable. 
        // IQueryable causes execution of the query on the server and hence author suggests using IEnumerable.
        // Need to research and understand further the difference between IEnumerable and IQueryable.
        
        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool includePointsOfInterest);

        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        bool CityExists(int cityId);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        bool Save();
    }
}
