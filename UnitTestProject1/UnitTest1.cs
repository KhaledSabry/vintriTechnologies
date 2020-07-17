using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using vintriTechnologies.Controllers;
using vintriTechnologies.Helper;
using vintriTechnologies.Model;
using Xunit; 
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using vintriTechnologies;
using System.Text.Json;
using System.Text; 

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private HttpClient _client;
         
        [TestInitialize]
        public void TestInt()
        {
            var rootDir = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location))));

            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .UseContentRoot(rootDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(rootDir)
                    .AddJsonFile("appsettings.json")
                    .Build()
                )
                .UseStartup<Startup>());
            _client = server.CreateClient();



            var Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); // define app setting 
            
            //var fixture = new Fixture(); //will be used if we needed fack Httpresponce - but we don't 

            //mockHttpMessageHandler.Protected()
            //    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .ReturnsAsync(new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = new StringContent(fixture.Create<String>()),
            //    });

            var client = new HttpClient();// (mockHttpMessageHandler.Object);
           
        }

        [TestMethod]
        [DynamicData(nameof(TestMethod1Data))]
        [DataTestMethod] 
        public async Task TestMethod1(string ExpectedJson, string keyword, List<inputTestDataModel> inputData)
        { // to test Task 1 and 2 
            await _client.GetStringAsync("beer/clear/database");

            foreach (var input in inputData)
            { // adding votes 
                HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(input.vote), Encoding.UTF8, "application/json");
                var rateResponse = await _client.PostAsync("beer/rate/" + input.beerId, content);
                rateResponse.EnsureSuccessStatusCode(); 
            }

            var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format("beer/search/{0}", keyword));
            var response = await _client.SendAsync(request)  ; 
            response.EnsureSuccessStatusCode();
            var resultjson = ( await response.Content.ReadAsAsync<OkObjectResult>()).Value;
             

            List<VoteSummaryModel> results = JsonSerializer.Deserialize<List<VoteSummaryModel>>(resultjson.ToString().Replace("\n","").Replace("\r", "").Replace("\t", ""));
            List<VoteSummaryModel> Expected = JsonSerializer.Deserialize<List<VoteSummaryModel>>(ExpectedJson.ToString().Replace("\n", "").Replace("\r", "").Replace("\t", ""));
            for (int i=0;i<  results.Count ;++i)
            { 
                Assert.AreEqual(Expected[i].id, results[i].id);
                Assert.AreEqual(Expected[i].name, results[i].name);
                for (int y = 0; y < results[i].userRatings.Count; ++y)
                { 
                    Assert.AreEqual(Expected[i].userRatings[y].rating, results[i].userRatings[y].rating);
                    Assert.AreEqual(Expected[i].userRatings[y].username, results[i].userRatings[y].username);
                    Assert.AreEqual(Expected[i].userRatings[y].comments, results[i].userRatings[y].comments); 
                }
            }

             
        }
        public static IEnumerable<object[]> TestMethod1Data
        {
            get
            {
                return new[]
                {
                    new object[] 
                    {
@"
[
  {
    ""id"": 3,
    ""name"": ""Berliner Weisse With Yuzu - B-Sides"",
    ""description"": ""Japanese citrus fruit intensifies the sour nature of this German classic."",
    ""userRatings"": [
      {
        ""username"": ""ahmed_ali@gmail.com"",
        ""rating"": 4,
        ""comments"": ""comments 2""
      },
      {
        ""username"": ""ffs@hotmail.com"",
        ""rating"": 1,
        ""comments"": ""comments 3""
      },
      {
        ""username"": ""test_user@hotmail.com"",
        ""rating"": 3,
        ""comments"": ""comments 4""
      }
    ]
  },
  {
    ""id"": 35,
    ""name"": ""Berliner Weisse With Raspberries And Rhubarb - B-Sides"",
    ""description"": ""Tart, dry and acidic with a punch of summer berry as rhubarb crumble."",
    ""userRatings"": []
  },
  {
    ""id"": 193,
    ""name"": ""Blitz Berliner Weisse"",
    ""description"": ""Our sour recipe for all fruit Blitz beers uses a process called kettle souring. In this we steep a bag of malt in the wort to allow the bacteria to grow in it."",
    ""userRatings"": []
    }
]",
                        "Berliner",
                        new List<inputTestDataModel>{
                            new inputTestDataModel(){beerId=4,vote= new VoteModel() { username = "khaled_sabry_f@hotmail.com", rating=2,comments= "comments 1" } },
                            new inputTestDataModel(){beerId=3, vote= new VoteModel() { username = "ahmed_ali@gmail.com", rating = 4, comments = "comments 2" } },
                            new inputTestDataModel(){ beerId=3,vote=  new VoteModel() { username = "ffs@hotmail.com", rating = 1, comments = "comments 3" } },
                            new inputTestDataModel(){ beerId=3, vote= new VoteModel() { username = "test_user@hotmail.com", rating = 3, comments = "comments 4" } },
                        }
                    } ,
                     
                    
                };
            }
        }





        [TestMethod]
        [DynamicData(nameof(TestMethod2Data))]
        [DataTestMethod]
        public async Task TestMethod2(HttpStatusCode expectedStatusCode, inputTestDataModel inputData)
        { // to model validation 
            await _client.GetStringAsync("beer/clear/database");

            HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(inputData.vote), Encoding.UTF8, "application/json");
            var rateResponse = await _client.PostAsync("beer/rate/" + inputData.beerId, content);
             

            Assert.AreEqual( rateResponse.StatusCode, expectedStatusCode);
        }
        public static IEnumerable<object[]> TestMethod2Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        HttpStatusCode.OK,new inputTestDataModel(){beerId=4,vote= new VoteModel() { username = "khaled_sabry_f@hotmail.com", rating=2,comments= "comments 1" } },
                    } ,
                    new object[]{
                        HttpStatusCode.BadRequest,new inputTestDataModel(){beerId=3, vote= new VoteModel() { username = "ahmed_ali@gmailcom", rating = 4, comments = "comments 2" } },
                    },
                    new object[]{
                      HttpStatusCode.BadRequest,new inputTestDataModel(){ beerId=3,vote=  new VoteModel() { username = "ffs@hotmail.com", rating = 6, comments = "comments 3" } },
                    },
                    new object[]{
                       HttpStatusCode.NotFound,new inputTestDataModel(){ beerId=3000, vote= new VoteModel() { username = "test_user@hotmail.com", rating = 3, comments = "comments 4" } },
                    }
                     
                };
            }
        }


        public class inputTestDataModel
        {
            public int beerId { get; set; }
            public VoteModel vote { get; set; } 
        }
}
   
}
