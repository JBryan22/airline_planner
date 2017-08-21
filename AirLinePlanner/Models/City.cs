using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirLinePlanner.Models;

namespace AirLinePlanner.Models
{
  public class City
  {
    private int _id;
    private string _name;

    public City(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = newCity.GetId() == _id;
        bool cityNameEquality = newCity.GetName() == _name;
        return (idEquality && cityNameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public void Save()
     {
       MySqlConnection mySqlConnection = DB.Connection();
       mySqlConnection.Open();

       var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

       MySqlParameter name = new MySqlParameter();
       name.ParameterName = "@name";
       name.Value = this._name;
       cmd.Parameters.Add(name);

       cmd.ExecuteNonQuery();
       _id = (int) cmd.LastInsertedId;
     }

    public static List<City> GetAll()
     {
       List<City> allCities = new List<City> {};

       MySqlConnection mySqlConnection = DB.Connection();
       mySqlConnection.Open();

       var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM cities;";

       var rdr = cmd.ExecuteReader() as MySqlDataReader;
       while(rdr.Read())
       {
         int cityId = rdr.GetInt32(0);
         string cityName = rdr.GetString(1);
         City newCity = new City(cityName, cityId);
         allCities.Add(newCity);
       }
       return allCities;
     }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM flights_cities WHERE city_id = @CityId;", conn);

      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@CityId";
      flightIdParameter.Value = this.GetId();

      cmd.Parameters.Add(flightIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static bool CityExists(string cityName)
    {
      bool exists = false;
      List<City> allCities = City.GetAll();
      foreach(var city in allCities)
      {
        string foundCityName = city.GetName().ToLower();
        if (foundCityName == cityName.ToLower())
        {
          exists = true;
          break;
        }
      }
      return exists;
    }

    public void AddFlightToJoinTable(Flight newFlight)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights_cities (city_id, flight_id) VALUES (@CityId, @FlightId);";

      MySqlParameter flight_id_param = new MySqlParameter();
      flight_id_param.ParameterName = "@FlightId";
      flight_id_param.Value = newFlight.GetId();
      cmd.Parameters.Add(flight_id_param);

      MySqlParameter city_id_param = new MySqlParameter();
      city_id_param.ParameterName = "@CityId";
      city_id_param.Value = this._id;
      cmd.Parameters.Add(city_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Flight> GetFlights()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT flight_id FROM flights_cities WHERE city_id = @cityId;";

      MySqlParameter cityIdParam = new MySqlParameter();
      cityIdParam.ParameterName = "@cityId";
      cityIdParam.Value = this._id;
      cmd.Parameters.Add(cityIdParam);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> flightIds = new List<int>();
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        flightIds.Add(flightId);
      }
      rdr.Dispose();

      List<Flight> allFlights = new List<Flight>();
      foreach (var flightIdloop in flightIds)
      {
        var flightQuery = conn.CreateCommand() as MySqlCommand;
        flightQuery.CommandText = @"SELECT * FROM flights WHERE id = @flightIdP;";

        MySqlParameter flightIdParam = new MySqlParameter();
        flightIdParam.ParameterName = "@flightIdP";
        flightIdParam.Value = flightIdloop;
        cmd.Parameters.Add(flightIdParam);

        var flightQueryRdr = flightQuery.ExecuteReader() as MySqlDataReader;
        while(flightQueryRdr.Read())
        {
          int flightIdNew = rdr.GetInt32(0);
          string departureCity = rdr.GetString(1);
          string arrivalCity = rdr.GetString(2);
          DateTime departureTime = rdr.GetDateTime(3);
          DateTime arrivalTime = rdr.GetDateTime(4);
          string flightStatus = rdr.GetString(5);
          Flight newFlight = new Flight(departureCity, arrivalCity, departureTime, arrivalTime, flightStatus, flightIdNew);
          allFlights.Add(newFlight);
        }
        flightQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }

    public static City Find(int id)
    {
      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();

      var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";
      while(rdr.Read())
      {
        cityId = rdr.GetInt32(0);
        cityName = rdr.GetString(1);
      }
      City newCity = new City(cityName, cityId);
      return newCity;
    }

    public static void DeleteAll()
    {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"DELETE FROM cities;";
       cmd.ExecuteNonQuery();
    }
  }
}
