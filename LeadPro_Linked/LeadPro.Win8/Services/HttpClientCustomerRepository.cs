using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeadPro.Model;
using Newtonsoft.Json;

namespace LeadPro.Win8.Services
{
    public class HttpClientCustomerRepository : ICustomerRepository
    {
        private const string ServerUri = "http://localhost:7700/api/";

        public Task<List<Customer>> Get()
        {
            return GetJson<List<Customer>>("customers/");
        }

        public Task<Customer> GetById(int customerId)
        {
            return GetJson<Customer>("customers/" + customerId);
        }

        public async void Save(Customer customer)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                if (customer.Id > 0)
                {
                    await client.PutAsync(ServerUri + "customers/" + customer.Id, content);
                }
                else
                {
                    await client.PostAsync(ServerUri + "customers/", content);
                }
            }
        }

        private async Task<T> GetJson<T>(string uri)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(ServerUri + uri);
                var json = await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}