using ContactsAppLibrary.Services.Auth.ApiHelper;
using ContactsAppLibrary.Services.Models;
using ContactsAppLibrary.Services.Models.Auth;
using System.Net.Http.Json;
//Contact Service To handle api endpint calls
namespace ContactsAppLibrary.Services.EndPoints
{
    public class ContactsEndPoint : IContactsEndPoint
    {

        private readonly IApiHelper _apiClient;
        private readonly AuthenticatedUserModel _authedModel;
        IList<ContactsModel>? services;
        private ContactsModel contact;

        HttpResponseMessage result;


        public ContactsEndPoint(IApiHelper apiHelper, AuthenticatedUserModel authedModel)
        {
            _apiClient = apiHelper;
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
                if (_authedModel != null && _authedModel.accessToken != null)
                {
                    _apiClient.InitializeClient(_authedModel.accessToken);
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

            try
            {
                if (_authedModel != null && _authedModel.accessToken != null)
                {
                    _apiClient.InitializeClient(_authedModel.accessToken);
                    contact = new ContactsModel();
                    contact = await _apiClient.ApiClient.GetFromJsonAsync<ContactsModel>($"/api/Contacts/{id}");
                }
                return contact;
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
                if (_authedModel != null && _authedModel.accessToken != null)
                {
                    result = new HttpResponseMessage();
                    _apiClient.InitializeClient(_authedModel.accessToken);
                    var httpResponseMessage = await _apiClient.ApiClient.DeleteAsync($"/api/Contacts?Id={id}");
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

            try
            {
                if (_authedModel != null && _authedModel.accessToken != null)
                {
                    result = new HttpResponseMessage();
                    _apiClient.InitializeClient(_authedModel.accessToken);
                    result = await _apiClient.ApiClient.PostAsJsonAsync("/api/Contacts", contact);

                }

                return result.IsSuccessStatusCode;
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

                if (_authedModel != null && _authedModel.accessToken != null)
                {
                    result = new HttpResponseMessage();
                    result = await _apiClient.ApiClient.PutAsJsonAsync($"/api/Contacts/{contact.Id}", contact);
                }
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

    }
}
