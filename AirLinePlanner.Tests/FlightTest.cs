using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirLinePlanner.Models;
using System.Collections.Generic;
using System;

namespace AirLinePlanner.Tests
{
  [TestClass]
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=	8889;database=airline_planner_test;";
    }
    [TestMethod]
    public void Equals_TrueForSameDescription_City()
    {
      //Arrange, Act
      Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time", 1);

      Flight testFlight2 = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time", 1);

      bool result = testFlight.Equals(testFlight2);

      //Assert
      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Flight.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }


    [TestMethod]
    public void Save_FlightSavesToDatabase_FlightList()
    {
      //Arrange
      Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight.Save();
      List<Flight> expected = new List<Flight>{testFlight};

      //Act
      List<Flight> actual = Flight.GetAll();
      //Assert
      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesFlightFromDatabase_FlightList()
    {
      //Arrange
      Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 09, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight.Save();

      Flight testFlight2 = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight2.Save();


      //Act
      testFlight2.Delete();
      List<Flight> resultCategories = Flight.GetAll();
      List<Flight> testFlightList = new List<Flight> {testFlight};

      //Assert
      CollectionAssert.AreEqual(testFlightList, resultCategories);
    }

    [TestMethod]
    public void GetCitiesFromJoinTable_ReturnsAllFlightCity_CityList()
    {
      //Arrange
      Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight.Save();

      City testCity = new City("Seattle");
      testCity.Save();

      City testCity2 = new City("Boston");
      testCity2.Save();

      testFlight.AddCityToJoinTable(testCity);
      testFlight.AddCityToJoinTable(testCity2);

      //Act

      List<City> savedCitys = testFlight.GetCitiesFromJoinTable();
      List<City> testList = new List<City> {testCity, testCity2};

      //Assert
      CollectionAssert.AreEqual(testList, savedCitys);
    }

    [TestMethod]
    public void AddCityToJoinTable_AddCityToJoinTable_Void()
    {
      //Arrange

      Flight testFlight2 = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight2.Save();

      City testCity = new City("Seattle");
      testCity.Save();

      City testCity2 = new City("Boston");
      testCity2.Save();

      //Act
      testFlight2.AddCityToJoinTable(testCity);
      testFlight2.AddCityToJoinTable(testCity2);

      List<City> result = testFlight2.GetCitiesFromJoinTable();
      List<City> testList = new List<City>{testCity, testCity2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesFlightAssociationsFromJoinTable_Void()
    {
      //Arrange

      City testCity = new City("Seattle");
      testCity.Save();

      Flight testFlight = new Flight("Seattle", "Boston", new DateTime(2017, 08, 15, 9, 30, 0), new DateTime(2017, 08, 15, 5, 45, 0), "on time");
      testFlight.Save();
      testFlight.AddCityToJoinTable(testCity);
      //Act
      testFlight.Delete();

      List<Flight> resultCityFlights = testCity.GetFlights();
      List<Flight> testCityFlights = new List<Flight> {};

      //Assert
      CollectionAssert.AreEqual(testCityFlights, resultCityFlights);
    }
    public void Dispose()
    {
      City.DeleteAll();
      Flight.DeleteAll();
    }
  }
}
