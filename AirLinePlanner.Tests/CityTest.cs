using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirLinePlanner.Models;

namespace AirLinePlanner.Tests
{

[TestClass]
public class CityTest : IDisposable
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
    }
    public void Dispose()
    {
      City.DeleteAll();
      Flight.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = City.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_TrueForSameDescription_City()
    {
      //Arrange, Act
      City firstCity = new City("Seattle");
      City secondCity = new City("Seattle");

      //Assert
      Assert.AreEqual(firstCity, secondCity);
    }

    [TestMethod]
    public void Save_CitySavesToDatabase_CityList()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      foreach(var city in result)
      {
        Console.WriteLine("RESulst============"+ city.GetName());
      }

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsCityInDatabase_City()
    {
      //Arrange
      City testCity = new City("Boston");
      testCity.Save();

      //Act
      City result = City.Find(testCity.GetId());

      //Assert
      Assert.AreEqual(testCity, result);
    }
  [TestMethod]
  public void AddFlight_AddsFlightToCity_FlightList()
  {
    //Arrange
    City testCity = new City("Seattle");
    testCity.Save();

    Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
    testFlight.Save();

    //Act
    testCity.AddFlightToJoinTable(testFlight);

    List<Flight> result = testCity.GetFlights();
    List<Flight> testList = new List<Flight>{testFlight};

    //Assert
    CollectionAssert.AreEqual(testList, result);
  }

  [TestMethod]
  public void GetFlights_ReturnsAllCityFlights_FlightList()
  {
    //Arrange
    City testCity = new City("Portland");
    testCity.Save();

    Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
    testFlight.Save();

    Flight testFlight2 = new Flight("New York", "Los Angelos", new DateTime(2017, 08, 16, 9, 30, 0), new DateTime(2017, 08, 15, 7, 45, 0), "on time");
    testFlight2.Save();

    //Act
    testCity.AddFlightToJoinTable(testFlight2);
    List<Flight> result = testCity.GetFlights();
    List<Flight> testList = new List<Flight> {testFlight2};

    //Assert
    CollectionAssert.AreEqual(testList, result);
  }
    [TestMethod]
    public void Delete_DeletesCityAssociationsFromDatabase_CityList()
    {
      //Arrange
      Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight.Save();

      string testCityString = "Seattle";
      City testCity = new City (testCityString);
      testCity.Save();

      //Act
      testCity.AddFlightToJoinTable(testFlight);
      testCity.Delete();

      List<City> resultFlightCities = testFlight.GetCitiesFromJoinTable();
      List<City> testFlightCities = new List<City> {};

      //Assert
      CollectionAssert.AreEqual(testFlightCities, resultFlightCities);
    }
    // [TestMethod]
    // public void FinishCity_CityGetsCompletedInDatabaseWhenItsMarkedAsFinished_True()
    // {
    //   var myCity = new City("Mow the lawn");
    //   myCity.Save();
    //   myCity.FinishCity();
    //
    //   var expected = new List<City>{myCity};
    //
    //   var result = City.GetFinished();
    //
    //   CollectionAssert.AreEqual(expected, result);
    // }

    // [TestMethod]
    // public void GetAll_GetsAllCityFromDatabaseSortedbyDuedateInAscending_CityList()
    // {
    //   //Arrange
    //   DateTim = new DateTime(2017, 8, 20);
    //   DateTime today = new DateTime(2017, 8, 21);
    //
    //   City testCity = new City("Home stuff");
    //   testCity.Save();
    //   City testCity1 = new City("Home stuff", today);
    //   testCity1.Save();
    //   List<City> expected = new List<City>{testCity, testCity1};
    //
    //   //Act
    //   List<City> actual = City.GetAll();
    //
    //
    //   //Assert
    //   CollectionAssert.AreEqual(expected, actual);
    // }

  }
}
