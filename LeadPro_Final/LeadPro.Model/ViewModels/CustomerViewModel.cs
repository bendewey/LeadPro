using System;
using System.IO;

namespace LeadPro.Model.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private long _id;
        private string _firstName;
        private string _lastName;
        private string _address1;
        private string _address2;
        private string _city;
        private string _state;
        private string _zipCode;
        private string _phone;
        private Stream _imageStream;
        private string _imageBase64;

        public CustomerViewModel(Customer customer)
        {
            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Address1 = customer.Address1;
            Address2 = customer.Address2;
            City = customer.City;
            State = customer.State;
            ZipCode = customer.ZipCode;
            Phone = customer.Phone;
            ImageStream = ConvertBase64ToStream(customer.ImageBase64);
        }

        public long Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value, "Id"); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value, "FirstName"); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value, "LastName"); }
        }

        public string Address1
        {
            get { return _address1; }
            set { SetProperty(ref _address1, value, "Address1"); }
        }

        public string Address2
        {
            get { return _address2; }
            set { SetProperty(ref _address2, value, "Address2"); }
        }

        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value, "City"); }
        }

        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value, "State"); }
        }

        public string ZipCode
        {
            get { return _zipCode; }
            set { SetProperty(ref _zipCode, value, "ZipCode"); }
        }

        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value, "Phone"); }
        }

        public Stream ImageStream
        {
            get { return _imageStream; }
            set
            {
                SetProperty(ref _imageStream, value, "ImageStream");
                _imageBase64 = ConvertStreamToBase64(value);
            }
        }

        private static Stream ConvertBase64ToStream(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                return null;
            }

            var bytes = Convert.FromBase64String(base64);
            return new MemoryStream(bytes);
        }

        private static string ConvertStreamToBase64(Stream stream)
        {
            if (stream == null)
                return null;

            if (!stream.CanRead)
                return null;

            // No support for images bigger than 2GB
            if (stream.Length > (long) int.MaxValue)
                return null;

            var bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            return Convert.ToBase64String(bytes);
        }

        public Customer AsCustomer()
        {
            return new Customer()
                {
                    Id = Id,
                    FirstName = FirstName,
                    LastName = LastName,
                    Address1 = Address1,
                    Address2 = Address2,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    Phone = Phone,
                    ImageBase64 = _imageBase64
                };
        }
    }
}