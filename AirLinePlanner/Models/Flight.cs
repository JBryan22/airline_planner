using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirLinePlanner.Models
{
  public class Flight
  {
    private int _id;
    private string _departureCity;
    private string _arrivalCity;
    private DateTime _departureTime;
    private DateTime _arrivalTime;
    private string _flightStatus;

    public Flight(string departureCity, string arrivalCity, DateTime departureTime, DateTime arrivalTime, string flightStatus, int id = 0)
    {
      _id = id;
      _departureCity = departureCity;
      _arrivalCity = arrivalCity;
      _departureTime = departureTime;
      _arrivalTime = arrivalTime;
      _flightStatus = flightStatus;
    }

  public int GetId()
  {
    return _id;
  }
  public string GetDepartureCity()
  {
    return _departureCity;
  }
  public string GetArrivalCity()
  {
    return _arrivalCity;
  }

  public DateTime GetDepartureTime()
  {
    return _departureTime;
  }
  public DateTime GetArrivalTime()
  {
    return _arrivalTime;
  }
  public string GetFlightStatus()
  {
    return _flightStatus;
  }
  public override bool Equals(System.Object otherFlight)
  {
    if (!(otherFlight is Flight))
    {
      return false;
    }
    else
    {
       Flight newFlight = (Flight) otherFlight;
       bool idEquality = this.GetId() == newFlight.GetId();
       Console.WriteLine("id equality" + idEquality);

       bool departureCityEquality = this._departureCity == newFlight.GetDepartureCity();
       Console.WriteLine(departureCityEquality);

       bool departureTimeEquality = this._departureTime == newFlight.GetDepartureTime();
       Console.WriteLine(departureTimeEquality);

       bool arrivalTimeEquality = newFlight.GetArrivalTime() == this._arrivalTime;
       Console.WriteLine(arrivalTimeEquality);

       bool arrivalCityEquality = newFlight.GetArrivalCity() == this._arrivalCity;
       Console.WriteLine(arrivalCityEquality);

       bool flightStatusEquality = newFlight.GetFlightStatus() == this._flightStatus;
       Console.WriteLine(flightStatusEquality);

       return (idEquality && departureCityEquality && departureTimeEquality && arrivalTimeEquality &&  arrivalCityEquality && flightStatusEquality);
     }
  }

  public override int GetHashCode()
  {
   return this.GetDepartureCity().GetHashCode();
  }

  public void Save()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO flights (departure_city, arrival_city, departure_time, arrival_time, flight_status) VALUES (@departureCity, @arrivalCity, @departureTime, @arrivalTime, @flightStatus);";

    MySqlParameter departureCityNew = new MySqlParameter();
    departureCityNew.ParameterName = "@departureCity";
    departureCityNew.Value = this._departureCity;
    cmd.Parameters.Add(departureCityNew);

    MySqlParameter arrivalCityNew = new MySqlParameter();
    arrivalCityNew.ParameterName = "@arrivalCity";
    arrivalCityNew.Value = this._arrivalCity;
    cmd.Parameters.Add(arrivalCityNew);

    MySqlParameter departureTimeNew = new MySqlParameter();
    departureTimeNew.ParameterName = "@departureTime";
    departureTimeNew.Value = this._departureTime;
    cmd.Parameters.Add(departureTimeNew);

    MySqlParameter arrivalTimeNew = new MySqlParameter();
    arrivalTimeNew.ParameterName = "@arrivalTime";
    arrivalTimeNew.Value = this._arrivalTime;
    cmd.Parameters.Add(arrivalTimeNew);

    MySqlParameter flightStatusNew = new MySqlParameter();
    flightStatusNew.ParameterName = "@flightStatus";
    flightStatusNew.Value = this._flightStatus;
    cmd.Parameters.Add(flightStatusNew);

    cmd.ExecuteNonQuery();
    _id = (int) cmd.LastInsertedId;
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
  }

  public static List<Flight> GetAll()
  {
    List<Flight> allFlights = new List<Flight> {};
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT * FROM flights ORDER BY departure_city;";

    var rdr = cmd.ExecuteReader() as MySqlDataReader;
    while(rdr.Read())
    {
      int flightId = rdr.GetInt32(0);
      string departureCity = rdr.GetString(1);
      string arrivalCity = rdr.GetString(2);
      DateTime departureTime = rdr.GetDateTime(3);
      DateTime arrivalTime = rdr.GetDateTime(4);
      string flightStatus = rdr.GetString(5);
      Flight newFlight = new Flight(departureCity, arrivalCity, departureTime, arrivalTime, flightStatus, flightId);
      allFlights.Add(newFlight);
    }
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
    return allFlights;
  }

  public static Flight Find(int id)
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";

    MySqlParameter searchId = new MySqlParameter();
    searchId.ParameterName = "@searchId";
    searchId.Value = id;
    cmd.Parameters.Add(searchId);

    var rdr = cmd.ExecuteReader() as MySqlDataReader;

    int flightId = 0;
    string departureCity = "";
    string arrivalCity = "";
    DateTime departureTime = DateTime.Now;
    DateTime arrivalTime = DateTime.Now;
    string flightStatus = "";

    while(rdr.Read())
    {
      flightId = rdr.GetInt32(0);
      departureCity = rdr.GetString(1);
      arrivalCity = rdr.GetString(2);
      departureTime = rdr.GetDateTime(3);
      arrivalTime = rdr.GetDateTime(4);
      flightStatus = rdr.GetString(5);
    }
    Flight newFlight = new Flight(departureCity, arrivalCity, departureTime, arrivalTime, flightStatus, flightId);
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }

    return newFlight;
  }

  // public void UpdateDescription(string newDescription)
  // {
  //   MySqlConnection conn = DB.Connection();
  //   conn.Open();
  //   var cmd = conn.CreateCommand() as MySqlCommand;
  //   cmd.CommandText = @"UPDATE tasks SET description = @newDescription WHERE id = @searchId;";
  //
  //   MySqlParameter searchId = new MySqlParameter();
  //   searchId.ParameterName = "@searchId";
  //   searchId.Value = _id;
  //   cmd.Parameters.Add(searchId);
  //
  //   MySqlParameter description = new MySqlParameter();
  //   description.ParameterName = "@newDescription";
  //   description.Value = newDescription;
  //   cmd.Parameters.Add(description);
  //
  //   cmd.ExecuteNonQuery();
  //   _description = newDescription;
  //   conn.Close();
  //   if (conn != null)
  //   {
  //     conn.Dispose();
  //   }
  // }

  // public void FinishTask()
  // {
  //   MySqlConnection conn = DB.Connection();
  //   conn.Open();
  //
  //   var cmd = conn.CreateCommand() as MySqlCommand;
  //   cmd.CommandText = @"UPDATE tasks SET done = @done WHERE id = @searchId;";
  //
  //   MySqlParameter taskId = new MySqlParameter();
  //   taskId.ParameterName = "@searchId";
  //   taskId.Value = this._id;
  //   cmd.Parameters.Add(taskId);
  //
  //   MySqlParameter taskDone = new MySqlParameter();
  //   taskDone.ParameterName = "@done";
  //   taskDone.Value = 1;
  //   cmd.Parameters.Add(taskDone);
  //
  //   cmd.ExecuteNonQuery();
  //   _done = true;
  //   conn.Close();
  //   if (conn != null)
  //   {
  //     conn.Dispose();
  //   }
  // }

  // public static List<Task> GetFinished()
  // {
  //   MySqlConnection conn = DB.Connection();
  //   conn.Open();
  //
  //   var cmd = conn.CreateCommand() as MySqlCommand;
  //   cmd.CommandText = @"SELECT * FROM tasks WHERE done = @done ORDER BY duedate;";
  //
  //   MySqlParameter taskDone = new MySqlParameter();
  //   taskDone.ParameterName = "@done";
  //   taskDone.Value = 1;
  //   cmd.Parameters.Add(taskDone);
  //
  //   var rdr = cmd.ExecuteReader() as MySqlDataReader;
  //
  //   var allTasks = new List<Task>();
  //
  //   while (rdr.Read())
  //   {
  //     int id = rdr.GetInt32(0);
  //     string description = rdr.GetString(1);
  //     DateTime dueDate = rdr.GetDateTime(3);
  //     bool done = rdr.GetBoolean(2);
  //     // done == 1 ? _done = true : _done = false;
  //     var newTask = new Task(description, dueDate, done, id);
  //     allTasks.Add(newTask);
  //   }
  //   conn.Close();
  //   if(conn != null)
  //   {
  //     conn.Dispose();
  //   }
  //   return allTasks;
  // }

  // public static List<Task> GetUnFinished()
  // {
  //   MySqlConnection conn = DB.Connection();
  //   conn.Open();
  //
  //   var cmd = conn.CreateCommand() as MySqlCommand;
  //   cmd.CommandText = @"SELECT * FROM tasks WHERE done = @done  ORDER BY duedate;";
  //
  //   MySqlParameter taskDone = new MySqlParameter();
  //   taskDone.ParameterName = "@done";
  //   taskDone.Value = 0;
  //   cmd.Parameters.Add(taskDone);
  //
  //   var rdr = cmd.ExecuteReader() as MySqlDataReader;
  //
  //   var allTasks = new List<Task>();
  //
  //   while (rdr.Read())
  //   {
  //     int id = rdr.GetInt32(0);
  //     string description = rdr.GetString(1);
  //     DateTime dueDate = rdr.GetDateTime(2);
  //     bool done = rdr.GetBoolean(3);
  //     // done == 1 ? _done = true : _done = false;
  //     var newTask = new Task(description, dueDate, done, id);
  //     allTasks.Add(newTask);
  //   }
  //   conn.Close();
  //   if(conn != null)
  //   {
  //     conn.Dispose();
  //   }
  //   return allTasks;
  // }

  public void Delete()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();

    MySqlCommand cmd = new MySqlCommand("DELETE FROM flights WHERE id = @FlightId; DELETE FROM flights_cities WHERE flight_id = @FlightId;", conn);

    MySqlParameter flightIdParameter = new MySqlParameter();
    flightIdParameter.ParameterName = "@FlightId";
    flightIdParameter.Value = this.GetId();

    cmd.Parameters.Add(flightIdParameter);
    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }

  public static void DeleteAll()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"DELETE FROM flights;";
    cmd.ExecuteNonQuery();
    conn.Close();
    if (conn != null)
    {
        conn.Dispose();
    }
  }

  public void AddCityToJoinTable(City newCity)
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO flights_cities (city_id, flight_id) VALUES (@CityId, @FlightId);";

    MySqlParameter cityIdParam = new MySqlParameter();
    cityIdParam.ParameterName = "@CityId";
    cityIdParam.Value = newCity.GetId();
    cmd.Parameters.Add(cityIdParam);

    MySqlParameter flight_id_param = new MySqlParameter();
    flight_id_param.ParameterName = "@FlightId";
    flight_id_param.Value = _id;
    cmd.Parameters.Add(flight_id_param);

    cmd.ExecuteNonQuery();
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
  }

  public List<City> GetCitiesFromJoinTable()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT city_id FROM flights_cities WHERE flight_id = @flightId;";

    MySqlParameter flightIdParameter = new MySqlParameter();
    flightIdParameter.ParameterName = "@flightId";
    flightIdParameter.Value = this._id;
    cmd.Parameters.Add(flightIdParameter);

    var rdr = cmd.ExecuteReader() as MySqlDataReader;

    List<int> cityIds = new List<int> {};
    while(rdr.Read())
    {
      int cityId = rdr.GetInt32(0);
      cityIds.Add(cityId);
    }
    rdr.Dispose();

    List<City> cities = new List<City> {};
    foreach (int cityId in cityIds)
    {
      var cityQuery = conn.CreateCommand() as MySqlCommand;
      cityQuery.CommandText = @"SELECT * FROM cities WHERE id = @CityId;";

      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = cityId;
      cityQuery.Parameters.Add(cityIdParameter);

      var cityQueryRdr = cityQuery.ExecuteReader() as MySqlDataReader;
      while(cityQueryRdr.Read())
      {
        int thisCityId = cityQueryRdr.GetInt32(0);
        string cityName = cityQueryRdr.GetString(1);
        City foundCity = new City(cityName, thisCityId);
        cities.Add(foundCity);
      }
      cityQueryRdr.Dispose();
    }
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
    return cities;
  }

  }
}
