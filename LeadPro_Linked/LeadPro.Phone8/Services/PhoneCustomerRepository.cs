using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LeadPro.Model;
using Newtonsoft.Json;

namespace LeadPro.Phone8.Services
{
    public class PhoneCustomerRepository : ICustomerRepository
    {
        private const string ServerUri = "http://192.168.1.3:7700/api/";

        public Task<List<Customer>> Get()
        {
            return GetJson<List<Customer>>("customers/");
        }

        public Task<Customer> GetById(int customerId)
        {
            return GetJson<Customer>("customers/" + customerId);
        }

        public void Save(Customer customer)
        {
            HttpWebRequest client;
            if (customer.Id > 0)
            {
                client = (HttpWebRequest)WebRequest.Create(ServerUri + "customers/" + customer.Id);
                client.Method = "PUT";
            }
            else
            {
                client = (HttpWebRequest)WebRequest.Create(ServerUri + "customers/");
                client.Method = "POST";
            }

            client.ContentType = "application/json";

            var json = JsonConvert.SerializeObject(customer);
            var factory = new TaskFactory<Stream>();
            factory.FromAsync(client.BeginGetRequestStream, client.EndGetRequestStream, null).ContinueWith(t =>
            {
                var stream = t.Result;
                var writer = new StreamWriter(stream);
                writer.Write(json);
                writer.Flush();

                var responseFactory = new TaskFactory<WebResponse>();
                return responseFactory.FromAsync(client.BeginGetResponse, client.EndGetResponse, null).ContinueWith(ts =>
                {
                    // do nothing
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task<T> GetJson<T>(string uri)
        {
            var client = (HttpWebRequest)HttpWebRequest.Create(ServerUri + uri);

            var factory = new TaskFactory<WebResponse>();
            return factory.FromAsync(client.BeginGetResponse, client.EndGetResponse, null).ContinueWith(t =>
            {
                using (var response = t.Result.GetResponseStream())
                {
                    var reader = new StreamReader(response);
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            });
        }
    }
}
