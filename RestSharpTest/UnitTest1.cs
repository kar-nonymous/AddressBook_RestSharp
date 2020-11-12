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
        /// UC 22:
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
        /// <summary>
        /// UC 23:
        /// POST api will add multiple contacts to the json file created
        /// </summary>
        [TestMethod]
        public void OnCallingPOSTApi_ShouldAddMultipleContacts()
        {
            List<Contacts> contacts = new List<Contacts>();
            contacts.Add(new Contacts { FirstName = "Peter", LastName = "Parker", Address = "Queens", City = "New York", State = "New York", Zip = 281006, PhoneNo = 7206183244, Email = "peter.parker@gmail.com" });
            contacts.Add(new Contacts { FirstName = "Steve", LastName = "Jobs", Address = "San Francisco", City = "San Francisco", State = "California", Zip = 281001, PhoneNo = 8265800789, Email = "steve.jobs@apple.com" });
            contacts.ForEach(contact =>
            {
                //Arrange
                RestRequest request = new RestRequest("/contacts", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("FirstName", contact.FirstName);
                jObjectBody.Add("LastName", contact.LastName);
                jObjectBody.Add("Address", contact.Address);
                jObjectBody.Add("City", contact.City);
                jObjectBody.Add("State", contact.State);
                jObjectBody.Add("Zip", contact.Zip);
                jObjectBody.Add("PhoneNo", contact.PhoneNo);
                jObjectBody.Add("Email", contact.Email);

                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
                Assert.AreEqual(contact.FirstName, dataResponse.FirstName);
                Assert.AreEqual(contact.LastName, dataResponse.LastName);
                Assert.AreEqual(contact.Address, dataResponse.Address);
                Assert.AreEqual(contact.City, dataResponse.City);
                Assert.AreEqual(contact.State, dataResponse.State);
                Assert.AreEqual(contact.Zip, dataResponse.Zip);
                Assert.AreEqual(contact.PhoneNo, dataResponse.PhoneNo);
                Assert.AreEqual(contact.Email, dataResponse.Email);
                System.Console.WriteLine(response.Content);
            });
        }
    }
}
