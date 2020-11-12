using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RestSharpTest
{
    /// <summary>
    /// Creating the class to take the different entries of a contact
    /// </summary>
    public class Contacts
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public long PhoneNo { get; set; }
        public string Email { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Calling RestClient class
        /// </summary>
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }
        /// <summary>
        /// UC 1:
        /// Setting up the method to get all the contacts.
        /// </summary>
        /// <returns></returns>
        private IRestResponse GetContactList()
        {
            //Arrange
            RestRequest request = new RestRequest("/contacts", Method.GET);

            //Act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingGETApi_ShouldReturnContactList()
        {
            IRestResponse response = GetContactList();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Contacts> dataResponse = JsonConvert.DeserializeObject<List<Contacts>>(response.Content);
            Assert.AreEqual(3, dataResponse.Count);
            foreach (Contacts contact in dataResponse)
            {
                System.Console.WriteLine("FirstName: " + contact.FirstName + " LastName: " + contact.LastName + " Address: " + contact.Address);
            }
        }
    }
}
