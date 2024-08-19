using ContactsAppLibrary.Services.Auth.ApiHelper;
using ContactsAppLibrary.Services.Models;
using ContactsAppLibrary.Services.Models.Auth;
using System.Net.Http.Headers;
using System.Net.Http.Json;
//Contact Service To handle api endpint calls
namespace ContactsAppLibrary.Services.EndPoints
{
    public class ContactsEndPoint : IContactsEndPoint
    {
        IApiHelper? _apiHelper;
        private readonly IApiHelper _apiClient;
        private readonly AuthenticatedUserModel _authedModel;
        IList<ContactsModel>? services;


        public ContactsEndPoint(IApiHelper aPIHelper, AuthenticatedUserModel authedModel)
        {
            _apiClient = aPIHelper;
            _authedModel = authedModel;
        }
        /// <summary>
        /// This method is used to Get a list of Service from the API
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ContactsModel>> GetContacts()
        {
            try
            {
                if (_authedModel.accessToken != null)
                {
                    _apiClient.ApiClient.DefaultRequestHeaders.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                    _apiClient.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authedModel.accessToken}");

                    services = await _apiClient.ApiClient.GetFromJsonAsync<List<ContactsModel>>("/api/Contacts");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return services.ToList();
        }



        //Get A single Contact By Id
        public async Task<ContactsModel> GetContact(int id)
        {
            ContactsModel contact = new ContactsModel();
            try
            {
                if (_authedModel.accessToken != null)
                {
                    _apiClient.ApiClient.DefaultRequestHeaders.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                    _apiClient.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authedModel.accessToken}");

                    contact = await _apiClient.ApiClient.GetFromJsonAsync<ContactsModel>($"api/Contacts?Id={id}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return contact;
        }

        //Deleting the Contact
        public async Task DeleteContact(int id)
        {
            try
            {
                if (_authedModel.accessToken != null)
                {
                    _apiClient.ApiClient.DefaultRequestHeaders.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                    _apiClient.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authedModel.accessToken}");

                    var httpResponseMessage = await _apiClient.ApiClient.DeleteAsync($"api/Contacts?id={id}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Insert a new Contact
        public async Task<bool> InsertContact(ContactsModel contact)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                if (_authedModel.accessToken != null)
                {
                    _apiClient.ApiClient.DefaultRequestHeaders.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                    _apiClient.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authedModel.accessToken}");

                    result = await _apiClient.ApiClient.PostAsJsonAsync("/api/Contacts", contact);

                }

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Update a contact 
        public async Task<bool> UpdateContact(ContactsModel contact)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage();
                if (_authedModel.accessToken != null)
                {
                    _apiClient.ApiClient.DefaultRequestHeaders.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Clear();
                    _apiClient.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                    _apiClient.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authedModel.accessToken}");

                    result = await _apiClient.ApiClient.PutAsJsonAsync("/api/Contacts", contact);

                }

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }

    }
}
