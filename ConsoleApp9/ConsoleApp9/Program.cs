using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace ConsoleApp9
{
    class Program
    {
        private const string EndpointUrl = "https://mclass22.documents.azure.com:443/";
        private const string PrimaryKey = "sI2PrmdRFKsz6FKRdKlqQ8MZF7BcPJDVBWfIOMrkMkoeBCwUUP7MG9thgnFUbG7VvYyB4iYpAa5w1T3cpnCJJw==";
        private DocumentClient client;

        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task GetStartedDemo()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            // ADD THIS PART TO YOUR CODE
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "FamilyDB" });

            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("FamilyDB"), new DocumentCollection { Id = "FamilyCollection" });

            student st = new student { Id = "100", name = "modi" };

            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri("FamilyDB", "FamilyCollection", "100"));
                
            }
            catch (DocumentClientException de)
            {

                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("FamilyDB", "FamilyCollection"), st);
                  

            }
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<student> familyQuery = this.client.CreateDocumentQuery<student>(
                    UriFactory.CreateDocumentCollectionUri("FamilyDB", "FamilyCollection"), queryOptions);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (student stu in familyQuery)
            {
                Console.WriteLine("\tRead {0}", stu);
            }

        }
    }

    public class student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
